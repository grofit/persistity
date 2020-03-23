using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistity.Endpoints;
using Persistity.Pipelines.Steps;
using Persistity.Pipelines.Steps.Types;

namespace Persistity.Pipelines.Builders
{
    public class PipelineBuilder
    {
        public PipelineNeedsObjectBuilder StartFromInput()
        { return new PipelineNeedsObjectBuilder(new List<IPipelineStep>()); }
        
        public PipelineNeedsObjectBuilder StartFrom(Func<Task<object>> method)
        { return new PipelineNeedsObjectBuilder(new List<IPipelineStep>{ new ReceiveMethodStep(method)}); }
        
        public PipelineNeedsObjectBuilder StartFrom(Func<object, Task<object>> method)
        { return new PipelineNeedsObjectBuilder(new List<IPipelineStep>{ new ReceiveMethodStep(method)}); }
        
        public PipelineNeedsDataBuilder StartFrom(IReceiveDataEndpoint endpoint)
        { return new PipelineNeedsDataBuilder(new List<IPipelineStep>{ new ReceiveEndpointStep(endpoint)}); }
    }
}