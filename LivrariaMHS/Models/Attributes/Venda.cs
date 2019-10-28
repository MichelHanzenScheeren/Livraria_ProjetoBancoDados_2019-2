using LivrariaMHS.Models.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LivrariaMHS.Models.Attributes
{
    public class Venda
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [DataType(DataType.DateTime, ErrorMessage = "Data inválida!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Data { get; set; }

        [ValidarNumero(1)]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        public int Quantidade { get; set; }

        [Display(Name = "Valor Unitário (R$)")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        [ValidarNumero(0)]
        public decimal ValorUnitario { get; set; }

        public int ClienteID { get; set; }
        public virtual Cliente Cliente { get; set; }

        public int LivroID { get; set; }
        public virtual Livro Livro { get; set; }
    }
}
