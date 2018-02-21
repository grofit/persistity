using System;
using System.Threading.Tasks;

namespace Persistity.Pipelines
{
    public interface IReceiveDataPipeline
    {
        Task<T> Execute<T>(object state);
    }
}