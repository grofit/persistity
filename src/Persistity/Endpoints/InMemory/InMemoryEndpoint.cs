﻿using System.Threading.Tasks;
using Persistity.Core;
using Persistity.Core.Data;

namespace Persistity.Endpoints.InMemory
{
    public class InMemoryEndpoint : IReceiveDataEndpoint, ISendDataEndpoint
    {
        private DataObject _inMemoryStore;

        public Task<DataObject> Receive()
        { return Task.FromResult(_inMemoryStore); }

        public Task<object> Send(DataObject data)
        {
            _inMemoryStore = data;
            return Task.FromResult<object>(null);
        }
    }
}