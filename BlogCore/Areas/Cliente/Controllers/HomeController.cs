using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlogCore.Models;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models.ViewModels;

namespace BlogCore.Areas.Cliente.Controllers;

[Area("Cliente")]
public class HomeController : Controller
{
    private readonly IContenedorTrabajo _contenedorTrabajo;

    public HomeController(IContenedorTrabajo contenedorTrabajo)
    {
        _contenedorTrabajo = contenedorTrabajo;
    }
    #region Vistas principales
    public IActionResult Index()
    {
        HomeViewModel homeViewModel = new()
        {
            Slider = _contenedorTrabajo.Slider.GetAll(),
            ListaArticulos = _contenedorTrabajo.Articulo.GetAll(),
            ListaCategorias = _contenedorTrabajo.Categoria.GetAll()
        };
        return View(homeViewModel);
    }

    public IActionResult Details(int? id)
    {
        if (id == null || id == 0)
            return RedirectToAction("Index", "Home");
        var articulo = _contenedorTrabajo.Articulo.GetFirstOrDefault(a => a.Id == id, includeProperties: "Categoria");
        if (articulo == null)
            return RedirectToAction("Index", "Home");
        return View(articulo);
    }

    #endregion

    #region Api llamadas
    [HttpGet]
    public IActionResult GetArticulos()
    {
        return Json(new
        {
            data = _contenedorTrabajo.Articulo.GetAll(
            includeProperties: "Categoria"
        )
        });
    }

    [HttpPost]
    public IActionResult GetArticuloByCategoria(int id)
    {
        return Json(new
        {
            data = _contenedorTrabajo.Articulo.GetAll(
                a => a.CategoriaId == id,
                includeProperties: "Categoria"
            )
        });
    }
    #endregion

    #region Error
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    #endregion
}
