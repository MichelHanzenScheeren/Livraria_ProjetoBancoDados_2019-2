using Data.Configurations;
using Model.Attributes;

namespace Data.Repositories
{
    public class CidadeRepository : Repository<Cidade>
    {
        public CidadeRepository(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
