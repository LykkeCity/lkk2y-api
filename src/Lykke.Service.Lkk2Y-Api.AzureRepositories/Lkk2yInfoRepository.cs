using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Lkk2Y_Api.Core;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Lkk2Y_Api.AzureRepositories
{

    public class Lkk2yInfoEntity : TableEntity, ILkk2yInfo
    {
        public static string GeneratePartitionKey()
        {
            return "Info";
        }

        public static string GenerateRowKey()
        {
            return "Info";
        }        

        public string FundsRecieved { get; set; }

        public string FundsGoal { get; set; }


        public static Lkk2yInfoEntity Create()
        {
            return new Lkk2yInfoEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(),
                FundsRecieved = "$12K",
                FundsGoal = "$100K"
            };
        }

    }

    public class Lkk2yInfoRepository : ILkk2yInfoRepository
    {
        private readonly INoSQLTableStorage<Lkk2yInfoEntity> _tableStorage;

        private Lkk2yInfoEntity _cache;
        public Lkk2yInfoRepository(INoSQLTableStorage<Lkk2yInfoEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<ILkk2yInfo> GetInfoAsync()
        {

            try
            {

                var partitionKey = Lkk2yInfoEntity.GeneratePartitionKey();
                var rowKey = Lkk2yInfoEntity.GenerateRowKey();

                _cache = await _tableStorage.GetDataAsync(partitionKey, rowKey);

                if (_cache == null){
                    _cache = Lkk2yInfoEntity.Create();
                    await _tableStorage.InsertOrReplaceAsync(_cache);
                }

                return _cache;

            }
            catch
            {
                return _cache;
            }

        }
        

    }
}
