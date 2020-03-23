using System;
using System.Threading.Tasks;
using LazyData;

namespace Persistity.Processors.Encoding
{
    public class Base64DecodeProcessor : IProcessor
    {
        public Task<DataObject> Process(DataObject data)
        {
            var byteData = Convert.FromBase64String(data.AsString);
            return Task.FromResult(new DataObject(byteData));
        }
    }
}