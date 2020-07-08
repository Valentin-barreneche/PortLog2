using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImportacionWebAPI2
{
    [Table("Importaciones")]
    public class Importacion
    {
        public int Id { get; set; }
        [Required]
        public DateTime FechaIngreso { get; set; }
        [Required]
        public DateTime FechaSalidaPrevista { get; set; }
        public Producto Producto { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal PrecioPorUnidad { get; set; }
        public DateTime? FechaSalidaReal { get; set; }  
        public string MatriculaCamion { get; set; }
        public string Direccion { get; set; }
        public int? CiUser { get; set; }                

    }
}
