using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace KafkaListener
{
    class Program
    {
        static void Main(string[] args)
        {
            var kafka = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP-SERVERS");
            var config = new Dictionary<string, object>
            {
                { "group.id", "sample-consumer" },
                { "bootstrap.servers", "172.24.0.5:46912" },
                { "enable.auto.commit", "false"}
            };

            using (var consumer = new Consumer<Null, string>(config, null, new StringDeserializer(Encoding.UTF8)))
            {
                consumer.Subscribe(new string[] { "orders" });

                consumer.OnMessage += (_, msg) =>
                {
                    ProcessNewOrder(msg);
                    consumer.CommitAsync(msg);
                };

                while (true)
                {
                    consumer.Poll(100);
                }
            }
        }

        private static void ProcessNewOrder(Message<Null, string> msg)
        {
            Console.WriteLine($"Topic: {msg.Topic} Partition: {msg.Partition} Offset: {msg.Offset} {msg.Value}");

            var order = JsonConvert.DeserializeObject<Order>(msg.Value);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var serviceCollection = new ServiceCollection();
            var contextOptions = new DbContextOptionsBuilder<ShopContext>()
                .UseSqlServer(
                    configuration["DbConnection"]).Options;
            serviceCollection.AddSingleton(contextOptions);
            serviceCollection.AddDbContext<ShopContext>();
            serviceCollection.AddDbContextPool<ShopContext>(options => options.UseLazyLoadingProxies());
            var provider = serviceCollection.BuildServiceProvider();

            var context = provider.GetService<ShopContext>();

            context.Orders.Add(order);
            context.SaveChanges();
        }
    }
}
