using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sistema_ventas_peliculas_2.Models
{
    public class PeliculasyAlmacen
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


    }
}