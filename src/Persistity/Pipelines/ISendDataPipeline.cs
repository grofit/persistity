using System;
using System.Threading.Tasks;

namespace Persistity.Pipelines
{
    public interface ISendDataPipeline
    {
        Task<object> Execute<T>(T data, object state = null);
    }
}