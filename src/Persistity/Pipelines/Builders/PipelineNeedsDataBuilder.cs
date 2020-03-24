using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LazyData;
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
        
        public PipelineNeedsObjectBuilder ThenInvoke(Func<DataObject, object, Task<object>> method)
        {
            _steps.Add(new SendDataToObjectMethodStep(method));
            return new PipelineNeedsObjectBuilder(_steps);
        }
        
        public PipelineNeedsObjectBuilder ThenInvoke(Func<DataObject, Task<object>> method)
        {
            _steps.Add(new SendDataToObjectMethodStep(method));
            return new PipelineNeedsObjectBuilder(_steps);
        }
        
        public PipelineNeedsDataBuilder ThenInvoke(Func<DataObject, object, Task<DataObject>> method)
        {
            _steps.Add(new SendDataToDataMethodStep(method));
            return this;
        }
        
        public PipelineNeedsDataBuilder ThenInvoke(Func<DataObject, Task<DataObject>> method)
        {
            _steps.Add(new SendDataToDataMethodStep(method));
            return this;
        }
        
        public PipelineNeedsObjectBuilder ThenSendTo(ISendDataEndpoint endpoint)
        {
            _steps.Add(new SendEndpointStep(endpoint));
            return new PipelineNeedsObjectBuilder(_steps);
        }

        public IEnumerable<IPipelineStep> BuildSteps()
        { return _steps;  }
        
        public IFlowPipeline Build()
        { return new DefaultPipeline(_steps); }
    }
}