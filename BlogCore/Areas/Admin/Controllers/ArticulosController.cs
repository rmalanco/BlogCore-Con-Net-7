using System.Diagnostics;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = ConstantsAuth.AdminRole)]
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ArticulosController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostEnvironment = hostEnvironment;
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
            ArticuloViewModel articuloViewModel = new()
            {
                Articulo = new Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };
            return View(articuloViewModel);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloViewModel articuloViewModel = new()
            {
                Articulo = new Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };
            if (id != null)
            {
                articuloViewModel.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
            }
            return View(articuloViewModel);
        }

        #endregion

        #region llamadas al controlador
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ArticuloViewModel articuloViewModel)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                if (archivos.Count > 0) // Verifica si hay archivos adjuntos
                {
                    if (articuloViewModel.Articulo.Id == 0)
                    {
                        // Nuevo Articulo
                        string nombreArchivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                        var extension = Path.GetExtension(archivos[0].FileName);
                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                        {
                            archivos[0].CopyTo(fileStreams);
                        }
                        articuloViewModel.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                    }
                }
                else
                {
                    articuloViewModel.Articulo.UrlImagen = @"\imagenes\articulos\default.png";
                }

                articuloViewModel.Articulo.FechaCreacion = DateTime.Now;
                _contenedorTrabajo.Articulo.Add(articuloViewModel.Articulo);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            articuloViewModel.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(articuloViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticuloViewModel articuloViewModel)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;
                var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(articuloViewModel.Articulo.Id);
                if (archivos.Count > 0)
                {
                    // Editar Imagen
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);
                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));
                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + nuevaExtension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }
                    articuloViewModel.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + nuevaExtension;
                    _contenedorTrabajo.Articulo.Update(articuloViewModel.Articulo);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    articuloViewModel.Articulo.UrlImagen = articuloDesdeDb.UrlImagen;
                }
                _contenedorTrabajo.Articulo.Update(articuloViewModel.Articulo);
                _contenedorTrabajo.Save();
                return RedirectToAction(nameof(Index));
            }
            articuloViewModel.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(articuloViewModel);
        }
        #endregion

        #region llamadas a la api
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new
            {
                data = _contenedorTrabajo.Articulo.GetAll(
                includeProperties: "Categoria"
                )
            });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(id);
            string rutaDirectorioPrincipal = _hostEnvironment.WebRootPath;
            // comprobamos que la imagen no sea la imagen por defecto "default.png"
            if (articuloDesdeDb.UrlImagen != @"\imagenes\articulos\default.png")
            {
                // Eliminamos la imagen
                var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloDesdeDb.UrlImagen.TrimStart('\\'));
                if (System.IO.File.Exists(rutaImagen))
                {
                    System.IO.File.Delete(rutaImagen);
                }
            }
            if (articuloDesdeDb == null)
            {
                return Json(new { success = false, message = "Error al borrar el articulo" });
            }
            _contenedorTrabajo.Articulo.Remove(articuloDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Articulo borrado correctamente" });
        }
        #endregion

        #region Llamada de error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}