using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.ViewModels
{
    public class ProductoViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Codigo { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public float PesoUnidad { get; set; }
        public string RutCliente { get; set; }
        public string NombreCliente { get; set; }
    }
}