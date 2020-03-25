using System.Collections.Generic;
using Persistity.Flow.Steps.Types;

namespace Persistity.Flow.Pipelines
{
    public abstract class BuiltPipeline : FlowPipeline
    {
        public BuiltPipeline()
        {
            Steps = BuildSteps();
        }

        protected abstract IEnumerable<IPipelineStep> BuildSteps();
    }
}