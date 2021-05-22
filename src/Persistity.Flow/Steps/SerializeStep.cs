using System.Threading.Tasks;
using Persistity.Core.Serialization;
using Persistity.Flow.Steps.Types;

namespace Persistity.Flow.Steps
{
    public class SerializeStep : IPipelineStep, IReturnsData
    {
        private readonly object _args;
        private readonly ISerializer _serializer;

        public SerializeStep(ISerializer serializer, object args = null)
        {
            _args = args;
            _serializer = serializer;
        }

        public Task<object> Execute(object data, object state = null)
        { return Task.FromResult((object)_serializer.Serialize(data, _args)); }
    }
}