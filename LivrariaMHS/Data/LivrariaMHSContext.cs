using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Models
{
    public class LivrariaMHSContext : DbContext
    {
        public LivrariaMHSContext (DbContextOptions<LivrariaMHSContext> options)
            : base(options)
        {
        }

        public DbSet<LivrariaMHS.Models.Attributes.Cliente> Cliente { get; set; }
    }
}
