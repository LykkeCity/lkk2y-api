using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.Lkk2Y_Api.AzureRepositories;
using Lykke.Service.Lkk2Y_Api.Core;
using Lykke.Service.Lkk2Y_Api.Core.Services;
using Lykke.Service.Lkk2Y_Api.Services;
using Lykke.Service.Lkk2Y_Api.Settings;
using Lykke.SettingsReader;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.Lkk2Y_Api.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<Lkk2Y_ApiSettings> _settings;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public ServiceModule(IReloadingManager<Lkk2Y_ApiSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // TODO: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            //  builder.RegisterType<QuotesPublisher>()
            //      .As<IQuotesPublisher>()
            //      .WithParameter(TypedParameter.From(_settings.CurrentValue.QuotesPublication))

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.RegisterType<Lkk2yToChf>()
                .As<ILkk2yToChf>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.RegisterType<RateConverterService>()
                .As<RateConverterService>();

            // TODO: Add your dependencies here

            builder.Populate(_services);

            BindRepositories(builder);

            builder.RegisterInstance<IRateConverterClient>(new RateConverterClient(_settings.CurrentValue.RateConverterUrl));

        }

        private void BindRepositories(ContainerBuilder builder)
        {

             builder.RegisterInstance<ILkk2YOrdersRepository>(
                new Lkk2YOrderRepository(
                    AzureTableStorage<Lkk2YOrderEntity>.Create(_settings.ConnectionString(x => x.Db.Lkk2yConnString), "Orders", _log),
                    AzureTableStorage<Lkk2YTotalEntity>.Create(_settings.ConnectionString(x => x.Db.Lkk2yConnString), "OrdersTotal", _log)
                    ));


             builder.RegisterInstance<ILkk2yInfoRepository>(
                new Lkk2yInfoRepository(
                    AzureTableStorage<Lkk2yInfoEntity>.Create(_settings.ConnectionString(x => x.Db.Lkk2yConnString), "Info", _log)));

        }

    }
}
