
using DbManagerDark1.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GalaManager1.Models
{
    [DarkTable(IsMappedByLabels = false, IsStoreProcedure = false, IsView = false)]
    public class Coleccion
    {
        [DarkColumn(IsMapped = true, IsKey = true)]
        public int IdColeccion { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }
    }
    [DarkTable(IsMappedByLabels = false, IsStoreProcedure = false, IsView = false)]
    public class ColeccionDato
    {
        [DarkColumn(IsMapped = true, IsKey = true)]
        public int IdColeccionDato { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public int IdColeccion { get; set; }
    }
    [DarkTable(IsMappedByLabels = false, IsStoreProcedure = false, IsView = false)]
    public class Comprador
    {
        [DarkColumn(IsMapped = true, IsKey = true)]
        public int IdComprador { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public string NombreCompleto { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public bool Activo { get; set; }
    }
    [DarkTable(IsMappedByLabels = false, IsStoreProcedure = false, IsView = false)]
    public class CompradorProducto
    {
        [DarkColumn(IsMapped = true, IsKey = true)]
        public int IdCompradorProducto { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public int IdProducto { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public DateTime Modificado { get; set; }
    }
    [DarkTable(IsMappedByLabels = false, IsStoreProcedure = false, IsView = false)]
    public class Abono
    {
        [DarkColumn(IsMapped = true, IsKey = true)]
        public int IdAbono { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public int IdComprador { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public float Monto { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public string Comentarios { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public DateTime Modificado { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public bool Eliminado { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public int IdTipo { get; set; }
    }
    [DarkTable(IsMappedByLabels = false, IsStoreProcedure = false, IsView = false)]
    public class Venta
    {
        [DarkColumn(IsMapped = true, IsKey = true)]
        public int IdVenta { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public string Codigo { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public float Monto { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public DateTime Creado { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public DateTime Modificado { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public int IdComprador { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public bool Eliminado { get; set; }
    }
    [DarkTable(IsMappedByLabels = false, IsStoreProcedure = false, IsView = false)]
    public class Producto
    {
        [DarkColumn(IsMapped = true, IsKey = true)]
        public int IdProducto { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public string Codigo { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public string Descripcion { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public int IdTipo { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public bool Activo { get; set; }
        [DarkColumn(IsMapped = true, IsKey = false)]
        public DateTime Comprado { get; set; }
    }
}
