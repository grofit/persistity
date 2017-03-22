using System;
using System.Collections.Generic;
using System.Linq;
using Persistity.Endpoints;
using Persistity.Processors;
using Persistity.Transformers;

namespace Persistity.Pipelines
{
    public class SendDataPipeline : ISendDataPipeline
    {
        public ITransformer Transformer { get; private set; }
        public IEnumerable<IProcessor> Processors { get; private set; }
        public ISendData SendToEndpoint { get; private set; }

        public SendDataPipeline(ITransformer transformer, ISendData sendToEndpoint, IEnumerable<IProcessor> processors = null)
        {
            Transformer = transformer;
            Processors = processors;
            SendToEndpoint = sendToEndpoint;
        }

        public void Execute<T>(T data, Action onSuccess, Action<Exception> onError) where T : new()
        {
            var output = Transformer.Transform(data);

            if (Processors != null && Processors.Any())
            {
                foreach (var processor in Processors)
                { output = processor.Process(output); }
            }

            SendToEndpoint.Execute(output, onSuccess, onError);
        }
    }
}