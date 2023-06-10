using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class ContenedorTrabajo : IContenedorTrabajo
    {
        public ICategoriaRepository Categoria { get; private set; }
        public IArticuloRepository Articulo { get; private set; }
        // public ISliderRepository Slider { get; private set; }
        // public IUsuarioRepository Usuario { get; private set; }
        // public IComentarioRepository Comentario { get; private set; }
        // public ICarritoCompraRepository CarritoCompra { get; private set; }
        // public IOrdenCompraRepository OrdenCompra { get; private set; }

        private readonly ApplicationDbContext _db;

        public ContenedorTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Categoria = new CategoriaRepository(_db);
            Articulo = new ArticuloRepository(_db);
            // Slider = new SliderRepository(_db);
            // Usuario = new UsuarioRepository(_db);
            // Comentario = new ComentarioRepository(_db);
            // CarritoCompra = new CarritoCompraRepository(_db);
            // OrdenCompra = new OrdenCompraRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}