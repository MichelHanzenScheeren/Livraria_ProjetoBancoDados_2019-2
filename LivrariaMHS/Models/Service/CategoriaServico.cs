using LivrariaMHS.Data;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Models.Service
{
    public class CategoriaServico : Repository<Categoria>
    {
        public CategoriaServico(LivrariaMHSContext context) : base(context)
        {

        }
    }
}
