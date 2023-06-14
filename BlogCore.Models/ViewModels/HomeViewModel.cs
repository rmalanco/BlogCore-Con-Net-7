using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCore.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Slider> Slider { get; set; }
        public IEnumerable<Articulo> ListaArticulos { get; set; }
        public IEnumerable<Categoria> ListaCategorias { get; set; }
    }
}