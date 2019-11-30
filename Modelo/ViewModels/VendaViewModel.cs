using Model.Attributes;
using System.Collections.Generic;

namespace Model.ViewModels
{
    public class VendaViewModel
    {
        public Venda Venda { get; set; }

        public ICollection<Cliente> Clientes { get; set; }

        public ICollection<Livro> Livros { get; set; }
    }
}
