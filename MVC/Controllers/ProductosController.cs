using Dominio;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Collections.Generic;
using MVC.Models;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class ProductosController : Controller
    {
        private MiContextoContext db = new MiContextoContext();

        public ActionResult Index()
        {
            if (Session["cedula"] != null)
            {
                var listaProds1 = db.Productos
                             .Include(p => p.Cliente)
                             .ToList();
                
                List<ProductoViewModel> listaProdsViewModels = new List<ProductoViewModel>();
                foreach (var unProd in listaProds1)
                {
                    ProductoViewModel unProdViewModel = new ProductoViewModel();
                    unProdViewModel.Id = unProd.Id;
                    unProdViewModel.Nombre = unProd.Nombre;
                    unProdViewModel.PesoUnidad = unProd.PesoUnidad;
                    unProdViewModel.Codigo = unProd.Codigo;
                    unProdViewModel.NombreCliente = unProd.Cliente.Nombre;
                    unProdViewModel.RutCliente = unProd.Cliente.Rut;

                    listaProdsViewModels.Add(unProdViewModel);
                }
                return View(listaProdsViewModels);
            }
            else
            {
                return Redirect("/home/Index");
            }
        }

        public ActionResult Details(int? id)
        {
        
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Codigo,Nombre,PesoUnidad")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Codigo,Nombre,PesoUnidad")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
