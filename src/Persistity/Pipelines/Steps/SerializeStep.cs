using System.Threading.Tasks;
using LazyData.Serialization;
using Persistity.Pipelines.Steps.Types;

namespace Persistity.Pipelines.Steps
{
    public class SerializeStep : IPipelineStep, IReturnsData
    {
        private readonly bool _persistTypes;
        private readonly ISerializer _serializer;

        public SerializeStep(ISerializer serializer, bool persistTypes = true)
        {
            _persistTypes = persistTypes;
            _serializer = serializer;
        }

        public Task<object> Execute(object data, object state = null)
        { return Task.FromResult((object)_serializer.Serialize(data, _persistTypes)); }
    }
}