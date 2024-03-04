using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppContainer.Contract
{
    public class PageNotFoundException : Exception
    {
        public PageNotFoundException(string message) 
            : base(message)
        {
        }
    }
}
