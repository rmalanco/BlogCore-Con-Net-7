using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsuariosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        // GET
        [HttpGet]
        public IActionResult Index()
        {
            // obtener todos los usuarios
            return View(
                _contenedorTrabajo.Usuario.GetAll()
            );

            // obtener todos los usuarios menos el actual
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //return View(
            //    _contenedorTrabajo.Usuario.GetAll(u => u.Id != usuarioActual.Value)
            //);
        }

        // GET - Bloquear
        // [HttpGet]
        // // [ActionName("Bloquear")]
        // public IActionResult Bloquear(string id)
        // {
        //     if (id == null || id.Trim().Length == 0)
        //     {
        //         return NotFound();
        //     }

        //     _contenedorTrabajo.Usuario.BloquearUsuario(id);
        //     return RedirectToAction(nameof(Index));
        // }

        // // GET - Desbloquear
        // [HttpGet]
        // // [ActionName("Desbloquear")]
        // public IActionResult Desbloquear(string id)
        // {
        //     if (id == null || id.Trim().Length == 0)
        //     {
        //         return NotFound();
        //     }

        //     _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
        //     return RedirectToAction(nameof(Index));
        // }

        #region Api llamadas
        // GET - Api - Obtener todos los usuarios
        [HttpGet]
        public IActionResult GetUsuarios()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return Json(new
            {
                data = _contenedorTrabajo.Usuario.GetUsuariosRoles(u => u.Id != usuarioActual.Value)
            });
        }
        // POST - Api - Bloquear usuario
        [HttpPost]
        public IActionResult Bloquear(string id)
        {
            if (id == null || id.Trim().Length == 0)
            {
                return NotFound();
            }

            _contenedorTrabajo.Usuario.BloquearUsuario(id);
            return Json(new { success = true, message = "Operación exitosa." });
        }
        // POST - Api - Desbloquear usuario
        [HttpPost]
        public IActionResult Desbloquear(string id)
        {
            if (id == null || id.Trim().Length == 0)
            {
                return NotFound();
            }

            _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
            return Json(new { success = true, message = "Operación exitosa." });
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