using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LivrariaMHS.Models;
using LivrariaMHS.Models.Attributes;
using System.Diagnostics;
using LivrariaMHS.Data.Repositories;
using System.Linq;

namespace LivrariaMHS.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteRepository _clienteServico;
        private readonly RuaRepository _ruaServico;
        private readonly BairroRepository _bairroServico;
        private readonly CidadeRepository _cidadeServico;

        public ClientesController(ClienteRepository clienteServico, 
            RuaRepository ruaServico, BairroRepository bairroServico, CidadeRepository cidadeServico)
        {
            _clienteServico = clienteServico;
            _ruaServico = ruaServico;
            _bairroServico = bairroServico;
            _cidadeServico = cidadeServico;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _clienteServico.GetAllAsync());
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);

            await VerificarEndereco(cliente);

            await _clienteServico.InsertAsync(cliente);
            TempData["Concluido"] = "Cliente Cadastrado!";
            return RedirectToAction(nameof(Index));
        }

        private async Task VerificarEndereco(Cliente cliente)
        {
            var rua = await _ruaServico.FindFirstAsync(x => x.Nome == cliente.Rua.Nome);
            if (rua == null)
            {
                await _ruaServico.InsertAsync(cliente.Rua);
                cliente.RuaID = (await _ruaServico.LastAsync()).ID;
            }
            else
                cliente.RuaID = rua.ID;

            var bairro = await _bairroServico.FindFirstAsync(x => x.Nome == cliente.Bairro.Nome);
            if (bairro == null)
            {
                await _bairroServico.InsertAsync(cliente.Bairro);
                cliente.BairroID = (await _bairroServico.LastAsync()).ID;
            }
            else
                cliente.BairroID = bairro.ID;

            var cidade = await _cidadeServico.FindFirstAsync(x => x.Nome == cliente.Cidade.Nome && x.Estado == cliente.Cidade.Estado);
            if (cidade == null)
            {
                await _cidadeServico.InsertAsync(cliente.Cidade);
                cliente.CidadeID = (await _cidadeServico.LastAsync()).ID;
            }
            else
                cliente.CidadeID = cidade.ID;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Cliente Inválido!" });

            var cliente = await _clienteServico.FindByIdAsync(x => x.ID == id, "Bairro", "Cidade", "Rua");

            if (cliente == null)
                return RedirectToAction(nameof(Error), new { message = "Cliente não encontrado!" });

            return View(cliente);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Cliente Inválido!" });

            var cliente = await _clienteServico.FindByIdAsync(x => x.ID == id, "Bairro", "Cidade", "Rua");

            if (cliente == null)
                return RedirectToAction(nameof(Error), new { message = "Cliente não encontrado!" });

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (!ModelState.IsValid)
                return View(cliente);
            if (id != cliente.ID)
                return RedirectToAction(nameof(Error), new { message = "O ID Informado não corresponde ao ID do cliente!" });

            try
            {
                await VerificarEndereco(cliente);
                await _clienteServico.UpdateAsync(cliente);
                TempData["Concluido"] = "Cadastro Editado!";
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

            var cliente = await _clienteServico.FindByIdAsync(x => x.ID == id, "Bairro", "Cidade", "Rua");
            if (cliente == null)
                return RedirectToAction(nameof(Error), new { message = "Cliente não encontrado!" });

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _clienteServico.FindByIdAsync(x => x.ID == id, "Rua", "Cidade", "Bairro");
            await _clienteServico.RemoveAsync(cliente);
            TempData["Concluido"] = "Cadastro Apagado!";
            return RedirectToAction(nameof(Index));
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

        [ValidateAntiForgeryToken]
        [Route("/ClientesController/PesquisarClientes")]
        [HttpGet]
        public async Task<IActionResult> PesquisarClientes(string termo)
        {
            if (!string.IsNullOrEmpty(termo))
                return PartialView("_clientPartial", (await _clienteServico.FindAsync(x => x.Nome.Contains(termo, StringComparison.OrdinalIgnoreCase) || x.CPF.Contains(termo, StringComparison.OrdinalIgnoreCase))).OrderBy(x => x.Nome));
            else
                return PartialView("_clientPartial", (await _clienteServico.GetAllAsync()).OrderBy(x => x.Nome));
        }
    }
}
