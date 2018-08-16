using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlassDetctionSystem.Controller;


namespace GlassDetctionSystem
{
    public partial class Calibration : Form
    {
        private CalibrationController _controller;
        public CalibrationController controller
        {
            get { return _controller; }
            set
            {
                this._controller = value;
                this.textBox1.DataBindings.Add("Text", controller.model, "ExposureTime");
            }


        }

        public Calibration()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            controller.model.GetImage();
            if (controller.model.status == 0)
            {
                string AppPath = System.IO.Directory.GetCurrentDirectory(); //获取当前应用的文件路径
                string FilePath;
                FilePath = AppPath + @"\test2.bmp";
                pictureBox1.ImageLocation = FilePath;
                pictureBox4.Visible = true;
                label4.Visible = true;
                pictureBox10.Visible = false;
                label10.Visible = false;
            }
            else
            {
                pictureBox4.Visible = false;
                label4.Visible = false;
                pictureBox10.Visible = true;
                label10.Visible = true;
                label10.Text = "相机错误："+controller.model.ErrorMessage;
            }
            
        }



        private void Calibration_Load(object sender, EventArgs e)
        {
            
            controller.model.AutoExposureMode();
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;

            button4.Enabled = false;

            pictureBox2.Visible = false;
            pictureBox8.Visible = true;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            string AppPath = System.IO.Directory.GetCurrentDirectory(); //获取当前应用的文件路径
            string FilePath;
            FilePath = AppPath + @"\th(4).gif";
            pictureBox1.ImageLocation = FilePath;

            pictureBox4.Visible = false;
            label4.Visible = false;
            pictureBox10.Visible = false;
            label10.Visible = false;

            pictureBox2.Visible = true;
            pictureBox8.Visible = false;



        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            bool flag;
            flag = timer1.Enabled;
            if (flag)
            {
                timer1.Enabled = false;
            }
            controller.model.ChangeExposureMode();
            pictureBox3.Visible = false;
            pictureBox9.Visible = true;
            timer1.Enabled = flag;
            label3.Visible = true;
            textBox1.Visible = true;
            
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            bool flag;
            flag = timer1.Enabled;
            if (flag)
            {
                timer1.Enabled = false;
            }
            controller.model.AutoExposureMode();
            pictureBox3.Visible = true;
            pictureBox9.Visible = false;
            label3.Visible = false;
            textBox1.Visible = false;
            timer1.Enabled = flag;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            
        }

        private void Calibration_Click(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }
    }
}
