using System.Reflection;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Notification;
using Restaurant.Notification.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<NotifyConsumer>().Endpoint(e => e.Temporary = true);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqps://vavewpmm:g2_Mb_Y4Sj7lhMP9W01CdtM7a70K48xn@beaver.rmq.cloudamqp.com/vavewpmm");
        cfg.UseMessageRetry(r =>
        {
            r.Exponential(5,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(100),
                TimeSpan.FromSeconds(5));
            r.Ignore<StackOverflowException>();
            r.Ignore<ArgumentNullException>(x => x.Message.Contains("Consumer"));
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddSingleton<Notifier>();

var app = builder.Build();

app.UseSwagger().UseSwaggerUI();

app.Run();