using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LivrariaMHS.Data.Repositories;
using LivrariaMHS.Models;
using LivrariaMHS.Models.Attributes;
using LivrariaMHS.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LivrariaMHS.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CategoriaRepository _categoriaServico;
        private readonly LivroCategoriaRepository _livroCategoriaServico;

        public CategoriasController(CategoriaRepository categoriaServico, LivroCategoriaRepository livroCategoriaServico)
        {
            _categoriaServico = categoriaServico;
            _livroCategoriaServico = livroCategoriaServico;
    }

        public async Task<IActionResult> Index()
        {
            return View( (await _categoriaServico.GetAllAsync()).OrderBy(x => x.Nome));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (!ModelState.IsValid)
                return View(categoria);

            if(await VerificarCriterioUnico(categoria))
                return View(categoria);


            await _categoriaServico.InsertAsync(categoria);
            return RedirectToAction(nameof(Index));
        }

        public async Task<bool> VerificarCriterioUnico(Categoria categoria)
        {
            if(await _categoriaServico.ExistAsync(x => x.Nome == categoria.Nome))
            {
                TempData["CustomError"] = "Já existe uma categoria cadastrada com o nome informado!";
                ModelState.AddModelError(string.Empty, TempData["CustomError"].ToString());
                return true;
            }
            return false;
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Categoria Inválida!" });
            }
            var categoria = await _categoriaServico.FindByIdAsync(x => x.ID == id);
            if (categoria == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Categoria não encontrada!" });
            }
            var livrosCategorias = await _livroCategoriaServico.FindAsync(x => x.CategoriaID == categoria.ID, "Livro", "Livro.Autor");
            return View(new CategoriaViewModel { Categoria = categoria, LivrosCategorias = livrosCategorias});
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "Categoria Inválida!" });

            var categoria = await _categoriaServico.FindByIdAsync(x => x.ID == id);

            if (categoria == null)
                return RedirectToAction(nameof(Error), new { message = "Categoria não encontrada!" });

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Categoria categoria)
        {
            if (!ModelState.IsValid)
                return View(categoria);

            if (id != categoria.ID)
                return RedirectToAction(nameof(Error), new { message = "O ID Informado não corresponde ao ID dacategoria!" });

           
            if (await VerificarEdicao(id, categoria))
                return View(categoria);

            try
            {
                await _categoriaServico.UpdateAsync(categoria);
                return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException erro)
            {
                return RedirectToAction(nameof(Error), new { message = erro.Message });
            }
        }

        public async Task<bool> VerificarEdicao(int id, Categoria categoria)
        {
            
            if (await _categoriaServico.ExistAsync(x => x.Nome == categoria.Nome && x.ID != id))
            {
                TempData["CustomError"] = "Já existe uma categoria cadastrada com o nome informado!";
                ModelState.AddModelError(string.Empty, TempData["CustomError"].ToString());
                return true;
            }
            
            return false;
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return RedirectToAction(nameof(Error), new { message = "ID Inválido!" });

            var categoria = await _categoriaServico.FindByIdAsync(x => x.ID == id);

            if (categoria == null)
                return RedirectToAction(nameof(Error), new { message = "Categoria não encontrada!" });

            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoria = await _categoriaServico.FindFirstAsync(x => x.ID == id);
            await _categoriaServico.RemoveAsync(categoria);
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
    }
}