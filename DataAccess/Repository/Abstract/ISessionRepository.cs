
using Core.Models;

namespace DataAccess.Repository.Abstract;

public interface ISessionRepository
{
    Task<bool> StartAsync(string userId, string cabinet, int slot);
    Task<IEnumerable<SessionModel>> GetAllAsync(string userId);
}
