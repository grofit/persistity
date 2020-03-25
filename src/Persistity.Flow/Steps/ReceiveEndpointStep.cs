using System.Threading.Tasks;
using Persistity.Endpoints;
using Persistity.Flow.Steps.Types;

namespace Persistity.Flow.Steps
{
    public class ReceiveEndpointStep : IPipelineStep, IReturnsData
    {
        private readonly IReceiveDataEndpoint _endpoint;

        public ReceiveEndpointStep(IReceiveDataEndpoint endpoint)
        { _endpoint = endpoint; }

        public async Task<object> Execute(object data = null, object state = null)
        { return await _endpoint.Receive(); }
    }
}