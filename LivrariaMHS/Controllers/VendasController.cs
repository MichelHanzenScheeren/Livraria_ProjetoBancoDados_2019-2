using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Model.Attributes;
using Model.ViewModels;

namespace LivrariaMHS.Controllers
{
    public class VendasController : Controller
    {
        private readonly VendaRepository _vendaServico;
        private readonly ClienteRepository _clienteServico;
        private readonly LivroRepository _livroServico;

        public VendasController(VendaRepository vendaServico, ClienteRepository clienteServico, LivroRepository livroServico)
        {
            _vendaServico = vendaServico;
            _clienteServico = clienteServico;
            _livroServico = livroServico;
        }
        public async Task<IActionResult> Index()
        {
            return View((await _vendaServico.GetAllAsync("Cliente", "Livro")).OrderByDescending(x => x.Data));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string pesquisa, DateTime? data)
        {
            if(string.IsNullOrEmpty(pesquisa) && !(data.HasValue))
                return View((await _vendaServico.GetAllAsync("Cliente", "Livro")).OrderByDescending(x => x.Data));

            List<Venda> filtro = new List<Venda>();
            if (!string.IsNullOrEmpty(pesquisa))
            {
                ViewData["pesquisa"] = pesquisa;
                filtro = await _vendaServico.FindAllAsync(x => x.Livro.Titulo.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) || x.Livro.Autor.Nome.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) || x.Cliente.Nome.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) || x.Cliente.CPF.Contains(pesquisa), "Cliente", "Livro");
            }
            if(data.HasValue)
            {
                ViewData["date"] = data.Value.ToString("yyyy-MM-dd");
                foreach (var item in await _vendaServico.FindAllAsync(x => x.Data.Date == data.Value.Date, "Cliente", "Livro"))
                {
                    filtro.Add(item);
                }
            }
            if (filtro.Any())
                filtro.OrderByDescending(x => x.Data);

            return View(filtro.Distinct().ToList());
        }

        public async Task<IActionResult> Create()
        {
            Venda venda = new Venda() { Data = DateTime.Now };
            return View(new VendaViewModel() { Venda = venda, Livros = (await _livroServico.GetAllAsync()).OrderBy(x => x.Titulo).ToList(), Clientes = (await _clienteServico.GetAllAsync()).OrderBy(x => x.Nome).ToList()});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venda venda)
        {
            if (!ModelState.IsValid)
                return View(new VendaViewModel() { Venda = venda, Livros = (await _livroServico.GetAllAsync()).OrderBy(x => x.Titulo).ToList(), Clientes = (await _clienteServico.GetAllAsync()).OrderBy(x => x.Nome).ToList() });

            var livro = await _livroServico.FindFirstAsync(x => x.ID == venda.LivroID);
            venda.ValorUnitario = livro.Preco;
            await _vendaServico.AddAsync(venda);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Venda Inválida!" });

            var venda = await _vendaServico.FindFirstAsync(x => x.ID == id, "Livro", "Cliente");

            if (venda == null)
                return RedirectToAction(nameof(Error), new { message = "Venda não encontrada!" });

            return View(venda);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Venda Inválida!" });

            var venda = await _vendaServico.FindFirstAsync(x => x.ID == id, "Livro", "Cliente");

            if (venda == null)
                return RedirectToAction(nameof(Error), new { message = "Venda não encontrada!" });

            return View(new VendaViewModel() { Venda = venda, Livros = (await _livroServico.GetAllAsync()).OrderBy(x => x.Titulo).ToList(), Clientes = (await _clienteServico.GetAllAsync()).OrderBy(x => x.Nome).ToList() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Venda venda)
        {
            if (!ModelState.IsValid)
                return View(new VendaViewModel() { Venda = venda, Livros = (await _livroServico.GetAllAsync()).OrderBy(x => x.Titulo).ToList(), Clientes = (await _clienteServico.GetAllAsync()).OrderBy(x => x.Nome).ToList() });

            if (id != venda.ID)
                return RedirectToAction(nameof(Error), new { message = "O ID Informado não corresponde ao ID  da uma Venda!" });

            try
            {
                await _vendaServico.UpdateAsync(venda);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException erro)
            {
                return RedirectToAction(nameof(Error), new { message = erro.Message });
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "ID Inválido!" });

            var venda = await _vendaServico.FindFirstAsync(x => x.ID == id, "Livro", "Cliente");

            if (venda == null)
                return RedirectToAction(nameof(Error), new { message = "Venda não encontrada!" });

            return View(venda);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _vendaServico.FindFirstAsync(x => x.ID == id);
            await _vendaServico.RemoveAsync(categoria);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Informacoes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Informacoes(string tipo, DateTime dataInicio, DateTime dataFim)
        {
            ViewData["tipo"] = tipo;
            ViewData["dataInicio"] = dataInicio.ToString("yyyy-MM-dd");
            ViewData["dataFim"] = dataFim.ToString("yyyy-MM-dd");
            if (tipo == "media")
            {
                var resultado = _vendaServico.ValorMedioDasVendas(dataInicio, dataFim);
                if (resultado == null)
                    ViewData["resultado"] = 0;
                else
                    ViewData["resultado"] = resultado;
            }
            else
            {
                var resultado = _vendaServico.ValorTotalDasVendas(dataInicio, dataFim);
                if (resultado == null)
                    ViewData["resultado"] = 0;
                else
                    ViewData["resultado"] = resultado;
            }
            return View();
        }

        public IActionResult Error(string message)
        {
            var viewModelError = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModelError);
        }
    }
}