using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dominio;
using System.Data.Entity;
using MVC.Models;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        static MiContextoContext db = new MiContextoContext();


        [HttpGet]
        public ActionResult Index()
        {
            if (Session["cedula"] != null)
            {
                return Redirect("/home/bienvenido");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Index(string cedula, string password)
        {
            if (cedula.Length != 0 && password.Length != 0)
            {
                Int32.TryParse(cedula, out int ciInt);
                Usuario usuarioIngresado = db.Usuarios.Where(u => u.Cedula == ciInt).FirstOrDefault<Usuario>();
                if (usuarioIngresado != null)
                {
                    if (usuarioIngresado.Contrasenia == password)
                    {
                        Session["cedula"] = usuarioIngresado.Cedula;
                        Session["nombre"] = usuarioIngresado.Nombre;
                        Session["rol"] = usuarioIngresado.Rol;
                        return Redirect("/Home/Bienvenido");
                    }
                    else
                    {
                        ViewBag.mensaje = "La password no es correcta.";
                    }
                }
                else
                {
                    ViewBag.mensaje = "El Usuario no existe.";
                }
            }
            else
            {
                ViewBag.mensaje = "Los campos cedula y password no pueden ser nulos.";
            }

            return View();
        }

        public ActionResult Bienvenido() {
            if (Session["cedula"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/home/index");
            }
        }


        public ActionResult Salir()
        {
            Session["rol"] = null;
            Session["cedula"] = null;
            return RedirectToAction("index");
        }

        public ActionResult Precarga()
        {
            string rutaWeb = HttpRuntime.AppDomainAppPath + @"..";
            bool cargaOk = CargarDatos.InsertarDatos(rutaWeb);
            if (cargaOk)
            {
                ViewBag.Mensaje ="Precarga realizada con exito.";
            }
            else {
                ViewBag.Mensaje ="Precarga realizada con exito.";
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}