using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutboxPublisherApplication.WorkerService.Outboxes
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
