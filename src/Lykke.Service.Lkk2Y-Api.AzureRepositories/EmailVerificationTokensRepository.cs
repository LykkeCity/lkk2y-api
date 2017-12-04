using System;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Lkk2Y_Api.Core;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Lkk2Y_Api.AzureRepositories
{


    public class EmailTokenEntity : TableEntity
    {

        public static string GeneratePartitionKey(string email)
        {
            return email.ToLower();
        }

        public static string GeneratePartitionKeyAsToken(string token)
        {
            return token.ToLower();
        }
       

        public static EmailTokenEntity Create(string email)
        {
            return new EmailTokenEntity
            {
                PartitionKey = GeneratePartitionKey(email),
                RowKey = Guid.NewGuid().ToString("N").ToLower()
            };
        }


        public static EmailTokenEntity GenerateTokenIndex(EmailTokenEntity src)
        {
            return new EmailTokenEntity
            {
                PartitionKey = src.RowKey,
                RowKey = src.PartitionKey
            };
        }
        
    }
    
    
    
    public class EmailVerificationTokensRepository : IEmailVerificationTokensRepository
    {
        private readonly INoSQLTableStorage<EmailTokenEntity> _tableStorage;

        public EmailVerificationTokensRepository(INoSQLTableStorage<EmailTokenEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }



        private async Task<EmailTokenEntity> GetTokenEntity(string email)
        {
            var partitionKey = EmailTokenEntity.GeneratePartitionKey(email);

            var entities = await _tableStorage.GetDataAsync(partitionKey);


            return entities.FirstOrDefault();
            
            
        }


        private async Task<EmailTokenEntity> GenerateNewToken(string email)
        {
            
            var newEntity = EmailTokenEntity.Create(email);

            await _tableStorage.InsertAsync(newEntity);

            var newIndex = EmailTokenEntity.GenerateTokenIndex(newEntity);

            await _tableStorage.InsertAsync(newIndex);

            return newEntity;

        }

        public async Task<string> GetTokenAsync(string email)
        {

            var entity = await GetTokenEntity(email) ?? await GenerateNewToken(email);

            return entity.RowKey;

        }

        public async Task<string> VerifyTokenAsync(string token)
        {
            var partitionKey = EmailTokenEntity.GeneratePartitionKeyAsToken(token);

            var entity = (await _tableStorage.GetDataAsync(partitionKey)).FirstOrDefault();

            return entity?.RowKey;
        }
        
    }
    
}
