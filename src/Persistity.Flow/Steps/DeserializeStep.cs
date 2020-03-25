using System.Threading.Tasks;
using LazyData;
using LazyData.Serialization;
using Persistity.Flow.Steps.Types;

namespace Persistity.Flow.Steps
{
    public class DeserializeStep : IPipelineStep, IExpectsData, IReturnsObject
    {
        private readonly IDeserializer _deserializer;

        public DeserializeStep(IDeserializer deserializer)
        { _deserializer = deserializer; }

        public Task<object> Execute(object data, object state = null)
        { return Task.FromResult(_deserializer.Deserialize((DataObject)data)); }
    }
}