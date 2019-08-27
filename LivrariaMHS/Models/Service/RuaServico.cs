using LivrariaMHS.Data;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Models.Service
{
    public class RuaServico : Repository<Rua>
    {
        public RuaServico(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
