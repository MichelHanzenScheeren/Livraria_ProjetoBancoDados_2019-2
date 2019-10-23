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
using LivrariaMHS.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LivrariaMHS.Controllers
{
    public class LivrosController : Controller
    {
        private readonly LivroServico _livroServico;
        private readonly AutorServico _autorServico;
        private readonly CategoriaServico _categoriaServico;
        private readonly LivroCategoriaServico _livroCategoriaServico;

        public LivrosController(LivroServico livroServico, AutorServico autorServico, CategoriaServico categoriaServico, LivroCategoriaServico livroCategoriaServico)
        {
            _livroServico = livroServico;
            _autorServico = autorServico;
            _categoriaServico = categoriaServico;
            _livroCategoriaServico = livroCategoriaServico;
        }

        public async Task<IActionResult> Index()
        {
            return View((await _livroServico.GetAllAsync("Autor")).OrderBy(x => x.Titulo));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string pesquisa)
        {
            if (!string.IsNullOrEmpty(pesquisa))
            {
                ViewData["pesquisa"] = pesquisa;
                return View(await _livroServico.FindAsync(x => x.Titulo.Contains(pesquisa, StringComparison.OrdinalIgnoreCase) || x.Autor.Nome.Contains(pesquisa, StringComparison.OrdinalIgnoreCase), "Autor"));
            }
            else
                return View((await _livroServico.GetAllAsync("Autor")).OrderBy(x => x.Titulo));
        }

        public async Task<IActionResult> Create()
        {
            return View(new LivroViewModel { Categorias = await _categoriaServico.GetAllAsync() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Livro livro, int[] categoriasID)
        {
            if (!ModelState.IsValid)
                return View(new LivroViewModel { Livro = livro, Categorias = await _categoriaServico.GetAllAsync()});

            if (categoriasID.Length == 0)
            {
                TempData["CustomError"] = "Informe pelo menos uma categoria!";
                ModelState.AddModelError(string.Empty, TempData["CustomError"].ToString());
                return View(new LivroViewModel { Livro = livro, Categorias = await _categoriaServico.GetAllAsync() } );
            }

            await VerificarCadastroAutor(livro);
            await _livroServico.InsertAsync(livro);

            int idLivro = (await _livroServico.LastAsync()).ID;
            foreach (var item in categoriasID)
            {
                LivroCategoria livroCategoria = new LivroCategoria() { LivroID = idLivro, CategoriaID = item };
                await _livroCategoriaServico.InsertAsync(livroCategoria);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task VerificarCadastroAutor(Livro livro)
        {
            Autor autor = await _autorServico.FindFirstAsync(x => x.Nome == livro.Autor.Nome);
            if (autor is null)
            {
                await _autorServico.InsertAsync(livro.Autor);
                autor = await _autorServico.LastAsync();
            }
            livro.AutorID = autor.ID;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Livro Inválido!" });

            var livro = await _livroServico.FindByIdAsync(x => x.ID == id, "Autor", "LivrosCategorias", "LivrosCategorias.Categoria");

            if (livro == null)
                return RedirectToAction(nameof(Error), new { message = "Livro não encontrado!" });

            return View(livro);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Livro Inválido!" });

            var livro = await _livroServico.FindByIdAsync(x => x.ID == id, "Autor", "LivrosCategorias");
            if (livro == null)
                return RedirectToAction(nameof(Error), new { message = "Livro não encontrado!" });

            var categorias = await _categoriaServico.GetAllAsync();
            var selecionados = livro.LivrosCategorias.Select(x => x.CategoriaID);
            ViewBag.categorias = new MultiSelectList(categorias, "ID","Nome", selecionados );

            return View(new LivroViewModel() { Livro = livro, Categorias = categorias });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Livro livro, int[] categoriasID)
        {
            if (!ModelState.IsValid)
                return View(new LivroViewModel { Livro = livro, Categorias = await _categoriaServico.GetAllAsync() });

            if (id != livro.ID)
                return RedirectToAction(nameof(Error), new { message = "O ID Informado não corresponde ao ID do Livro!" });

            if (categoriasID.Length == 0)
            {
                TempData["CustomError"] = "Informe pelo menos uma categoria!";
                ModelState.AddModelError(string.Empty, TempData["CustomError"].ToString());
                var categorias = await _categoriaServico.GetAllAsync();
                ViewBag.categorias = new MultiSelectList(categorias, "ID", "Nome");
                livro.AutorID = Convert.ToInt32(Request.Form["autorAntigoID"]);
                return View(new LivroViewModel { Livro = livro, Categorias = categorias });
            }

            try
            {
                await VerificarAlteracoesCategorias(id, categoriasID);
                await VerificarCadastroAutor(livro);
                await _livroServico.UpdateAsync(livro);
                await ValidarExistenciaAutor(Convert.ToInt32(Request.Form["autorAntigoID"]));
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException erro)
            {
                return RedirectToAction(nameof(Error), new { message = erro.Message });
            }
        }

        private async Task VerificarAlteracoesCategorias(int livroID, int[] categoriasID)
        {
            List<LivroCategoria> livrosCategorias = await _livroCategoriaServico.FindAsync(x => x.LivroID == livroID);
            
            foreach (var item in categoriasID)
            {
                
                if (!(livrosCategorias.Exists(x => x.CategoriaID == item)))
                {
                    LivroCategoria novo = new LivroCategoria() { LivroID = livroID, CategoriaID = item };
                    await _livroCategoriaServico.InsertAsync(novo);
                }
                else
                    livrosCategorias.RemoveAll(x => x.CategoriaID == item);
            }
            foreach (var item in livrosCategorias)
            {
                await _livroCategoriaServico.RemoveAsync(item);
            }
            
        }

        private async Task ValidarExistenciaAutor(int idAutor)
        {
            
            if (!(await _livroServico.ExistAsync(x => x.AutorID == idAutor)))
                await _autorServico.RemoveAsync(await _autorServico.FindByIdAsync(x => x.ID == idAutor)); 
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Livro Inválido!" });

            var livro = await _livroServico.FindByIdAsync(x => x.ID == id, "Autor", "LivrosCategorias", "LivrosCategorias.Categoria");
            if (livro == null)
                return RedirectToAction(nameof(Error), new { message = "Livro não encontrado!" });

            return View(livro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           var livro = await _livroServico.FindByIdAsync(x => x.ID == id);
            try
            {
                await _livroServico.RemoveAsync(livro);
                await ValidarExistenciaAutor(livro.AutorID);
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

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
