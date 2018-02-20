using System.Net;
using LazyData;

namespace Persistity.Processors.Encoding
{
    public class UrlDecodeProcessor : IProcessor
    {
        public DataObject Process(DataObject data)
        {
            var escapedData = WebUtility.UrlDecode(data.AsString);
            return new DataObject(escapedData);
        }
    }
}