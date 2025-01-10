using System.Net;
using System.Threading.Tasks;
using Persistity.Core;
using Persistity.Core.Data;

namespace Persistity.Processors.Encoding
{
    public class UrlEncodeProcessor : IProcessor
    {
        public Task<DataObject> Process(DataObject data)
        {
            var escapedData = WebUtility.UrlEncode(data.AsString);
            return Task.FromResult(new DataObject(escapedData));
        }
    }
}