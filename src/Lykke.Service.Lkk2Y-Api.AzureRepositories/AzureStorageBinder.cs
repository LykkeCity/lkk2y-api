using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.Lkk2Y_Api.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.Lkk2Y_Api.AzureRepositories
{
    public static class AzureStorageBinder
    {

        public static void BindAzureStorageRepositories(this IServiceCollection serviceCollection,
            SettingsReader.IReloadingManager<string> connectionStringManager, ILog log)
        {

           // var lkk2yOrderRepository = new AzureTableStorage<Lkk2yOrderEntity>.Create(connectionStringManager, "Orders", log);

            //serviceCollection.AddSingleton<ILkk2yOrdersRepository>(new Lkk2yOrderRepository(lkk2yOrderRepository));

            

        }

    }
}