using Microsoft.EntityFrameworkCore;

namespace NetX.RBAC.Service
{
    public class DemoDbContext : DbContext
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options)
        {

        }

        public DbSet<sys_dept> sys_dept { get; set; }
    }

    public class sys_dept
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
    }
}
