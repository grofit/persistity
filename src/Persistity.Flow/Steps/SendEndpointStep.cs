using System.Threading.Tasks;
using Persistity.Core;
using Persistity.Core.Data;
using Persistity.Endpoints;
using Persistity.Flow.Steps.Types;

namespace Persistity.Flow.Steps
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