using Data.Configurations;
using Model.Attributes;

namespace Data.Repositories
{
    public class RuaRepository : Repository<Rua>
    {
        public RuaRepository(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
