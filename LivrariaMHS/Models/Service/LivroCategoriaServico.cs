﻿using LivrariaMHS.Data;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Models.Service
{
    public class LivroCategoriaServico : Repository<LivroCategoria>
    {
        public LivroCategoriaServico(LivrariaMHSContext context) : base(context)
        {
        }
    }
}
