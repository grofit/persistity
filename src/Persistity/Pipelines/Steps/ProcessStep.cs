using System.Threading.Tasks;
using LazyData;
using Persistity.Pipelines.Steps.Types;
using Persistity.Processors;

namespace Persistity.Pipelines.Steps
{
    public class ProcessStep : IPipelineStep, IExpectsData, IReturnsData
    {
        private readonly IProcessor _processor;

        public ProcessStep(IProcessor processor)
        { _processor = processor; }

        public async Task<object> Execute(object data, object state = null)
        { return await _processor.Process((DataObject)data); }
    }
}