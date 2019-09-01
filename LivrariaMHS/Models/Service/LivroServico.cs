using LivrariaMHS.Data;
using LivrariaMHS.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LivrariaMHS.Models.Service
{
    public class LivroServico : Repository<Livro>
    {
        public LivroServico(LivrariaMHSContext context) : base(context)
        {

        }

        public List<string> Find(string search)
        {
            return _context.Set<Livro>().Where(x => x.Titulo.Contains(search)).Select(x => x.Titulo).ToList();
        }
    }
}
