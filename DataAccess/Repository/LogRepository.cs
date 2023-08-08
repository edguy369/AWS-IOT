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

    public async Task<bool> CloseAsync(string orderNumber, DateTime timestamp)
    {
        try
        {
            const string query = "UPDATE Cabinets_Log SET end_time = @pTime WHERE order_num = @pOrder;";
            using var conn = new NpgsqlConnection(_connectionString);
            var transactionResult = await conn.ExecuteAsync(query, new
            {
                pTime = timestamp,
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
            const string query = "SELECT * FROM Cabinets_Log ORDER BY id DESC;";
            using var conn = new NpgsqlConnection(_connectionString);
            var transactionResult = await conn.QueryAsync<LogModel>(query);

            return transactionResult;
        }
        catch (Exception e)
        {
            throw new Exception($"An error ocurred getting all the logs", e);
        }
    }

    public async Task<string> GetActiveAsync(string cabinetId, string userId, int slot)
    {
        try
        {
            const string query = "SELECT order_num FROM Cabinets_Log WHERE cabinet_uuid = @pCabinet AND user_uuid = @pUser AND slot_id = @pSlot;";
            using var conn = new NpgsqlConnection(_connectionString);
            var transactionResult = await conn.QueryFirstOrDefaultAsync<string>(query, new
            {
                pCabinet = cabinetId,
                pUser = userId,
                pSlot = slot
            });

            return transactionResult;
        }
        catch (Exception e)
        {
            throw new Exception($"An error ocurred getting all the logs", e);
        }
    }
}
