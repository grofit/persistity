using System;
using Persistity.Endpoints;
using Persistity.Endpoints.Files;
using Persistity.Processors;
using Persistity.Transformers;
using Persistity.Transformers.Binary;

namespace Tests.Editor
{
    public class Pipeline
    {

    }

    public class SaveBinaryFilePipeline : Pipeline
    {
        public IBinaryTransformer BinaryTransformer { get; set; }
        public WriteFile WriteFile { get; set; }

        public void Execute<T>(T data) where T : new()
        {
            var output = BinaryTransformer.Transform(data);
            WriteFile.Execute(output, null, null);
        }
    }

    public interface IPipeline<T>
    {
        void Execute<TIn>(TIn data, Action onSuccess, Action<Exception> onError) where TIn : new();
    }

    public abstract class SendDataPipeline<T> : IPipeline<T>
    {
        public ITransformer<T> Transformer { get; private set; }
        public IProcessor<T> Processor { get; private set; }
        public ISendData<T> SendToEndpoint { get; private set; }

        protected SendDataPipeline(ITransformer<T> transformer, ISendData<T> sendToEndpoint, IProcessor<T> processor = null)
        {
            Transformer = transformer;
            Processor = processor;
            SendToEndpoint = sendToEndpoint;
        }

        public void Execute<TIn>(TIn data, Action onSuccess, Action<Exception> onError) where TIn : new()
        {
            var output = Transformer.Transform(data);

            if (Processor != null)
            { output = Processor.Process(output); }

            SendToEndpoint.Execute(output, onSuccess, onError);
        }
    }
}