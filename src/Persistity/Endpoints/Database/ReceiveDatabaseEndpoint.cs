using System.Data;
using System.Threading.Tasks;
using LazyData;
using Persistity.Extensions;

namespace Persistity.Endpoints.Database
{
    public abstract class ReceiveDatabaseEndpoint : IReceiveDataEndpoint
    {
        public IDbConnection Connection { get; }
        public object Data { get; }
        public abstract bool ReturnAsJson { get; }

        protected ReceiveDatabaseEndpoint(IDbConnection connection, object data = null)
        {
            Connection = connection;
            Data = data;
        }

        protected abstract string QueryToRun();

        public Task<DataObject> Receive()
        {
            string result;
            Connection.Open();
            using (var command = Connection.CreateCommand())
            {
                var sql = QueryToRun();
                command.CommandText = sql;
                command.CommandType = CommandType.Text;

                var reader = command.ExecuteReader();
                var table = new DataTable();
                table.Load(reader);

                result = ReturnAsJson ? table.ToJson() : table.ToXml();
            }
            Connection.Close();

            var data = new DataObject(result);
            return Task.FromResult(data);
        }
    }
}