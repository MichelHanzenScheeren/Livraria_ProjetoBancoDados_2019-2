using LivrariaMHS.Models;
using LivrariaMHS.Models.Attributes;
using Microsoft.EntityFrameworkCore;

namespace LivrariaMHS.Data.Repositories
{
    public class LivroRepository : Repository<Livro>
    {
        public LivroRepository(LivrariaMHSContext context) : base(context)
        {
        }

        public void AlterPrecoLivros(decimal porcentagem, string tipo)
        {
            _context.Database.ExecuteSqlCommand("EXEC AlterPrecoLivros {0}, {1}", porcentagem, tipo);
        }
    }
}
