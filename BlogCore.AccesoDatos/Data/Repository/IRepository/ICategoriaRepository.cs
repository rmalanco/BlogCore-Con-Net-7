using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlogCore.Models;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        IEnumerable<SelectListItem> GetListaCategorias();

        void Update(Categoria categoria);

        Categoria ExisteCategoria(string nombreCategoria);
        Categoria ExisteCategoriaOrden(int ordenCategoria);
        Categoria ExisteCategoriaSinEllaMisma(string nombreCategoria, int idCategoria);
        Categoria ExisteCategoriaOrdenSinEllaMisma(int ordenCategoria, int idCategoria);
    }
}