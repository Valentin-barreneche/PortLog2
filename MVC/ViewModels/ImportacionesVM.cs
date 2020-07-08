using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Dominio;
using MVC.Models;

namespace MVC.ViewModels
{
    public class ImportacionesVM
    {
        [Display(Name = "Filtros")]
        public SelectList Filtros { get; set; }

        public string DatoFiltro { get; set; }

        public int Id { get; set; }

        public List<Importacion> Importaciones { get; set; }

        public ImportacionesVM(List<Importacion> import)
        {
            CargarFiltros();
            this.Importaciones = import;
        }

        public ImportacionesVM() { }

        public void CargarFiltros()
        {
            List<Filtro> lista = new List<Filtro>()
            {
                new Filtro(){  Id = 0, Valor= ""},
                new Filtro(){  Id = 1, Valor= "Código"},
                new Filtro(){  Id = 2, Valor= "Rut"},
                new Filtro(){  Id = 3, Valor= "Nombre producto"},
                new Filtro(){  Id = 4, Valor= "Pendientes de salida"},
            };

            this.Filtros = new SelectList(lista, "Id", "Valor");
        }
    }
}