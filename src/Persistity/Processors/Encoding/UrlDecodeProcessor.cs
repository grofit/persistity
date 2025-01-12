﻿using System.Net;
using System.Threading.Tasks;
using Persistity.Core;
using Persistity.Core.Data;

namespace Persistity.Processors.Encoding
{
    public class UrlDecodeProcessor : IProcessor
    {
        public Task<DataObject> Process(DataObject data)
        {
            var escapedData = WebUtility.UrlDecode(data.AsString);
            return Task.FromResult(new DataObject(escapedData));
        }
    }
}