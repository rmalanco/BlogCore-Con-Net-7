using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BlogCore.Models;
using BlogCore.Models.ViewModels;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IUsuarioRepository: IRepository<ApplicationUser>
    {
        void BloquearUsuario(string IdUsuario);
        void DesbloquearUsuario(string IdUsuario);
        IEnumerable<UsuarioViewModel> GetUsuariosRoles(Expression<Func<UsuarioViewModel, bool>> condition);
    }
}