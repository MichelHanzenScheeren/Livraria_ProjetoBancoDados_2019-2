using Data.Configurations;
using Model.Attributes;

namespace Data.Repositories
{
    public class BairroRepository : Repository<Bairro>
    {
        public BairroRepository(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
