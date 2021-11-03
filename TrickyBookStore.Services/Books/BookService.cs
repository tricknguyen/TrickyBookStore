using System;
using System.Collections.Generic;
using TrickyBookStore.Models;
using System.Linq;

namespace TrickyBookStore.Services.Books
{
    internal class BookService : IBookService
    {
        public IList<Book> GetBooks(params long[] ids)
        {
            List<Book> result = new List<Book>();
            foreach (long id in ids)
            {
                var book = Store.Books.Data.FirstOrDefault(b => b.Id == id);
                if (book != null)
                {
                    result.Add(book);
                }
            }
            return result;
        }

        public IList<Book> GetBooks(List<PurchaseTransaction> purchaseTransactions)
        {
            List<Book> result = new List<Book>();
            foreach (var item in purchaseTransactions)
            {
                var book = Store.Books.Data.FirstOrDefault(b => b.Id == item.BookId);
                if (book != null)
                {
                    result.Add(book);
                }
            }
            return result;
        }
    }
}
