using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace ApiClient
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Connect to http://localhost:50987");
            HttpClient client = new HttpClient {BaseAddress = new Uri("http://localhost:50987/api")};

            for (int i = 0; i < 100; i++)
            {
                var order = new { Name = $"Order{new Random().Next(Int32.MinValue, Int32.MaxValue)}" };

                client.PostAsync("orders", new StringContent(JsonConvert.SerializeObject(order)));

                Console.WriteLine($"Posted order with name {order.Name}");
            }

            Console.WriteLine("Processing finished.");
            Console.ReadKey();
        }
    }
}
