using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Persistity.Core;
using Persistity.Core.Serialization;
using Persistity.Endpoints;
using Persistity.Flow.Pipelines;
using Persistity.Flow.Steps;
using Persistity.Flow.Steps.Types;
using Persistity.Processors;

namespace Persistity.Flow.Builders
{
    public class PipelineNeedsDataBuilder
    {
        private List<IPipelineStep> _steps;

        public PipelineNeedsDataBuilder(List<IPipelineStep> steps)
        { _steps = steps; }
        
        public PipelineNeedsObjectBuilder DeserializeWith(IDeserializer deserializer, Type type, object args = null)
        {
            _steps.Add(new DeserializeStep(deserializer, type, args));
            return new PipelineNeedsObjectBuilder(_steps);
        }
        
        public PipelineNeedsObjectBuilder DeserializeWith<T>(IDeserializer deserializer, object args = null)
        { return DeserializeWith(deserializer, typeof(T), args); }
        
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