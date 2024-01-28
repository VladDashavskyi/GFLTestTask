using GFLTestTask.Dal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFLTestTask.Dal.Interfaces
{
    public interface IApplicationContext
    {
        DbSet<User> User { get; set; }
        DbSet<Position> Position { get; set; }
        DbSet<Employee> Employee { get; set; }
        Task<bool> SaveChangesAsync();
    }
}
