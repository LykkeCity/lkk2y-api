using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Lkk2Y_Api.Core;

namespace Lykke.Service.Lkk2Y_Api.Services
{
    public static class DoubleCheckers
    {
        
        
        private static readonly Dictionary<string, List<ILkk2YOrder>> Orders = new Dictionary<string, List<ILkk2YOrder>>();


        private static bool CheckAndAdd(ILkk2YOrder newOrder)
        {
            var email = newOrder.Email.ToLower();

            if (!Orders.ContainsKey(email))
            {
                Orders.Add(email, new List<ILkk2YOrder>{newOrder});
                return false;
            }

            var orders = Orders[email];


            if (orders.Any(order => newOrder.Amount == order.Amount && newOrder.Currency == order.Currency))
                return true;
            
            orders.Add(newOrder);
            return false;

        }
        
        public static bool HasDouble(ILkk2YOrder order)
        {

            lock (Orders)
            {
                return CheckAndAdd(order);
            }
            
        } 
        
    }
}
