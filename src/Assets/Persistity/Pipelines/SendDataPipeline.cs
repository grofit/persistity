using System;
using System.Collections.Generic;
using System.Linq;
using Persistity.Convertors;
using Persistity.Endpoints;
using Persistity.Processors;
using Persistity.Serialization;

namespace Persistity.Pipelines
{
    /*
        var saveToBinaryFilePipeline = new PipelineBuilder()
            .ConvertObject()
            .SerializeWith(transformer)
            .ProcessWith(encryptionProcessor)
            .SendTo(writeFileEndpoint)
            .Build();
     */

    public class SendDataPipeline : ISendDataPipeline
    {
        public ISerializer Serializer { get; private set; }
        public IEnumerable<IConvertor> Convertors { get; private set; }
        public IEnumerable<IProcessor> Processors { get; private set; }
        public ISendDataEndpoint SendToEndpoint { get; private set; }

        public SendDataPipeline(ISerializer serializer, ISendDataEndpoint sendToEndpoint, IEnumerable<IProcessor> processors = null, IEnumerable<IConvertor> convertors = null)
        {
            Serializer = serializer;
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

            var output = Serializer.SerializeData(obj);

            if (Processors != null && Processors.Any())
            {
                foreach (var processor in Processors)
                { output = processor.Process(output); }
            }

            SendToEndpoint.Execute(output, onSuccess, onError);
        }
    }
}