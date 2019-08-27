using LivrariaMHS.Data;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Models.Service
{
    public class BairroServico : Repository<Bairro>
    {
        public BairroServico(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
