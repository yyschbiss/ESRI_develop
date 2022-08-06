using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DXApplication1
{
    public partial class ImageViewer : Form
    {
        private string nowShow = "";
        private string nowPath = "";
        private int nowCount = 0;
        public string[] fileNames = { "IsBasecase", "IsRCP26", "IsRCP45", "IsRCP85" };

        public ImageViewer()
        {
            InitializeComponent();
        }
        public ImageViewer(string nowShowItem)
        {
            nowShow = nowShowItem;
            InitializeComponent();
            Myinit();
        }

        private void Myinit()
        {
            switch (nowShow)
            {
                case "Is":
                    fileNames[0] = "IsBaseline";
                    fileNames[1] = "IsRCP26";
                    fileNames[2] = "IsRCP45";
                    fileNames[3] = "IsRCP85";
                    break;
                case "Ir":
                    fileNames[0] = "IrBaseline";
                    fileNames[1] = "IrRCP26";
                    fileNames[2] = "IrRCP45";
                    fileNames[3] = "IrRCP85";
                    break;
                case "Ia":
                    fileNames[0] = "IaBaseline";
                    fileNames[1] = "IaRCP26";
                    fileNames[2] = "IaRCP45";
                    fileNames[3] = "IaRCP85";
                    break;
                case "Ic":
                    fileNames[0] = "IcBaseline";
                    fileNames[1] = "IcRCP26";
                    fileNames[2] = "IcRCP45";
                    fileNames[3] = "IcRCP85";
                    break;
                default:
                    break;
            }
            //暂时有bug不进行显示
            AutoSizeBtn.Visible = false;
            ZoomInBtn.Visible = false;
            ZoomOutBtn.Visible = false;
            //已废弃
            ShowPicBtn.Visible = false;

            nowPath = "../../../Resources/mapResult/" + fileNames[nowCount] + ".png";
            pictureBox1.Load(nowPath);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Refresh();

        }
        private void showPic()
        {
            nowPath = "../../../Resources/mapResult/" + fileNames[nowCount] + ".png";
            pictureBox1.Load(nowPath);
        }

        private void ShowPicBtn_Click(object sender, EventArgs e)
        {

            
        }

        private void LastPicBtn_Click(object sender, EventArgs e)
        {
            if (nowCount > 0)
            {
                nowCount--;
                showPic();
            }
            else
                MessageBox.Show("已经是第一张了");
        }

        private void NextPicBtn_Click(object sender, EventArgs e)
        {
            if (nowCount >= 0 && nowCount < 3)
            {
                nowCount++;
                showPic();
            }
            else
                MessageBox.Show("已经是最后一张了");
        }

        private void ZoomInBtn_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            Image img = pictureBox1.Image;
            Size size = new Size(img.Width + 100, img.Height + 100);

            Bitmap bm = new Bitmap(img, size);
            Graphics g = Graphics.FromImage(bm);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            pictureBox1.Image = bm;
        }

        private void ZoomOutBtn_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            Image img = pictureBox1.Image;
            Size size = new Size(img.Width + 100, img.Height + 100);

            Bitmap bm = new Bitmap(img, size);
            Graphics g = Graphics.FromImage(bm);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

            pictureBox1.Image = bm;
        }

        private void AutoSizeBtn_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Refresh();
        }
    }
}
