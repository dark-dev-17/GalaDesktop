using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GalaManager1.Tools
{
    public static class Funciones
    {
        public static void ComboRender(ComboBox comboBox, object Data, string key, string Value, int Selected = 0)
        {
            comboBox.DataSource = new BindingSource(Data, null);
            comboBox.DisplayMember = Value;
            comboBox.ValueMember = key;
            comboBox.SelectedIndex = 0;
        }
    }
}
