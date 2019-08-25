using LivrariaMHS.Data;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Models.Service
{
    public class ClienteServico : Repository<Cliente>
    {
        public ClienteServico(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
