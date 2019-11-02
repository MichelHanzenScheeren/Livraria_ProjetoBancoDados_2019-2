using LivrariaMHS.Models;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Data.Repositories
{
    public class CategoriaRepository : Repository<Categoria>
    {
        public CategoriaRepository(LivrariaMHSContext context) : base(context)
        {

        }
    }
}
