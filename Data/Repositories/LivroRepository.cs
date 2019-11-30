using Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Model.Attributes;

namespace Data.Repositories
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
