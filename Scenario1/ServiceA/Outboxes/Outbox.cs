namespace ServiceA.Outboxes
{
    public class Outbox
    {
        public Outbox(Guid idempotentToken)
        {
            this.IdempotentToken = idempotentToken;
        }

        public Outbox() : this(Guid.NewGuid())
        {

        }
        public int Id { get; set; }
        public Guid? IdempotentToken { get; set; }
        public DateTime? WritedDate { get; set; }
        public string EventType { get; set; }
        public string Payload { get; set; }
        public bool Sended { get; set; }
    }
}
