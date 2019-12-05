using Data.Configurations;
using Model.Attributes;

namespace Data.Repositories
{
    public class ClienteRepository : Repository<Cliente>
    {
        public ClienteRepository(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
