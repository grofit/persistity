using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyData.Serialization;
using Persistity.Endpoints;
using Persistity.Pipelines.Steps;
using Persistity.Pipelines.Steps.Types;
using Persistity.Transformers;

namespace Persistity.Pipelines.Builders
{
    public class PipelineNeedsObjectBuilder
    {
        private List<IPipelineStep> _steps;

        public PipelineNeedsObjectBuilder(List<IPipelineStep> steps)
        { _steps = steps; }

        public PipelineNeedsObjectBuilder TransformWith(ITransformer transformer)
        {
            _steps.Add(new TransformStep(transformer));
            return this;
        }
        
        public PipelineNeedsObjectBuilder ThenInvoke(Func<object, object, Task<object>> method)
        {
            _steps.Add(new SendMethodStep(method));
            return this;
        }
        
        public PipelineNeedsObjectBuilder ThenInvoke(Func<object, Task<object>> method)
        {
            _steps.Add(new SendMethodStep(method));
            return this;
        }
        
        public PipelineNeedsDataBuilder SerializeWith(ISerializer serializer, bool persistType = true)
        {
            _steps.Add(new SerializeStep(serializer, persistType));
            return new PipelineNeedsDataBuilder(_steps);
        }
        
        public PipelineNeedsDataBuilder ThenReceiveFrom(IReceiveDataEndpoint endpoint)
        {
            _steps.Add(new ReceiveEndpointStep(endpoint));
            return new PipelineNeedsDataBuilder(_steps);
        }

        public PipelineNeedsObjectBuilder ThenReceiveFrom(Func<Task<object>> method)
        {
            _steps.Add(new ReceiveMethodStep(method));
            return new PipelineNeedsObjectBuilder(_steps);
        }

        public PipelineNeedsObjectBuilder ThenReceiveFrom(Func<object, Task<object>> method)
        {
            _steps.Add(new ReceiveMethodStep(method));
            return new PipelineNeedsObjectBuilder(_steps);
        }
        
        public IFlowPipeline Build()
        { return new DefaultPipeline(_steps); }
    }
}