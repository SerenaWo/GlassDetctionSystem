using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlassDetctionSystem
{
    public partial class Look : Form
    {
        //图像的缩放比 以及 其相对于pictureBox的偏移量
        double rate;
        double leftOffset;
        double upOffset;
        int hImagW=2448;
        int hImagH=2048;

        public Look(Image image)
        {
            InitializeComponent();
            this.LookPicture.Image = image;
        }
        /// <summary>
        /// 等比例缩放pictureBox1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            Size t = LookPicture.Size;
            if (e.Delta > 0)
            {
                if (t.Width + 50 <= 2600)
                {
                    LookPicture.Width += 50;
                    LookPicture.Height += 50 * t.Height / t.Width;
                }
            }
            if (e.Delta < 0)
            {
                if (t.Width - 50 >= 544)
                {
                    LookPicture.Width -= 50;
                    LookPicture.Height -= 50 * t.Height / t.Width;
                }
            }
            //更新pictureBox1
            LookPicture.Refresh();
            updateRateAndOffsets(hImagW, hImagH);

        }
        /// <summary>
        /// 更新图像缩放比以及相对于pictureBox的偏移量，当图像尺寸变化是需要用到.
        /// 注：该函数一定要在需要repaintPictureBoxGraphics()以及circleFlaw()前使用
        /// </summary>
        /// <param name="hImagW"></param>
        /// <param name="hImagH"></param>
        private void updateRateAndOffsets(int hImagW, int hImagH)
        {
            if (hImagW == 0 || hImagH == 0)
            {
                return;
            }
            //根据pictureBox的宽高计算picture的宽高
            double pictureHeight;
            double pictureWidth;
            if ((float)LookPicture.Width / (float)LookPicture.Height > (float)hImagW / (float)hImagH)
            {
                pictureHeight = LookPicture.Height;
                pictureWidth = pictureHeight * hImagW / hImagH;
            }
            else
            {
                pictureWidth = LookPicture.Width;
                pictureHeight = pictureWidth * hImagH / hImagW;
            }

            //计算左上角偏移量和缩放比率
            leftOffset = (LookPicture.Width - pictureWidth) / 2;
            upOffset = (LookPicture.Height - pictureHeight) / 2;
            rate = (double)pictureWidth / (double)hImagW;
        }


    }
}
