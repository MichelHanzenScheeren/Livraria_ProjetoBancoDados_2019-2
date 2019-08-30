using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LivrariaMHS.Models;
using LivrariaMHS.Models.Attributes;
using LivrariaMHS.Models.Service;
using LivrariaMHS.Models.Excpetions;
using System.Diagnostics;

namespace LivrariaMHS.Controllers
{
    public class LivrosController : Controller
    {
        private readonly LivroServico _livroServico;
        private readonly AutorServico _autorServico;

        public LivrosController(LivroServico livroServico, AutorServico autorServico)
        {
            _livroServico = livroServico;
            _autorServico = autorServico;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _livroServico.GetAllAsync("Autor"));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Livro livro)
        {
            if (!ModelState.IsValid)
                return View(livro);

            livro.AutorID = await VerificarCadastroAutor(livro);
            await _livroServico.InsertAsync(livro);
            return RedirectToAction(nameof(Index));
        }

        private async Task<int> VerificarCadastroAutor(Livro livro)
        {
            Autor autor = await _autorServico.FindFirstAsync(x => x.Nome == livro.Autor.Nome);
            if (autor is null)
            {
                await _autorServico.InsertAsync(livro.Autor);
                autor = await _autorServico.LastAsync();
            }
            return autor.ID;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Livro Inválido!" });

            var livro = await _livroServico.FindByIdAsync(x => x.ID == id, "Autor");
            if (livro == null)
                return RedirectToAction(nameof(Error), new { message = "Livro não encontrado!" });

            return View(livro);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Livro Inválido!" });

            var livro = await _livroServico.FindByIdAsync(x => x.ID == id, "Autor");
            if (livro == null)
                return RedirectToAction(nameof(Error), new { message = "Livro não encontrado!" });

            return View(livro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Livro livro)
        {
            if (!ModelState.IsValid)
                return View(livro);

            if (id != livro.ID)
                return RedirectToAction(nameof(Error), new { message = "O ID Informado não corresponde ao ID do Livro!" });

            try
            {
                livro.AutorID = await VerificarCadastroAutor(livro);
                await _livroServico.UpdateAsync(livro);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException erro)
            {
                return RedirectToAction(nameof(Error), new { message = erro.Message });
            }
        }

        private async Task ValidarExistenciaAutor(string nome)
        {
            Autor autor = await _autorServico.FindFirstAsync(x => x.Nome == nome);
            if (autor != null)
            {
                if (!(await _livroServico.ExistAsync(x => x.AutorID == autor.ID)))
                    await _autorServico.RemoveAsync(autor);
            }
             
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Livro Inválido!" });

            var livro = await _livroServico.FindByIdAsync(x => x.ID == id, "Autor");
            if (livro == null)
                return RedirectToAction(nameof(Error), new { message = "Livro não encontrado!" });

            return View(livro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           var livro = await _livroServico.FindByIdAsync(x => x.ID == id, "Autor");
            try
            {
                string autor = livro.Autor.Nome;
                await _livroServico.RemoveAsync(livro);
                await ValidarExistenciaAutor(autor);
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException)
            {
                return RedirectToAction(nameof(Error), new { message = "NÃO É POSSÍVEL DELETAR, POIS EXISTEM OUTROS ELEMENTOS DEPENDENTES!" });
            }
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
