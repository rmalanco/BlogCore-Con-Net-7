using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Inicializador
{
    public class InicializadorDB : IInicializadorDB
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        //Creamos el constructor
        public InicializadorDB(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Inicializar()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }

            if (_db.Roles.Any(ro => ro.Name == ConstantsAuth.AdminRole)) return;

            //Creación de roles
            _roleManager.CreateAsync(new IdentityRole(ConstantsAuth.AdminRole)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(ConstantsAuth.UserRole)).GetAwaiter().GetResult();
            //Creación del usuario inicial
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@localhost.com",
                Email = "admin@localhost.com",
                EmailConfirmed = true,
                Nombre = "Administrador"
            }, "Admin2016$").GetAwaiter().GetResult();
            ApplicationUser? usuario = _db.ApplicationUser.Where(us => us.Email == "admin@localhost.com").FirstOrDefault();
            // if (usuario != null)
            //     _userManager.AddToRoleAsync(usuario, ConstantsAuth.AdminRole).GetAwaiter().GetResult();
            _ = usuario != null ? _userManager.AddToRoleAsync(usuario, ConstantsAuth.AdminRole).GetAwaiter().GetResult() : null;
        }
    }
}