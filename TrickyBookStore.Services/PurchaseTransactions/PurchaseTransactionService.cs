using System;
using System.Collections.Generic;
using TrickyBookStore.Models;
using TrickyBookStore.Services.Books;
using System.Linq;

namespace TrickyBookStore.Services.PurchaseTransactions
{
    internal class PurchaseTransactionService : IPurchaseTransactionService
    {
        IBookService BookService { get; }

        public PurchaseTransactionService(IBookService bookService)
        {
            BookService = bookService;
        }

        public IList<PurchaseTransaction> GetPurchaseTransactions(long customerId, DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            IList<PurchaseTransaction> result = new List<PurchaseTransaction>();

            result = Store.PurchaseTransactions.Data.Where(r => r.CustomerId == customerId && r.CreatedDate >= fromDate && r.CreatedDate <= toDate).ToList();

            if (result == null)
                return null;
           
            return result;

        }

        public double GetTotalReceiptPrice(List<Book> books, List<Subscription> subscriptions)
        {
            double total = 0, priceOldBook = 0, priceNewBook = 0;
            if (subscriptions == null)
            {
                foreach (var book in books)
                {
                    total += book.Price;
                }
                return total;
            }
            List<Book> oldBook = new List<Book>();
            List<Book> newBook = new List<Book>();

            foreach (var book in books)
            {
                if (book.IsOld) oldBook.Add(book);
                else
                    newBook.Add(book);
            }
            priceOldBook = GetPriceOldBook(oldBook, subscriptions);
            priceNewBook = GetPriceNewBook(newBook, subscriptions);

            total = priceOldBook + priceNewBook;

            return total;
        }

        public double GetPriceOldBook(List<Book> books, List<Subscription> subscriptions)
        {
            double total = 0;
            bool isCalculated;
            foreach (var book in books)
            {
                isCalculated = false;
                foreach (var subcription in subscriptions)
                {                                           
                    var subcriptionAddicted = subscriptions.FirstOrDefault(s => s.BookCategoryId == book.CategoryId);
           
                    if (subcriptionAddicted != null)
                    {
                        total += 0;
                        isCalculated = true;
                    }

                    if (subcription.BookCategoryId == null)
                    {
                        total += book.Price - ((book.Price * subcription.PriceDetails["OldBookDiscount"]) / 100);
                        isCalculated = true;
                    }               
                                     
                    if (isCalculated)
                    {
                        break;
                    }                  
                }
                if(!isCalculated)
                {
                    total += book.Price;
                }
            }
            return total;
        }

        public double GetPriceNewBook(List<Book> books, List<Subscription> subscriptions)
        {
            double total=0;
            bool isCalculated;          
            foreach (var book in books)
            {
                isCalculated = false;
                foreach (var subcription in subscriptions)
                {
                    if (subcription.BookCategoryId == book.CategoryId)
                    {   
                        if(subcription.PriceDetails["DiscountedBooks"] > 0)
                        {
                            total += book.Price - ((book.Price * 15) / 100);
                            subcription.PriceDetails["DiscountedBooks"]--;
                            isCalculated = true;
                        }                      
                    }

                    if (subcription.BookCategoryId == null)
                    {
                        if (subcription.PriceDetails["DiscountedBooks"] > 0)
                        {
                            total += book.Price - ((book.Price * subcription.PriceDetails["NewBookDiscount"]) / 100);
                            subcription.PriceDetails["DiscountedBooks"]--;
                            isCalculated = true;
                        }                        
                    }
                   
                    if (isCalculated)
                    {
                        break;
                    }
                }
                if (!isCalculated)
                {
                    total += book.Price;
                }
            }
           return total;
        }

    }
}
