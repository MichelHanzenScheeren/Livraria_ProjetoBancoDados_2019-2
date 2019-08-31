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
            return View(await _livroServico.GetAllAsync("Autor"));
        }

        public async Task<IActionResult> Create()
        {
            var categorias = await _categoriaServico.GetAllAsync();
            var viewModel = new LivroViewModel { Categorias = categorias };
            return View(viewModel);
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

            livro.AutorID = await VerificarCadastroAutor(livro);
            await _livroServico.InsertAsync(livro);
            int idLivro = (await _livroServico.LastAsync()).ID;

            foreach (var item in categoriasID)
            {
                LivroCategoria livroCategoria = new LivroCategoria() { LivroID = idLivro, CategoriaID = item };
                await _livroCategoriaServico.InsertAsync(livroCategoria);
            }

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
                return View(livro);

            if (id != livro.ID)
                return RedirectToAction(nameof(Error), new { message = "O ID Informado não corresponde ao ID do Livro!" });

            if (categoriasID.Length == 0)
            {
                TempData["CustomError"] = "Informe pelo menos uma categoria!";
                ModelState.AddModelError(string.Empty, TempData["CustomError"].ToString());
                var categorias = await _categoriaServico.GetAllAsync();
                ViewBag.categorias = new MultiSelectList(categorias, "ID", "Nome");
                return View(new LivroViewModel { Livro = livro, Categorias = categorias });
            }

            try
            {
                await VerificarAlteracoesCategorias(id, categoriasID);
                livro.AutorID = await VerificarCadastroAutor(livro);
                await _livroServico.UpdateAsync(livro);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException erro)
            {
                return RedirectToAction(nameof(Error), new { message = erro.Message });
            }
        }

        private async Task VerificarAlteracoesCategorias(int livroID, int[] categoriasID)
        {
            List<LivroCategoria> categorias = await _livroCategoriaServico.FindAsync(x => x.LivroID == livroID);
            
            foreach (var item in categoriasID)
            {
                
                if (!(categorias.Exists(x => x.CategoriaID == item)))
                {
                    LivroCategoria novo = new LivroCategoria() { LivroID = livroID, CategoriaID = item };
                    await _livroCategoriaServico.InsertAsync(novo);
                }
                categorias.RemoveAll(x => x.CategoriaID == item);
            }
            foreach (var item in categorias)
            {
                await _livroCategoriaServico.RemoveAsync(item);
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

            var livro = await _livroServico.FindByIdAsync(x => x.ID == id, "Autor", "LivrosCategorias", "LivrosCategorias.Categoria");
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
