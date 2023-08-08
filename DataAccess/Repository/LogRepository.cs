using Core.Models;
using Dapper;
using DataAccess.Config;
using DataAccess.Repository.Abstract;
using Npgsql;

namespace DataAccess.Repository;


internal class LogRepository : ILogRepository
{
    private readonly string _connectionString;
    public LogRepository(ConnectionString connectionString)
    {
        _connectionString = connectionString.Value;
    }

    public async Task<bool> AddAsync(string cabinetId, string userId, int slot, string orderNumber)
    {
        try
        {
            const string query = "INSERT INTO Cabinets_Log ( cabinet_uuid, user_uuid, slot_id, order_num) VALUES (@pCabinet, @pUser, @pSlot, @pOrder);";
            using var conn = new NpgsqlConnection(_connectionString);
            var transactionResult = await conn.ExecuteAsync(query, new
            {
                pCabinet = cabinetId,
                pUser = userId,
                pSlot = slot,
                pOrder = orderNumber
            });

            return transactionResult == 1;
        }
        catch (Exception e)
        {
            throw new Exception($"An error ocurred Creating the log entry", e);
        }
    }

    public async Task<IEnumerable<LogModel>> GetAllAsync()
    {
        try
        {
            const string query = "SELECT * FROM Cabinets_Log;";
            using var conn = new NpgsqlConnection(_connectionString);
            var transactionResult = await conn.QueryAsync<LogModel>(query);

            return transactionResult;
        }
        catch (Exception e)
        {
            throw new Exception($"An error ocurred getting all the logs", e);
        }
    }
}
