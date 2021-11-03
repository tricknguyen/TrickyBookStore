using System;
using System.Collections.Generic;
using System.Linq;
using TrickyBookStore.Models;
using TrickyBookStore.Services.Books;
using TrickyBookStore.Services.Customers;
using TrickyBookStore.Services.PurchaseTransactions;
using TrickyBookStore.Services.Subscriptions;

namespace TrickyBookStore.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        ICustomerService CustomerService { get; }
        IPurchaseTransactionService PurchaseTransactionService { get; }
        ISubscriptionService SubscriptionService { get; }
        IBookService BookService { get; }

        public PaymentService(ICustomerService customerService, 
            IPurchaseTransactionService purchaseTransactionService, ISubscriptionService subscriptionService,
            IBookService bookService)
        {
            CustomerService = customerService;
            PurchaseTransactionService = purchaseTransactionService;
            SubscriptionService = subscriptionService;
            BookService = bookService;
        }

        public double GetPaymentAmount(long customerId, DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            double total = 0, priceSubcription = 0, priceReceipt = 0;
            Customer customer = CustomerService.GetCustomerById(customerId);

            IList<PurchaseTransaction> listReceipt = PurchaseTransactionService.GetPurchaseTransactions(customerId, fromDate, toDate);

            IList<Subscription> listSubcription = SubscriptionService.GetSubscriptions(customer.SubscriptionIds.ToArray());

            IList<Book> listBook = BookService.GetBooks(listReceipt.ToList());

            priceSubcription = SubscriptionService.GetTotalSubcriptionPrice(listSubcription.ToList());

            priceReceipt = PurchaseTransactionService.GetTotalReceiptPrice(listBook.ToList(), listSubcription.ToList());

            total = priceSubcription + priceReceipt;
            return total;
        }
    }
}
