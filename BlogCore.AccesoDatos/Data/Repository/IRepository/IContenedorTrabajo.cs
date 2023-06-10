using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IContenedorTrabajo : IDisposable
    {
        ICategoriaRepository Categoria { get; }
        IArticuloRepository Articulo { get; }
        // ISliderRepository Slider { get; }
        // IUsuarioRepository Usuario { get; }
        // IComentarioRepository Comentario { get; }
        // ICarritoCompraRepository CarritoCompra { get; }
        // IOrdenCompraRepository OrdenCompra { get; }
        void Save();
    }
}