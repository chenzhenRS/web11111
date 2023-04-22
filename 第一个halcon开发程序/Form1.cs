using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace 第一个halcon开发程序
{
    public partial class Form1 : Form
    {
        HObject image = new HObject();
        HWindow []Display = new HWindow[2];
        
        public Form1()
        {
            InitializeComponent();

            HOperatorSet.ReadImage(out image, "printer_chip/printer_chip_01");
            disp_image(image);
            Display[0]= this.hWindowControl1.HalconWindow;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            HRegion ho_ROI0 = new HRegion();
            HXLDCont ho_Cross = new HXLDCont();
            //矩形
            ho_ROI0.GenRectangle1((HTuple)148, 314, 521, 826);
            hWindowControl1.HalconWindow.SetColor("blue");
            hWindowControl1.HalconWindow.DispObj(ho_ROI0);
        }
        public void disp_image(HObject ho_image)//显示图片窗口设置
        {
            HTuple width, height;
            HOperatorSet.GetImageSize(ho_image, out width, out height);
            // hWindowControl1.HalconWindow.SetPart(0,0, height, width);

            #region 设置分辨率

            double mWidth = (1.0) * width / hWindowControl1.Width;
            double mHeight = (1.0) * height / hWindowControl1.Height;
            HTuple row1, column1, row2, colum2;
            if (mWidth > mHeight)
            {
                row1 = -(1.0) * ((hWindowControl1.Height * mWidth) - height) / 2;
                column1 = 0;
                row2 = row1 + hWindowControl1.Height * mWidth;
                colum2 = column1 + hWindowControl1.Width * mWidth;
            }
            else
            {
                row1 = 0;
                column1 = -(1.0) * ((hWindowControl1.Width * mHeight) - width) / 2;
                row2 = row1 + hWindowControl1.Height * mHeight;
                colum2 = column1 + hWindowControl1.Width * mHeight;

            }

            HOperatorSet.SetPart(hWindowControl1.HalconWindow, row1, column1, row2, colum2);

            #endregion

            hWindowControl1.HalconWindow.DispObj(ho_image);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)//保存图像
        {
            string str;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = @"E:\";  //打开初始目录
            saveFileDialog.Title = "选择保存文件";
            saveFileDialog.Filter = "图像文件(*.bmp)|*.bmp|图像文件(*.jpg)|*.jpg";
            saveFileDialog.FilterIndex = 1;   //获取第1个过滤条件开始的文件拓展名
            saveFileDialog.FileName = System.DateTime.Now.ToString("yyyyMMddHHmmss");//设置默认文件名
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string fileName = saveFileDialog.FileName;
                    if (!fileName.Equals(""))
                    {
                        if (saveFileDialog.FilterIndex == 1)
                            HOperatorSet.WriteImage(image, "bmp", 0, fileName);
                        else
                            HOperatorSet.WriteImage(image, "jpg", 0, fileName);
                        str = "图像保存完成";
                        MessageBox.Show(str);
                    }
                }
                catch (Exception ex)
                {
                    str = "图像保存失败：" + ex.Message.ToString();
                    MessageBox.Show(str);
                }

            }

        }


        private void 适应图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            复位(0);
 
        }
        public void 复位(int num)
        {
            Disp_Image(image, Display[num]);
           // MPL[num].zoom_1 = 0;
           
        }
        public void Disp_Image(HObject image, HTuple Hwn) //适应图像
        {
            if (image.IsInitialized())
            {
                HTuple width, height, HW_y, HW_x, HW_n;
                double ratio_img;
                double ratio_win;
                try
                {
                    if (image != null)
                    {
                        HOperatorSet.GetImageSize(image, out width, out height);
                        //获取halcon窗体大小
                        HOperatorSet.GetWindowExtents(Hwn, out HW_n, out HW_n, out HW_y, out HW_x);
                        ratio_win = (double)HW_y / (double)HW_x;
                        ratio_img = (double)width / (double)height;
                        int _beginRow, _begin_Col, _endRow, _endCol;
                        if (ratio_win >= ratio_img)
                        {
                            _beginRow = 0;
                            _endRow = (int)height.D - 1;
                            _begin_Col = (int)(-width.D * (ratio_win / ratio_img - 1d) / 2d);
                            _endCol = (int)(width.D + width.D * (ratio_win / ratio_img - 1d) / 2d);
                        }
                        else
                        {
                            _begin_Col = 0;
                            _endCol = (int)width.D - 1;
                            _beginRow = (int)(-height.D * (ratio_img / ratio_win - 1d) / 2d);
                            _endRow = (int)(height.D + height.D * (ratio_img / ratio_win - 1d) / 2d);
                        }
                        HOperatorSet.SetPart(Hwn, _beginRow, _begin_Col, _endRow, _endCol);
                        HOperatorSet.ClearWindow(Hwn);
                        HOperatorSet.DispObj(image, Hwn);

                    }
                }
                catch (HalconException e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }
       
    }
}
