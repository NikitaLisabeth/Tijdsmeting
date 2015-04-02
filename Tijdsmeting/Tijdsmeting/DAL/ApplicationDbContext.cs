using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tijdsmeting.Model;

namespace Tijdsmeting.DAL
{
    class ApplicationDbContext : DbContext
    {
        public DbSet<Runner> Runners { get; set; }
        
    }
}
