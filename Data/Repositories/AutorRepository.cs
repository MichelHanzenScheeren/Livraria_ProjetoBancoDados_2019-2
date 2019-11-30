using Data.Configurations;
using Model.Attributes;

namespace Data.Repositories
{
    public class AutorRepository : Repository<Autor>
    {
        public AutorRepository(LivrariaMHSContext context) : base(context)
        {

        }
    }
}
