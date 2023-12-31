using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using BlogCore.Data;
using BlogCore.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using BlogCore.Utilidades;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantsAuth.AdminRole)]
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public CategoriasController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        #region llamadas a la vista
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Categoria());
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var categoria = _contenedorTrabajo.Categoria.Get(id.GetValueOrDefault());
            if (categoria == null)
                return NotFound();

            return View(categoria);
        }

        #endregion

        #region llamadas al controlador
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                var objDesdeDb = _contenedorTrabajo.Categoria.ExisteCategoria(categoria.Nombre);
                if (objDesdeDb != null)
                {
                    ModelState.AddModelError("", "La categoría ya existe");
                    return View(categoria);
                }
                var objDesdeDb2 = _contenedorTrabajo.Categoria.ExisteCategoriaOrden(categoria.Orden);
                if (objDesdeDb2 != null)
                {
                    ModelState.AddModelError("", "El orden ya existe");
                    return View(categoria);
                }
                _contenedorTrabajo.Categoria.Add(categoria);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                var objDesdeDb = _contenedorTrabajo.Categoria.ExisteCategoriaSinEllaMisma(categoria.Nombre, categoria.Id);
                if (objDesdeDb != null)
                {
                    ModelState.AddModelError("", "La categoría ya existe");
                    return View(categoria);
                }
                var objDesdeDb2 = _contenedorTrabajo.Categoria.ExisteCategoriaOrdenSinEllaMisma(categoria.Orden, categoria.Id);
                if (objDesdeDb2 != null)
                {
                    ModelState.AddModelError("", "El orden ya existe");
                    return View(categoria);
                }
                _contenedorTrabajo.Categoria.Update(categoria);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }
        #endregion

        #region llamadas a la api
        // [AllowAnonymous] // para permitir que se pueda acceder a la api sin estar logueado
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Categoria.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objDesdeDb = _contenedorTrabajo.Categoria.Get(id);
            if (objDesdeDb == null)
                return Json(new { success = false, message = "Error borrando categoría" });

            _contenedorTrabajo.Categoria.Remove(objDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Categoría borrada correctamente" });
        }
        #endregion

        #region Lllamada de error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}