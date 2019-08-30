﻿using LivrariaMHS.Data;
using LivrariaMHS.Models.Attributes;

namespace LivrariaMHS.Models.Service
{
    public class LivroServico : Repository<Livro>
    {
        public LivroServico(LivrariaMHSContext context) : base(context)
        {

        }
    }
}