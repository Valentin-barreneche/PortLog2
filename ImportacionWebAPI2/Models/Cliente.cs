using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImportacionWebAPI2
{
    [Table("Clientes")]
    public class Cliente
    {
        public int Id { get; set; }
        [Required]
        public string Rut { get; set; }
        [Required]
        public string Nombre { get; set; }
 
    }
}
