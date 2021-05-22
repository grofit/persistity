using System;
using System.Threading.Tasks;
using Persistity.Core;

namespace Persistity.Processors.Encoding
{
    public class Base64EncodeProcessor : IProcessor
    {
        public Task<DataObject> Process(DataObject data)
        {
            var base64String = Convert.ToBase64String(data.AsBytes);
            return Task.FromResult(new DataObject(base64String));
        }
    }
}