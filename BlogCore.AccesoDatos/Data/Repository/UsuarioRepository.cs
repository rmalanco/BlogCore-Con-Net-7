using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;
using BlogCore.Models.ViewModels;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class UsuarioRepository : Repository<ApplicationUser>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _db;

        public UsuarioRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void BloquearUsuario(string IdUsuario)
        {
            var usuarioDesdeDb = _db.ApplicationUser.FirstOrDefault(u => u.Id == IdUsuario);
            usuarioDesdeDb.LockoutEnd = DateTime.Now.AddYears(1000);
            _db.SaveChanges();
        }

        public void DesbloquearUsuario(string IdUsuario)
        {
            var usuarioDesdeDb = _db.ApplicationUser.FirstOrDefault(u => u.Id == IdUsuario);
            usuarioDesdeDb.LockoutEnd = DateTime.Now;
            _db.SaveChanges();
        }

        public IEnumerable<UsuarioViewModel> GetUsuariosRoles(Expression<Func<UsuarioViewModel, bool>> condition)
        {
            return _db.ApplicationUser
                    .Select(u => new UsuarioViewModel
                    {
                        Id = u.Id,
                        Nombre = u.Nombre,
                        Email = u.Email,
                        PhoneNumber = u.PhoneNumber,
                        IdRol = _db.UserRoles.FirstOrDefault(ur => ur.UserId == u.Id).RoleId,
                        Roles = _db.Roles
                            .Where(r => r.Id == _db.UserRoles.FirstOrDefault(ur => ur.UserId == u.Id).RoleId)
                            .Select(r => new RolViewModel { Id = r.Id, Nombre = r.Name })
                            .OrderBy(r => r.Nombre)
                            .ToList(),
                        LockoutEnd = u.LockoutEnd
                    })
                    .Where(condition)
                    .ToList();
        }
    }
}