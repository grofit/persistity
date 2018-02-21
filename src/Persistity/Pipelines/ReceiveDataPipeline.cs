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
    public class ReceiveDataPipeline : IReceiveDataPipeline
    {
        public IDeserializer Deserializer { get; }
        public IEnumerable<ITransformer> Transformers { get; }
        public IEnumerable<IProcessor> Processors { get; }
        public IReceiveDataEndpoint ReceiveFromEndpoint { get; }

        public ReceiveDataPipeline(IDeserializer deserializer, IReceiveDataEndpoint receiveFromEndpoint, IEnumerable<IProcessor> processors = null, IEnumerable<ITransformer> transformers = null)
        {
            Deserializer = deserializer;
            Processors = processors;
            Transformers = transformers;
            ReceiveFromEndpoint = receiveFromEndpoint;
        }

        public ReceiveDataPipeline(IDeserializer deserializer, IReceiveDataEndpoint receiveFromEndpoint, params IProcessor[] processors) : this(deserializer, receiveFromEndpoint, processors, null)
        {}

        public virtual async Task<T> Execute<T>(object state)
        {
            var data = await ReceiveFromEndpoint.Receive();
            var output = await RunProcessors(data);
            var model = Deserializer.Deserialize(output);
            return (T)RunTransformers(model);
        }

        protected object RunTransformers(object data)
        {
            if (Transformers != null)
            {
                foreach (var convertor in Transformers)
                { data = convertor.TransformFrom(data); }
            }
            return data;
        }

        protected async Task<DataObject> RunProcessors(DataObject data)
        {
            if (Processors == null || !Processors.Any()) 
            { return data; }
            
            foreach (var processor in Processors)
            { data = await processor.Process(data); }
            return data;
        }
    }
}