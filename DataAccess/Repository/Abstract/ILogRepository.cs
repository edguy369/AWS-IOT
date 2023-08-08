using Core.Models;

namespace DataAccess.Repository.Abstract;

public interface ILogRepository
{
    Task<IEnumerable<LogModel>> GetAllAsync();
    Task<bool> AddAsync(string cabinetId, string userId, int slot, string orderNumber);
    Task<bool> CloseAsync(string orderNumber, DateTime timestamp);
    Task<string> GetActiveAsync(string cabinetId, string userId, int slot);
}
