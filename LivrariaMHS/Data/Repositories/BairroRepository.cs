using LivrariaMHS.Models;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Data.Repositories
{
    public class BairroRepository : Repository<Bairro>
    {
        public BairroRepository(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
