

namespace ServiceB.Consumers
{
    public class MessageSendedEvent
    {
        public MessageSendedEvent(Guid idempotentToken)
        {
            IdempotentToken = idempotentToken;
        }

        public Guid IdempotentToken { get; set; }
        
        public Guid MessageId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

    }
}
