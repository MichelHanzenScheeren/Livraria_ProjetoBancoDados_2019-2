using LivrariaMHS.Data;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Models.Service
{
    public class VendaServico : Repository<Venda>
    {
        public VendaServico(LivrariaMHSContext context) : base(context)
        {

        }
    }
}
