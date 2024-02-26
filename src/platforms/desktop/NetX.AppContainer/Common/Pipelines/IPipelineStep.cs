using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer
{
    public interface IPipelineStep<TResult>
    {
        Task ExecuteAsync(IPipelineContext<TResult> context);
    }
}
