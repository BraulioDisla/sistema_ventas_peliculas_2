using sistema_ventas_peliculas_2.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sistema_ventas_peliculas_2.Controllers
{
    public class CompraController : Controller
    {
        // GET: Compra
        public ActionResult Index()
        {
            return View();
        }
      
        // GET: Compra/Create/13
        [HttpGet]
        public ActionResult Create(int id)
        {
            string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
            Compras compra = new Compras(); // Inicializa un nuevo objeto de Compras

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Obtener información de la película basada en el ID
                    string getPeliculaSql = "SELECT * FROM Peliculas WHERE IdPeliculas = @IdPeliculas";
                    using (SqlCommand getPeliculaCommand = new SqlCommand(getPeliculaSql, connection))
                    {
                        getPeliculaCommand.Parameters.AddWithValue("@IdPeliculas", id);
                        using (SqlDataReader reader = getPeliculaCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Aquí podrías rellenar propiedades específicas de 'compra' si las tienes.
                                compra.IdPeliculas = id; // Establece el ID de la película
                                                         // Si tienes propiedades adicionales en 'Peliculas', como Nombre, Precio, etc., puedes asignarlas aquí
                            }
                            else
                            {
                                // Si no se encuentra la película, puedes redirigir o mostrar un mensaje de error
                                TempData["Message"] = "La película no existe.";
                                return RedirectToAction("Index", "Pelicula");
                            }
                        }
                    }
                }

                return View(compra); // Retorna la vista con el modelo 'compra'
            }
            catch (Exception ex)
            {
                // Manejar la excepción o registrar el error
                TempData["Message"] = "Ocurrió un error al cargar los datos de la película: " + ex.Message;
                return RedirectToAction("Index", "Pelicula"); // Redirigir a la acción Index en caso de error
            }
        }

        [HttpPost]
        public ActionResult Create(Compras compra)
        {
            string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";

            // Verificar si la cantidad comprada es válida
            if (compra.CantidadComprada <= 0)
            {
                TempData["Message"] = "No se puede seleccionar la película porque la cantidad comprada es cero.";
                TempData["MessageType"] = "warning";  // Mensaje de advertencia
                return RedirectToAction("Index", "Pelicula");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Verificar si hay suficiente cantidad disponible en el almacén
                    string checkQuantitySql = "SELECT CantidadDisponible FROM Almacen WHERE IdPeliculas = @IdPeliculas";
                    int cantidadDisponible = 0;

                    using (SqlCommand checkCommand = new SqlCommand(checkQuantitySql, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@IdPeliculas", compra.IdPeliculas);
                        cantidadDisponible = (int)checkCommand.ExecuteScalar();
                    }

                    if (cantidadDisponible <= 0 || cantidadDisponible < compra.CantidadComprada)
                    {
                        TempData["Message"] = "No hay suficientes unidades disponibles.";
                        TempData["MessageType"] = "danger";  // Mensaje de error
                        return RedirectToAction("Index", "Pelicula");
                    }

                    // Insertar el registro de compra
                    string insertSql = @"INSERT INTO Compras (UsuarioId, IdPeliculas, FechaCompra, EstadoPago, CantidadComprada)
                                 VALUES (@UsuarioId, @IdPeliculas, @FechaCompra, @EstadoPago, @CantidadComprada);
                                 SELECT SCOPE_IDENTITY();";

                    using (SqlCommand insertCommand = new SqlCommand(insertSql, connection))
                    {
                        int usuarioId = 1;  // Ejemplo de ID de usuario
                        DateTime fechaCompra = DateTime.Now;
                        string estadoPago = "En Proceso";

                        insertCommand.Parameters.AddWithValue("@UsuarioId", usuarioId);
                        insertCommand.Parameters.AddWithValue("@IdPeliculas", compra.IdPeliculas);
                        insertCommand.Parameters.AddWithValue("@FechaCompra", fechaCompra);
                        insertCommand.Parameters.AddWithValue("@EstadoPago", estadoPago);
                        insertCommand.Parameters.AddWithValue("@CantidadComprada", compra.CantidadComprada);

                        int compraId = Convert.ToInt32(insertCommand.ExecuteScalar());

                        // Actualizar la cantidad en el almacén
                        string updateAlmacenSql = "UPDATE Almacen SET CantidadDisponible = CantidadDisponible - @CantidadComprada WHERE IdPeliculas = @IdPeliculas";

                        using (SqlCommand updateAlmacenCommand = new SqlCommand(updateAlmacenSql, connection))
                        {
                            updateAlmacenCommand.Parameters.AddWithValue("@CantidadComprada", compra.CantidadComprada);
                            updateAlmacenCommand.Parameters.AddWithValue("@IdPeliculas", compra.IdPeliculas);
                            updateAlmacenCommand.ExecuteNonQuery();
                        }

                        TempData["Message"] = "Compra exitosa.";
                        TempData["MessageType"] = "success";  // Mensaje de éxito
                    }
                }

                return RedirectToAction("Index", "Pelicula");  // Redirigir después de la compra
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Ocurrió un error al procesar la compra: " + ex.Message;
                TempData["MessageType"] = "danger";  // Mensaje de error
                return RedirectToAction("Index", "Pelicula");
            }
        }


    }

}
