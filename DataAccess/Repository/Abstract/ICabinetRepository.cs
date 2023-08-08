using Core.Models;

namespace DataAccess.Repository.Abstract;
public interface ICabinetRepository
{
    Task<bool> CreateAsync(string deviceId, string macAddress, string name, string gps);
    Task<IEnumerable<CabinetModel>> GetAllAsync();
    Task<CabinetModel> GetByIdAsync(string id);
    Task<int> CountSlotsAsync(string id);
    Task<int> CheckLastSlotOpenedAsync(string id);
}
