using System;
using System.Collections.Generic;
using TrickyBookStore.Models;
using System.Linq;
namespace TrickyBookStore.Services.Subscriptions
{
    internal class SubscriptionService : ISubscriptionService
    {
        public IList<Subscription> GetSubscriptions(params int[] ids)
        {
            IList<Subscription> result = new List<Subscription>();
            foreach (int i in ids)
            {
                var item = Store.Subscriptions.Data.FirstOrDefault(s => s.Id == i);
                if(item != null)
                {
                    result.Add(item);
                }
            }            
            return result.OrderByDescending(c => c.Priority).ToList();
        }

        public double GetTotalSubcriptionPrice(List<Subscription> subscriptions)
        {
            double total = 0;
            foreach (var item in subscriptions)
            {
                total += item.PriceDetails["FixPrice"];
            }
            return total;
        }
    }
}
