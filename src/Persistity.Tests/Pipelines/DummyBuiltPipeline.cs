using System.Collections.Generic;
using System.Threading.Tasks;
using Persistity.Flow.Builders;
using Persistity.Flow.Pipelines;
using Persistity.Flow.Steps.Types;
using Persistity.Pipelines;

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