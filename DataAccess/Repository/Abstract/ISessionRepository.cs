
using Core.Models;

namespace DataAccess.Repository.Abstract;

public interface ISessionRepository
{
    Task<bool> StartAsync(string userId, string cabinet, int slot);
    Task<bool> EndAsync(string userId, string cabinet, int slot);
    Task<DateTime> GetStartSessionAsync(string userId, string cabinet, int slot);
    Task<IEnumerable<SessionModel>> GetAllAsync(string userId);
}
