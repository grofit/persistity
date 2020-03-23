using System.Collections.Generic;
using Persistity.Pipelines.Steps.Types;

namespace Persistity.Pipelines
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