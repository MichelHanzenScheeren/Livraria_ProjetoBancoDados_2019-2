using Data.Configurations;
using Model.Attributes;

namespace Data.Repositories
{
    public class LivroCategoriaRepository : Repository<LivroCategoria>
    {
        public LivroCategoriaRepository(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
