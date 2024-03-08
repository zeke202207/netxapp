using System;
using System.Collections.Generic;
using System.Text;

namespace NetX.AppCore.Contract
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EventBusHanderAttribute : Attribute
    {
        public EventBusHanderAttribute()
        {
        }
    }
}
