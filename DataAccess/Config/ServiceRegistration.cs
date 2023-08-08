using DataAccess.Repository;
using DataAccess.Repository.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Config
{
    public static class ServiceRegistration
    {
        public static void ConfigureDataAccessLayer(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = new ConnectionString(config["ConnectionString"]);
            services.AddSingleton(connectionString);

            services.AddTransient<ICabinetRepository, CabinetRepository>();
            services.AddTransient<ISlotRepository, SlotRepository>();
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<ISessionRepository, SessionRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        
    }
}
