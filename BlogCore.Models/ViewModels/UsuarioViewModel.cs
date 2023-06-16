using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BlogCore.Models.ViewModels
{
    public class UsuarioViewModel
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdRol { get; set; }
        public IEnumerable<RolViewModel> Roles { get; set; }
        // campo LockoutEnd para saber si el usuario está bloqueado o no
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}