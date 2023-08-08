using DataAccess.Repository.Abstract;

namespace DataAccess.Repository;
internal class UnitOfWork : IUnitOfWork
{
    public ICabinetRepository Cabinets { get; }
    public ISlotRepository Slots { get; }
    public ILogRepository Logs { get; }
    public ISessionRepository Sessions { get; }
    public UnitOfWork(ICabinetRepository cabinets, 
        ISlotRepository slots,
        ILogRepository logs,
        ISessionRepository sessions)
    {
        Cabinets = cabinets;
        Slots = slots;
        Logs = logs;
        Sessions = sessions;
    }
}
