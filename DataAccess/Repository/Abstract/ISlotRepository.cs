using Core.Models;

namespace DataAccess.Repository.Abstract;
public interface ISlotRepository
{
    Task<bool> CreateAsync(string cabinetId, int id);
    Task<bool> CheckAvailabilityAsync(string cabinetId, int id);
    Task<SlotModel> GetByIdAsync(string cabinetId, int id);
    Task<bool> OpenAsync(string cabinetId, int id, DateTime timeStamp);
    Task<bool> CloseAsync(string cabinetId, int id, DateTime timeStamp);
    Task<bool> ReportErrorAsync(string cabinetId, int id, DateTime timeStamp);
}
