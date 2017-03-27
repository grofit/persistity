using System;
using System.Collections.Generic;
using System.Linq;
using Persistity.Convertors;
using Persistity.Endpoints;
using Persistity.Processors;
using Persistity.Transformers;

namespace Persistity.Pipelines
{
    /*
        var saveToBinaryFilePipeline = new PipelineBuilder()
            .ConvertObject()
            .TransformWith(transformer)
            .ProcessWith(encryptionProcessor)
            .SendTo(writeFileEndpoint)
            .Build();
     */

    public class SendDataPipeline : ISendDataPipeline
    {
        public ITransformer Transformer { get; private set; }
        public IEnumerable<IConvertor> Convertors { get; private set; }
        public IEnumerable<IProcessor> Processors { get; private set; }
        public ISendDataEndpoint SendToEndpoint { get; private set; }

        public SendDataPipeline(ITransformer transformer, ISendDataEndpoint sendToEndpoint, IEnumerable<IProcessor> processors = null, IEnumerable<IConvertor> convertors = null)
        {
            Transformer = transformer;
            Processors = processors;
            Convertors = convertors;
            SendToEndpoint = sendToEndpoint;
        }

        public void Execute<T>(T data, Action<object> onSuccess, Action<Exception> onError) where T : new()
        {
            object obj = data;

            if (Convertors != null)
            {
                foreach(var convertor in Convertors)
                { obj = convertor.ConvertTo(obj); }
            }

            var output = Transformer.Transform(obj.GetType(), obj);

            if (Processors != null && Processors.Any())
            {
                foreach (var processor in Processors)
                { output = processor.Process(output); }
            }

            SendToEndpoint.Execute(output, onSuccess, onError);
        }
    }
}