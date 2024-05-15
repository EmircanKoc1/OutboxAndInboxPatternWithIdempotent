using Dapper;
using MassTransit;
using OutboxPublisherApplication.WorkerService.Outboxes;
using ServiceB.Consumers;
using Shared.Constants;
using Shared.Extensions;
using System.Data;
using System.Data.SqlClient;

namespace OutboxPublisherApplication.WorkerService.BackgroundServices
{
    public class MessageSenderBackgroundService : BackgroundService
    {
        readonly ISendEndpointProvider _sendEnpointProvider;
        readonly ILogger<MessageSenderBackgroundService> _logger;
        public MessageSenderBackgroundService(ISendEndpointProvider sendEnpointProvider, ILogger<MessageSenderBackgroundService> logger)
        {
            _sendEnpointProvider = sendEnpointProvider;
            _logger = logger;
        }


        //public override Task StartAsync(CancellationToken cancellationToken)
        //{
        //    Console.WriteLine("lkamkldnakldnklandsa");
        //    _logger.LogInformation($"{nameof(MessageSenderBackgroundService)} Started");
        //    return Task.CompletedTask;
        //}

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(MessageSenderBackgroundService)} Executed");

            var sendEnpoint = await _sendEnpointProvider.GetSendEndpoint(new Uri($"queue:{Queues.MessageQueue}"));

            _logger.LogInformation("Created SendEndpoint");


            while (!stoppingToken.IsCancellationRequested)
            {

                using IDbConnection connection = new SqlConnection(ConnectionStrings.SqlServer_ServiceA);

                _logger.LogInformation("Created SqlConnection");

                List<Outbox> outbox = connection.Query<Outbox>(DbCommandAndQueries.Get_OutboxTable).ToList();

                _logger.LogInformation("Queried Outbox Table");

                foreach (var outboxItem in outbox)
                {

                    MessageSendedEvent @event = outboxItem.Payload.Deserialize<MessageSendedEvent>();

                    _logger.LogInformation("Created MessageSendedEvent");

                    await sendEnpoint.Send<MessageSendedEvent>(@event);

                    _logger.LogInformation("Sended MessageSendedEvent");

                    await connection.ExecuteAsync(DbCommandAndQueries.Update_OutboxTable, new { IdempotentId = outboxItem.Id });

                    _logger.LogInformation("Updated MessageSendedEvent in the Db");

                    await Task.Delay(TimeSpan.FromSeconds(3));

                }
                _logger.LogInformation("exit loop");


            }

        }
        //public override Task StopAsync(CancellationToken cancellationToken)
        //{
        //    _logger.LogInformation($"{this.GetType().Name} Stopped");
        //    return Task.CompletedTask;
        //}

    }
}
