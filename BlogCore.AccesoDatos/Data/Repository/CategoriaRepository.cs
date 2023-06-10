using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Categoria ExisteCategoria(string nombreCategoria)
        {
            return _db.Categoria.FirstOrDefault(s => s.Nombre.ToLower().Trim() == nombreCategoria.ToLower().Trim());
        }

        public Categoria ExisteCategoriaOrden(int ordenCategoria)
        {
            return _db.Categoria.FirstOrDefault(s => s.Orden == ordenCategoria);
        }

        public Categoria ExisteCategoriaOrdenSinEllaMisma(int ordenCategoria, int idCategoria)
        {
            return _db.Categoria.FirstOrDefault(s => s.Orden == ordenCategoria && s.Id != idCategoria);
        }

        public Categoria ExisteCategoriaSinEllaMisma(string nombreCategoria, int idCategoria)
        {
            return _db.Categoria.FirstOrDefault(s => s.Nombre.ToLower().Trim() == nombreCategoria.ToLower().Trim() && s.Id != idCategoria);
        }

        public IEnumerable<SelectListItem> GetListaCategorias()
        {
            return _db.Categoria.Select(i => new SelectListItem()
            {
                Text = i.Nombre,
                Value = i.Id.ToString()
            });
        }

        public void Update(Categoria categoria)
        {
            var objDesdeDb = _db.Categoria.FirstOrDefault(s => s.Id == categoria.Id);
            objDesdeDb.Nombre = categoria.Nombre;
            objDesdeDb.Orden = categoria.Orden;
            _db.SaveChanges();
        }
    }
}