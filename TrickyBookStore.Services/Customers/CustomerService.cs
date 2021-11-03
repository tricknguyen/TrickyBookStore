using System;
using TrickyBookStore.Models;
using TrickyBookStore.Services.Subscriptions;
using System.Linq;

namespace TrickyBookStore.Services.Customers
{
    internal class CustomerService : ICustomerService
    {
        ISubscriptionService SubscriptionService { get; }

        public CustomerService(ISubscriptionService subscriptionService)
        {
            SubscriptionService = subscriptionService;
        }

        public Customer GetCustomerById(long id)
        {
            Customer customer = Store.Customers.Data.FirstOrDefault(c => c.Id == id);
            if (customer != null)
            {
                return customer;
            }
            else return null;
        }
    }
}
