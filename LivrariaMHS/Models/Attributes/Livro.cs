using LivrariaMHS.Models.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LivrariaMHS.Models.Attributes
{
    public class Livro
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Título")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [StringLength(70, MinimumLength = 5, ErrorMessage = "{0} deve ter entre 5 e 70 caracteres!")]
        public string Titulo { get; set; }

        [Display(Name = "Páginas")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [ValidarNumero(1)]
        public int Paginas { get; set; }

        [Display(Name = "Preço (R$)")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [ValidarNumero(0)]
        public decimal Preco { get; set; }


        [Display(Name = "Edição")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [ValidarNumero(1)]
        public int Edicao { get; set; }

        [Display(Name = "Lançamento")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [ValidarNumero(1000)]
        public int Ano { get; set; }

        [Required]
        public int AutorID { get; set; }
        public virtual Autor Autor { get; set; }

        public virtual List<LivroCategoria> LivrosCategorias { get; set; }

        // Imagem:
        public string Nome { get; set; }
        public byte[] Dados { get; set; }
        public string ContentType { get; set; }

    }
}
