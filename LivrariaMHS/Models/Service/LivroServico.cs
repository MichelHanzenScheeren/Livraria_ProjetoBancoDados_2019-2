using LivrariaMHS.Data;
using LivrariaMHS.Models.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LivrariaMHS.Models.Service
{
    public class LivroServico : Repository<Livro>
    {
        public LivroServico(LivrariaMHSContext context) : base(context)
        {
        }

        public void AlterPrecoLivros(decimal porcentagem, string tipo)
        {
            _context.Database.ExecuteSqlCommand("EXEC AlterPrecoLivros {0}, {1}", porcentagem, tipo);
        }
    }
}
