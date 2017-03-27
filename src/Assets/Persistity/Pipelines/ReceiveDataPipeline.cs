using System;
using System.Collections.Generic;
using System.Linq;
using Persistity.Convertors;
using Persistity.Endpoints;
using Persistity.Processors;
using Persistity.Transformers;

namespace Persistity.Pipelines
{
    public class ReceiveDataPipeline : IReceiveDataPipeline
    {
        public ITransformer Transformer { get; private set; }
        public IEnumerable<IConvertor> Convertors { get; private set; }
        public IEnumerable<IProcessor> Processors { get; private set; }
        public IReceiveDataEndpoint ReceiveFromEndpoint { get; private set; }

        public ReceiveDataPipeline(ITransformer transformer, IReceiveDataEndpoint receiveFromEndpoint, IEnumerable<IProcessor> processors = null, IEnumerable<IConvertor> convertors = null)
        {
            Transformer = transformer;
            Processors = processors;
            Convertors = convertors;
            ReceiveFromEndpoint = receiveFromEndpoint;
        }

        public ReceiveDataPipeline(ITransformer transformer, IReceiveDataEndpoint receiveFromEndpoint, params IProcessor[] processors)
        {
            Transformer = transformer;
            Processors = processors;
            ReceiveFromEndpoint = receiveFromEndpoint;
        }

        public void Execute<TDataType>(Action<object> onSuccess, Action<Exception> onError) where TDataType : new()
        {
            ReceiveFromEndpoint.Execute(x =>
            {
                var output = x;
                if (Processors != null && Processors.Any())
                {
                    foreach (var processor in Processors)
                    { output = processor.Process(output); }
                }

                object model = Transformer.Transform<TDataType>(output);
                if (Convertors != null)
                {
                    foreach (var convertor in Convertors)
                    { model = convertor.ConvertTo(model); }
                }

                onSuccess(model);
            }, onError);
        }
    }
}