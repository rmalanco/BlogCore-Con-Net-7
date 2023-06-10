using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Articulo
    {
        // Propiedades
        // Campo Id
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Nombre del Articulo")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres")]
        public string Nombre { get; set; }
        // Campo Descripcion
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Descripcion del Articulo")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres")]
        public string Descripcion { get; set; }
        // Campo Fecha Creacion
        [Display(Name = "Fecha de Creacion")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm tt}", ApplyFormatInEditMode = false)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        // Campo Url Imagen
        [DataType(DataType.ImageUrl)]
        [Display(Name = "Imagen")]
        public string UrlImagen { get; set; }
        // Relacion con Categoria
        [Required]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public Categoria Categoria { get; set; }
    }
}