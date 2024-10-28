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
        //public ActionResult Create(int id)
        //{
        //    string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
        //    // Crear una lista para almacenar las compras
        //    var compras = new List<Compras>();

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            // Insertar un nuevo registro en la tabla de compras
        //            string insertSql = @"INSERT INTO Compras (UsuarioId, IdPeliculas, FechaCompra, EstadoPago)
        //                         VALUES (@UsuarioId, @IdPeliculas, @FechaCompra, @EstadoPago);
        //                         SELECT SCOPE_IDENTITY();";
        //            using (SqlCommand insertCommand = new SqlCommand(insertSql, connection))
        //            {
        //                // Obtener el ID del usuario actualmente autenticado (ejemplo)
        //                int usuarioId = 1;
        //                //ObtenerUsuarioIdAutenticado();
        //                // Obtener la fecha y hora actual para la fecha de compra
        //                //string fechaCompra = DateTime.Now.ToString("yyyy-MM-dd");
        //                //string fechaCompra = DateTime.Now.ToString("dd'/'MM'/'yyyy");
        //                //DateTime fecha_I = DateTime.Parse(fechaCompra);
        //                //string fecha = fecha_I.ToString("dd-MMM-yyyy");
        //                DateTime fechaCompra = DateTime.Now.Date;
        //                // Definir el estado de pago (ejemplo)
        //                string estadoPago = "En Proceso";

        //                // Asignar los valores a los parámetros de la consulta
        //                insertCommand.Parameters.AddWithValue("@UsuarioId", usuarioId);
        //                insertCommand.Parameters.AddWithValue("@IdPeliculas", id);
        //                insertCommand.Parameters.AddWithValue("@FechaCompra", fechaCompra);
        //                insertCommand.Parameters.AddWithValue("@EstadoPago", estadoPago);

        //                int compraId = Convert.ToInt32(insertCommand.ExecuteScalar());

        //                // Obtener la cantidad comprada
        //                int cantidadComprada = 1; // Puedes ajustar esto según tu lógica de negocio

        //                // Actualizar la cantidad disponible en la tabla de almacén
        //                string updateSql = "UPDATE Almacen SET CantidadDisponible = CantidadDisponible - @CantidadComprada WHERE IdPeliculas = @IdPeliculas";
        //                using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
        //                {
        //                    updateCommand.Parameters.AddWithValue("@CantidadComprada", cantidadComprada);
        //                    updateCommand.Parameters.AddWithValue("@IdPeliculas", id);
        //                    updateCommand.ExecuteNonQuery();
        //                }
        //                if (cantidadComprada != 1)
        //                {
        //                    ViewBag.Message = "Compra exitosa.";
        //                }
        //                else
        //                {
        //                    ViewBag.Message = "Terminaron.";
        //                }
        //                // Obtener los detalles de la compra recién insertada
        //                string selectSql = "SELECT IdCompras, UsuarioId, IdPeliculas, FechaCompra, EstadoPago FROM Compras WHERE IdCompras = @IdCompras";
        //                using (SqlCommand selectCommand = new SqlCommand(selectSql, connection))
        //                {
        //                    selectCommand.Parameters.AddWithValue("@IdCompras", compraId);
        //                    using (SqlDataReader reader = selectCommand.ExecuteReader())
        //                    {
        //                        if (reader.Read())
        //                        {
        //                            var compra = new Compras()
        //                            {
        //                                IdCompras = reader.GetInt32(reader.GetOrdinal("IdCompras")),
        //                                UsuarioId = reader.GetInt32(reader.GetOrdinal("UsuarioId")),
        //                                IdPeliculas = reader.GetInt32(reader.GetOrdinal("IdPeliculas")),
        //                                FechaCompra = reader.GetDateTime(reader.GetOrdinal("fechaCompra")),
        //                                EstadoPago = reader.GetString(reader.GetOrdinal("EstadoPago"))
        //                            };
        //                            compras.Add(compra);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        return View(compras.AsEnumerable());
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejar la excepción o registrar el error
        //        // Puedes elegir devolver una vista de error o redirigir a una página de error
        //        return View("Error");
        //    }
        //}

        //public ActionResult Create(int id)
        //{
        //    string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
        //    var compras = new List<Compras>();

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();

        //            // Verificar si hay suficiente cantidad disponible antes de realizar la compra
        //            string checkQuantitySql = "SELECT CantidadDisponible FROM Almacen WHERE IdPeliculas = @IdPeliculas";
        //            int cantidadDisponible = 0;

        //            using (SqlCommand checkCommand = new SqlCommand(checkQuantitySql, connection))
        //            {
        //                checkCommand.Parameters.AddWithValue("@IdPeliculas", id);
        //                cantidadDisponible = (int)checkCommand.ExecuteScalar(); // Obtener la cantidad disponible actual
        //            }

        //            if (cantidadDisponible <= 0)
        //            {
        //                // Almacenar el mensaje en TempData
        //                TempData["Message"] = "No hay suficientes unidades disponibles.";
        //                return RedirectToAction("Index","Pelicula"); // Redirige a la acción Index

        //            }

        //            // Insertar el registro de compra si hay cantidad disponible
        //            string insertSql = @"INSERT INTO Compras (UsuarioId, IdPeliculas, FechaCompra, EstadoPago)
        //                  VALUES (@UsuarioId, @IdPeliculas, @FechaCompra, @EstadoPago);
        //                  SELECT SCOPE_IDENTITY();";

        //            using (SqlCommand insertCommand = new SqlCommand(insertSql, connection))
        //            {
        //                int usuarioId = 1; // Ejemplo de ID de usuario
        //                DateTime fechaCompra = DateTime.Now.Date;
        //                string estadoPago = "En Proceso";

        //                insertCommand.Parameters.AddWithValue("@UsuarioId", usuarioId);
        //                insertCommand.Parameters.AddWithValue("@IdPeliculas", id);
        //                insertCommand.Parameters.AddWithValue("@FechaCompra", fechaCompra);
        //                insertCommand.Parameters.AddWithValue("@EstadoPago", estadoPago);

        //                int compraId = Convert.ToInt32(insertCommand.ExecuteScalar());

        //                // Reducir la cantidad disponible en el almacén
        //                string updateSql = "UPDATE Almacen SET CantidadDisponible = CantidadDisponible - @CantidadComprada WHERE IdPeliculas = @IdPeliculas";
        //                int cantidadComprada = 1; // Definir la cantidad comprada

        //                using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
        //                {
        //                    updateCommand.Parameters.AddWithValue("@CantidadComprada", cantidadComprada);
        //                    updateCommand.Parameters.AddWithValue("@IdPeliculas", id);
        //                    updateCommand.ExecuteNonQuery();
        //                }

        //                //ViewBag.Message = "Compra exitosa.";
        //                // Almacenar el mensaje en TempData
        //                TempData["Message"] = "Compra exitosa.";
        //            }
        //        }

        //        return View(compras.AsEnumerable());
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejar la excepción o registrar el error
        //        return View("Error");
        //    }
        //}

        //public ActionResult Create(int id)
        //{
        //    string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
        //    var compras = new List<Compras>();

        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();

        //            // Verificar si hay suficiente cantidad disponible en el almacén
        //            string checkQuantitySql = "SELECT CantidadDisponible FROM Almacen WHERE IdPeliculas = @IdPeliculas";
        //            int cantidadDisponible = 0;

        //            using (SqlCommand checkCommand = new SqlCommand(checkQuantitySql, connection))
        //            {
        //                checkCommand.Parameters.AddWithValue("@IdPeliculas", id);
        //                cantidadDisponible = (int)checkCommand.ExecuteScalar(); // Obtener la cantidad disponible actual
        //            }

        //            if (cantidadDisponible <= 0)
        //            {
        //                // Almacenar el mensaje en TempData
        //                TempData["Message"] = "No hay suficientes unidades disponibles.";
        //                return RedirectToAction("Index", "Pelicula"); // Redirige a la acción Index
        //            }

        //            // Obtener la cantidad comprada desde la tabla Peliculas
        //            string getCantidadCompradaSql = "SELECT CantidadComprada FROM Peliculas WHERE IdPeliculas = @IdPeliculas";
        //            int cantidadComprada = 0;

        //            using (SqlCommand getCantidadCompradaCommand = new SqlCommand(getCantidadCompradaSql, connection))
        //            {
        //                getCantidadCompradaCommand.Parameters.AddWithValue("@IdPeliculas", id);
        //                cantidadComprada = (int)getCantidadCompradaCommand.ExecuteScalar(); // Obtener la cantidad comprada actual
        //            }

        //            // Insertar el registro de compra si hay cantidad disponible
        //            string insertSql = @"INSERT INTO Compras (UsuarioId, IdPeliculas, FechaCompra, EstadoPago)
        //              VALUES (@UsuarioId, @IdPeliculas, @FechaCompra, @EstadoPago);
        //              SELECT SCOPE_IDENTITY();";

        //            using (SqlCommand insertCommand = new SqlCommand(insertSql, connection))
        //            {
        //                int usuarioId = 1; // Ejemplo de ID de usuario
        //                DateTime fechaCompra = DateTime.Now.Date;
        //                string estadoPago = "En Proceso";

        //                insertCommand.Parameters.AddWithValue("@UsuarioId", usuarioId);
        //                insertCommand.Parameters.AddWithValue("@IdPeliculas", id);
        //                insertCommand.Parameters.AddWithValue("@FechaCompra", fechaCompra);
        //                insertCommand.Parameters.AddWithValue("@EstadoPago", estadoPago);

        //                int compraId = Convert.ToInt32(insertCommand.ExecuteScalar());

        //                // Reducir la cantidad disponible en el almacén
        //                string updateAlmacenSql = "UPDATE Almacen SET CantidadDisponible = CantidadDisponible - @CantidadComprada WHERE IdPeliculas = @IdPeliculas";

        //                using (SqlCommand updateAlmacenCommand = new SqlCommand(updateAlmacenSql, connection))
        //                {
        //                    updateAlmacenCommand.Parameters.AddWithValue("@CantidadComprada", cantidadComprada);
        //                    updateAlmacenCommand.Parameters.AddWithValue("@IdPeliculas", id);
        //                    updateAlmacenCommand.ExecuteNonQuery();
        //                }

        //                // Actualizar la cantidad comprada en la tabla Peliculas
        //                string updatePeliculasSql = "UPDATE Peliculas SET CantidadComprada = CantidadComprada + @CantidadComprada WHERE IdPeliculas = @IdPeliculas";

        //                using (SqlCommand updatePeliculasCommand = new SqlCommand(updatePeliculasSql, connection))
        //                {
        //                    updatePeliculasCommand.Parameters.AddWithValue("@CantidadComprada", cantidadComprada);
        //                    updatePeliculasCommand.Parameters.AddWithValue("@IdPeliculas", id);
        //                    updatePeliculasCommand.ExecuteNonQuery();
        //                }

        //                // Almacenar el mensaje en TempData
        //                TempData["Message"] = "Compra exitosa.";
        //            }

        //            // Obtener la compra relacionada con la película específica
        //            string getComprasSql = "SELECT * FROM Compras WHERE IdPeliculas = @IdPeliculas";

        //            using (SqlCommand getComprasCommand = new SqlCommand(getComprasSql, connection))
        //            {
        //                getComprasCommand.Parameters.AddWithValue("@IdPeliculas", id);
        //                using (SqlDataReader reader = getComprasCommand.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        var compra = new Compras
        //                        {
        //                            IdCompras = (int)reader["IdCompras"],
        //                            UsuarioId = (int)reader["UsuarioId"],
        //                            IdPeliculas = (int)reader["IdPeliculas"],
        //                            FechaCompra = (DateTime)reader["FechaCompra"],
        //                            EstadoPago = reader["EstadoPago"].ToString()
        //                        };
        //                        compras.Add(compra);
        //                    }
        //                }
        //            }
        //        }

        //        // Retornar la vista con la lista de compras filtrada por la película
        //        return View(compras.AsEnumerable());
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejar la excepción o registrar el error
        //        return View("Error");
        //    }
        //}
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
