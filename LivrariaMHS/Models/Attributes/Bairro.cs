using System.ComponentModel.DataAnnotations;

namespace LivrariaMHS.Models.Attributes
{
    public class Bairro
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Bairro")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [StringLength(70, MinimumLength = 3, ErrorMessage = "{0} deve ter entre 10 e 70 caracteres!")]
        public string Nome { get; set; }
    }
}
