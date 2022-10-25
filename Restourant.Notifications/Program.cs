using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using MassTransit;
using Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Restaurant.Notification;
using Restourant.Notifications;

public class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, service) =>
        {
            service.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetExecutingAssembly());
                x.AddConsumer<NotifierTableBookedConsumer>();
                //x.AddConsumer<KitchenReadyConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("amqps://vavewpmm:g2_Mb_Y4Sj7lhMP9W01CdtM7a70K48xn@beaver.rmq.cloudamqp.com/vavewpmm", h =>
                    {
                        //h.Username("vavewpmm");
                        //h.Password("g2_Mb_Y4Sj7lhMP9W01CdtM7a70K48xn");
                    });
                    cfg.UseMessageRetry(r =>
                    {
                        r.Exponential(5,
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(100),
                            TimeSpan.FromSeconds(5));
                        r.Ignore<StackOverflowException>();
                        r.Ignore<ArgumentNullException>(x => x.Message.Contains("Consumer"));
                    });
                });
                service.AddSingleton<Notifier>();
                //service.AddMassTransitHostedService(true);
            });
        });
}