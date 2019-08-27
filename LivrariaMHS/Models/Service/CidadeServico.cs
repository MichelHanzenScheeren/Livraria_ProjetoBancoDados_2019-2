using LivrariaMHS.Data;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Models.Service
{
    public class CidadeServico : Repository<Cidade>
    {
        public CidadeServico(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
