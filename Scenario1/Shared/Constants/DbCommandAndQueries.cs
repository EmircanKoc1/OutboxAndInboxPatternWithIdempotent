namespace Shared.Constants
{
    public static class DbCommandAndQueries
    {
        public const string Get_OutboxTable = @"select * from Outbox where Sended = 0 AND WritedDate IS NULL";

        public const string Update_OutboxTable = "update Outbox SET WritedDate = GETDATE() , Sended = 1 Where IdempotentToken = '@IdempotentToken'";


    }
}
