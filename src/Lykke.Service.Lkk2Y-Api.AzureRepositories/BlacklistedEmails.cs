using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Lkk2Y_Api.Core;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Lkk2Y_Api.AzureRepositories
{
    public class BlacklistedEmailEntity : TableEntity
    {
        public static string GeneratePartitionKey()
        {
            return "b";
        }

        public static string GenerateRowKey(string email)
        {
            return email.ToLower();
        }


        public static BlacklistedEmailEntity Create(string email)
        {
            return new BlacklistedEmailEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(email)
            };
        }
        
    }
    
    public class BlacklistedEmailsRepository : IBlacklistedEmailsRepository
    {
        private readonly INoSQLTableStorage<BlacklistedEmailEntity> _tableStorage;

        public BlacklistedEmailsRepository(INoSQLTableStorage<BlacklistedEmailEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task AddAsync(string email)
        {
            var newEntity = BlacklistedEmailEntity.Create(email);
            await _tableStorage.InsertOrReplaceAsync(newEntity);
        }

        public async Task<bool> IsBlacklistedAsync(string email)
        {
            var partitionKey = BlacklistedEmailEntity.GeneratePartitionKey();
            var rowKey = BlacklistedEmailEntity.GenerateRowKey(email);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey) != null;
        }
    }
}
