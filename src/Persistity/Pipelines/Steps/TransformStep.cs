using System.Threading.Tasks;
using Persistity.Pipelines.Steps.Types;
using Persistity.Transformers;

namespace Persistity.Pipelines.Steps
{
    public class TransformStep : IPipelineStep, IExpectsObject
    {
        private readonly ITransformer _transformer;

        public TransformStep(ITransformer transformer)
        { _transformer = transformer; }

        public Task<object> Execute(object data, object state = null)
        { return Task.FromResult(_transformer.Transform(data)); }
    }
}