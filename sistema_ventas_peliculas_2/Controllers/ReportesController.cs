using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace sistema_ventas_peliculas_2.Controllers
{
    public class ReportesController : Controller
    {
        // Replace with your actual connection string.
        private readonly string connectionString = "Server=ONA-DTC-DIS-19;Database=Ventas_Peliculas;Trusted_Connection=True;";

        public ActionResult Index()
        {
            DataSet ds = GetDataSet();

            // Pass the DataSet to the View.
            return View(ds);
        }

        // Method to retrieve data from all tables and store it in a DataSet.
        private DataSet GetDataSet()
        {
            DataSet ds = new DataSet();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Define queries for each table.
                string almacenQuery = "SELECT * FROM Almacen";
                string comprasQuery = "SELECT * FROM Compras";
                string peliculasQuery = "SELECT * FROM Peliculas";
                string usuariosQuery = "SELECT * FROM Usuarios";

                // Use SqlDataAdapters to fill the DataSet.
                SqlDataAdapter almacenAdapter = new SqlDataAdapter(almacenQuery, connection);
                SqlDataAdapter comprasAdapter = new SqlDataAdapter(comprasQuery, connection);
                SqlDataAdapter peliculasAdapter = new SqlDataAdapter(peliculasQuery, connection);
                SqlDataAdapter usuariosAdapter = new SqlDataAdapter(usuariosQuery, connection);

                // Fill the DataSet with the tables.
                almacenAdapter.Fill(ds, "Almacen");
                comprasAdapter.Fill(ds, "Compras");
                peliculasAdapter.Fill(ds, "Peliculas");
                usuariosAdapter.Fill(ds, "Usuarios");
            }

            return ds;
        }
    }
}