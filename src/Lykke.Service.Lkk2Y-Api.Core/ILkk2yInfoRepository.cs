using System.Threading.Tasks;

namespace Lykke.Service.Lkk2Y_Api.Core
{
    public interface ILkk2yInfo
    {

        string FundsRecieved { get; }
        
        string FundsGoal { get; }

    }


    public interface ILkk2yInfoRepository
    {
        Task<ILkk2yInfo> GetInfoAsync();
    }
}