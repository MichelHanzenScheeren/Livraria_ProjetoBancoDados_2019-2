using LivrariaMHS.Data;
using LivrariaMHS.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivrariaMHS.Models.Service
{
    public class BairroServico : Repository<Bairro>
    {
        public BairroServico(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
