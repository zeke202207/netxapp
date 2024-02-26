using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetX.AppContainer
{
    public interface IPipelineContext<TResult>
    {
        CancellationToken CancellationToken { get; }
        TResult Result { get; set; }
        // other properties or methods as needed for inter-step communication
    }
}
