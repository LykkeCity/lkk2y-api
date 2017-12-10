using System;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Lkk2Y_Api.Core;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Lkk2Y_Api.AzureRepositories
{
    
    public class Lkk2YOrderEntity : TableEntity, ILkk2YOrder
    {
        internal static string GeneratePartitionKey()
        {
            return "o";
        }
        
        internal static string GenerateIgnoredPartitionKey()
        {
            return "i";
        }


        public double Amount { get; set; }

        public string Country { get; set; }

        public string Currency { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double UsdAmount { get; set; }
        
        public string Ip { get; set; }

        public static Lkk2YOrderEntity Create(ILkk2YOrder src)
        {

            return new Lkk2YOrderEntity
            {
                PartitionKey = GeneratePartitionKey(),
                Amount = src.Amount,
                Country = src.Country,
                Currency = src.Currency,
                Email = src.Email,
                FirstName = src.FirstName,
                LastName = src.LastName,
                UsdAmount = src.UsdAmount,
                Ip = src.Ip
            };

        }


        public static Lkk2YOrderEntity CreateIgnored(ILkk2YOrder src)
        {
            var result = Create(src);
            result.PartitionKey = GenerateIgnoredPartitionKey();
            return result;
        }

    }

    public class Lkk2YTotalEntity : TableEntity
    {

        public static string GeneratePartitionKey()
        {
            return "t";
        }

        public static string GenerateRowKey()
        {
            return "t";
        }


        public double Total { get; set; }


        public static Lkk2YTotalEntity Create()
        {
            return new Lkk2YTotalEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey()
            };
        }

    }

    public class Lkk2YOrderRepository : ILkk2YOrdersRepository
    {
        private readonly INoSQLTableStorage<Lkk2YOrderEntity> _tableStorage;

        readonly INoSQLTableStorage<Lkk2YTotalEntity> _totalTableStorage;

        public Lkk2YOrderRepository(INoSQLTableStorage<Lkk2YOrderEntity> tableStorage, 
                                    INoSQLTableStorage<Lkk2YTotalEntity> totalTableStorage)
        {
            _totalTableStorage = totalTableStorage;
            _tableStorage = tableStorage;

            InitTotal().Wait();
        }


        private async Task InitTotal()
        {


            try
            {
                var newEntity = Lkk2YTotalEntity.Create();
                await _totalTableStorage.InsertAsync(newEntity);
            }
            catch (Exception)
            {
                Console.WriteLine("Total entity already exists");
            }

        }

        private async Task UpdateTotalAsync(double newTotal)
        {

            var partitionKey = Lkk2YTotalEntity.GeneratePartitionKey();
            var rowKey = Lkk2YTotalEntity.GenerateRowKey();

            await _totalTableStorage.ReplaceAsync(partitionKey, rowKey, entity =>
            {
                entity.Total = newTotal;

                if (entity.Total > Lkk2YConstants.MaxIcoSize)
                    entity.Total = Lkk2YConstants.MaxIcoSize;
                
                return entity;
            });

        }

        public async Task RegisterAsync(DateTime dateTime, ILkk2YOrder order)
        {
            var newEntity = Lkk2YOrderEntity.Create(order);
            await _tableStorage.InsertAndGenerateRowKeyAsDateTimeAsync(newEntity, dateTime);
        }

        public async Task RegisterIgnoredAsync(DateTime dateTime, ILkk2YOrder order)
        {
            var newEntity = Lkk2YOrderEntity.CreateIgnored(order);
            await _tableStorage.InsertAndGenerateRowKeyAsDateTimeAsync(newEntity, dateTime);
        }

        public async Task<double> GetUsdTotalAsync()
        {
            var partitionKey = Lkk2YTotalEntity.GeneratePartitionKey();
            var rowKey = Lkk2YTotalEntity.GenerateRowKey();

            var entity = await _totalTableStorage.GetDataAsync(partitionKey, rowKey);

            return entity?.Total ?? 0;
        }

        public async Task<double> CalcTotalAsync()
        {
            var partitionKey = Lkk2YOrderEntity.GeneratePartitionKey();

            var total = 0.0;

            await _tableStorage.GetDataByChunksAsync(partitionKey,
                chunk => total += chunk.Sum(entity => entity.UsdAmount));


            return total;


        }

        public async Task<double> UpdateTotalAsync()
        {

            var total = await CalcTotalAsync();

            await UpdateTotalAsync(total);

            return total;

        }
        
    }
    
}
