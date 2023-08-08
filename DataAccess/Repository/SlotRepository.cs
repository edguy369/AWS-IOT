using Core.Models;
using Dapper;
using DataAccess.Config;
using DataAccess.Repository.Abstract;
using Npgsql;

namespace DataAccess.Repository;

internal class SlotRepository : ISlotRepository
{
    private readonly string _connectionString;
    public SlotRepository(ConnectionString connectionString)
    {
        _connectionString = connectionString.Value;
    }

    public async Task<bool> CheckAvailabilityAsync(string cabinetId, int id)
    {
        try
        {
            const string query = "SELECT status_id FROM Cabinets_Slot WHERE cabinet_uuid = @pCabinetId AND id = @pId;";
            using var conn = new NpgsqlConnection(_connectionString);
            var transactionResult = await conn.ExecuteScalarAsync<int>(query, new
            {
                pId = id,
                pCabinetId = cabinetId
            });

            return transactionResult == 1;
        }
        catch (Exception e)
        {
            throw new Exception($"An error ocurred Creating the cabinet pod", e);
        }
    }

    public async Task<bool> CloseAsync(string cabinetId, int id, DateTime timeStamp)
    {
        try
        {
            const string query = "UPDATE Cabinets_Slot SET status_id = 1, last_update = @pTime WHERE cabinet_uuid = @pCabinetId AND id = @pId;";
            using var conn = new NpgsqlConnection(_connectionString);
            var transactionResult = await conn.ExecuteAsync(query, new
            {
                pId = id,
                pCabinetId = cabinetId,
                pTime = timeStamp
            });

            return transactionResult != 0;
        }
        catch (Exception e)
        {
            throw new Exception($"An error ocurred Creating the cabinet pod", e);
        }
    }

    public async Task<bool> CreateAsync(string cabinetId, int id)
    {
        try
        {
            const string query = "INSERT INTO Cabinets_Slot (id, status_id, cabinet_uuid) VALUES (@pId, @pStatus, @pCabinetId);";
            using var conn = new NpgsqlConnection(_connectionString);
            var transactionResult = await conn.ExecuteAsync(query, new
            {
                pId = id,
                pStatus = 1,
                pCabinetId = cabinetId
            });

            return transactionResult != 0;
        }
        catch (Exception e)
        {
            throw new Exception($"An error ocurred Creating the cabinet pod", e);
        }
    }

    public async Task<SlotModel> GetByIdAsync(string cabinetId, int id)
    {
        try
        {
            const string query = "SELECT status_id FROM Cabinets_Slot WHERE cabinet_uuid = @pCabinetId AND id = @pId;";
            using var conn = new NpgsqlConnection(_connectionString);
            var transactionResult = await conn.QueryFirstOrDefaultAsync<SlotModel>(query, new
            {
                pId = id,
                pCabinetId = cabinetId
            });

            return transactionResult;
        }
        catch (Exception e)
        {
            throw new Exception($"An error ocurred Creating the cabinet pod", e);
        }
    }

    public async Task<bool> OpenAsync(string cabinetId, int id, DateTime timeStamp)
    {
        try
        {
            const string query = "UPDATE Cabinets_Slot SET status_id = 2, last_update = @pTime WHERE cabinet_uuid = @pCabinetId AND id = @pId; " +
                "UPDATE Cabinets_Last_Slot SET slot_opened = @pId WHERE cabinet_uuid = @pCabinetId;";
            using var conn = new NpgsqlConnection(_connectionString);
            var transactionResult = await conn.ExecuteAsync(query, new
            {
                pId = id,
                pCabinetId = cabinetId,
                pTime = timeStamp
            });

            return transactionResult != 0;
        }
        catch (Exception e)
        {
            throw new Exception($"An error ocurred Creating the cabinet pod", e);
        }
    }

    public async Task<bool> ReportErrorAsync(string cabinetId, int id, DateTime timeStamp)
    {
        try
        {
            const string query = "UPDATE Cabinets_Slot SET status_id = 3, last_update = @pTime WHERE cabinet_uuid = @pCabinetId AND id = @pId;";
            using var conn = new NpgsqlConnection(_connectionString);
            var transactionResult = await conn.ExecuteAsync(query, new
            {
                pId = id,
                pCabinetId = cabinetId,
                pTime = timeStamp
            });

            return transactionResult != 0;
        }
        catch (Exception e)
        {
            throw new Exception($"An error ocurred Creating the cabinet pod", e);
        }
    }
}
