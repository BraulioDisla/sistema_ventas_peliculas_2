using sistema_ventas_peliculas_2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace sistema_ventas_peliculas_2.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";

        // Método para obtener la lista de roles desde la base de datos
        private List<string> GetRolesFromDatabase()
        {
            List<string> roles = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT Rol FROM Usuarios";  // Obtener roles únicos

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(reader["Rol"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener roles: " + ex.Message);
            }

            return roles;
        }

        // GET: Usuario/Login
        [HttpGet]
        public ActionResult Login()
        {
            // Cargar los roles desde la base de datos y pasarlos a la vista
            ViewBag.Roles = new SelectList(GetRolesFromDatabase());
            return View();
        }

        // POST: Usuario/Login
        [HttpPost]
        // [ValidateAntiForgeryToken]  // Puedes habilitarlo si quieres protección contra CSRF.
        public ActionResult Login(string User, string Pass, string Rol)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consulta SQL con parámetros.
                    string sql = @"
                SELECT * 
                FROM Usuarios 
                WHERE NombreUsuario = @NombreUsuario 
                  AND Contrasena = @Contrasena 
                  AND Rol = @Rol";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        // Asignar los valores recibidos a los parámetros.
                        command.Parameters.AddWithValue("@NombreUsuario", User);
                        command.Parameters.AddWithValue("@Contrasena", Pass);  // Considera hashear la contraseña.
                        command.Parameters.AddWithValue("@Rol", Rol);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Usuario encontrado, iniciar sesión.
                                FormsAuthentication.SetAuthCookie(User, true);

                                // Guardar detalles del usuario en la sesión.
                                Session["IdUsuario"] = reader["IdUsuarios"];
                                Session["NombreUsuario"] = reader["NombreUsuario"];
                                Session["CorreoElectronico"] = reader["CorreoElectronico"];
                                Session["Rol"] = reader["Rol"];

                                // Guardar el nombre en ViewBag
                                ViewBag.NombreUsuario = reader["NombreUsuario"].ToString();
                                // Redirigir según el rol del usuario.
                                string userRole = reader["Rol"].ToString();
                                switch (userRole)
                                {
                                    case "Administrador":
                                        return RedirectToAction("Index", "Almacen");
                                    case "Usuario":
                                        return RedirectToAction("Index", "Pelicula");
                                    //case "Invitado":
                                    //    return RedirectToAction("Index", "Pelicula");
                                    default:
                                        ViewBag.ErrorMessage = "Rol no reconocido.";
                                        break;
                                }
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Nombre de usuario, contraseña o rol inválidos.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al intentar iniciar sesión: " + ex.Message;
            }

            // Volver a cargar los roles en caso de error.
            ViewBag.Roles = new SelectList(GetRolesFromDatabase());
            return View();
        }

        // GET: Usuario/Salir
        public ActionResult Salir()
        {
            // Limpiar las variables de sesión
            Session.Clear();
            Session.Abandon();

            // Cerrar la cookie de autenticación
            FormsAuthentication.SignOut();

            // Redirigir al login
            return RedirectToAction("Invitado", "Pelicula");
        }

        // Método para obtener los roles (puedes ajustar este método según tu necesidad)
        private List<string> GetRolesFromDatabase1()
        {
            return new List<string> { "Administrador", "Usuario"}; // Roles estáticos
        }

        [HttpGet]
        public ActionResult Registro()
        {
            ViewBag.Roles = new SelectList(GetRolesFromDatabase1());
            return View();
        }

        [HttpPost]
        public ActionResult Registro(string NombreUsuario, string CorreoElectronico, string Contrasena, string Rol)
        {
            // Validar los roles permitidos
            var validRoles = new List<string> { "Administrador", "Usuario" }; //, "Invitado"

            if (!validRoles.Contains(Rol))
            {
                ViewBag.ErrorMessage = "Rol no válido. Por favor selecciona un rol válido.";
                ViewBag.Roles = new SelectList(GetRolesFromDatabase());
                return View();
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Verificar si el nombre de usuario o correo ya existen
                    string checkUserQuery = @"
                SELECT COUNT(*) 
                FROM Usuarios 
                WHERE NombreUsuario = @NombreUsuario OR CorreoElectronico = @CorreoElectronico";

                    using (SqlCommand checkCommand = new SqlCommand(checkUserQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
                        checkCommand.Parameters.AddWithValue("@CorreoElectronico", CorreoElectronico);

                        int existingUsers = (int)checkCommand.ExecuteScalar();
                        if (existingUsers > 0)
                        {
                            ViewBag.ErrorMessage = "El nombre de usuario o correo ya están registrados.";
                            ViewBag.Roles = new SelectList(GetRolesFromDatabase());
                            return View();
                        }
                    }

                    // Insertar el nuevo usuario
                    string insertQuery = @"
                INSERT INTO Usuarios (NombreUsuario, CorreoElectronico, Contrasena, Rol) 
                VALUES (@NombreUsuario, @CorreoElectronico, @Contrasena, @Rol)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
                        command.Parameters.AddWithValue("@CorreoElectronico", CorreoElectronico);
                        command.Parameters.AddWithValue("@Contrasena", Contrasena); // Considerar hashear la contraseña.
                        command.Parameters.AddWithValue("@Rol", Rol);

                        command.ExecuteNonQuery();

                        ViewBag.SuccessMessage = "Usuario registrado exitosamente.";
                        ViewBag.Roles = new SelectList(GetRolesFromDatabase());
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al registrar el usuario: " + ex.Message;
                ViewBag.Roles = new SelectList(GetRolesFromDatabase());
                return View();
            }
        }

        

        // Método para obtener roles desde la base de datos
        private List<string> GetRolesFromDatabase2()
        {
            List<string> roles = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT Rol FROM Usuarios"; // Obtener roles únicos
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(reader["Rol"].ToString());
                    }
                }
            }
            return roles;
        }

        // GET: Usuario/Index
        public ActionResult Index()
        {
            List<Usuarios> usuarios = new List<Usuarios>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT IdUsuarios, NombreUsuario, CorreoElectronico, Rol FROM Usuarios";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuarios
                        {
                            IdUsuarios = (int)reader["IdUsuarios"],
                            NombreUsuario = reader["NombreUsuario"].ToString(),
                            CorreoElectronico = reader["CorreoElectronico"].ToString(),
                            Rol = reader["Rol"].ToString()
                        });
                    }
                }
            }
            return View(usuarios);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            ViewBag.Roles = new SelectList(GetRolesFromDatabase2().Select(r => new { Value = r, Text = r }), "Value", "Text");
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "INSERT INTO Usuarios (NombreUsuario, CorreoElectronico, Contrasena, Rol) VALUES (@Nombre, @Correo, @Contrasena, @Rol)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Nombre", usuario.NombreUsuario);
                            command.Parameters.AddWithValue("@Correo", usuario.CorreoElectronico);
                            command.Parameters.AddWithValue("@Contrasena", usuario.Contrasena); // Considera encriptar la contraseña
                            command.Parameters.AddWithValue("@Rol", usuario.Rol);
                            command.ExecuteNonQuery();
                        }
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error al crear usuario: " + ex.Message;
                }
            }
            ViewBag.Roles = new SelectList(GetRolesFromDatabase2().Select(r => new { Value = r, Text = r }), "Value", "Text");
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            Usuarios usuario = GetUsuarioById(id);
            if (usuario == null)
            {
                return HttpNotFound(); // Manejo de usuario no encontrado
            }
            ViewBag.Roles = new SelectList(GetRolesFromDatabase2().Select(r => new { Value = r, Text = r }), "Value", "Text");
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit(Usuarios usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "UPDATE Usuarios SET NombreUsuario = @Nombre, CorreoElectronico = @Correo, Rol = @Rol WHERE IdUsuarios = @Id";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Id", usuario.IdUsuarios);
                            command.Parameters.AddWithValue("@Nombre", usuario.NombreUsuario);
                            command.Parameters.AddWithValue("@Correo", usuario.CorreoElectronico);
                            command.Parameters.AddWithValue("@Rol", usuario.Rol);
                            command.ExecuteNonQuery();
                        }
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error al editar usuario: " + ex.Message;
                }
            }
            ViewBag.Roles = new SelectList(GetRolesFromDatabase2().Select(r => new { Value = r, Text = r }), "Value", "Text");
            return View(usuario);
        }

        // GET: Usuario/Delete/5
        public ActionResult Delete(int id)
        {
            Usuarios usuario = GetUsuarioById(id);
            if (usuario == null)
            {
                return HttpNotFound(); // Manejo de usuario no encontrado
            }
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Usuarios WHERE IdUsuarios = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        command.ExecuteNonQuery();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Error al eliminar usuario: " + ex.Message;
                return View();
            }
        }

        // Método auxiliar para obtener un usuario por ID
        private Usuarios GetUsuarioById(int id)
        {
            Usuarios usuario = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Usuarios WHERE IdUsuarios = @Id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuario = new Usuarios
                            {
                                IdUsuarios = (int)reader["IdUsuarios"],
                                NombreUsuario = reader["NombreUsuario"].ToString(),
                                CorreoElectronico = reader["CorreoElectronico"].ToString(),
                                Rol = reader["Rol"].ToString()
                            };
                        }
                    }
                }
            }
            return usuario;
        }
    }

}

