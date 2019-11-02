using LivrariaMHS.Models;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Data.Repositories
{
    public class CidadeRepository : Repository<Cidade>
    {
        public CidadeRepository(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
