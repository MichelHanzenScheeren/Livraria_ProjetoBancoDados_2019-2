using System.ComponentModel.DataAnnotations;

namespace LivrariaMHS.Models.Attributes
{
    public class Categoria
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "{0} deve ter entre 5 e 50 caracteres!")]
        public string Nome { get; set; }
    }
}
