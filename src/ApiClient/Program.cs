using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace ApiClient
{
    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("Connect to http://localhost:5585/");
            HttpClient client = new HttpClient {BaseAddress = new Uri("http://localhost:5585/api/") };
            //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            for (int i = 0; i < 100; i++)
            {
                var order = new { Number = new Random().Next(Int32.MinValue, Int32.MaxValue), StatusValue = 0, Id = 0 };

                var a = client.PostAsync("orders", new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json")).Result;

                Console.WriteLine($"Posted order with name {order.Number}");
            }

            Console.WriteLine("Processing finished.");
            Console.ReadKey();
        }
    }
}
