using Core.Models;
using Dapper;
using DataAccess.Config;
using DataAccess.Repository.Abstract;
using Npgsql;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace DataAccess.Repository
{
    internal class CabinetRepository : ICabinetRepository
    {
        private readonly string _connectionString;
        public CabinetRepository(ConnectionString connectionString)
        {
            _connectionString = connectionString.Value;
        }

        public async Task<int> CheckLastSlotOpenedAsync(string id)
        {
            try
            {
                const string query = "SELECT slot_opened FROM Cabinets_Last_Slot WHERE cabinet_uuid = @pId;";
                using var conn = new NpgsqlConnection(_connectionString);
                var transactionResult = await conn.ExecuteScalarAsync<int>(query, new
                {
                    pId = id
                });

                return transactionResult;
            }
            catch (Exception e)
            {
                throw new Exception($"An error ocurred counting the cabinet slots", e);
            }
        }

        public async Task<int> CountSlotsAsync(string id)
        {
            try
            {
                const string query = "SELECT COUNT(b.id) FROM Cabinets_Cabinet a INNER JOIN Cabinets_Slot b ON a.uuid = b.cabinet_uuid WHERE a.uuid = @pId GROUP BY a.uuid;";
                using var conn = new NpgsqlConnection(_connectionString);
                var transactionResult = await conn.ExecuteScalarAsync<int>(query, new
                {
                    pId = id
                });

                return transactionResult;
            }
            catch (Exception e)
            {
                throw new Exception($"An error ocurred counting the cabinet slots", e);
            }
        }

        public async Task<bool> CreateAsync(string deviceId, string macAddress, string name, string gps)
        {
            try
            {
                const string query = "INSERT INTO Cabinets_Cabinet (uuid, mac_addr, name, gps_point) VALUES (@pDeviceId, @pMacAddress, @pName, @pGps); " +
                    "INSERT INTO Cabinets_Last_Slot (cabinet_uuid, slot_opened) VALUES (@pDeviceId, 0)";
                using var conn = new NpgsqlConnection(_connectionString);
                var transactionResult = await conn.ExecuteAsync(query, new
                {
                    pDeviceId = deviceId,
                    pMacAddress = macAddress,
                    pName = name,
                    pGps = gps
                });

                return transactionResult != 0;
            }
            catch (Exception e)
            {
                throw new Exception($"An error ocurred Creating the cabinet", e);
            }
        }

        public async Task<IEnumerable<CabinetModel>> GetAllAsync()
        {
            try
            {
                const string query = "SELECT a.uuid, a.mac_addr, a.name, a.gps_point, COUNT(b.id) as slot_count, SUM(CASE WHEN b.status_id = 1 THEN 1 ELSE 0 END) as available_slots FROM Cabinets_Cabinet a INNER JOIN Cabinets_Slot b ON a.uuid = b.cabinet_uuid GROUP BY a.uuid;";
                using var conn = new NpgsqlConnection(_connectionString);
                var transactionResult = await conn.QueryAsync<CabinetModel>(query);

                return transactionResult;
            }
            catch (Exception e)
            {
                throw new Exception($"An error ocurred getting all the cabinet", e);
            }
        }

        public async Task<CabinetModel> GetByIdAsync(string id)
        {
            try
            {
                var cabinetRepository = new Dictionary<string, CabinetModel>();
                const string query = "SELECT a.uuid, a.mac_addr, a.name, a.gps_point, b.cabinet_uuid, b.id, b.last_update, b.status_id, c.id, c.name FROM Cabinets_Cabinet a INNER JOIN Cabinets_Slot b ON a.uuid = b.cabinet_uuid INNER JOIN Cabinets_Slots_Status c ON b.status_id = c.id WHERE a.uuid = @pId;";
                using var conn = new NpgsqlConnection(_connectionString);
                var transactionResult = await conn.QueryAsync<CabinetModel, SlotModel, SlotStatusModel, CabinetModel>(query,
                (cabinet, slot, status) =>
                {
                    if (!cabinetRepository.TryGetValue(cabinet.uuid, out CabinetModel? myCabinet))
                    {
                        myCabinet = cabinet;
                        myCabinet.slots = new List<SlotModel>();
                        cabinetRepository.Add(myCabinet.uuid, myCabinet);
                    }
                    if(slot != null)
                    {
                        slot.status = status;
                        myCabinet.slots.Add(slot);
                    }

                    return myCabinet;
                }, 
                new
                {
                    pId = id
                }, splitOn: "cabinet_uuid,status_id");

                return transactionResult.Distinct().FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new Exception($"An error ocurred getting the cabinet", e);
            }
        }
    }
}
