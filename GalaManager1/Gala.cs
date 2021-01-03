using DbManagerDark1.DbManager;
using DbManagerDark1.Managers;
using GalaManager1.Models;
using System;

namespace GalaManager1
{
    public class Gala
    {
        #region Propiedades
        protected DarkConnectionMySQL Dbgala;
        #endregion

        #region Variales de accesos
        public DarkManagerMySQL<Coleccion> Coleccion { get; internal set; }
        public DarkManagerMySQL<ColeccionDato> ColeccionDato { get; internal set; }
        public DarkManagerMySQL<Comprador> Comprador { get; internal set; }
        public DarkManagerMySQL<CompradorProducto> CompradorProducto { get; internal set; }
        public DarkManagerMySQL<Abono> Abono { get; internal set; }
        public DarkManagerMySQL<Venta> Venta { get; internal set; }
        public DarkManagerMySQL<Producto> Producto { get; internal set; }
        public DarkManagerMySQL<viewMaxVentacomprador> viewMaxVentacomprador { get; internal set; }
        #endregion

        #region Metodos
        public void LoadObject(GalaObject galaObject)
        {
            if (Dbgala == null)
                throw new Exceptions.Excep { ErrorCode = -100, Category = Exceptions.TypeException.Error, Description = "Por favor conecta db Gala Db" };

            if (galaObject == GalaObject.Coleccion)
                Coleccion = new DarkManagerMySQL<Coleccion>(Dbgala);
            if (galaObject == GalaObject.ColeccionDato)
                ColeccionDato = new DarkManagerMySQL<ColeccionDato>(Dbgala);
            if (galaObject == GalaObject.Comprador)
                Comprador = new DarkManagerMySQL<Comprador>(Dbgala);
            if (galaObject == GalaObject.CompradorProducto)
                CompradorProducto = new DarkManagerMySQL<CompradorProducto>(Dbgala);
            if (galaObject == GalaObject.Abono)
                Abono = new DarkManagerMySQL<Abono>(Dbgala);
            if (galaObject == GalaObject.Venta)
                Venta = new DarkManagerMySQL<Venta>(Dbgala);
            if (galaObject == GalaObject.Producto)
                Producto = new DarkManagerMySQL<Producto>(Dbgala);
            if (galaObject == GalaObject.viewMaxVentacomprador)
                viewMaxVentacomprador = new DarkManagerMySQL<viewMaxVentacomprador>(Dbgala);
        }
        /// <summary>
        /// Conectar
        /// </summary>
        public void OpenConnection()
        {
            Dbgala = new DarkConnectionMySQL("Server=localhost;Port=3306;Database=gala;Uid=galauser;Pwd=C0nnect+1;Allow Zero Datetime=True;Convert Zero Datetime=True;Persist Security Info=True");
            Dbgala.OpenConnection();
        }
        /// <summary>
        /// Desconectar
        /// </summary>
        public void CloseConnection()
        {
            if (Dbgala != null)
            {
                Dbgala.CloseDataBaseAccess();
                Dbgala = null;
            }
        }
        public void StartTransaction()
        {
            if (Dbgala != null)
            {
                Dbgala.StartTransaction();
            }
        }
        public void Commit()
        {
            if (Dbgala != null)
            {
                Dbgala.Commit();
            }
        }
        public void RollBack()
        {
            if (Dbgala != null)
            {
                Dbgala.RolBack();
            }
        }
        public void ReOpenConection()
        {
            if (Dbgala != null)
            {
                Dbgala.ReOpenConection();
            }
        }
        #endregion
    }

    public enum GalaObject
    {
        Coleccion = 1,
        ColeccionDato = 2,
        Comprador = 3,
        CompradorProducto = 4,
        Abono = 5,
        Venta = 6,
        Producto = 7,
        viewMaxVentacomprador = 8,
    }
}
