using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAddone.Data
{
    public class BilibiliDataContext : DbContext
    {
        public DbSet<TasksEntity> Tasks { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }

        public BilibiliDataContext(DbContextOptions options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BilibiliDataContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
