using System.Collections.Generic;
using Persistity.Endpoints;
using Persistity.Extensions;
using Persistity.Processors;
using Persistity.Serialization;

namespace Persistity.Pipelines.Builders
{
    public class SendPipelineBuilder
    {
        private ISerializer _serializer;
        private ISendDataEndpoint _sendDataEndpointStep;
        private IList<IProcessor> _processors;

        public SendPipelineBuilder(ISerializer serializer)
        {
            _serializer = serializer;
            _processors = new List<IProcessor>();
        }

        public SendPipelineBuilder ProcessWith(IProcessor processor)
        {
            _processors.Add(processor);
            return this;
        }

        public SendPipelineBuilder ProcessWith(params IProcessor[] processors)
        {
            _processors.AddRange(processors);
            return this;
        }

        public SendPipelineBuilder SendTo(ISendDataEndpoint sendDataEndpoint)
        {
            _sendDataEndpointStep = sendDataEndpoint;
            return this;
        }

        public ISendDataPipeline Build()
        {
            return new SendDataPipeline(_serializer, _sendDataEndpointStep, _processors);
        }
    }
}