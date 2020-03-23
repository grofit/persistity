using System.Collections.Generic;
using System.Threading.Tasks;
using Persistity.Pipelines;
using Persistity.Pipelines.Builders;
using Persistity.Pipelines.Steps.Types;

namespace Persistity.Tests.Pipelines
{
    public class DummyBuiltPipeline : BuiltPipeline
    {
        protected override IEnumerable<IPipelineStep> BuildSteps()
        {
            return new PipelineBuilder()
                .StartFromInput()
                .ThenInvoke(Task.FromResult)
                .ThenInvoke(Task.FromResult)
                .ThenInvoke(Task.FromResult)
                .BuildSteps();
        }
    }
}