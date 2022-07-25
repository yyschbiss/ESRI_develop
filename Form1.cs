using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesRaster;
using ESRI.ArcGIS.Carto;
namespace DXApplication1
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public Form1()
        {
            ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.EngineOrDesktop);
            ESRI.ArcGIS.RuntimeManager.BindLicense(ESRI.ArcGIS.ProductCode.Engine);
            InitializeComponent();
        }

        public string[] OpenShapeFile()
        {
            string[] ShpFile = new string[2];
            OpenFileDialog OpenShpFile = new OpenFileDialog();
            OpenShpFile.Title = "打开Shape文件";
            OpenShpFile.InitialDirectory = "E:";
            OpenShpFile.Filter = "Shape文件(*.shp)|*.shp";

            if (OpenShpFile.ShowDialog() == DialogResult.OK)
            {
                string ShapPath = OpenShpFile.FileName;
                //利用"\\"将文件路径分成两部分
                int Position = ShapPath.LastIndexOf("\\");

                string FilePath = ShapPath.Substring(0, Position);
                string ShpName = ShapPath.Substring(Position + 1);
                ShpFile[0] = FilePath;

                ShpFile[1] = ShpName;

            }
            return ShpFile;
        }
        /*用于分割文件路径和文件名
         * 返回一个string数组；
         * 索引0为FilePath
         * 索引1为FileName
        */
        private string[] splitThePath(string path)
        {
            string[] PathInfo = new string[2];
            //利用"\\"将文件路径分成两部分
            int Position = path.LastIndexOf("\\");

            string FilePath = path.Substring(0, Position);
            string FileName = path.Substring(Position + 1);
            PathInfo[0] = FilePath;

            PathInfo[1] = FileName;
            return PathInfo;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {

        }

        private void barButtonItem1_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem35_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private void barButtonItem33_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutUS aboutusform = new AboutUS();
            aboutusform.Show();
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        private void barButtonItem1_ItemClick_2(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFiledialog;
            openFiledialog = new OpenFileDialog();
            openFiledialog.Title = "打开地图文档文件";
            openFiledialog.Filter = "地图文档(.mxd)|*.mxd";
            if (openFiledialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFiledialog.FileName;
                if (axMapControl1.CheckMxFile(filePath))
                {
                    axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerHourglass;
                    axMapControl1.LoadMxFile(filePath, 0, Type.Missing);
                    axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerDefault;
                    axMapControl1.Extent = axMapControl1.FullExtent;
                }
                else
                {
                    MessageBox.Show(filePath + "不是有效的地图文档");
                }
            }
        }

        private void barButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            string[] FileInfo = OpenShapeFile();
            try
            {
                axMapControl1.AddShapeFile(FileInfo[0], FileInfo[1]);
            }
            catch (Exception excep)
            {
                MessageBox.Show("添加shp文件失败" + excep.ToString());
            }

        }

        // 打开栅格图像的辅助函数
        private IRasterWorkspace GetRasterWorkspace(string pWsName)
        {
            try
            {
                IWorkspaceFactory pWorkFact = new RasterWorkspaceFactoryClass();
                return pWorkFact.OpenFromFile(pWsName, 0) as IRasterWorkspace;
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建工作空间失败" + ex.ToString());
                return null;
            }
        }

        private IRasterDataset OpenFileRasterDataset(string pFolderName, string pFileName)
        {
            IRasterWorkspace pRasterWorkspace = GetRasterWorkspace(pFolderName);
            IRasterDataset pRasterDataset = pRasterWorkspace.OpenRasterDataset(pFileName);
            return pRasterDataset;
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string[] tifFile = new string[2];
            System.Windows.Forms.OpenFileDialog openFiledialog;
            openFiledialog = new OpenFileDialog();
            openFiledialog.Title = "打开Landsat图像";
            openFiledialog.Filter = "卫星影像(.tif)|*.tif";
            if (openFiledialog.ShowDialog() == DialogResult.OK)
            {
                string[] FileInfo = new string[2];
                FileInfo = splitThePath(openFiledialog.FileName);
                IRasterDataset tifdataset =  OpenFileRasterDataset(FileInfo[0],FileInfo[1]);
                IRasterLayer rasterlayer = new RasterLayerClass();
                rasterlayer.CreateFromDataset(tifdataset);
                axMapControl1.Map.AddLayer(rasterlayer as ILayer);
                axMapControl1.Extent = axMapControl1.FullExtent;
            }
            else
            {
                MessageBox.Show("文件打开出错");
            }
        }
    }
}
