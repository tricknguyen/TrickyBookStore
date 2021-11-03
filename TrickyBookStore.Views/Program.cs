using Microsoft.Extensions.DependencyInjection;
using System;
using TrickyBookStore.Services.Payment;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Hosting;
using TrickyBookStore.Services;

namespace TrickyBookStore.Views
{
    class Program
    {
        private static IPaymentService services;
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddService()
                .BuildServiceProvider();

            

            Console.WriteLine("Input customer id: ");
            int id = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Input year: ");
            int year = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Input month: ");
            int month = Int32.Parse(Console.ReadLine());

            DateTimeOffset startDate = new DateTimeOffset(new DateTime(year, month, 1));
            DateTimeOffset endDate = new DateTimeOffset(new DateTime(year, month, 28));

            var paymentService = serviceProvider.GetService<IPaymentService>();
            double paymentAmount = paymentService.GetPaymentAmount(id, startDate, endDate);
            Console.WriteLine("Payment Amount: "+paymentAmount);
        }
       
    }
}
