using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistema_ventas_peliculas_2.Models
{
    public class PeliculasyCompra
    {
        public int IdPeliculas { get; set; }
        public string Titulo { get; set; }
        public string Genero { get; set; }
        public string Director { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public bool Disponibilidad { get; set; }
        public int CantidadDisponible { get; set; }
        public int IdCompras { get; set; }
        public int UsuarioId { get; set; }
        public Nullable<System.DateTime> FechaCompra { get; set; }
        public string EstadoPago { get; set; }
        public int IdAlmacen { get; set; }
        public string Ubicacion { get; set; }

    }
}