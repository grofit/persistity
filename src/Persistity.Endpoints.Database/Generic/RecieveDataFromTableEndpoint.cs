using System.Data;

namespace Persistity.Endpoints.Database.Generic
{
    public class RecieveDataFromTableEndpoint : ReceiveDatabaseEndpoint
    {
        public RecieveDataFromTableEndpoint(IDbConnection connection) : base(connection)
        {}

        public override bool ReturnAsJson { get; }

        protected override string QueryToRun()
        {
            throw new System.NotImplementedException("This is still a todo");
        }
    }
}