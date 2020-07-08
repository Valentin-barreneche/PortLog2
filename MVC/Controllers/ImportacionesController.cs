using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Dominio;
using Newtonsoft.Json;
using System.Text;
using MVC.ViewModels;
using MVC.Models;

namespace MVC.Controllers
{
    public class ImportacionesController : Controller
    {
        private string url = ConfigurationManager.AppSettings["urlWebAPI"];

        [HttpGet]
        public ActionResult Index()
        {

            if (Session["cedula"] != null)
            {

                ImportacionesVM vm = new ImportacionesVM();

                List<Importacion> lista = FiltroApiImportaciones(url + "/getall");
                vm = new ImportacionesVM(lista);
                return View(vm);
            }
            else
            {
                return Redirect("/home/Index");
            }

        }

        [HttpPost]
        public ActionResult Index(ImportacionesVM vm)
        {

            if (Session["cedula"] != null)
            {
                List<Importacion> importaciones = new List<Importacion>();

                if (vm.Id == 0) //trae todo
                    importaciones = FiltroApiImportaciones(url + "/getall");
                else if (vm.Id == 1) // filtra por codigo
                    importaciones = FiltroApiImportaciones(url + "/getByProdCode/" + vm.DatoFiltro);
                else if (vm.Id == 2) //filtra por rut
                    importaciones = FiltroApiImportaciones(url + "/getByClientRut/" + vm.DatoFiltro);
                else if (vm.Id == 3) //filtra por nombre producto
                    importaciones = FiltroApiImportaciones(url + "/GetByProdName/" + vm.DatoFiltro);
                else if (vm.Id == 4)//filtra los que deberían estar fuera de depósito
                    importaciones = FiltroApiImportaciones(url + "/GetByDate");

                vm.CargarFiltros();
                vm.Importaciones = importaciones;
                if (importaciones == null || importaciones.Count() == 0)
                    ViewBag.Error = "No se obtuvieron datos.";
                return View(vm);
            }
            else
            {
                return Redirect("/home/Index");
            }
        }

        public ActionResult Create()
        {
            if (Session["cedula"] != null)
            {
                return View();
            }
            else
            {
                return Redirect("/home/Index");
            }
        }

        [HttpPost]
        public ActionResult Create(NuevaImportacionViewModel nuevo)
        {
            if (Session["cedula"] != null)
            {
                Importacion nuevaImportacion = new Importacion();
                bool existe = false;

                using (MiContextoContext db = new MiContextoContext())
                {
                    var listaProds = db.Productos
                                 .Join(db.Clientes, prod => prod.Cliente.Id, c => c.Id, (prod, c) => new { prod, c })
                                 .Where(e => e.prod.Codigo == nuevo.CodigoProd).FirstOrDefault();
                    if (listaProds != null)
                    {
                        nuevaImportacion.Producto = listaProds.prod;
                        existe = true;
                    }
                }
                if (existe)
                {
                    nuevaImportacion.Cantidad = nuevo.Cantidad;
                    nuevaImportacion.FechaIngreso = nuevo.FechaIngreso;
                    nuevaImportacion.FechaSalidaPrevista = nuevo.FechaSalidaPrevista;
                    nuevaImportacion.PrecioPorUnidad = nuevo.PrecioPorUnidad;
                    try
                    {
                        Uri uri = new Uri(url + "/Post/");

                        HttpClient cliente = new HttpClient();

                        Task<HttpResponseMessage> tarea = cliente.PostAsJsonAsync(uri, nuevaImportacion);
                        tarea.Wait();

                        if (!tarea.Result.IsSuccessStatusCode)
                        {
                            ViewBag.Error = tarea.Result.StatusCode;
                            return View(nuevo);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    catch
                    {
                        return View(nuevo);
                    }
                }
                else
                {
                    ViewBag.Mensaje = "No existe producto con tal codigo";
                    return View(nuevo);
                }
            }
            else
            {
                return Redirect("/home/Index");
            }
        }

        [HttpGet]
        public ActionResult DarSalida(int id)
        {
            if (Session["rol"].ToString() == "deposito")
            {
                Importacion impor = null;

                Uri uri = new Uri(url + "/Get/" + id);

                HttpClient cliente = new HttpClient();

                Task<HttpResponseMessage> tarea = cliente.GetAsync(uri);
                tarea.Wait();

                if (tarea.Result.IsSuccessStatusCode)
                {
                    Task<string> tarea2 = tarea.Result.Content.ReadAsStringAsync();
                    tarea2.Wait();

                    string json = tarea2.Result;
                    impor = JsonConvert.DeserializeObject<Importacion>(json);
                }
                else
                {
                    ViewBag.Error = tarea.Result.StatusCode;
                }
                return View(impor);
            }
            else
            {
                return Redirect("/home/Index");
            }
        }



        [HttpPost]
        public ActionResult DarSalida(Importacion importacion)
        {
            if (Session["rol"].ToString() == "deposito")
            {
                bool matriculaValida = importacion.ValidarMatricula();
                if (matriculaValida && importacion.Direccion != null)
                {
                    try
                    {
                        Uri uri = new Uri(url + "/Put/" + importacion.Id);

                        HttpClient cliente = new HttpClient();

                        importacion.FechaSalidaReal = DateTime.Now;
                        Int32.TryParse(Session["cedula"].ToString(), out int s);
                        importacion.CiUser = s;

                        Task<HttpResponseMessage> tarea = cliente.PutAsJsonAsync(uri, importacion);
                        tarea.Wait();

                        if (!tarea.Result.IsSuccessStatusCode)
                        {
                            Task<string> tarea2 = tarea.Result.Content.ReadAsStringAsync();
                            tarea2.Wait();

                            return View(importacion);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    catch
                    {
                        return View(importacion);
                    }
                }
                else
                {
                    ViewBag.Mensaje = "La matricula no es valida.";
                    return View(importacion);
                }
            }
            else
            {
                return Redirect("/home/Index");
            }
        }
        
        //METODO QUE SOLICITA A LA API LOS DATOS FILTRADOS
        public List<Importacion> FiltroApiImportaciones(string url)
        {
            List<Importacion> importaciones = new List<Importacion>();

            Uri uri = new Uri(url);

            HttpClient cliente = new HttpClient();

            Task<HttpResponseMessage> tarea = cliente.GetAsync(uri);
            tarea.Wait();

            if (tarea.Result.IsSuccessStatusCode)
            {
                Task<string> tarea2 = tarea.Result.Content.ReadAsStringAsync();
                tarea2.Wait();

                string json = tarea2.Result;
                importaciones = JsonConvert.DeserializeObject<List<Importacion>>(json);
            }
            else
            {
                ViewBag.Error = tarea.Result.StatusCode;
            }
            return importaciones;
        }

    }
}