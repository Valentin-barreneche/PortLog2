using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Dominio;
using System.Data.Entity;

namespace MVC.Models
{
    public class CargarDatos
    {
        static MiContextoContext db = new MiContextoContext();

        public static bool InsertarDatos(string rutaWeb)
        {
            bool exito = true;

            CargarUsuarios(rutaWeb);
            CargarClientes(rutaWeb);
            CargarProductos(rutaWeb);
            CargarImportaciones(rutaWeb);

            return exito;

        }

        public static void CargarUsuarios(string rutaWeb)
        {
            string rutaRelativa = rutaWeb + @"\MVC\Carga\";
            StreamReader sr = new StreamReader(rutaRelativa + "Usuarios.txt");

            string linea = sr.ReadLine();

            while (linea !=null)
            {
                string[] textoLinea = linea.Split(new char[] { '#' });
                //cargar datos en objeto
                Int32.TryParse(textoLinea[0], out int ci);

                Usuario unUsr = new Usuario()
                {
                    Cedula = ci,
                    Contrasenia = textoLinea[1],
                    Nombre = textoLinea[2],
                    Rol = textoLinea[3]
                };
                
                var query = db.Usuarios
                                       .Where(u => u.Cedula == unUsr.Cedula)
                                       .FirstOrDefault<Usuario>();

                if (query == null)
                {
                    //guardar en bd
                    db.Usuarios.Add(unUsr);
                    db.SaveChanges();
                }
                linea = sr.ReadLine(); //mueve a la siguiente linea
            }
            sr.Close();
        }

        public static void CargarClientes(string rutaWeb)
        {
            string rutaRelativa = rutaWeb + @"\MVC\Carga\";
            StreamReader sr = new StreamReader(rutaRelativa + "Clientes.txt");

            string linea = sr.ReadLine();

            while (linea != null)
            {
                string[] textoLinea = linea.Split(new char[] { '#' });

                Cliente unCli = new Cliente ()
                {
                    Rut = textoLinea[0],
                    Nombre = textoLinea[1],
                };

                var query = db.Clientes
                                .Where(c => c.Rut == unCli.Rut)
                                .FirstOrDefault<Cliente>();

                if (query == null)
                {
                    //guardar en bd
                    db.Clientes.Add(unCli);
                    db.SaveChanges();
                }

                linea = sr.ReadLine(); //mueve a la siguiente linea
            }
            sr.Close();
        }

        public static void CargarProductos(string rutaWeb)
        {
            string rutaRelativa = rutaWeb + @"\MVC\Carga\";
            StreamReader sr = new StreamReader(rutaRelativa + "Productos.txt");

            string linea = sr.ReadLine();

            while (linea != null)
            {
                string[] textoLinea = linea.Split(new char[] { '#' });
                //cargar datos en objeto
                string rut = textoLinea[3];

                var queryIdCliente = db.Clientes
                                       .Where(c => c.Rut == rut)
                                       .FirstOrDefault<Cliente>();

                Producto unProd = new Producto()
                {
                    Codigo = textoLinea[0],
                    Nombre = textoLinea[1],
                    PesoUnidad = float.Parse(textoLinea[2]),
                    Cliente = queryIdCliente
                };

                var query = db.Productos
                                       .Where(prod => prod.Codigo == unProd.Codigo)
                                       .FirstOrDefault<Producto>();

                if (query == null)
                {
                    //guardar en bd
                    db.Productos.Add(unProd);
                    db.SaveChanges();
                }

                linea = sr.ReadLine(); //mueve a la siguiente linea
            }
            sr.Close();
        }

        
        public static void CargarImportaciones(string rutaWeb)
        {
            string rutaRelativa = rutaWeb + @"\MVC\Carga\";
            StreamReader sr = new StreamReader(rutaRelativa + "Importacion.txt");

            string linea = sr.ReadLine();

            while (linea != null)
            {
                string[] textoLinea = linea.Split(new char[] { '#' });

                string codigo = textoLinea[5];
                //traigo el producto
                var queryProd = db.Productos
                                       .Where(prod => prod.Codigo == codigo)
                                       .FirstOrDefault<Producto>();
                Int32.TryParse(textoLinea[1], out int cant);

                Importacion unaImp = new Importacion()
                {
                    Cantidad = cant,
                    PrecioPorUnidad = decimal.Parse(textoLinea[2]),
                    FechaIngreso = DateTime.Parse(textoLinea[3]),
                    FechaSalidaPrevista = DateTime.Parse(textoLinea[4]),
                    Producto = queryProd
                };

                var queryImp = db.importaciones
                                        .Where(i => i.Cantidad == unaImp.Cantidad)
                                        .Where(i => i.PrecioPorUnidad == unaImp.PrecioPorUnidad)
                                        .Where(i => i.FechaIngreso == unaImp.FechaIngreso)
                                        .Where(i => i.FechaSalidaPrevista == unaImp.FechaSalidaPrevista)
                                        .Where(i => i.Producto.Codigo == unaImp.Producto.Codigo)
                                        .FirstOrDefault<Importacion>();

                if(queryImp == null)
                {
                    db.importaciones.Add(unaImp);
                    db.SaveChanges();
                }

                linea = sr.ReadLine(); //mueve a la siguiente linea
            }
            sr.Close();
        }
    }
}