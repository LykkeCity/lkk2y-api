using System;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Lkk2Y_Api.Core;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Lkk2Y_Api.AzureRepositories
{
    public class Lkk2yOrderEntity : TableEntity, ILkk2yOrder
    {

        public static string GeneratePartitionKey()
        {
            return "o";
        }



        public double Amount { get; set; }

        public string Country { get; set; }

        public string Currency { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public static Lkk2yOrderEntity Create(ILkk2yOrder src)
        {

            return new Lkk2yOrderEntity
            {
                PartitionKey = GeneratePartitionKey(),
                Amount = src.Amount,
                Country = src.Country,
                Currency = src.Currency,
                Email = src.Email,
                FirstName = src.FirstName,
                LastName = src.LastName
            };

        }

    }

    public class Lkk2yOrderRepository : ILkk2yOrdersRepository
    {
        public INoSQLTableStorage<Lkk2yOrderEntity> _tableStorage { get; }

        public Lkk2yOrderRepository(INoSQLTableStorage<Lkk2yOrderEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task RegisterAsync(DateTime dateTime, ILkk2yOrder order)
        {
            var newEntity = Lkk2yOrderEntity.Create(order);
            await _tableStorage.InsertAndGenerateRowKeyAsDateTimeAsync(newEntity, dateTime);
        }
    }
}