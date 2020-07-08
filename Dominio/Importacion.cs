using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace Dominio
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
        public DateTime? FechaSalidaReal { get; set; }  //revisar ?
        public string MatriculaCamion { get; set; }
        public string Direccion { get; set; }
        public int? CiUser { get; set; }                //revisar ?


        public bool ValidarMatricula()
        {
            string matricula = MatriculaCamion;
            if (string.IsNullOrWhiteSpace(matricula))
            {
                return false;
            }
            bool matriculaValida = false;


            string firstThreeChars = matricula.Substring(0,3);
            string lastFourChars = matricula.Substring(3, 4);

            bool firstThreeCharsAreCAPS = IsAllUpper(firstThreeChars);
            bool lastFourCharsAreNumbers = IsDigitsOnly(lastFourChars);

            if (firstThreeCharsAreCAPS && lastFourCharsAreNumbers) {
                matriculaValida = true;
            }

            return matriculaValida;
        }


        private static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private static bool IsAllUpper(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!Char.IsUpper(input[i]))
                    return false;
            }

            return true;
        }


    }
}
