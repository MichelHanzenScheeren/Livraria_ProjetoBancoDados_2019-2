using Data.Configurations;
using Model.Attributes;

namespace Data.Repositories
{
    public class CategoriaRepository : Repository<Categoria>
    {
        public CategoriaRepository(LivrariaMHSContext context) : base(context)
        {

        }
    }
}
