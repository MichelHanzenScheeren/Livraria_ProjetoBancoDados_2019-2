using LivrariaMHS.Models;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Data.Repositories
{
    public class ClienteRepository : Repository<Cliente>
    {
        public ClienteRepository(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
