using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LivrariaMHS.Models.Attributes
{
    public class LivroCategoria
    {
        [Key]
        public int ID { get; set; }

        public int LivroID { get; set; }
        public int CategoriaID { get; set; }

        public virtual Categoria Categoria { get; set; }
        public virtual Livro Livro { get; set; }
    }
}
