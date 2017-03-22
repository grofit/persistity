using Persistity.Endpoints;
using Persistity.Transformers;

namespace Persistity.Pipelines.Builders
{
    public class PipelineBuilder
    {
        public SendPipelineBuilder TransformWith(ITransformer transformer)
        { return new SendPipelineBuilder(transformer); }

        public ReceivePipelineBuilder RecieveFrom(IReceiveData recieveData)
        { return new ReceivePipelineBuilder(recieveData); }
    }
}