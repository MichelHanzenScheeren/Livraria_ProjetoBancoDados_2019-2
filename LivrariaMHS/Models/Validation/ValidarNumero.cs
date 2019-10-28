using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LivrariaMHS.Models.Validation
{
    public class ValidarNumero : ValidationAttribute
    {
        private int? _min { get; set; }
        private int? _max { get; set; }

        public ValidarNumero() : base("Valor inválido!")
        {
        }

        public ValidarNumero(int min, int max) : base("O valor deve ser um número entre " + min + " e " + max)
        {
            _min = min;
            _max = max;
        }

        public ValidarNumero(int min) : base("O valor deve ser um número maior ou igual a " + min)
        {
            _min = min;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);

                if(!(value is decimal || value is int))
                    return new ValidationResult(errorMessage);

                decimal valor = Convert.ToDecimal(value);

                if (_min.HasValue)
                {
                    if (valor < _min.Value)
                        return new ValidationResult(errorMessage);
                }
                if (_max.HasValue)
                {
                    if(valor > _max.Value)
                        return new ValidationResult(errorMessage);
                }

                return ValidationResult.Success;
            }
            catch (Exception)
            {
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }
        }

    }
}
