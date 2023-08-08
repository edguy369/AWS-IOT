using Core.Models;

namespace DataAccess.Repository.Abstract;

public interface ILogRepository
{
    Task<IEnumerable<LogModel>> GetAllAsync();
    Task<bool> AddAsync(string cabinetId, string userId, int slot, string orderNumber);
}
