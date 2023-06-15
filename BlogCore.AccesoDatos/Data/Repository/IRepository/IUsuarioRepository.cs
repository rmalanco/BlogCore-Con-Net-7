using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogCore.Models;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IUsuarioRepository: IRepository<ApplicationUser>
    {
        void BloquearUsuario(string IdUsuario);
        void DesbloquearUsuario(string IdUsuario);
    }
}