using GalaManager1.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaManager1.Controllers
{
    public class AbonoCtrl
    {
        #region Atributos
        public CultureInfo culture { get; internal set; }
        public string specifier { get; internal set; }
        public Gala Gala { get; internal set; }
        public Comprador comprador { get; internal set; }
        #endregion

        #region Constructores
        public AbonoCtrl(Gala Gala)
        {
            this.Gala = Gala;
        }
        #endregion
        #region Metodos
        //public void FillData(DatGridView table, int IndexSelected)
        //{

        //}
        public void Seleccionar(int IndexSelected)
        {
            comprador = Gala.Comprador.Get(IndexSelected);

            if (comprador == null)
            {
                throw new GalaManager1.Exceptions.Excep
                {
                    ErrorCode = 10,
                    Description = "No se ha encontrado el comprador",
                    Category = GalaManager1.Exceptions.TypeException.Info
                };
            }
        }
        #endregion
    }
}
