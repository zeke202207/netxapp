using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.Data
{
    public class CategoryEntity
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string ParentPath { get; set; }
        public string Name { get; set; }
        public int CategoryType { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
