using System.Collections.Generic;
using Persistity.Endpoints;
using Persistity.Extensions;
using Persistity.Processors;
using Persistity.Serialization;

namespace Persistity.Pipelines.Builders
{
    public class ReceivePipelineBuilder
    {
        private IReceiveDataEndpoint _receiveDataEndpointStep;
        private IDeserializer _deserializer;
        private IList<IProcessor> _processors;

        public ReceivePipelineBuilder(IReceiveDataEndpoint receiveDataEndpointStep)
        {
            _receiveDataEndpointStep = receiveDataEndpointStep;
            _processors = new List<IProcessor>();
        }

        public ReceivePipelineBuilder ProcessWith(IProcessor processor)
        {
            _processors.Add(processor);
            return this;
        }

        public ReceivePipelineBuilder ProcessWith(params IProcessor[] processors)
        {
            _processors.AddRange(processors);
            return this;
        }


        public ReceivePipelineBuilder DeserializeWith(IDeserializer deserializer)
        {
            _deserializer = deserializer;
            return this;
        }

        public IReceiveDataPipeline Build()
        {
            return new ReceiveDataPipeline(_deserializer, _receiveDataEndpointStep, _processors);
        }
    }
}