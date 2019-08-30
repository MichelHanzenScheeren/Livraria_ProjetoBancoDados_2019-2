using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LivrariaMHS.Models.Attributes
{
    public class Autor
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Autor")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "{0} deve ter entre 5 e 70 caracteres!")]
        public string Nome { get; set; }

        public virtual List<Livro> Livros { get; set; }

        public Autor()
        {
            Livros = new List<Livro>();
        }
    }
}
