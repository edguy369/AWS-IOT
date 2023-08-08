using Core.Models;
using Dapper;
using DataAccess.Config;
using DataAccess.Repository.Abstract;
using Npgsql;

namespace DataAccess.Repository
{
    internal class SessionRepository : ISessionRepository
    {
        private readonly string _connectionString;
        public SessionRepository(ConnectionString connectionString)
        {
            _connectionString = connectionString.Value;
        }

        public async Task<bool> EndAsync(string userId, string cabinet, int slot)
        {
            try
            {
                const string query = "DELETE FROM Cabinets_Slot_Session WHERE user_uuid = @pUser AND cabinet_uuid = @pCabinet AND slot_id = @pSlot;";
                using var conn = new NpgsqlConnection(_connectionString);
                var transactionResult = await conn.ExecuteAsync(query, new
                {
                    pCabinet = cabinet,
                    pUser = userId,
                    pSlot = slot,

                });

                return transactionResult == 1;
            }
            catch (Exception e)
            {
                throw new Exception($"An error ocurred Creating the session", e);
            }
        }

        public async Task<IEnumerable<SessionModel>> GetAllAsync(string userId)
        {
            try
            {
                const string query = "SELECT * FROM Cabinets_Slot_Session WHERE user_uuid = @pUser;";
                using var conn = new NpgsqlConnection(_connectionString);
                var transactionResult = await conn.QueryAsync<SessionModel>(query, new
                {
                    pUser = userId,

                });

                return transactionResult;
            }
            catch (Exception e)
            {
                throw new Exception($"An error ocurred Getting the sessions for the user", e);
            }
        }

        public async Task<DateTime> GetStartSessionAsync(string userId, string cabinet, int slot)
        {
            try
            {
                const string query = "SELECT start_time FROM Cabinets_Slot_Session WHERE user_uuid = @pUser AND cabinet_uuid = @pCabinet AND slot_id = @pSlot;";
                using var conn = new NpgsqlConnection(_connectionString);
                var transactionResult = await conn.QueryFirstAsync<DateTime>(query, new
                {
                    pCabinet = cabinet,
                    pUser = userId,
                    pSlot = slot,

                });

                return transactionResult;
            }
            catch (Exception e)
            {
                throw new Exception($"An error ocurred Getting the sessions for the user", e);
            }
        }

        public async Task<bool> StartAsync(string userId, string cabinet, int slot)
        {
            try
            {
                const string query = "INSERT INTO Cabinets_Slot_Session ( cabinet_uuid, user_uuid, slot_id) VALUES (@pCabinet, @pUser, @pSlot);";
                using var conn = new NpgsqlConnection(_connectionString);
                var transactionResult = await conn.ExecuteAsync(query, new
                {
                    pCabinet = cabinet,
                    pUser = userId,
                    pSlot = slot,
   
                });

                return transactionResult == 1;
            }
            catch (Exception e)
            {
                throw new Exception($"An error ocurred Creating the session", e);
            }
        }
    }
}
