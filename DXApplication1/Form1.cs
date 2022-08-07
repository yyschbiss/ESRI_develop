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
using ESRI.ArcGIS.Geometry;
using GISEditor.EditTool.BasicClass;
using GISEditor.EditTool.Tool;
using GISEditor.EditTool.Command;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Output;


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
        #region 常量

        // 一系列为了TocControl右键菜单实现的全局变量
        // ------------------------------------
        // 存储所点击的要素类型
        esriTOCControlItem pItem = esriTOCControlItem.esriTOCControlItemNone;
        // 地图对象
        IBasicMap ptocMap = null;
        // 图层对象
        ILayer pLayer = null;
        object unk = null;
        object data = null;
        // 点击的要素图层
        IFeatureLayer pTocFeatureLayer = null;
        Attribute_Form frmAttribute = null;

        //-----------------------------------------
        //一系列为了编辑功能定义的全局变量
        private string sMxdPath = Application.StartupPath;
        private IMap pMap = null;
        private IActiveView pActiveView = null;
        private List<ILayer> plstLayers = null;
        private IFeatureLayer pCurrentLyr = null;
        private IEngineEditor pEngineEditor = null;
        private IEngineEditTask pEngineEditTask = null;
        private IEngineEditLayers pEngineEditLayers = null;
        private bool EditBarVisibale = false;

        //-----------------------------------------





        #endregion

        #region 文件操作
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
            toolStrip1.Visible = EditBarVisibale;
            ChangeButtonState(false);
        }

        #endregion

        #region 杂项

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
            ImageViewer IsViewer = new ImageViewer("Is");
            IsViewer.Show();
        }

        private void barButtonItem33_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AboutUS aboutusform = new AboutUS();
            aboutusform.Show();
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }

        //ENVI操作
        private void barButtonItem24_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //初始化ENVI

            COM_IDL_connectLib.COM_IDL_connectClass oComIDL = new COM_IDL_connectLib.COM_IDL_connectClass();

            oComIDL.CreateObject(0, 0, 0);

            //文件打开

            OpenFileDialog pOpenFile = new OpenFileDialog();

            pOpenFile.Title = "打开栅格文件";

            //文件选择

            if (pOpenFile.ShowDialog() == DialogResult.OK)

                //打开显示栅格文件

                OpenRaster(pOpenFile.FileName);

            //调用ENVI进行栅格放大*2处理示例

            SaveFileDialog pSaveFile = new SaveFileDialog();

            pSaveFile.Title = "输出放大后影像";

            if (pSaveFile.ShowDialog() == DialogResult.OK)
            {

                //执行重采样

                oComIDL.ExecuteString(".compile '" + System.IO.Directory.GetCurrentDirectory() + @"\IDLmctk.pro'");

                //oComIDL.ExecuteString(@"s = obj_new('object_envi_resize','" + pOpenFile.FileName + "','" + pSaveFile.FileName + "')");

                oComIDL.ExecuteString("s.EXECUTERESIZE,2,2,0");

                oComIDL.ExecuteString("Obj_destroy,s");

                //加载放大后影像

                OpenRaster(pSaveFile.FileName);

            }

        }

        //TocControls右键菜单查看属性表
        private void hellpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // frmAttribute是类里面定义的全局变量，也就是属性表窗体
            if (frmAttribute == null || frmAttribute.IsDisposed)
            {
                frmAttribute = new Attribute_Form();
            }
            // 一定要记得将要素图层赋值给属性表窗体
            frmAttribute.CurFeatureLayer = pTocFeatureLayer;
            // 初始化窗体
            frmAttribute.InitUI();
            // 显示窗体
            frmAttribute.ShowDialog();

        }

        private void barButtonItem19_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        //指针
        private void barButtonItem51_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        #endregion

        #region 加载文件
        //打开MXD文档
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

        //打开Shapefile
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

        //加载landsat
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
                IRasterDataset tifdataset = OpenFileRasterDataset(FileInfo[0], FileInfo[1]);
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

        private void OpenRaster(string rasterFileName)
        {

            //文件名处理

            string ws = System.IO.Path.GetDirectoryName(rasterFileName);

            string fbs = System.IO.Path.GetFileName(rasterFileName);

            //创建工作空间

            IWorkspaceFactory pWork = new RasterWorkspaceFactoryClass();

            //打开工作空间路径，工作空间的参数是目录，不是具体的文件名

            IRasterWorkspace pRasterWS = (IRasterWorkspace)pWork.OpenFromFile(ws, 0);

            //打开工作空间下的文件，

            IRasterDataset pRasterDataset = pRasterWS.OpenRasterDataset(fbs);

            IRasterLayer pRasterLayer = new RasterLayerClass();

            pRasterLayer.CreateFromDataset(pRasterDataset);

            //添加到图层控制中

            axMapControl1.Map.AddLayer(pRasterLayer as ILayer);

        }


        #endregion

        #region 鹰眼
        // 进行鹰眼功能的实现
        //------------------

        private void axMapControl1_OnMapReplaced_1(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMapReplacedEvent e)
        {
            this.axMapControl2.Map = new MapClass();
            for (int i = 1; i <= this.axMapControl1.LayerCount; i++)// 添加主地图控件中的所有图层到鹰眼控件中
            {
                this.axMapControl2.AddLayer(this.axMapControl1.get_Layer(this.axMapControl1.LayerCount - i));
            }
            this.axMapControl2.Extent = this.axMapControl1.FullExtent;// 设置 MapControl 显示范围至数据的全局范围         
            this.axMapControl2.Refresh();// 刷新鹰眼控件地图

            //更新布局视角
            CopyToPageLayout();
        }

        private void axMapControl1_OnExtentUpdated_1(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            // 得到新范围
            IEnvelope pEnv = (IEnvelope)e.newEnvelope;
            IGraphicsContainer pGra = axMapControl2.Map as IGraphicsContainer;
            IActiveView pAv = pGra as IActiveView;
            // 在绘制前，清除 axMapControl2 中的任何图形元素
            pGra.DeleteAllElements();
            IRectangleElement pRectangleEle = new RectangleElementClass();
            IElement pEle = pRectangleEle as IElement;
            pEle.Geometry = pEnv;
            // 设置鹰眼图中的红线框
            IRgbColor pColor = new RgbColorClass();
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            pColor.Transparency = 255;
            // 产生一个线符号对象
            ILineSymbol pOutline = new SimpleLineSymbolClass();
            pOutline.Width = 2;
            pOutline.Color = pColor;
            // 设置颜色属性
            pColor = new RgbColorClass();
            pColor.Red = 255;
            pColor.Green = 0;
            pColor.Blue = 0;
            pColor.Transparency = 0;
            // 设置填充符号的属性
            IFillSymbol pFillSymbol = new SimpleFillSymbolClass();
            pFillSymbol.Color = pColor;
            pFillSymbol.Outline = pOutline;
            IFillShapeElement pFillShapeEle = pEle as IFillShapeElement;
            pFillShapeEle.Symbol = pFillSymbol;
            pGra.AddElement((IElement)pFillShapeEle, 0);
            // 刷新
            pAv.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void axMapControl2_OnMouseDown_1(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            if (this.axMapControl2.Map.LayerCount != 0)
            {
                // 按下鼠标左键移动矩形框
                if (e.button == 1)
                {
                    IPoint pPoint = new PointClass();
                    pPoint.PutCoords(e.mapX, e.mapY);
                    IEnvelope pEnvelope = this.axMapControl1.Extent;
                    pEnvelope.CenterAt(pPoint);
                    this.axMapControl1.Extent = pEnvelope;
                    this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                }
                // 按下鼠标右键绘制矩形框
                else if (e.button == 2)
                {
                    IEnvelope pEnvelop = this.axMapControl2.TrackRectangle();
                    this.axMapControl1.Extent = pEnvelop;

                    this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
                }
            }
        }

        private void axMapControl2_OnMouseMove_1(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseMoveEvent e)
        {
            if (e.button != 1) return;
            IPoint pPoint = new PointClass();
            pPoint.PutCoords(e.mapX, e.mapY);
            this.axMapControl1.CenterAt(pPoint);
            this.axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, null, null);
        }

        // 鹰眼功能的开关
        private void barButtonItem20_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (panel1.Visible == true)
                panel1.Visible = false;
            else
                panel1.Visible = true;
        }

        #endregion

        #region 地图浏览
        //放大
        private void barButtonItem47_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand pZoomIn = new ControlsMapZoomInToolClass();
            pZoomIn.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = pZoomIn as ITool;
            axPageLayoutControl1.CurrentTool = pZoomIn as ITool;
        }

        //缩小
        private void barButtonItem48_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand pZoomOut = new ControlsMapZoomOutToolClass();
            pZoomOut.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = pZoomOut as ITool;
            axPageLayoutControl1.CurrentTool = pZoomOut as ITool;
        }

        //空间测量
        private void barButtonItem52_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand pMapMeature = new ControlsMapMeasureToolClass();
            pMapMeature.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = pMapMeature as ITool;
            axPageLayoutControl1.CurrentTool = pMapMeature as ITool; ;
        }

        //属性获取
        private void barButtonItem53_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand pIdentify = new ControlsMapIdentifyToolClass();
            pIdentify.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = pIdentify as ITool;
            axPageLayoutControl1.CurrentTool = pIdentify as ITool;

        }

        //全图
        private void barButtonItem50_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            axMapControl1.Extent = axMapControl1.FullExtent;
            axPageLayoutControl1.Extent = axPageLayoutControl1.FullExtent;
        }

        //漫游
        private void barButtonItem49_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ICommand pPan = new ControlsMapPanToolClass();
            pPan.OnCreate(axMapControl1.Object);
            axMapControl1.CurrentTool = pPan as ITool;
            axPageLayoutControl1.CurrentTool = pPan as ITool;
        }

        #endregion

        #region TocControl右键菜单
        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            try
            {
                if (e.button == 2)           // 判断是不是右键点击； 1是左键，2是右键
                {

                    // 以引用的方式传递参数，方便将值带出来
                    axTOCControl1.HitTest(e.x, e.y, ref pItem, ref ptocMap, ref pLayer, ref unk, ref data);
                    // 点击的要素图层
                    pTocFeatureLayer = pLayer as IFeatureLayer;
                    // 判断是否是图层要素
                    if (pItem == esriTOCControlItem.esriTOCControlItemLayer && pTocFeatureLayer != null)
                    {
                        // 打开右键菜单，参数为当前鼠标的位置
                        contextMenuStrip1.Show(Control.MousePosition);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void 移除图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (pTocFeatureLayer != null)
            {
                for (int i = 0; i < axMapControl1.LayerCount; i++)
                {
                    if (axMapControl1.get_Layer(i) == pTocFeatureLayer)//通过for循环得到选中图层索引，并直接调用DeleteLayer方法定点删除
                        axMapControl1.DeleteLayer(i);
                }
                axMapControl1.ActiveView.Refresh();
            }

        }
        #endregion

        #region 文件保存
        //另存为
        private void barButtonItem18_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SaveFileDialog pSaveFiledialog = new SaveFileDialog();
                pSaveFiledialog.Title = "另存为";
                pSaveFiledialog.OverwritePrompt = true;
                pSaveFiledialog.Filter = "ArcMap文档（*.mxd）|*.mxd";
                pSaveFiledialog.RestoreDirectory = true;
                if (pSaveFiledialog.ShowDialog() == DialogResult.OK)
                {
                    string sFilePath = pSaveFiledialog.FileName;
                    IMapDocument pMapdocument = new MapDocumentClass();
                    pMapdocument.New(sFilePath);
                    pMapdocument.ReplaceContents(axMapControl1.Map as IMxdContents);
                    // Save参数1：是否保存为相对路径；参数2：是否创建缩略图
                    pMapdocument.Save(true, true);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        //保存
        private void saveBtn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                string sMxdFileName = axMapControl1.DocumentFilename;
                IMapDocument pMapdocument = new MapDocumentClass();
                if (sMxdFileName != null && axMapControl1.CheckMxFile(sMxdFileName))
                {
                    if (pMapdocument.get_IsReadOnly(sMxdFileName))
                    {
                        MessageBox.Show("本文档只读，不能保存。");
                        pMapdocument.Close();
                        return;
                    }
                }
                else
                {
                    SaveFileDialog pSaveFileDialog = new SaveFileDialog();
                    pSaveFileDialog.Title = "请选择保存路径";
                    pSaveFileDialog.OverwritePrompt = true;
                    pSaveFileDialog.Filter = "ArcMap文档（*.mxd）|*.mxd";
                    pSaveFileDialog.RestoreDirectory = true;
                    if (pSaveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        sMxdFileName = pSaveFileDialog.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
                pMapdocument.New(sMxdFileName);
                pMapdocument.ReplaceContents(axMapControl1.Map as IMxdContents);
                pMapdocument.Save(pMapdocument.UsesRelativePaths, true);
                pMapdocument.Close();
                MessageBox.Show("保存地图成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        #endregion

        #region 图层编辑
        // 此处开始图层编辑的一系列函数
        // --------------------------
        private void ChangeButtonState(bool bEnable)
        {
            btnStartEdit.Enabled = !bEnable;
            btnSaveEdit.Enabled = bEnable;
            btnEndEdit.Enabled = bEnable;

            cmbSelLayer.Enabled = bEnable;

            btnSelFeat.Enabled = bEnable;
            btnSelMove.Enabled = bEnable;
            btnAddFeature.Enabled = bEnable;
            btnDelFeature.Enabled = bEnable;
            btnAttributeEdit.Enabled = bEnable;
            btnUndo.Enabled = bEnable;
            btnRedo.Enabled = bEnable;
            btnMoveVertex.Enabled = bEnable;
            btnAddVertex.Enabled = bEnable;
            btnDelVertex.Enabled = bEnable;
        }

        private void InitEditObject()
        {
            try
            {
                ChangeButtonState(false);
                pEngineEditor = new EngineEditorClass();
                MapManager.EngineEditor = pEngineEditor;
                pEngineEditTask = pEngineEditor as IEngineEditTask;
                pEngineEditLayers = pEngineEditor as IEngineEditLayers;

                pMap = axMapControl1.Map;
                pActiveView = pMap as IActiveView;
                plstLayers = MapManager.GetLayers(pMap);
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void InitComboBox(List<ILayer> plstLyr)
        {
            cmbSelLayer.Items.Clear();
            for (int i = 0; i < plstLyr.Count; i++)
            {
                if (!cmbSelLayer.Items.Contains(plstLyr[i].Name))
                {
                    cmbSelLayer.Items.Add(plstLyr[i].Name);
                }
            }
            if (cmbSelLayer.Items.Count != 0) cmbSelLayer.SelectedIndex = 0;
        }

        //编辑模式开关
        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InitEditObject();
            if (EditBarVisibale == false)
            {
                toolStrip1.Visible = true;
                EditBarVisibale = true;
            }
            else
            {
                EditBarVisibale = false;
                toolStrip1.Visible = false;
            }
        }


        private void btnStartEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (plstLayers == null || plstLayers.Count == 0)
                {
                    MessageBox.Show("请加载编辑图层！", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                pMap.ClearSelection();
                pActiveView.Refresh();
                InitComboBox(plstLayers);
                ChangeButtonState(true);
                //如果编辑已经开始，则直接退出
                if (pEngineEditor.EditState != esriEngineEditState.esriEngineStateNotEditing)
                    return;
                if (pCurrentLyr == null) return;
                //获取当前编辑图层工作空间
                IDataset pDataSet = pCurrentLyr.FeatureClass as IDataset;
                IWorkspace pWs = pDataSet.Workspace;
                //设置编辑模式，如果是ArcSDE采用版本模式
                if (pWs.Type == esriWorkspaceType.esriRemoteDatabaseWorkspace)
                {
                    pEngineEditor.EditSessionMode = esriEngineEditSessionMode.esriEngineEditSessionModeVersioned;
                }
                else
                {
                    pEngineEditor.EditSessionMode = esriEngineEditSessionMode.esriEngineEditSessionModeNonVersioned;
                }
                //设置编辑任务
                pEngineEditTask = pEngineEditor.GetTaskByUniqueName("ControlToolsEditing_CreateNewFeatureTask");
                pEngineEditor.CurrentTask = pEngineEditTask;// 设置编辑任务
                pEngineEditor.EnableUndoRedo(true); //是否可以进行撤销、恢复操作
                pEngineEditor.StartEditing(pWs, pMap); //开始编辑操作
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void btnSaveEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ICommand m_saveEditCom = new SaveEditCommandClass();
                m_saveEditCom.OnCreate(axMapControl1.Object);
                m_saveEditCom.OnClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void btnEndEdit_Click(object sender, EventArgs e)
        {
            try
            {
                ICommand m_stopEditCom = new StopEditCommandClass();
                m_stopEditCom.OnCreate(axMapControl1.Object);
                m_stopEditCom.OnClick();
                ChangeButtonState(false);
                axMapControl1.CurrentTool = null;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerDefault;
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void btnSelFeat_ItemClick(object sender, EventArgs e)
        {
            try
            {
                ICommand m_SelTool = new SelectFeatureToolClass();
                m_SelTool.OnCreate(axMapControl1.Object);
                m_SelTool.OnClick();

                axMapControl1.CurrentTool = m_SelTool as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void btnSelMove_ItemClick(object sender, EventArgs e)
        {
            try
            {
                ICommand m_moveTool = new MoveFeatureToolClass();
                m_moveTool.OnCreate(axMapControl1.Object);
                m_moveTool.OnClick();
                axMapControl1.CurrentTool = m_moveTool as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void btnUndo_ItemClick(object sender, EventArgs e)
        {
            try
            {
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
                ICommand m_undoCommand = new UndoCommandClass();
                m_undoCommand.OnCreate(axMapControl1.Object);
                m_undoCommand.OnClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void btnRedo_ItemClick(object sender, EventArgs e)
        {
            try
            {
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
                ICommand m_redoCommand = new RedoCommandClass();
                m_redoCommand.OnCreate(axMapControl1.Object);
                m_redoCommand.OnClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void btnAddFeature_ItemClick(object sender, EventArgs e)
        {
            try
            {
                ICommand m_CreateFeatTool = new CreateFeatureToolClass();
                m_CreateFeatTool.OnCreate(axMapControl1.Object);
                m_CreateFeatTool.OnClick();
                axMapControl1.CurrentTool = m_CreateFeatTool as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void btnDelFeature_ItemClick(object sender, EventArgs e)
        {
            try
            {
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
                ICommand m_delFeatCom = new DelFeatureCommandClass();
                m_delFeatCom.OnCreate(axMapControl1.Object);
                m_delFeatCom.OnClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void btnAttributeEdit_ItemClick(object sender, EventArgs e)
        {
            try
            {
                ICommand m_AtrributeCom = new EditAtrributeToolClass();
                m_AtrributeCom.OnCreate(axMapControl1.Object);
                m_AtrributeCom.OnClick();
                axMapControl1.CurrentTool = m_AtrributeCom as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerArrow;
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void btnMoveVertex_ItemClick(object sender, EventArgs e)
        {
            try
            {
                ICommand m_MoveVertexTool = new MoveVertexToolClass();
                m_MoveVertexTool.OnCreate(axMapControl1.Object);
                axMapControl1.CurrentTool = m_MoveVertexTool as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void btnAddVertex_ItemClick(object sender, EventArgs e)
        {
            try
            {
                ICommand m_AddVertexTool = new AddVertexToolClass();
                m_AddVertexTool.OnCreate(axMapControl1.Object);
                axMapControl1.CurrentTool = m_AddVertexTool as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void btnDelVertex_ItemClick(object sender, EventArgs e)
        {
            try
            {
                ICommand m_DelVertexTool = new DelVertexToolClass();
                m_DelVertexTool.OnCreate(axMapControl1.Object);
                axMapControl1.CurrentTool = m_DelVertexTool as ITool;
                axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void cmbSelLayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sLyrName = cmbSelLayer.SelectedItem.ToString();
                pCurrentLyr = MapManager.GetLayerByName(pMap, sLyrName) as IFeatureLayer;
                //设置编辑目标图层
                pEngineEditLayers.SetTargetLayer(pCurrentLyr, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }

        private void Editchosebtn_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region 指数成果展示

        private void barButtonItem36_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ImageViewer IsViewer = new ImageViewer("Ir");
            IsViewer.Show();
        }

        private void barButtonItem37_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ImageViewer IsViewer = new ImageViewer("Ia");
            IsViewer.Show();
        }

        private void barButtonItem38_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ImageViewer IsViewer = new ImageViewer("Ic");
            IsViewer.Show();
        }

        private void barButtonItem39_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            SheetViewer mSheetViewer = new SheetViewer();
            mSheetViewer.Show();
            //这边预想应该传入一个参数表示要打开的表格
            //或者我们所有的表格都在一起，在excel中很多Sheet，然后默认打开？
        }

        #endregion

        #region 地图联动
        //地图复制函数
        private void CopyToPageLayout()
        {
            IObjectCopy pObjectCopy = new ObjectCopyClass();
            object copyFromMap = axMapControl1.Map;
            object copiedMap = pObjectCopy.Copy(copyFromMap);//复制地图到copiedMap中
            object copyToMap = axPageLayoutControl1.ActiveView.FocusMap;
            pObjectCopy.Overwrite(copiedMap, ref copyToMap); //复制地图
            axPageLayoutControl1.ActiveView.Refresh();
        }

        //axMapControl 加载的数据发生重绘时，需要联动
        private void axMapControl1_OnAfterScreenDraw(object sender, IMapControlEvents2_OnAfterScreenDrawEvent e)
        {
            IActiveView pActiveView = (IActiveView)axPageLayoutControl1.ActiveView.FocusMap;
            IDisplayTransformation displayTransformation = pActiveView.ScreenDisplay.DisplayTransformation;
            displayTransformation.VisibleBounds = axMapControl1.Extent;
            axPageLayoutControl1.ActiveView.Refresh();
            CopyToPageLayout();
        }
        //axMapControl 中的数据显示状况发生变化时，需要联动
        private void axMapControl1_OnViewRefreshed(object sender, IMapControlEvents2_OnViewRefreshedEvent e)
        {
            axTOCControl1.Update();
            CopyToPageLayout();
        }
        #endregion

        #region 制图功能

        //插入文本
        private void barButtonItem41_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            AddTitle addTitle = new AddTitle();
            addTitle.ShowDialog();
            string Title = addTitle.title;

            ITextElement textElement;
            ITextSymbol textSymbol;
            IColor color;
            IActiveView activeView = axPageLayoutControl1.PageLayout as IActiveView;
            IEnvelope envelope = new Envelope() as IEnvelope;
            envelope.PutCoords(7, 20, 15, 30);
            textElement = new TextElement() as ITextElement;
            IElement element = textElement as IElement;
            element.Geometry = envelope;
            textElement.Text = Title;
            textSymbol = new TextSymbol();
            IRgbColor pColor = new RgbColor();
            pColor.Red = 0;
            pColor.Green = 0;
            pColor.Blue = 0;
            color = pColor;
            textSymbol.Color = color;
            textSymbol.Size = 30;
            textElement.Symbol = textSymbol;
            IGraphicsContainer graphicsContainer = activeView as IGraphicsContainer;
            graphicsContainer.AddElement(element, 0);
            axPageLayoutControl1.ActiveView.Refresh();
        }

        private void barButtonItem42_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IActiveView pActiveView = axPageLayoutControl1.PageLayout as IActiveView;
            IMap pMap = pActiveView.FocusMap as IMap;
            IGraphicsContainer pGraphicsContainer = pActiveView as IGraphicsContainer;
            IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pMap) as IMapFrame;
            pActiveView = axPageLayoutControl1.PageLayout as IActiveView;
            pMap = pActiveView.FocusMap as IMap;
            pGraphicsContainer = pActiveView as IGraphicsContainer;
            pMapFrame = pGraphicsContainer.FindFrame(pMap) as IMapFrame;//设置比例尺样式
            IMapSurround pMapSurround;
            IScaleBar pScaleBar;
            pScaleBar = new ScaleLineClass();
            pScaleBar.Units = pMap.MapUnits;
            pScaleBar.Divisions = 2;
            pScaleBar.Subdivisions = 4;
            pScaleBar.DivisionsBeforeZero = 0;
            pScaleBar.LabelPosition = esriVertPosEnum.esriBelow;
            pScaleBar.LabelGap = 3.6;
            pScaleBar.LabelFrequency = esriScaleBarFrequency.esriScaleBarDivisionsAndFirstMidpoint;

            pMapSurround = pScaleBar;
            pMapSurround.Name = "ScaleBar";
            //定义UID
            UID uid = new UID();
            uid.Value = "esriCarto.ScaleLine";
            //定义MapSurroundFrame对象      
            IMapSurroundFrame pMapSurroundFrame = pMapFrame.CreateSurroundFrame(uid, null);
            pMapSurroundFrame.MapSurround = pMapSurround;
            //定义Envelope设置Element摆放的位置
            IEnvelope pEnvelope = new EnvelopeClass();
            //设置元素在pagelayout中的坐标位置，个人可调整
            pEnvelope.PutCoords(2, 1.5, 10, 2.5);
            IElement pElement = pMapSurroundFrame as IElement;
            pElement.Geometry = pEnvelope;
            pGraphicsContainer.AddElement(pElement, 0);
        }

        public static void InsertNorth(AxPageLayoutControl axPageLayout)
        {
            IElement pElement = axPageLayout.FindElementByName("MarkerNorthArrow");
            if (pElement != null)
            {
                axPageLayout.ActiveView.GraphicsContainer.DeleteElement(pElement);  //删除已经存在的图例
            }
            IPageLayout pPageLayout = axPageLayout.PageLayout;
            IGraphicsContainer pGraphicsContainer = pPageLayout as IGraphicsContainer;
            IActiveView pActiveView = pPageLayout as IActiveView;
            UID pID = new UIDClass();
            pID.Value = "esriCore.MarkerNorthArrow";

            IMapFrame pMapFrame = pGraphicsContainer.FindFrame(pActiveView.FocusMap) as IMapFrame;
            if (pMapFrame == null) return;
            IMapSurroundFrame pMapSurroundFrame = pMapFrame.CreateSurroundFrame(pID, null);
            if (pMapSurroundFrame == null) return;
            IEnvelope pEnv = new EnvelopeClass();
            pEnv.PutCoords(16, 25, 31, 40);
            pElement = (IElement)pMapSurroundFrame;
            pElement.Geometry = pEnv;
            pMapSurroundFrame.MapSurround.Name = "MarkerNorthArrow";
            INorthArrow pNorthArrow = pMapSurroundFrame.MapSurround as INorthArrow;
            pGraphicsContainer.AddElement(pElement, 0);
            axPageLayout.ActiveView.Refresh();
        }

        private void barButtonItem43_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InsertNorth(axPageLayoutControl1);
        }

        public void InsertLegend(AxPageLayoutControl axPageLayout)
        {
            //Get the GraphicsContainer
            IGraphicsContainer graphicsContainer = axPageLayoutControl1.GraphicsContainer;

            //Get the MapFrame
            IMapFrame mapFrame = (IMapFrame)graphicsContainer.FindFrame(axPageLayoutControl1.ActiveView.FocusMap);

            if (mapFrame == null) return;

            //Create a legend
            UID uID = new UIDClass();
            uID.Value = "esriCarto.Legend";

            //Create a MapSurroundFrame from the MapFrame
            IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(uID, null);
            if (mapSurroundFrame == null) return;
            if (mapSurroundFrame.MapSurround == null) return;
            //Set the name
            mapSurroundFrame.MapSurround.Name = "Legend";

            //Envelope for the legend
            IEnvelope envelope = new EnvelopeClass();
            envelope.PutCoords(16, 2, 19.4, 3.4);

            //Set the geometry of the MapSurroundFrame
            IElement element = (IElement)mapSurroundFrame;
            element.Geometry = envelope;

            //Add the legend to the PageLayout
            axPageLayoutControl1.AddElement(element, Type.Missing, Type.Missing, "Legend", 0);

            //Refresh the PageLayoutControl
            axPageLayoutControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void barButtonItem44_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            InsertLegend(axPageLayoutControl1);
        }
        #endregion

        /// <summary>
        /// 地图输出
        /// </summary>
        /// <param name="map">地图</param>
        /// <param name="outputFile">输出文件路径</param>
        /// <param name="resolution">输出精度</param>
        /// <returns>是否成功</returns>
        public static bool MapExport(IActiveView activeView, string outputFile, double resolution)
        {
            if (activeView == null) return false;

            IExport exporter = null;
            string extension = System.IO.Path.GetExtension(outputFile);
            switch (extension.ToLower())
            {
                case ".jpg":
                    exporter = new ExportJPEGClass() as IExport;
                    break;

                case ".pdf":
                    exporter = new ExportPDFClass() as IExport;
                    break;

                case ".png":
                    exporter = new ExportPNGClass() as IExport;
                    break;

                case ".tif":
                    exporter = new ExportTIFFClass() as IExport;
                    break;

                case ".bmp":
                    exporter = new ExportBMPClass() as IExport;
                    break;

                case ".gif":
                    exporter = new ExportGIFClass() as IExport;
                    break;

                default:
                    throw new Exception("输出图片格式不支持！");

            }

            try
            {
                double screenResolution = activeView.ScreenDisplay.DisplayTransformation.Resolution;

                exporter.ExportFileName = outputFile;
                exporter.Resolution = resolution;

                tagRECT exportRect = new tagRECT();
                exportRect.left = 0;
                exportRect.top = 0;
                exportRect.right = Convert.ToInt32(activeView.ExportFrame.right * (resolution / screenResolution));
                exportRect.bottom = Convert.ToInt32(activeView.ExportFrame.bottom * (resolution / screenResolution));

                IEnvelope pixelBoundsEnv = new EnvelopeClass();
                pixelBoundsEnv.PutCoords(exportRect.left, exportRect.top, exportRect.right, exportRect.bottom);
                exporter.PixelBounds = pixelBoundsEnv;

                int hDC = exporter.StartExporting();
                activeView.Output(hDC, Convert.ToInt32(resolution), ref exportRect, null, null);
                exporter.FinishExporting();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                exporter.Cleanup();
            }

        }

        private void barButtonItem45_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                
                SaveFileDialog pSaveFiledialog = new SaveFileDialog();
                pSaveFiledialog.Title = "另存为图片";
                pSaveFiledialog.OverwritePrompt = true;
                pSaveFiledialog.Filter = "Image Files (*.bmp, *.jpg,*.png,*tif,*pdf)|*.bmp;*.jpg;*.png;*.tif;*.pdf";
                pSaveFiledialog.RestoreDirectory = true;
                if (pSaveFiledialog.ShowDialog() == DialogResult.OK)
                {
                    string sFilePath = pSaveFiledialog.FileName;
                    IActiveView activeView = axPageLayoutControl1.ActiveView;
                    if (MapExport(activeView, sFilePath, 300))
                        MessageBox.Show("图片输出成功！");
                    else
                        MessageBox.Show("图片输出失败！");
                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了:" + ex.ToString(), "Error", MessageBoxButtons.OK);
            }
        }


    }
}
