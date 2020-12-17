using DbManagerDark1.Attributes;
using DbManagerDark1.DbManager;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DbManagerDark1.Managers
{
    public class DarkSpecialMSSQL<T> where T : new()
    {
        private DarkConnectionSqlSever dBConnection { get; set; }
        public DarkSpecialMSSQL(DarkConnectionSqlSever dBConnection)
        {
            this.dBConnection = dBConnection;
        }
        public List<T> GetSpecialStat(string SqlStatements)
        {
            return DataReader(SqlStatements);
        }
        private List<T> DataReader(string SqlStatements)
        {
            DarkTable tableDefinifiton = GetClassAttribute();

            System.Data.SqlClient.SqlDataReader Data = dBConnection.GetDataReader(SqlStatements);
            List<T> Response = new List<T>();
            while (Data.Read())
            {
                object exFormAsObj = Activator.CreateInstance(typeof(T));
                foreach (var prop in typeof(T).GetProperties())
                {
                    PropertyInfo propertyInfo = exFormAsObj.GetType().GetProperty(prop.Name);
                    DarkColumn hiddenAttribute = (DarkColumn)propertyInfo.GetCustomAttribute(typeof(DarkColumn));

                    if (hiddenAttribute == null)
                    {
                        throw new Exceptions.DarkExceptionSystem(string.Format("The attribute was not found in the attribute '{0}', if you don´t want to use mapTable, please set IsMappedByLabels = false", prop.Name));
                    }

                    if (hiddenAttribute.IsMapped)
                    {
                        string NombrePropiedad = "";
                        if (tableDefinifiton.IsMappedByLabels)
                        {
                            NombrePropiedad = hiddenAttribute.Name.Trim();
                        }
                        else
                        {
                            NombrePropiedad = prop.Name;
                        }
                        try
                        {
                            if (prop.PropertyType.Equals(typeof(DateTime)))
                            {
                                var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? DateTime.Now : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                                propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.DateTime), null);
                            }
                            if (prop.PropertyType.Equals(typeof(DateTime?)))
                            {
                                var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? null : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                                propertyInfo.SetValue(exFormAsObj, value, null);
                            }
                            if (prop.PropertyType.Equals(typeof(TimeSpan)))
                            {
                                var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? null : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                                propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                            }
                            if (prop.PropertyType.Equals(typeof(double)))
                            {
                                var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? 0 : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                                propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.Double), null);
                            }
                            if (prop.PropertyType.Equals(typeof(float)))
                            {
                                var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? 0 : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                                propertyInfo.SetValue(exFormAsObj, Convert.ToSingle(value), null);
                            }
                            if (prop.PropertyType.Equals(typeof(Decimal)))
                            {
                                var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? 0 : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                                propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.Double), null);
                            }
                            if (prop.PropertyType.Equals(typeof(string)))
                            {
                                var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? "" : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                                propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.String), null);
                            }
                            if (prop.PropertyType.Equals(typeof(bool)))
                            {
                                var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? false : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                                propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, TypeCode.Boolean), null);
                            }
                            if (prop.PropertyType.Equals(typeof(int)))
                            {
                                var value = Data.GetValue(Data.GetOrdinal(NombrePropiedad)) is System.DBNull ? 0 : Data.GetValue(Data.GetOrdinal(NombrePropiedad));
                                propertyInfo.SetValue(exFormAsObj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exceptions.DarkExceptionSystem(string.Format("The attribute'{0}' has an error, {1}", prop.Name, ex.Message));
                        }
                    }
                }
                Response.Add((T)exFormAsObj);
            }
            Data.Close();

            return Response;
        }
        private DarkTable GetClassAttribute()
        {
            DarkTable tableDefinifiton = (DarkTable)Attribute.GetCustomAttribute(typeof(T), typeof(DarkTable));

            if (tableDefinifiton == null)
            {
                throw new Exceptions.DarkExceptionSystem(string.Format("The attribute was not found in the class '{0}'.", typeof(T).Name));
            }
            return tableDefinifiton;
        }
        public string GetLastMessage()
        {
            return dBConnection.mensaje;
        }
    }
}
