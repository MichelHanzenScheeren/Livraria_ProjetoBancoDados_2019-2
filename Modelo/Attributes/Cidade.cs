using System.ComponentModel.DataAnnotations;

namespace Model.Attributes
{
    public class Cidade
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [StringLength(70, MinimumLength = 3, ErrorMessage = "{0} deve ter entre 10 e 70 caracteres!")]
        public string Nome { get; set; }

        [Display(Name = "Estado (sigla)")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "{0} inválido!")]
        public string Estado { get; set; }
    }
}
