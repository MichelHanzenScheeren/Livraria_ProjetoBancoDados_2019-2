using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using LivrariaMHS.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LivrariaMHS.Controllers
{
    [Route("api/autoComplete")]
    public class AutoCompletAPIController : Controller
    {
        private readonly CategoriaRepository _categoriaServico;
        private readonly AutorRepository _autorServico;

        public AutoCompletAPIController(CategoriaRepository categoriaServico, AutorRepository autorServico)
        {
            _categoriaServico = categoriaServico;
            _autorServico = autorServico;
        }

        [Produces("application/json")]
        [HttpGet("searchCategorias")]
        public async Task<IActionResult> searchCategorias()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = (await _categoriaServico.FindAsync(p => p.Nome.Contains(term, StringComparison.OrdinalIgnoreCase))).Select(x => x.Nome).ToArray();
                return Json(names);
            }
            catch
            {
                return BadRequest();
            }
        }

        [Produces("application/json")]
        [HttpGet("searchAutores")]
        public async Task<IActionResult> searchAutores()
        {
            try
            {
                string term = HttpContext.Request.Query["term"].ToString();
                var names = (await _autorServico.FindAsync(p => p.Nome.Contains(term, StringComparison.OrdinalIgnoreCase))).Select(x => x.Nome).ToArray();
                return Json(names);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
