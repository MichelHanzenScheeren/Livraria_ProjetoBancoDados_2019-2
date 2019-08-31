using LivrariaMHS.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LivrariaMHS.Models.ViewModels
{
    public class LivroViewModel
    {
        public Livro Livro { get; set; }

        [Required(ErrorMessage = "Selecione pelo menos uma categoria")]
        public ICollection<Categoria> Categorias { get; set; }
    }
}
