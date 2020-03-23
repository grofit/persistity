using System.Collections.Generic;
using LazyData.Serialization;
using Persistity.Endpoints;
using Persistity.Pipelines.Steps;
using Persistity.Pipelines.Steps.Types;
using Persistity.Processors;

namespace Persistity.Pipelines.Builders
{
    public class PipelineNeedsDataBuilder
    {
        private List<IPipelineStep> _steps;

        public PipelineNeedsDataBuilder(List<IPipelineStep> steps)
        { _steps = steps; }
        
        public PipelineNeedsObjectBuilder DeserializeWith(IDeserializer deserializer)
        {
            _steps.Add(new DeserializeStep(deserializer));
            return new PipelineNeedsObjectBuilder(_steps);
        }
        
        public PipelineNeedsDataBuilder ProcessWith(IProcessor processor)
        {
            _steps.Add(new ProcessStep(processor));
            return this;
        }
        
        public PipelineNeedsObjectBuilder ThenSendTo(ISendDataEndpoint endpoint)
        {
            _steps.Add(new SendEndpointStep(endpoint));
            return new PipelineNeedsObjectBuilder(_steps);
        }
        
        public IFlowPipeline Build()
        { return new DefaultPipeline(_steps); }
    }
}