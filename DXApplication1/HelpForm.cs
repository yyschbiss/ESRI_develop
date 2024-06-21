using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace DXApplication1
{
    public partial class HelpForm : DevExpress.XtraEditors.XtraForm
    {
        public HelpForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void HelpForm_Load(object sender, EventArgs e)
        {
            this.richTextBox1.Enabled = false;
        }
    }
}