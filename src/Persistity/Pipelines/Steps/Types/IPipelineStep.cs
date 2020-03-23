using System.Threading.Tasks;

namespace Persistity.Pipelines.Steps.Types
{
    /// <summary>
    /// This represents a step in a larger pipeline that takes an input and does something and then returns the step result
    /// </summary>
    /// <remarks>
    /// These are like the building blocks, so you may have a transformer -> transform -> serialize -> endpoint
    /// </remarks>
    public interface IPipelineStep
    {   
        Task<object> Execute(object data, object state = null);
    }
}