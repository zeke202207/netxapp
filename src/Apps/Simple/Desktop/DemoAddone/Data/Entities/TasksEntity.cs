using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.Data
{
    public class TasksEntity
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string TaskInfo { get; set; }
        public int IsCompleted { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
