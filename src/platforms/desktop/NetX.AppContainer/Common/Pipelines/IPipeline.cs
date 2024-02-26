using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.AppContainer
{
    public interface IPipeline<TResult>
    {
        Task<TResult> ExecuteAsync();
    }
}
