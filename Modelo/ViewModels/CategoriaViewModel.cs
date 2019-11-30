using Model.Attributes;
using System.Collections.Generic;

namespace Model.ViewModels
{
    public class CategoriaViewModel
    {
        public Categoria Categoria { get; set; }
        public IEnumerable<LivroCategoria> LivrosCategorias { get; set; }
    }
}
