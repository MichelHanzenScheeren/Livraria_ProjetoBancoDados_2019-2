using LivrariaMHS.Models;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Data.Repositories
{
    public class RuaRepository : Repository<Rua>
    {
        public RuaRepository(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
