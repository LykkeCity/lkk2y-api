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
        
    }
    
    public class BlacklistedEmails : IBlacklistedEmails
    {
        private readonly INoSQLTableStorage<BlacklistedEmailEntity> _tableStorage;

        public BlacklistedEmails(INoSQLTableStorage<BlacklistedEmailEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public async Task<bool> IsBlacklistedAsync(string email)
        {
            var partitionKey = BlacklistedEmailEntity.GeneratePartitionKey();
            var rowKey = BlacklistedEmailEntity.GenerateRowKey(email);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey) != null;
        }
    }
}
