using System;
using System.Collections.Generic;
using System.Linq;
using Persistity.Convertors;
using Persistity.Endpoints;
using Persistity.Processors;
using Persistity.Serialization;

namespace Persistity.Pipelines
{
    public class ReceiveDataPipeline : IReceiveDataPipeline
    {
        public IDeserializer Deserializer { get; private set; }
        public IEnumerable<IConvertor> Convertors { get; private set; }
        public IEnumerable<IProcessor> Processors { get; private set; }
        public IReceiveDataEndpoint ReceiveFromEndpoint { get; private set; }

        public ReceiveDataPipeline(IDeserializer deserializer, IReceiveDataEndpoint receiveFromEndpoint, IEnumerable<IProcessor> processors = null, IEnumerable<IConvertor> convertors = null)
        {
            Deserializer = deserializer;
            Processors = processors;
            Convertors = convertors;
            ReceiveFromEndpoint = receiveFromEndpoint;
        }

        public ReceiveDataPipeline(IDeserializer deserializer, IReceiveDataEndpoint receiveFromEndpoint, params IProcessor[] processors) : this(deserializer, receiveFromEndpoint, processors, null)
        {}

        public void Execute<T>(Action<T> onSuccess, Action<Exception> onError) where T: new()
        {
            ReceiveFromEndpoint.Execute(x =>
            {
                var output = x;
                if (Processors != null && Processors.Any())
                {
                    foreach (var processor in Processors)
                    { output = processor.Process(output); }
                }

                object model = Deserializer.DeserializeData(output);
                if (Convertors != null)
                {
                    foreach (var convertor in Convertors)
                    { model = convertor.ConvertTo(model); }
                }

                onSuccess((T)model);
            }, onError);
        }
    }
}