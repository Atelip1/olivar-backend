using System;
using System.Collections.Generic;

namespace OlivarBackend.Models
{
    public partial class RolPermiso
    {
        public int RolId { get; set; }
        public int PermisoId { get; set; }

        public virtual Role Rol { get; set; } = null!;
        public virtual Permiso Permiso { get; set; } = null!;
    }
}

