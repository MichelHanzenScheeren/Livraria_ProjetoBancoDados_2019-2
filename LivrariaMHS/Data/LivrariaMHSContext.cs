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

        public DbSet<Rua> Ruas { get; set; }

        public DbSet<Bairro> Bairros { get; set; }

        public DbSet<Cidade> Cidades { get; set; }

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Autor> Autores { get; set; }

        public DbSet<Livro> Livros { get; set; }




    }
}
