using System.Threading.Tasks;

namespace Persistity.Pipelines
{
    public interface IPipeline
    {
        Task<object> Execute(object input = null, object state = null);
    }
}