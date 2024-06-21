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
    public partial class SheetViewer : DevExpress.XtraEditors.XtraForm
    {
        public SheetViewer()
        {
            InitializeComponent();
        }

        private void SheetViewer_Load(object sender, EventArgs e)
        {
            string docPath = "../../../Resources/sheet/cost.xlsx";
            spreadsheetControl1.LoadDocument(docPath);
        }
    }
}