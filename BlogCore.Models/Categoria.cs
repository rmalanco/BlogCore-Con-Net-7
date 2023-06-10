using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [Display(Name = "Nombre de la categoría")]
        [StringLength(50, ErrorMessage = "El {0} debe ser al menos {2} y máximo {1} caracteres", MinimumLength = 3)]
        public string Nombre { get; set; }

        [Display(Name = "Orden de visualización")]
        [Range(1, 100, ErrorMessage = "El {0} debe ser entre {1} y {2}")]
        public int Orden { get; set; }
    }
}