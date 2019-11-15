using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LivrariaMHS.Models;
using LivrariaMHS.Models.Attributes;
using LivrariaMHS.Models.Excpetions;
using System.Diagnostics;
using LivrariaMHS.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Http;
using System.IO;
using LivrariaMHS.Data.Repositories;
using System.Net;
using OpenCvSharp;

namespace LivrariaMHS.Controllers
{
    public class LivrosController : Controller
    {
        private readonly LivroRepository _livroServico;
        private readonly AutorRepository _autorServico;
        private readonly CategoriaRepository _categoriaServico;
        private readonly LivroCategoriaRepository _livroCategoriaServico;

        public LivrosController(LivroRepository livroServico, AutorRepository autorServico, CategoriaRepository categoriaServico, LivroCategoriaRepository livroCategoriaServico)
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
        public async Task<IActionResult> Create(Livro livro, int[] categoriasID, IFormFile imagem)
        {
            if (!ModelState.IsValid)
                return View(new LivroViewModel { Livro = livro, Categorias = await _categoriaServico.GetAllAsync()});

            if (categoriasID.Length == 0)
            {
                TempData["CustomError"] = "Informe pelo menos uma categoria!";
                ModelState.AddModelError(string.Empty, TempData["CustomError"].ToString());
                ModelState.AddModelError("livro.LivrosCategorias", "Informe pelo menos uma categoria!");
                return View(new LivroViewModel { Livro = livro, Categorias = await _categoriaServico.GetAllAsync() } );
            }
            await VerificarCadastroAutor(livro);
            ConfigurarImagem(livro, imagem);
            await _livroServico.InsertAsync(livro);
            TempData["Concluido"] = "Livro Cadastrado!";

            int idLivro = (await _livroServico.LastAsync()).ID;
            foreach (var item in categoriasID)
            {
                LivroCategoria livroCategoria = new LivroCategoria() { LivroID = idLivro, CategoriaID = item };
                await _livroCategoriaServico.InsertAsync(livroCategoria);
            }
            return RedirectToAction(nameof(Index));
        }

        private void ConfigurarImagem(Livro livro, IFormFile imagem)
        {
            MemoryStream ms = new MemoryStream();
            if (imagem == null)
            {
                try
                {
                    FileStream Stream = new FileStream(Directory.GetCurrentDirectory() + "\\wwwroot\\img\\indisponivel.png", FileMode.Open, FileAccess.Read);
                    Stream.CopyTo(ms);
                    livro.Nome = "Indisponivel.png";
                    livro.ContentType = "image/png";
                    string teste = Directory.GetCurrentDirectory();
                    Stream.Dispose();
                    livro.Dados = ms.ToArray();
                }
                catch (Exception)
                {
                    return;
                }
            }
            else
            {
                imagem.OpenReadStream().CopyTo(ms);
                Mat mat = Mat.FromImageData(ms.ToArray());
                Cv2.Resize(mat, mat, new Size { Height = 450, Width = 350 });
                livro.Nome = imagem.FileName;
                livro.ContentType = imagem.ContentType;
                livro.Dados = mat.ToBytes();
            }
            
        }

        public async Task<FileContentResult> GetFoto(int id)
        {
            Livro livro = await _livroServico.FindByIdAsync(x => x.ID == id);
            if (livro != null)
            {
                return File(livro.Dados, livro.ContentType);
            }
            return null;
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
        public async Task<IActionResult> Edit(int id, Livro livro, int[] categoriasID, IFormFile imagem)
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
                ConfigurarImagem(livro, imagem);
                await _livroServico.UpdateAsync(livro);
                TempData["Concluido"] = "Livro Editado!";
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
           var livro = await _livroServico.FindByIdAsync(x => x.ID == id, "Autor", "LivrosCategorias");
            try
            {
                await _livroServico.RemoveAsync(livro);
                TempData["Concluido"] = "Livro Apagado!";
                return RedirectToAction(nameof(Index));
            }
            catch (IntegrityException)
            {
                return RedirectToAction(nameof(Error), new { message = "NÃO É POSSÍVEL DELETAR, POIS EXISTEM OUTROS ELEMENTOS DEPENDENTES!" });
            }
        }

        [HttpGet]
        public IActionResult Administrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Administrar(decimal porcentagem, string tipo)
        {
            _livroServico.AlterPrecoLivros(porcentagem, tipo);
            TempData["Concluido"] = "Atualização Efetuada!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
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
