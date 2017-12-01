using System;
using System.Threading.Tasks;

namespace Lykke.Service.Lkk2Y_Api.Core
{

    public interface ILkk2YOrder
    {
        double Amount { get; }

        string Country { get; }

        string Currency { get; }

        string Email { get; }

        string FirstName { get; }

        string LastName { get; }

        double UsdAmount { get; }
        
        string Ip { get; }
    }

    public interface ILkk2YOrdersRepository
    {
        Task RegisterAsync(DateTime dateTime, ILkk2YOrder order);

        Task<double> GetUsdTotalAsync();
    }
    
}
