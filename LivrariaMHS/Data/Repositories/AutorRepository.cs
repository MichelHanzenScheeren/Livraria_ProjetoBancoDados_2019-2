using LivrariaMHS.Models;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Data.Repositories
{
    public class AutorRepository : Repository<Autor>
    {
        public AutorRepository(LivrariaMHSContext context) : base(context)
        {

        }
    }
}
