using Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.Attributes
{
    public class Cliente
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [StringLength(70, MinimumLength = 10, ErrorMessage = "{0} deve ter entre 10 e 70 caracteres!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [EnumDataType(typeof(Sexo))]
        public Sexo Sexo { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [EmailAddress(ErrorMessage = "Email inválido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "{0} inválido!")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [DataType(DataType.Date, ErrorMessage = "Data inválida!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Celular")]
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [Phone(ErrorMessage = "Telefone Inválido!")]
        [StringLength(14, MinimumLength = 13, ErrorMessage = "Numero inváido!")]
        public string Telefone { get; set; }

        // ENDEREÇO
        [Required(ErrorMessage = "Preenchimento obrigatório!")]
        [StringLength(8, MinimumLength = 2, ErrorMessage = "{0} inválido!")]
        public string Numero { get; set; }

        public int RuaID { get; set; }
        public virtual Rua Rua { get; set; }

        public int BairroID { get; set; }
        public virtual Bairro Bairro { get; set; }

        public int CidadeID { get; set; }
        public virtual Cidade Cidade { get; set; }

        public virtual List<Venda> Vendas { get; set; }
    }
}
