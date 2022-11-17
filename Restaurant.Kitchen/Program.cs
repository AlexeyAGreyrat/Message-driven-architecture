using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Kitchen;
using Restaurant.Kitchen.Consumers;
using Restaurant.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer().AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<BookingCancellationConsumer>(cfg =>
    {
        cfg.UseMessageRetry(r => r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)));
        cfg.UseScheduledRedelivery(r => r.Incremental(3, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10)));
    })
        .Endpoint(e => e.Temporary = true);
    x.AddConsumer<PreorderDishConsumer>(cfg =>
    {
        cfg.UseMessageRetry(r => r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)));
        cfg.UseScheduledRedelivery(r => r.Incremental(3, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10)));
    })
        .Endpoint(e => e.Temporary = true);

    x.AddDelayedMessageScheduler();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("amqps://vavewpmm:g2_Mb_Y4Sj7lhMP9W01CdtM7a70K48xn@beaver.rmq.cloudamqp.com/vavewpmm");
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddSingleton<Chef>();

var app = builder.Build();

app.UseSwagger().UseSwaggerUI();

app.MapPost("order", (Dish dish) => Results.Problem("Not released feature"));

app.Run();