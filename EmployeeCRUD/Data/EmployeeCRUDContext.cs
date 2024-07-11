using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeCRUD.Models;

namespace EmployeeCRUD.Data
{
    public class EmployeeCRUDContext : DbContext
    {
        public EmployeeCRUDContext (DbContextOptions<EmployeeCRUDContext> options)
            : base(options)
        {
        }

        public string ConnectionString => Database.GetDbConnection().ConnectionString;

        public DbSet<EmployeeCRUD.Models.Employee> Employee { get; set; } = default!;
    }
}
