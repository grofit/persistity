using System.Data;
using System.Threading.Tasks;
using LazyData;

namespace Persistity.Endpoints.Database
{
    public abstract class SendDatabaseEndpoint : ISendDataEndpoint
    {
        public IDbConnection Connection { get; }

        protected SendDatabaseEndpoint(IDbConnection connection)
        {
            Connection = connection;
        }

        protected abstract string QueryToRun(DataObject data);

        public Task<object> Send(DataObject data)
        {
            object result;
            Connection.Open();
            using (var command = Connection.CreateCommand())
            {
                var sql = QueryToRun(data);
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                result = command.ExecuteNonQuery();
            }
            Connection.Close();

            return Task.FromResult(result);
        }
    }
}