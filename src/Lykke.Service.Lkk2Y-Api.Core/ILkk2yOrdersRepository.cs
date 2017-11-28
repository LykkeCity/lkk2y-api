using System;
using System.Threading.Tasks;

namespace Lykke.Service.Lkk2Y_Api.Core
{

    public interface ILkk2yOrder
    {
        double Amount { get; }

        string Country { get; }

        string Currency { get; }

        string Email { get; }

        string FirstName { get; }

        string LastName { get; }
    }

    public interface ILkk2yOrdersRepository
    {
        Task RegisterAsync(DateTime dateTime, ILkk2yOrder order);
    }
    
}