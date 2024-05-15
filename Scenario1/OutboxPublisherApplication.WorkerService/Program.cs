
using MassTransit;
using OutboxPublisherApplication.WorkerService.BackgroundServices;

var builder = Host.CreateApplicationBuilder(args);


builder.Services.AddHostedService<MessageSenderBackgroundService>(provider =>
{
    IServiceScope scope = provider.CreateScope();
    ISendEndpointProvider endpointProvider = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();


    var scope2 = provider.CreateScope();
    var logger = scope2.ServiceProvider.GetRequiredService<ILogger<MessageSenderBackgroundService>>();


    return new MessageSenderBackgroundService(
        sendEnpointProvider: endpointProvider, logger);


});



builder.Services.AddMassTransit(configurator =>
{

    configurator.UsingRabbitMq((_context, _configurator) =>
    {
        _configurator.Host(builder.Configuration.GetConnectionString("RabbitMQ"));


    });


});


var host = builder.Build();
host.Run();
