using System.Diagnostics;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlidersController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostEnvironment;
        public SlidersController(IContenedorTrabajo contenedorTrabajo, IWebHostEnvironment hostEnvironment)
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
            return View(new Slider());
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var slider = _contenedorTrabajo.Slider.Get(id.GetValueOrDefault());
            if (slider == null)
                return NotFound();

            return View(slider);
        }

        #endregion

        #region llamadas al controlador
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Slider slider)
        {
            if (ModelState.IsValid)
            {
                var objDesdeDb = _contenedorTrabajo.Slider.ExisteSlider(slider.Nombre);
                if (objDesdeDb != null)
                {
                    ModelState.AddModelError("", "El slider ya existe");
                    return View(slider);
                }
                else
                {
                    string rutaPrincipal = _hostEnvironment.WebRootPath;
                    var archivos = HttpContext.Request.Form.Files;
                    if (slider.Id == 0)
                    {
                        // Nuevo slider
                        string nombreArchivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                        var extension = Path.GetExtension(archivos[0].FileName);
                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                        {
                            archivos[0].CopyTo(fileStreams);
                        }
                        slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + extension;

                        _contenedorTrabajo.Slider.Add(slider);
                        _contenedorTrabajo.Save();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Slider slider)
        {
            if (ModelState.IsValid)
            {
                var objDesdeDb = _contenedorTrabajo.Slider.ExisteSliderSinEllaMisma(slider.Nombre, slider.Id);
                if (objDesdeDb != null)
                {
                    ModelState.AddModelError("", "Ya existe un slider con ese nombre");
                    return View(slider);
                }
                else
                {
                    string rutaPrincipal = _hostEnvironment.WebRootPath;
                    var archivos = HttpContext.Request.Form.Files;
                    var sliderDesdeDb = _contenedorTrabajo.Slider.Get(slider.Id);
                    if (archivos.Count > 0)
                    {
                        // Editar imagen
                        string nombreArchivo = Guid.NewGuid().ToString();
                        var subidas = Path.Combine(rutaPrincipal, @"imagenes\sliders");
                        var extension = Path.GetExtension(archivos[0].FileName);

                        var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                        var rutaImagen = Path.Combine(rutaPrincipal, sliderDesdeDb.UrlImagen.TrimStart('\\'));

                        if (System.IO.File.Exists(rutaImagen))
                        {
                            System.IO.File.Delete(rutaImagen);
                        }

                        using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + nuevaExtension), FileMode.Create))
                        {
                            archivos[0].CopyTo(fileStreams);
                        }
                        slider.UrlImagen = @"\imagenes\sliders\" + nombreArchivo + nuevaExtension;
                        sliderDesdeDb.UrlImagen = slider.UrlImagen;
                    }

                    sliderDesdeDb.Nombre = slider.Nombre;
                    sliderDesdeDb.Estado = slider.Estado;

                    _contenedorTrabajo.Slider.Update(sliderDesdeDb);
                    _contenedorTrabajo.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(slider);
        }
        #endregion

        #region llamadas a la api
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Slider.GetAll() });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objDesdeDb = _contenedorTrabajo.Slider.Get(id);
            if (objDesdeDb == null)
                return Json(new { success = false, message = "Error borrando slider" });

            string rutaPrincipal = _hostEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaPrincipal, objDesdeDb.UrlImagen.TrimStart('\\'));

            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }

            _contenedorTrabajo.Slider.Remove(objDesdeDb);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Slider borrado correctamente" });
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