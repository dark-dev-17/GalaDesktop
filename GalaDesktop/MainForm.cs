using GalaManager1;
using GalaManager1.Models;
using GalaManager1.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GalaDesktop
{
    public partial class MainForm : Form
    {
        #region Atributos de gala
        private Gala Gala;
        private Comprador comprador;
        private List<Abono> ListaAbonos;
        private List<Venta> ListaVentas;
        private List<Comprador> ListaCompradores;
        private List<int> ListaAbonosEliminados;
        private List<int> ListaVentaEliminados;
        private List<int> ListaCompradorEliminados;
        #endregion

        #region Atributos de sistema
        private CultureInfo culture;
        private string specifier;
        #endregion


        public MainForm()
        {
            InitializeComponent();

            #region Gala Startup
            Gala = new Gala();
            Gala.OpenConnection();
            Gala.LoadObject(GalaObject.Comprador);
            Gala.LoadObject(GalaObject.Abono);
            Gala.LoadObject(GalaObject.ColeccionDato);
            Gala.LoadObject(GalaObject.Producto);
            Gala.LoadObject(GalaObject.Venta);
            Gala.LoadObject(GalaObject.viewMaxVentacomprador);
            #endregion


            specifier = "C";
            culture = CultureInfo.CreateSpecificCulture("eu-ES");

            loadData();
        }

        private void loadData()
        {
            Funciones.ComboRender(cmb_Comprador, Gala.Comprador.GetOpenquery("Where activo = 1", "order by nombrecompleto asc"), "IdComprador", "NombreCompleto");
            ClientesRefresh(true);
        }
        public void LoadDashboard()
        {
            this.chart1.Series.Clear();

            // Data arrays
            string[] seriesArray = { "Venta", "Abono" };
            object venta = Gala.Venta.GetColun($"select sum(Monto) from venta where Creado between '{dateTimePicker1.Value.ToString("yyyy-MM-dd")}' and '{dateTimePicker2.Value.ToString("yyyy-MM-dd")}'");
            object Abono = Gala.Abono.GetColun($"select sum(Monto) from Abono where Creado between '{dateTimePicker1.Value.ToString("yyyy-MM-dd")}' and '{dateTimePicker2.Value.ToString("yyyy-MM-dd")}'");
            float[] pointsArray = { float.Parse(venta +""), float.Parse(Abono + "") };
            // Set palette
            this.chart1.Palette = ChartColorPalette.EarthTones;
            // Set title
            this.chart1.Titles.Add("Animals");
            // Add series.
            for (int i = 0; i < seriesArray.Length; i++)
            {
                Series series = this.chart1.Series.Add(seriesArray[i]);
                series.Points.Add(pointsArray[i]);
            }

            var re_maximos = Gala.viewMaxVentacomprador.Get();
            re_maximos.ForEach(a => {
                dataGridView4.Rows.Add(new object[] {
                    a.NombreCompleto,
                    a.Monto
                });
            });
        }
        private void button1_Click(object sender, EventArgs e)
        {
            RefreshAbonos(true);
        }

        private void abono_btn_edit_Click(object sender, EventArgs e)
        {
            
        }

        private void RefreshAbonos(bool ForceGet = false)
        {
            if(ForceGet == true)
            {
                comprador = Gala.Comprador.Get((int)cmb_Comprador.SelectedValue);
            }

            if (comprador != null)
            {
                #region Abonos
                dataGridView1.Rows.Clear();
                if (ForceGet == true)
                {
                    ListaAbonos = Gala.Abono.GetOpenquery($"where IdComprador = {cmb_Comprador.SelectedValue} and eliminado = 0", "order by Creado desc");
                }


                ListaAbonos.ForEach(abono => {
                    dataGridView1.Rows.Add(new object[] {
                        abono.IdAbono + "",
                        abono.Monto + "",
                        abono.Comentarios,
                        null,
                        abono.Creado,
                        abono.Modificado,
                        abono.IdTipo
                    });
                });

                var re_TipoAbono = Gala.ColeccionDato.Get("1", "IdColeccion");
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    var row = dataGridView1.Rows[i];
                    if (!string.IsNullOrEmpty((string)row.Cells[0].Value))
                    {

                        DataGridViewComboBoxCell cbCell = (DataGridViewComboBoxCell)row.Cells[3];
                        cbCell.DataSource = re_TipoAbono;
                        cbCell.DisplayMember = "Descripcion";
                        cbCell.ValueMember = "IdColeccionDato";
                        if (row.Cells[6].Value != null && row.Cells[6].Value + "" != "0")
                        {
                            cbCell.Value = Int32.Parse(row.Cells[6].Value + "");
                        }

                    }

                }

                Abo_txt_respuesta.Text = $"Total de registros {ListaAbonos.Count}";
                label2.Text = $"Abonado: ${ListaAbonos.Sum(a => a.Monto).ToString(specifier, culture)}";
                #endregion

                #region Venta
                dataGridView2.Rows.Clear();
                if (ForceGet == true)
                {
                    ListaVentas = Gala.Venta.GetOpenquery($"where IdComprador = {cmb_Comprador.SelectedValue} and eliminado = 0", "order by Creado desc");
                }
                ListaVentas.ForEach(Venta => {
                    dataGridView2.Rows.Add(new object[] { 
                        Venta.IdVenta,
                        Venta.Codigo,
                        Venta.Descripcion,
                        Venta.Monto,
                        Venta.Creado,
                        Venta.Modificado
                    });
                });

                label4.Text = $"Venta:  ${ListaVentas.Sum(a => a.Monto).ToString(specifier, culture)}";
                #endregion
            }
            else
            {

            }
        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if(ListaAbonosEliminados is null)
            {
                ListaAbonosEliminados = new List<int>();
            }
            string id = (string)e.Row.Cells[0].Value;
            if (!string.IsNullOrEmpty(id))
            {
                ListaAbonosEliminados.Add(Int32.Parse(id));
            }
                
        }

        private void dataGridView1_ControlAdded(object sender, ControlEventArgs e)
        {
            var re_TipoAbono = Gala.ColeccionDato.Get("1", "IdColeccion");
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                var row = dataGridView1.Rows[i];
                if (string.IsNullOrEmpty((string)row.Cells[1].Value) || string.IsNullOrEmpty((string)row.Cells[2].Value))
                {
                    DataGridViewComboBoxCell cbCell = (DataGridViewComboBoxCell)row.Cells[3];
                    cbCell.DataSource = re_TipoAbono;
                    cbCell.DisplayMember = "Descripcion";
                    cbCell.ValueMember = "IdColeccionDato";
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine($"Terminando de editar {e.RowIndex} {e.ColumnIndex}");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshAbonos();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult pregunta = MessageBox.Show("¿Deseas guardar los cambios de realizados?", "Gala", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (pregunta == DialogResult.Yes)
            {
                Gala.StartTransaction();
                try
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        var row = dataGridView1.Rows[i];
                        //Console.WriteLine($"id: {row.Cells[0].Value} monto: {row.Cells[1].Value}");
                        if (string.IsNullOrEmpty((string)row.Cells[0].Value))
                        {
                            //validar para insertar nuevo
                            if (!string.IsNullOrEmpty((string)row.Cells[1].Value) || !string.IsNullOrEmpty((string)row.Cells[2].Value))
                            {
                                if (string.IsNullOrEmpty((string)row.Cells[1].Value))
                                {
                                    row.Cells[0].Style.BackColor = Color.Red;
                                    throw new GalaManager1.Exceptions.Excep
                                    {
                                        ErrorCode = 310,
                                        Description = "Error al guardar, por favor revisa los campos en rojo",
                                        Category = GalaManager1.Exceptions.TypeException.Error
                                    };
                                }
                                Abono abono = new Abono();
                                abono.Monto = float.Parse((string)row.Cells[1].Value);
                                abono.Comentarios = (string)row.Cells[2].Value;
                                abono.IdTipo = Int32.Parse((string)row.Cells[3].Value);
                                abono.Creado = DateTime.Now;
                                abono.Modificado = DateTime.Now;
                                abono.IdComprador = comprador.IdComprador;
                                Gala.Abono.Element = abono;
                                if (!Gala.Abono.Add())
                                {
                                    throw new GalaManager1.Exceptions.Excep
                                    {
                                        ErrorCode = 300,
                                        Description = "Error al guardar",
                                        Category = GalaManager1.Exceptions.TypeException.Error
                                    };
                                }
                            }
                        }
                        else
                        {
                            //validar para editar actual
                            var re_abono = Gala.Abono.GetString((string)row.Cells[0].Value);
                            if(re_abono != null)
                            {
                                var dato = string.IsNullOrEmpty(row.Cells[3].Value + "") ? "0" : row.Cells[3].Value + "";
                                if (re_abono.Monto != float.Parse((string)row.Cells[1].Value) || re_abono.Comentarios != (string)row.Cells[2].Value || re_abono.IdTipo != Int32.Parse(dato))
                                {
                                    
                                    re_abono.Monto = float.Parse((string)row.Cells[1].Value);
                                    re_abono.Comentarios = (string)row.Cells[2].Value;
                                    re_abono.IdTipo = Int32.Parse(dato);
                                    re_abono.Modificado = DateTime.Now;
                                    Gala.Abono.Element = re_abono;
                                    if (!Gala.Abono.Update())
                                    {
                                        throw new GalaManager1.Exceptions.Excep
                                        {
                                            ErrorCode = 200,
                                            Description = "Error al guardar",
                                            Category = GalaManager1.Exceptions.TypeException.Error
                                        };
                                    }
                                }
                            }
                        }
                    }

                    //eliminar eliminados
                    if(ListaAbonosEliminados != null && ListaAbonosEliminados.Count > 0)
                    {
                        ListaAbonosEliminados.ForEach(ab => {
                            var re_abono = Gala.Abono.Get(ab);
                            if (re_abono != null)
                            {
                                re_abono.Eliminado = true;
                                re_abono.Modificado = DateTime.Now;
                                Gala.Abono.Element = re_abono;
                                if (!Gala.Abono.Delete())
                                {
                                    throw new GalaManager1.Exceptions.Excep
                                    {
                                        ErrorCode = 100,
                                        Description = "Error al guardar",
                                        Category = GalaManager1.Exceptions.TypeException.Error
                                    };
                                }
                            }
                        });
                    }
                    RefreshAbonos(true);
                    if (ListaAbonosEliminados != null)
                    {
                        ListaAbonosEliminados.Clear();
                    }
                    Gala.Commit();
                }
                catch (GalaManager1.Exceptions.Excep ex)
                {
                    Gala.RollBack();
                    Gala.ReOpenConection();
                    if (ex.Category == GalaManager1.Exceptions.TypeException.Info)
                        MessageBox.Show(ex.Message, "Gala information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (ex.Category == GalaManager1.Exceptions.TypeException.Error)
                        MessageBox.Show(ex.Message, "Gala error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView2_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (ListaVentaEliminados is null)
            {
                ListaVentaEliminados = new List<int>();
            }
            string id = e.Row.Cells[0].Value + "";
            if (!string.IsNullOrEmpty(id))
            {
                ListaVentaEliminados.Add(Int32.Parse(id));
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult pregunta = MessageBox.Show("¿Deseas guardar los cambios de realizados?", "Gala", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (pregunta == DialogResult.Yes)
            {
                Gala.StartTransaction();
                try
                {
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {
                        var row = dataGridView2.Rows[i];
                        if (row.Cells[0].Value is null)
                        {
                            //crear
                            if (row.Cells[3].Value != null ||row.Cells[2].Value != null ||row.Cells[1].Value != null  )
                            {
                                if (row.Cells[3].Value is null)
                                {
                                    row.Cells[3].Style.BackColor = Color.Red;
                                    throw new GalaManager1.Exceptions.Excep
                                    {
                                        ErrorCode = 310,
                                        Description = "Error al guardar, por favor revisa los campos en rojo",
                                        Category = GalaManager1.Exceptions.TypeException.Error
                                    };
                                }

                                Venta venta = new Venta();
                                venta.Codigo = (string)row.Cells[1].Value;
                                venta.Descripcion = (string)row.Cells[2].Value;
                                venta.Monto = float.Parse(row.Cells[3].Value + "");
                                venta.Creado = DateTime.Now;
                                venta.Modificado = DateTime.Now;
                                venta.IdComprador = comprador.IdComprador;
                                Gala.Venta.Element = venta;
                                if (!Gala.Venta.Add())
                                {
                                    throw new GalaManager1.Exceptions.Excep
                                    {
                                        ErrorCode = 300,
                                        Description = "Error al guardar",
                                        Category = GalaManager1.Exceptions.TypeException.Error
                                    };
                                }
                            }
                            
                        }
                        else
                        {
                            //editar
                            var re_venta = Gala.Venta.Get((int)row.Cells[0].Value);
                            if (re_venta != null)
                            {
                                var dato = string.IsNullOrEmpty(row.Cells[3].Value + "") ? "0" : row.Cells[3].Value + "";
                                if (re_venta.Monto != (float)row.Cells[3].Value || re_venta.Descripcion != (string)row.Cells[2].Value || re_venta.Codigo != (string)row.Cells[1].Value)
                                {
                                    re_venta.Codigo = (string)row.Cells[1].Value;
                                    re_venta.Descripcion = (string)row.Cells[2].Value;
                                    re_venta.Monto = float.Parse(row.Cells[3].Value + "");
                                    re_venta.Modificado = DateTime.Now;
                                    Gala.Venta.Element = re_venta;
                                    if (!Gala.Venta.Update())
                                    {
                                        throw new GalaManager1.Exceptions.Excep
                                        {
                                            ErrorCode = 200,
                                            Description = "Error al guardar",
                                            Category = GalaManager1.Exceptions.TypeException.Error
                                        };
                                    }
                                }
                            }
                        }
                    }

                    if (ListaVentaEliminados != null && ListaVentaEliminados.Count > 0)
                    {
                        ListaVentaEliminados.ForEach(ab => {
                            var re_venta = Gala.Venta.Get(ab);
                            if (re_venta != null)
                            {
                                re_venta.Eliminado = true;
                                re_venta.Modificado = DateTime.Now;
                                Gala.Venta.Element = re_venta;
                                if (!Gala.Venta.Delete())
                                {
                                    throw new GalaManager1.Exceptions.Excep
                                    {
                                        ErrorCode = 100,
                                        Description = "Error al guardar",
                                        Category = GalaManager1.Exceptions.TypeException.Error
                                    };
                                }
                            }
                        });
                    }
                    RefreshAbonos(true);
                    if(ListaVentaEliminados != null)
                    {
                        ListaVentaEliminados.Clear();
                    }
                    
                    Gala.Commit();
                }
                catch (GalaManager1.Exceptions.Excep ex)
                {
                    Gala.RollBack();
                    Gala.ReOpenConection();
                    if (ex.Category == GalaManager1.Exceptions.TypeException.Info)
                        MessageBox.Show(ex.Message, "Gala information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (ex.Category == GalaManager1.Exceptions.TypeException.Error)
                        MessageBox.Show(ex.Message, "Gala error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView2_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var producto = Gala.Producto.GetOpenquery($"where codigo like '%{dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value}%' limit 1");
            if(producto != null)
            {
                var row = dataGridView2.Rows[e.RowIndex];
                row.Cells[2].Value = producto.Descripcion;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DialogResult pregunta = MessageBox.Show("¿Deseas guardar los cambios de realizados?", "Gala", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (pregunta == DialogResult.Yes)
            {
                Gala.StartTransaction();
                try
                {
                    for (int i = 0; i < dataGridView3.Rows.Count; i++)
                    {
                        var row = dataGridView3.Rows[i];
                        if (row.Cells[0].Value is null)
                        {
                            if (row.Cells[1].Value != null)
                            {
                                var re_coprador = new Comprador();
                                re_coprador.NombreCompleto = (string)row.Cells[1].Value;
                                re_coprador.Activo = true;
                                Gala.Comprador.Element = re_coprador;
                                if (!Gala.Comprador.Add())
                                {
                                    throw new GalaManager1.Exceptions.Excep
                                    {
                                        ErrorCode = 100,
                                        Description = "Error al guardar",
                                        Category = GalaManager1.Exceptions.TypeException.Error
                                    };
                                }
                            }
                        }
                        else
                        {
                            var re_coprador = Gala.Comprador.Get((int)row.Cells[0].Value);
                            if(re_coprador != null)
                            {
                                re_coprador.NombreCompleto = (string)row.Cells[1].Value;
                                Gala.Comprador.Element = re_coprador;
                                if (!Gala.Comprador.Update())
                                {
                                    throw new GalaManager1.Exceptions.Excep
                                    {
                                        ErrorCode = 100,
                                        Description = "Error al guardar",
                                        Category = GalaManager1.Exceptions.TypeException.Error
                                    };
                                }
                            }
                        }
                    }
                    if (ListaCompradorEliminados != null && ListaCompradorEliminados.Count > 0)
                    {
                        ListaCompradorEliminados.ForEach(ab => {
                            var re_venta = Gala.Comprador.Get(ab);
                            if (re_venta != null)
                            {
                                re_venta.Activo = false;
                                Gala.Comprador.Element = re_venta;
                                if (!Gala.Comprador.Delete())
                                {
                                    throw new GalaManager1.Exceptions.Excep
                                    {
                                        ErrorCode = 100,
                                        Description = "Error al guardar",
                                        Category = GalaManager1.Exceptions.TypeException.Error
                                    };
                                }
                            }
                        });
                    }
                    ClientesRefresh(true);
                    if (ListaCompradorEliminados != null)
                    {
                        ListaCompradorEliminados.Clear();
                    }
                    Funciones.ComboRender(cmb_Comprador, Gala.Comprador.GetOpenquery("Where activo = 1","order by nombrecompleto asc"), "IdComprador", "NombreCompleto");
                    Gala.Commit();
                }
                catch (GalaManager1.Exceptions.Excep ex)
                {
                    Gala.RollBack();
                    Gala.ReOpenConection();
                    if (ex.Category == GalaManager1.Exceptions.TypeException.Info)
                        MessageBox.Show(ex.Message, "Gala information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (ex.Category == GalaManager1.Exceptions.TypeException.Error)
                        MessageBox.Show(ex.Message, "Gala error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ClientesRefresh(true);
        }

        private void ClientesRefresh(bool refreshForce = false)
        {
            if (refreshForce)
            {
                ListaCompradores = Gala.Comprador.GetOpenquery("where activo = 1", "order by nombrecompleto asc");
            }

            if(ListaCompradores != null)
            {
                dataGridView3.Rows.Clear();

                ListaCompradores.ForEach(Venta => {
                    dataGridView3.Rows.Add(new object[] {
                        Venta.IdComprador,
                        Venta.NombreCompleto,
                        Venta.Activo
                    });
                });
            }
        }

        private void dataGridView3_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (ListaCompradorEliminados is null)
            {
                ListaCompradorEliminados = new List<int>();
            }
            string id = e.Row.Cells[0].Value + "";
            if (!string.IsNullOrEmpty(id))
            {
                ListaCompradorEliminados.Add(Int32.Parse(id));
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ClientesRefresh();        
        }

        private void button9_Click(object sender, EventArgs e)
        {
            LoadDashboard();
        }
    }
}
