using sistema_ventas_peliculas_2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace sistema_ventas_peliculas_2.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class AlmacenController : Controller
    {
        //[Authorize(Roles = "Administrador")]
        // GET: Almacen
        public ActionResult Index()
        {
            List<Almacen> almacenList = new List<Almacen>();
            string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";

            // Join Almacen with Peliculas to get the movie details including Genero, Director, etc.
            string query = "SELECT a.IdAlmacen, a.IdPeliculas, a.CantidadDisponible, a.Ubicacion, p.Titulo, p.Genero, p.Director, p.Descripcion, p.Precio, p.Disponibilidad " +
                           "FROM Almacen a " +
                           "INNER JOIN Peliculas p ON a.IdPeliculas = p.IdPeliculas";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Almacen almacen = new Almacen
                    {
                        IdAlmacen = Convert.ToInt32(reader["IdAlmacen"]),
                        IdPeliculas = Convert.ToInt32(reader["IdPeliculas"]),
                        CantidadDisponible = Convert.ToInt32(reader["CantidadDisponible"]),
                        Ubicacion = reader["Ubicacion"].ToString(),

                        // Include all additional fields from the Peliculas table
                        Peliculas = new Peliculas
                        {
                            Titulo = reader["Titulo"].ToString(),
                            Genero = reader["Genero"].ToString(),
                            Director = reader["Director"].ToString(),
                            Descripcion = reader["Descripcion"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            Disponibilidad = Convert.ToBoolean(reader["Disponibilidad"])
                        }
                    };
                    almacenList.Add(almacen);
                }
            }

            return View(almacenList);
        }

        // GET: Almacen/Details/5
        public ActionResult Details(int id)
        {
            Almacen almacen = null;
            string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
            string query = "SELECT * FROM Almacen WHERE IdAlmacen = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    almacen = new Almacen
                    {
                        IdAlmacen = Convert.ToInt32(reader["IdAlmacen"]),
                        IdPeliculas = Convert.ToInt32(reader["IdPeliculas"]),
                        CantidadDisponible = Convert.ToInt32(reader["CantidadDisponible"]),
                        Ubicacion = reader["Ubicacion"].ToString()
                    };
                }
            }

            return View(almacen);
        }

        // GET: Almacen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Almacen/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Step 1: Check if the movie title exists in Peliculas
                    string checkMovieQuery = "SELECT COUNT(*) FROM Peliculas WHERE Titulo = @Titulo";
                    SqlCommand checkMovieCommand = new SqlCommand(checkMovieQuery, connection);
                    checkMovieCommand.Parameters.AddWithValue("@Titulo", collection["Titulo"]);

                    int count = (int)checkMovieCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        // Movie title already exists
                        ModelState.AddModelError("Titulo", "This movie title already exists.");
                        return View(); // Return the same view to display validation error
                    }

                    // Step 2: Insert the movie with additional fields
                    string insertMovieQuery = @"
            INSERT INTO Peliculas 
            (Titulo, Genero, Director, Descripcion, Precio, Disponibilidad) 
            OUTPUT INSERTED.IdPeliculas 
            VALUES (@Titulo, @Genero, @Director, @Descripcion, @Precio, @Disponibilidad)";

                    SqlCommand insertMovieCommand = new SqlCommand(insertMovieQuery, connection);
                    insertMovieCommand.Parameters.AddWithValue("@Titulo", collection["Titulo"]);
                    insertMovieCommand.Parameters.AddWithValue("@Genero", collection["Genero"]);
                    insertMovieCommand.Parameters.AddWithValue("@Director", collection["Director"]);
                    insertMovieCommand.Parameters.AddWithValue("@Descripcion", collection["Descripcion"]);
                    insertMovieCommand.Parameters.AddWithValue("@Precio", Convert.ToDecimal(collection["Precio"]));

                    // Convert the string to an integer for Disponibilidad
                    insertMovieCommand.Parameters.AddWithValue("@Disponibilidad", collection["Disponibilidad"] == "Available" ? 1 : 0);

                    int idPeliculas = (int)insertMovieCommand.ExecuteScalar();

                    // Step 3: Insert the new Almacen record
                    string insertAlmacenQuery = "INSERT INTO Almacen (IdPeliculas, CantidadDisponible, Ubicacion) VALUES (@IdPeliculas, @CantidadDisponible, @Ubicacion)";
                    SqlCommand insertAlmacenCommand = new SqlCommand(insertAlmacenQuery, connection);
                    insertAlmacenCommand.Parameters.AddWithValue("@IdPeliculas", idPeliculas);
                    insertAlmacenCommand.Parameters.AddWithValue("@CantidadDisponible", Convert.ToInt32(collection["CantidadDisponible"]));
                    insertAlmacenCommand.Parameters.AddWithValue("@Ubicacion", collection["Ubicacion"]);

                    insertAlmacenCommand.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Almacen/Edit/5
        public ActionResult Edit(int id)
        {
            Almacen almacen = new Almacen();
            almacen.Peliculas = new Peliculas(); // Inicializar el objeto Peliculas

            string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 1. Cargar los detalles del Almacen
                string almacenQuery = "SELECT * FROM Almacen WHERE IdAlmacen = @Id";
                SqlCommand almacenCommand = new SqlCommand(almacenQuery, connection);
                almacenCommand.Parameters.AddWithValue("@Id", id);

                SqlDataReader almacenReader = almacenCommand.ExecuteReader();
                if (almacenReader.Read())
                {
                    almacen.IdAlmacen = Convert.ToInt32(almacenReader["IdAlmacen"]);
                    almacen.IdPeliculas = Convert.ToInt32(almacenReader["IdPeliculas"]);
                    almacen.CantidadDisponible = Convert.ToInt32(almacenReader["CantidadDisponible"]);
                    almacen.Ubicacion = almacenReader["Ubicacion"].ToString();
                }
                almacenReader.Close();

                // 2. Cargar los detalles de la Pelicula relacionada
                string peliculaQuery = "SELECT * FROM Peliculas WHERE IdPeliculas = @IdPeliculas";
                SqlCommand peliculaCommand = new SqlCommand(peliculaQuery, connection);
                peliculaCommand.Parameters.AddWithValue("@IdPeliculas", almacen.IdPeliculas);

                SqlDataReader peliculaReader = peliculaCommand.ExecuteReader();
                if (peliculaReader.Read())
                {
                    almacen.Peliculas.IdPeliculas = Convert.ToInt32(peliculaReader["IdPeliculas"]);
                    almacen.Peliculas.Titulo = peliculaReader["Titulo"].ToString();
                    almacen.Peliculas.Genero = peliculaReader["Genero"].ToString();
                    almacen.Peliculas.Director = peliculaReader["Director"].ToString();
                    almacen.Peliculas.Descripcion = peliculaReader["Descripcion"].ToString();
                    almacen.Peliculas.Precio = Convert.ToDecimal(peliculaReader["Precio"]);
                    almacen.Peliculas.Disponibilidad = Convert.ToBoolean(peliculaReader["Disponibilidad"]);
                }
                peliculaReader.Close();
            }

            return View(almacen);
        }

        // POST: Almacen/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Almacen model)
        {
            try
            {
                string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Iniciar una transacción para asegurar que ambos cambios ocurran juntos
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // 1. Actualizar los datos de la película en la tabla Peliculas
                        string updatePeliculasQuery = @"
                UPDATE Peliculas
                SET Titulo = @Titulo, Genero = @Genero, Director = @Director, Descripcion = @Descripcion, 
                    Precio = @Precio, Disponibilidad = @Disponibilidad
                WHERE IdPeliculas = @IdPeliculas";

                        SqlCommand updatePeliculasCommand = new SqlCommand(updatePeliculasQuery, connection, transaction);
                        updatePeliculasCommand.Parameters.AddWithValue("@IdPeliculas", model.IdPeliculas);
                        updatePeliculasCommand.Parameters.AddWithValue("@Titulo", model.Peliculas.Titulo);
                        updatePeliculasCommand.Parameters.AddWithValue("@Genero", model.Peliculas.Genero);
                        updatePeliculasCommand.Parameters.AddWithValue("@Director", model.Peliculas.Director);
                        updatePeliculasCommand.Parameters.AddWithValue("@Descripcion", model.Peliculas.Descripcion);
                        updatePeliculasCommand.Parameters.AddWithValue("@Precio", model.Peliculas.Precio);
                        updatePeliculasCommand.Parameters.AddWithValue("@Disponibilidad", model.Peliculas.Disponibilidad);
                        updatePeliculasCommand.ExecuteNonQuery();

                        // 2. Actualizar los datos de Almacen en la tabla Almacen
                        string updateAlmacenQuery = @"
                UPDATE Almacen
                SET CantidadDisponible = @CantidadDisponible, Ubicacion = @Ubicacion
                WHERE IdAlmacen = @IdAlmacen";

                        SqlCommand updateAlmacenCommand = new SqlCommand(updateAlmacenQuery, connection, transaction);
                        updateAlmacenCommand.Parameters.AddWithValue("@CantidadDisponible", model.CantidadDisponible);
                        updateAlmacenCommand.Parameters.AddWithValue("@Ubicacion", model.Ubicacion);
                        updateAlmacenCommand.Parameters.AddWithValue("@IdAlmacen", model.IdAlmacen);
                        updateAlmacenCommand.ExecuteNonQuery();

                        // Si todo va bien, confirma la transacción
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Si hay un error, revierte la transacción
                        transaction.Rollback();
                        throw ex; // Lanza la excepción para manejarla fuera
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }



        // GET: Almacen/Delete/5
        public ActionResult Delete(int id)
        {
            Almacen almacen = null;
            string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
            string query = "SELECT * FROM Almacen WHERE IdAlmacen = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    almacen = new Almacen
                    {
                        IdAlmacen = Convert.ToInt32(reader["IdAlmacen"]),
                        IdPeliculas = Convert.ToInt32(reader["IdPeliculas"]),
                        CantidadDisponible = Convert.ToInt32(reader["CantidadDisponible"]),
                        Ubicacion = reader["Ubicacion"].ToString(),
                        Peliculas = new Peliculas() // Initialize Peliculas object
                    };
                }
                reader.Close();

                if (almacen != null)
                {
                    // 2. Cargar los detalles de la Pelicula relacionada
                    string peliculaQuery = "SELECT * FROM Peliculas WHERE IdPeliculas = @IdPeliculas";
                    SqlCommand peliculaCommand = new SqlCommand(peliculaQuery, connection);
                    peliculaCommand.Parameters.AddWithValue("@IdPeliculas", almacen.IdPeliculas);

                    SqlDataReader peliculaReader = peliculaCommand.ExecuteReader();
                    if (peliculaReader.Read())
                    {
                        almacen.Peliculas.IdPeliculas = Convert.ToInt32(peliculaReader["IdPeliculas"]);
                        almacen.Peliculas.Titulo = peliculaReader["Titulo"].ToString();
                        almacen.Peliculas.Genero = peliculaReader["Genero"].ToString();
                        almacen.Peliculas.Director = peliculaReader["Director"].ToString();
                        almacen.Peliculas.Descripcion = peliculaReader["Descripcion"].ToString();
                        almacen.Peliculas.Precio = Convert.ToDecimal(peliculaReader["Precio"]);
                        almacen.Peliculas.Disponibilidad = Convert.ToBoolean(peliculaReader["Disponibilidad"]);
                    }
                    peliculaReader.Close();
                }
            }

            if (almacen == null)
            {
                return HttpNotFound();
            }

            return View(almacen);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    // 1. Obtener el IdPeliculas del registro de Almacen que vamos a desactivar
                    string getIdPeliculasQuery = "SELECT IdPeliculas FROM Almacen WHERE IdAlmacen = @Id";
                    SqlCommand getIdPeliculasCommand = new SqlCommand(getIdPeliculasQuery, connection, transaction);
                    getIdPeliculasCommand.Parameters.AddWithValue("@Id", id);
                    int idPeliculas = (int)getIdPeliculasCommand.ExecuteScalar();

                    // 2. Desactivar el registro de Almacen (estableciendo CantidadDisponible a 0)
                    string updateAlmacenQuery = "UPDATE Almacen SET CantidadDisponible = 0 WHERE IdAlmacen = @Id";
                    SqlCommand updateAlmacenCommand = new SqlCommand(updateAlmacenQuery, connection, transaction);
                    updateAlmacenCommand.Parameters.AddWithValue("@Id", id);
                    updateAlmacenCommand.ExecuteNonQuery();

                    // 3. Verificar si quedan más registros de Almacen activos para esta película
                    string checkAlmacenQuery = "SELECT COUNT(*) FROM Almacen WHERE IdPeliculas = @IdPeliculas AND CantidadDisponible > 0";
                    SqlCommand checkAlmacenCommand = new SqlCommand(checkAlmacenQuery, connection, transaction);
                    checkAlmacenCommand.Parameters.AddWithValue("@IdPeliculas", idPeliculas);
                    int almacenActiveCount = (int)checkAlmacenCommand.ExecuteScalar();

                    // 4. Si no quedan más registros de Almacen activos, actualizar la disponibilidad de la película
                    if (almacenActiveCount == 0)
                    {
                        string updatePeliculaQuery = "UPDATE Peliculas SET Disponibilidad = 0 WHERE IdPeliculas = @IdPeliculas";
                        SqlCommand updatePeliculaCommand = new SqlCommand(updatePeliculaQuery, connection, transaction);
                        updatePeliculaCommand.Parameters.AddWithValue("@IdPeliculas", idPeliculas);
                        updatePeliculaCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return RedirectToAction("Index");
                }
                catch
                {
                    transaction.Rollback();
                    return View();
                }
            }
        }

    }
    
}

