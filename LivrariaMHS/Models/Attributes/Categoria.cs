using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LivrariaMHS.Models.Attributes
{
    public class Categoria
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "{0} deve ter entre 4 e 50 caracteres!")]
        public string Nome { get; set; }

        public virtual List<LivroCategoria> LivrosCategorias { get; set; }
    }
}
