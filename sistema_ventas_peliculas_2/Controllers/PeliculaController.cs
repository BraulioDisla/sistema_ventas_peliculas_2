using sistema_ventas_peliculas_2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace sistema_ventas_peliculas_2.Controllers
{
    //[Authorize(Roles = "Usuario")]
    public class PeliculaController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            List<PeliculasyAlmacen> peliculaList = new List<PeliculasyAlmacen>();
            string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
            //string query = "SELECT * FROM Peliculas where Disponibilidad = 1";
            // Actualizamos la consulta para obtener la cantidad de la tabla Almacen
            string query = @"
            SELECT P.IdPeliculas, P.Titulo, P.Genero, P.Director, P.Descripcion, P.Precio, P.Disponibilidad, A.CantidadDisponible 
            FROM Peliculas P
            INNER JOIN Almacen A ON P.IdPeliculas = A.IdPeliculas
            WHERE P.Disponibilidad = 1";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PeliculasyAlmacen pelicula = new PeliculasyAlmacen
                    {
                        IdPeliculas = Convert.ToInt32(reader["IdPeliculas"]),
                        Titulo = reader["Titulo"].ToString(),
                        Genero = reader["Genero"].ToString(),
                        Director = reader["Director"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        Disponibilidad = Convert.ToBoolean(reader["Disponibilidad"]),
                        CantidadDisponible = Convert.ToInt32(reader["CantidadDisponible"])


                    };
                    peliculaList.Add(pelicula);
                    
                }
            }

            return View(peliculaList);
        }

        // GET: Almacen/Details/5
        public ActionResult Details(int id)
        {
            Peliculas pelicula = null;
            string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
            string query = "SELECT * FROM Peliculas WHERE IdPeliculas = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    pelicula = new Peliculas
                    {
                        
                        IdPeliculas = Convert.ToInt32(reader["IdPeliculas"]),
                        Titulo = reader["Titulo"].ToString(),
                        Genero = reader["Genero"].ToString(),
                        Director = reader["Director"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        Disponibilidad = Convert.ToBoolean(reader["Disponibilidad"])
                     
                    };
                }
            }

            return View(pelicula);
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
                string query = "INSERT INTO Peliculas (IdPeliculas, Titulo, Genero, Director, Descripcion, Precio, Disponibilidad) VALUES (@IdPeliculas, @Titulo, @Genero, @Director, @Descripcion, @Precio, @Disponibilidad)";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@IdPeliculas", Convert.ToInt32(collection["IdPeliculas"]));
                    command.Parameters.AddWithValue("@Titulo", collection["Titulo"]);
                    command.Parameters.AddWithValue("@Genero", collection["Genero"]);
                    command.Parameters.AddWithValue("@Director", collection["Director"]);
                    command.Parameters.AddWithValue("@Descripcion", collection["Descripcion"]);
                    command.Parameters.AddWithValue("@Precio", Convert.ToDecimal(collection["Precio"]));
                    command.Parameters.AddWithValue("@Disponibilidad", Convert.ToBoolean(collection["Disponibilidad"]));
                    //command.Parameters.AddWithValue("@Ubicacion", collection["Ubicacion"]);
                    //command.Parameters.AddWithValue("@Ubicacion", collection["Ubicacion"]);

                    connection.Open();
                    command.ExecuteNonQuery();
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
            Peliculas pelicula = null;
            string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
            string query = "SELECT * FROM Peliculas WHERE IdPeliculas = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    pelicula = new Peliculas
                    {
                        IdPeliculas = Convert.ToInt32(reader["IdAlmacen"]),
                        Titulo = reader["Titulo"].ToString(),
                        Genero = reader["Genero"].ToString(),
                        Director = reader["Director"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        Disponibilidad = Convert.ToBoolean(reader["Disponibilidad"])
                        //CantidadDisponible = Convert.ToInt32(reader["CantidadDisponible"]),
                        //Ubicacion = reader["Ubicacion"].ToString()
                    };
                }
            }

            return View(pelicula);
        }

        // POST: Almacen/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
                string query = "UPDATE Peliculas SET IdPeliculas = @IdPeliculas, Titulo = @Titulo, Genero = @Genero, Director = @Director, Descripcion=@Descripcion,Precio=@Precio,Disponibilidad=@Disponibilidad WHERE IdPeliculas = @Id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@IdPeliculas", Convert.ToInt32(collection["IdPeliculas"]));
                    command.Parameters.AddWithValue("@Titulo", collection["Titulo"]);
                    command.Parameters.AddWithValue("@Genero", collection["Genero"]);
                    command.Parameters.AddWithValue("@Director", collection["Director"]);
                    command.Parameters.AddWithValue("@Descripcion", collection["Descripcion"]);
                    command.Parameters.AddWithValue("@Precio", Convert.ToDecimal(collection["Precio"]));
                    command.Parameters.AddWithValue("@Disponibilidad", Convert.ToBoolean(collection["Disponibilidad"]));
                    //command.Parameters.AddWithValue("@Ubicacion", collection["Ubicacion"]);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Almacen/Delete/5
        public ActionResult Delete(int id)
        {
            Peliculas pelicula = null;
            string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
            string query = "SELECT * FROM Peliculas WHERE IdPeliculas = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    pelicula = new Peliculas
                    {
                        IdPeliculas = Convert.ToInt32(reader["IdAlmacen"]),
                        Titulo = reader["Titulo"].ToString(),
                        Genero = reader["Genero"].ToString(),
                        Director = reader["Director"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        Disponibilidad = Convert.ToBoolean(reader["Disponibilidad"])
                    };
                }
            }

            return View(pelicula);
        }

        // POST: Almacen/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
                string query = "DELETE FROM Peliculas WHERE IdPeliculas = @Id";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Invitado()
        {
            List<PeliculasyAlmacen> peliculaList = new List<PeliculasyAlmacen>();
            string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";
            //string query = "SELECT * FROM Peliculas where Disponibilidad = 1";
            // Actualizamos la consulta para obtener la cantidad de la tabla Almacen
            string query = @"
            SELECT P.IdPeliculas, P.Titulo, P.Genero, P.Director, P.Descripcion, P.Precio, P.Disponibilidad, A.CantidadDisponible 
            FROM Peliculas P
            INNER JOIN Almacen A ON P.IdPeliculas = A.IdPeliculas
            WHERE P.Disponibilidad = 1";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    PeliculasyAlmacen pelicula = new PeliculasyAlmacen
                    {
                        IdPeliculas = Convert.ToInt32(reader["IdPeliculas"]),
                        Titulo = reader["Titulo"].ToString(),
                        Genero = reader["Genero"].ToString(),
                        Director = reader["Director"].ToString(),
                        Descripcion = reader["Descripcion"].ToString(),
                        Precio = Convert.ToDecimal(reader["Precio"]),
                        Disponibilidad = Convert.ToBoolean(reader["Disponibilidad"]),
                        CantidadDisponible = Convert.ToInt32(reader["CantidadDisponible"])


                    };
                    peliculaList.Add(pelicula);

                }
            }

            return View(peliculaList);
        }
    }
}