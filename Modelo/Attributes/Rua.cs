using System.ComponentModel.DataAnnotations;

namespace Model.Attributes
{
    public class Rua
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Rua")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "{0} deve ter entre 5 e 70 caracteres!")]
        public string Nome { get; set; }
    }
}
