using MassTransit;
using Microsoft.EntityFrameworkCore;
using ServiceB.Consumers;
using ServiceB.Contexts;
using Shared.Constants;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ServiceBDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddMassTransit(configurator =>
{

    configurator.AddConsumer<MessageSendedEventConsumer>();

    configurator.UsingRabbitMq((_context, _configurator) =>
    {


        _configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

        _configurator.ReceiveEndpoint(Queues.MessageQueue, e => e.ConfigureConsumer<MessageSendedEventConsumer>(_context));


    });


});

var app = builder.Build();



app.Run();
