using System;
using System.Threading.Tasks;
using Persistity.Core;
using Persistity.Core.Data;
using Persistity.Core.Serialization;
using Persistity.Flow.Steps.Types;

namespace Persistity.Flow.Steps
{
    public class DeserializeStep : IPipelineStep, IExpectsData, IReturnsObject
    {
        private readonly IDeserializer _deserializer;
        private readonly Type _type;
        private readonly object _args;

        public DeserializeStep(IDeserializer deserializer, Type type, object args = null)
        {
            _deserializer = deserializer;
            _type = type;
            _args = args;
        }

        public Task<object> Execute(object data, object state = null)
        { return Task.FromResult(_deserializer.Deserialize((DataObject)data, _type, _args)); }
    }
}