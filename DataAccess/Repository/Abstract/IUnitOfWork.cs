namespace DataAccess.Repository.Abstract;
public interface IUnitOfWork
{
    ICabinetRepository Cabinets { get; }
    ISlotRepository Slots { get; }
    ILogRepository Logs { get; }
    ISessionRepository Sessions { get; }
}
