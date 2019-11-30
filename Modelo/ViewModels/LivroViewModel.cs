using Model.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels
{
    public class LivroViewModel
    {
        public Livro Livro { get; set; }

        [Required(ErrorMessage = "Selecione pelo menos uma categoria")]
        public ICollection<Categoria> Categorias { get; set; }
    }
}
