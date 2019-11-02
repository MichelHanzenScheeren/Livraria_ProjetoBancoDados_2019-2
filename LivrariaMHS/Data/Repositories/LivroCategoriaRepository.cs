using LivrariaMHS.Models;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Data.Repositories
{
    public class LivroCategoriaRepository : Repository<LivroCategoria>
    {
        public LivroCategoriaRepository(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
