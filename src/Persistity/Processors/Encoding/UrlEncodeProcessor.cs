using System.Net;
using LazyData;

namespace Persistity.Processors.Encoding
{
    public class UrlEncodeProcessor : IProcessor
    {
        public DataObject Process(DataObject data)
        {
            var escapedData = WebUtility.UrlEncode(data.AsString);
            return new DataObject(escapedData);
        }
    }
}