using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ImportacionWebAPI2
{
    public class MiContextoContext : DbContext
    {
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Importacion> importaciones { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        public MiContextoContext() : base("ConexionWebAPI")
        {

        }
    }
}