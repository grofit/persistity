using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LazyData;
using LazyData.Serialization;
using Persistity.Endpoints;
using Persistity.Processors;
using Persistity.Transformers;

namespace Persistity.Pipelines
{
    public class SendDataPipeline : ISendDataPipeline
    {
        public ISerializer Serializer { get; }
        public IEnumerable<ITransformer> Transformers { get; }
        public IEnumerable<IProcessor> Processors { get; }
        public ISendDataEndpoint SendToEndpoint { get; }

        public SendDataPipeline(ISerializer serializer, ISendDataEndpoint sendToEndpoint, IEnumerable<IProcessor> processors = null, IEnumerable<ITransformer> transformers = null)
        {
            Serializer = serializer;
            Processors = processors;
            Transformers = transformers;
            SendToEndpoint = sendToEndpoint;
        }

        public virtual async Task<object> Execute<T>(T data, object state = null)
        {
            var transformedData = RunTransformers(data);
            var output = Serializer.Serialize(transformedData);
            var processedData = await RunProcessors(output);
            return await SendToEndpoint.Send(processedData);
        }

        protected virtual object RunTransformers(object data)
        {
            if (Transformers == null) { return data; }
            
            foreach (var convertor in Transformers)
            { data = convertor.TransformTo(data); }
            
            return data;
        }

        protected virtual async Task<DataObject> RunProcessors(DataObject data)
        {
            if (Processors != null && Processors.Any())
            {
                foreach (var processor in Processors)
                { data = await processor.Process(data); }
            }
            return data;
        }
    }
}