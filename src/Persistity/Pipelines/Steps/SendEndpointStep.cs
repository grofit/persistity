using System.Threading.Tasks;
using LazyData;
using Persistity.Endpoints;
using Persistity.Pipelines.Steps.Types;

namespace Persistity.Pipelines.Steps
{
    public class SendEndpointStep : IPipelineStep, IExpectsData
    {
        private readonly ISendDataEndpoint _endpoint;

        public SendEndpointStep(ISendDataEndpoint endpoint)
        { _endpoint = endpoint; }

        public Task<object> Execute(object data, object state = null)
        { return _endpoint.Send((DataObject)data); }
    }
}