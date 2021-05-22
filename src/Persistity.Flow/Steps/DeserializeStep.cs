using System;
using System.Threading.Tasks;
using Persistity.Core;
using Persistity.Core.Serialization;
using Persistity.Flow.Steps.Types;

namespace Persistity.Flow.Steps
{
    public class DeserializeStep : IPipelineStep, IExpectsData, IReturnsObject
    {
        private readonly IDeserializer _deserializer;
        private readonly Type _type;

        public DeserializeStep(IDeserializer deserializer, Type type)
        {
            _deserializer = deserializer;
            _type = type;
        }

        public Task<object> Execute(object data, object state = null)
        { return Task.FromResult(_deserializer.Deserialize((DataObject)data, _type)); }
    }
}