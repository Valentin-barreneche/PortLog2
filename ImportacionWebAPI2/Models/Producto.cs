//CLASE PRODUCTO
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImportacionWebAPI2
{
    [Table("Productos")]
    public class Producto
    {
        public int Id { get; set; }
        [Required]
        public string Codigo { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public float PesoUnidad { get; set; }
        public Cliente Cliente { get; set; }

    }
}