namespace ServiceB.Inboxes
{
    public class Inbox

    {
        public Inbox(Guid idempotentToken)
        {
            this.IdempotentToken = idempotentToken;
        }

        public Inbox() : this(Guid.NewGuid())
        {

        }
        public int Id { get; set; }

        public Guid? IdempotentToken { get; set; }
        public string Payload { get; set; }
        public bool Processed { get; set; }
        public DateTime? ProcessedDate { get; set; }


    }
}
