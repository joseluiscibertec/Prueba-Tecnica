using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
          base(options)
        {

        }
        public DbSet<Cartilla> Cartilla { get; set; }
    }
}
