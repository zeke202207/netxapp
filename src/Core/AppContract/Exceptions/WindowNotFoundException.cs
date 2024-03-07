using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppCore.Contract
{
    public class WindowNotFoundException : Exception
    {
        public WindowNotFoundException(string message) 
            : base(message)
        {
        }
    }
}
