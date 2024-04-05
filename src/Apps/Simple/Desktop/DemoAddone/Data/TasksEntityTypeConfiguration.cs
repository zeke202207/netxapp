using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.Data
{
    public class TasksEntityTypeConfiguration : IEntityTypeConfiguration<TasksEntity>
    {
        public void Configure(EntityTypeBuilder<TasksEntity> builder)
        {
            builder.ToTable("tasks");
            builder.HasKey(e => e.Id);
        }
    }
}
