using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class Permiso
{
    public int PermisoId { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Role> Rols { get; set; } = new List<Role>();

    public virtual ICollection<RolPermiso> RolPermisos { get; set; } = new List<RolPermiso>();

}
