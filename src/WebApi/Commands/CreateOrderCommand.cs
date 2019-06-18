using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;
using Domain;
using Newtonsoft.Json;

namespace WebApi.Commands
{
    public class CreateOrderCommand : ICreateOrderCommand
    {
        public async Task AddOrder(Order order)
        {
            var kafka = Environment.GetEnvironmentVariable("KAFKA_BOOTSTRAP-SERVERS");
            var config = new Dictionary<string, object>
            {
                { "bootstrap.servers", "10.5.0.2:55322" }
            };

            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
            {
                var orderData = JsonConvert.SerializeObject(order);

                await producer.ProduceAsync("orders", null, orderData);
                producer.Flush(100);
            }
        }
    }
}
