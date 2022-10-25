using MassTransit;
using Messaging;
using Microsoft.Extensions.Hosting;
using System.Text;

namespace Restourant.Notifications
{
    public class Worker : BackgroundService
    {
        private readonly Consumer _consumer;
        private readonly IBus _bus;
        private readonly Hall _hall;
        public Dish? dish;
        public Worker(IBus bus,Hall hall)
        {
            _bus = bus; 
            _hall = hall;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000,stoppingToken);
                Console.WriteLine("Привет! Желаете забронировать столик?");
                await _bus.Publish(new TableBooked(NewId.NextGuid(), NewId.NextSequentialGuid(), true, dish));
            }            
        }
    }
}
