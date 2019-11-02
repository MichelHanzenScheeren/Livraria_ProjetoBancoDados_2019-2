using LivrariaMHS.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LivrariaMHS.Models.ViewModels
{
    public class CategoriaViewModel
    {
        public Categoria Categoria { get; set; }
        public IEnumerable<LivroCategoria> LivrosCategorias { get; set; }
    }
}
