using MassTransit;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging
{
    public class Producer
    {
        private readonly string _queueName;
        private readonly string _hostName;

        public Producer(string queueName , string hostname)
        {
            _queueName = queueName;
            _hostName = "beaver.rmq.cloudamqp.com";
;
        }

        public void Send(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Port = 5672,
                UserName = "vavewpmm",
                Password = "g2_Mb_Y4Sj7lhMP9W01CdtM7a70K48xn",
                VirtualHost = "vavewpmm"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                "direct_exchange",
                "direct",
                false,
                false,
                null
            );

            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "direct_exchange",
                routingKey: _queueName,
                basicProperties: null,
                body: body);
        }

        
    }
}
