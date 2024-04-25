using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.ViewModels
{
    public class BreadCrumbItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool CanClick { get; set; } = true;
        public bool IsLast { get; set; } = true;
    }
}
