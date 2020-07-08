using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Configuration;

namespace ImportacionWebAPI2.Controllers
{
    [System.Web.Http.RoutePrefix("api/Importacion")]

    public class ImportacionController : ApiController
    {
        public IHttpActionResult GetAll()
        {
            List<Importacion> lstImportacion = new List<Importacion>();

            try
            {
                using (MiContextoContext db = new MiContextoContext())
                {

                    lstImportacion = db.importaciones
                                        .Include(i => i.Producto)
                                        .Include(i => i.Producto.Cliente)
                                        .ToList();
                }
            }
            catch
            {
                return InternalServerError();
            }

            return Ok(lstImportacion);
        }

        [System.Web.Http.HttpGet()]
        [System.Web.Http.Route("getByProdCode/{prodCode}")]
        public IHttpActionResult GetByProdCode(string prodCode)
        {
            List<Importacion> lstImportacion = new List<Importacion>();
            try
            {
                using (MiContextoContext db = new MiContextoContext())
                {
                    lstImportacion = db.importaciones
                                     .Include(i => i.Producto)
                                     .Include(i => i.Producto.Cliente)
                                     .Where(i => i.Producto.Codigo == prodCode)
                                     .ToList();
                }
            }
            catch
            {
                return InternalServerError();
            }
            return Ok(lstImportacion);
        }

        [System.Web.Http.HttpGet()]
        [System.Web.Http.Route("getByClientRut/{clientRut}")]
        public IHttpActionResult GetByClientRut(string clientRut)
        {
            List<Importacion> lstImportacion = new List<Importacion>();
            try
            {
                using (MiContextoContext db = new MiContextoContext())
                {
                    lstImportacion = db.importaciones
                                    .Include(i => i.Producto)
                                    .Include(i => i.Producto.Cliente)
                                    .Where(i => i.Producto.Cliente.Rut == clientRut)
                                    .ToList();
                }
            }
            catch
            {
                return InternalServerError();
            }
            return Ok(lstImportacion);
        }


        [System.Web.Http.HttpGet()]
        [System.Web.Http.Route("getByProdName/{prodName}")]
        public IHttpActionResult GetByProdName(string prodName)
        {
            List<Importacion> lstImportacion = new List<Importacion>();
            try
            {
                using (MiContextoContext db = new MiContextoContext())
                {
                    lstImportacion = db.importaciones
                                    .Include(i => i.Producto)
                                    .Include(i => i.Producto.Cliente)
                                    .Where(i => i.Producto.Nombre.Contains(prodName))
                                    .ToList();
                }
            }
            catch
            {
                return InternalServerError();
            }
            return Ok(lstImportacion);
        }

        [System.Web.Http.HttpGet()]
        [System.Web.Http.Route("getByDate/")]
        public IHttpActionResult GetByDate()
        {
            List<Importacion> lstImportacion = new List<Importacion>();

            try
            {
                using (MiContextoContext db = new MiContextoContext())
                {
                    lstImportacion = db.importaciones
                                    .Include(i => i.Producto)
                                    .Include(i => i.Producto.Cliente)
                                    .Where(i => i.FechaSalidaPrevista < DateTime.Today && i.FechaSalidaReal == null)
                                    .ToList();
                }
            }
            catch
            {
                return InternalServerError();
            }
            return Ok(lstImportacion);
        }

        public IHttpActionResult Get(int id)
        {
            Importacion impor = null;
            try
            {
                using (MiContextoContext db = new MiContextoContext())
                {
                    impor = db.importaciones.Include(i => i.Producto)
                                    .Include(i => i.Producto.Cliente)
                                    .Where(i => i.Id == id)
                                    .FirstOrDefault();
                    if (impor == null) return NotFound();
                }
            }
            catch
            {
                return InternalServerError();
            }

            return Ok(impor);
        }

        public IHttpActionResult Put(int id, Importacion a_modificar)
        {
            if (id != a_modificar.Id) return BadRequest();

            try
            {
                using (MiContextoContext db = new MiContextoContext())
                {
                    int cantidad = db.importaciones.Count(p => p.Id == id);
                    if (cantidad == 0) return NotFound();

                    db.Entry(a_modificar).State = EntityState.Modified;
                    db.SaveChanges();

                    return Ok(a_modificar);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        public IHttpActionResult Post(Importacion nuevo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                using (MiContextoContext db = new MiContextoContext())
                {
                    db.importaciones.Add(nuevo);
                    db.SaveChanges();
                }
            }
            catch
            {
                return InternalServerError();
            }
            return Created("api/productos/" + nuevo.Id, nuevo);
        }
    }
}






