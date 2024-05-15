using MassTransit;
using Microsoft.EntityFrameworkCore;
using ServiceB.Contexts;
using ServiceB.Entities;
using ServiceB.Inboxes;
using Shared.Extensions;

namespace ServiceB.Consumers
{
    public class MessageSendedEventConsumer : IConsumer<MessageSendedEvent>
    {
        readonly ILogger<MessageSendedEventConsumer> _logger;
        readonly ServiceBDbContext _context;
        public MessageSendedEventConsumer(ILogger<MessageSendedEventConsumer> logger,
        ServiceBDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Consume(ConsumeContext<MessageSendedEvent> context)
        {
            var @event = context.Message;

            _logger.LogInformation("Message recevied");
            _logger.LogInformation($"idempotent token : {@event.IdempotentToken}");
            _logger.LogInformation($"Message Id : {@event.MessageId}, Message Title : {@event.Title} , Message Content  : {@event.Content}");

            var isExists = await _context.Inbox.AnyAsync(i => i.IdempotentToken.Equals(@event.IdempotentToken));

            if (isExists)
                return;


            Message message = new()
            {
                Id = @event.MessageId,
                Content = @event.Content,
                Title = @event.Title
            };

            Inbox inbox = new(@event.IdempotentToken)
            {
                Payload = message.Serialize(),
                Processed = false
            };

            await _context.Inbox.AddAsync(inbox);
            await _context.SaveChangesAsync();

            inbox = null;
            message = null;

            IEnumerable<Inbox> inboxes = await _context.Inbox
                .Where(i => i.Processed.Equals(false))
                .OrderByDescending(i => i.ProcessedDate)
                .ToListAsync();

            foreach (Inbox data in inboxes)
            {
                message = data.Payload.Deserialize<Message>();

                _logger.LogInformation($"Idempotent Token : {data.IdempotentToken}");
                _logger.LogInformation($"Message Id : {message.Id}");
                _logger.LogInformation($"Message Title: {message.Title}");
                _logger.LogInformation($"Message Content : {message.Content}");

                data.ProcessedDate = DateTime.Now;
                data.Processed = true;

                await _context.SaveChangesAsync();

                _logger.LogInformation("Message Processed");
            }






        }
    }
}
