using Microsoft.EntityFrameworkCore;
using NetX.RBAC.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetX.RBAC.Service
{
    public class RBACDbContext : DbContext
    {
        public RBACDbContext(DbContextOptions<RBACDbContext> options) : base(options)
        {

        }

        public DbSet<sys_user> sys_user { get; set; }
        //public DbSet<Role> Roles { get; set; }
        //public DbSet<Permission> Permissions { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }
        //public DbSet<RolePermission> RolePermissions { get; set; }
    }
}
