namespace DataAccess.Repository.Abstract;
public interface ISlotRepository
{
    Task<bool> CreateAsync(string cabinetId, int id);
    Task<bool> CheckAvailabilityAsync(string cabinetId, int id);
    Task<bool> OpenAsync(string cabinetId, int id, DateTime timeStamp);
}
