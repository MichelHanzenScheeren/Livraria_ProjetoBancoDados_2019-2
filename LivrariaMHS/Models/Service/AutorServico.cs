using LivrariaMHS.Data;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Models.Service
{
    public class AutorServico : Repository<Autor>
    {
        public AutorServico(LivrariaMHSContext context) : base(context)
        {

        }
    }
}
