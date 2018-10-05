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
    public partial class TempTest : Form
    {
        private TempTestController _controller;
        public TempTestController controller
        {
            get { return _controller; }
            set
            {
                this._controller = value;
                this.textBox1.DataBindings.Add("Text", controller.model, "AOIp");
                this.textBox2.DataBindings.Add("Text", controller.model, "ExposureTime");

            }


        }

        public TempTest()
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
           
                
            }
            else
            {
               
            }
        }

        private void TempTest_Load(object sender, EventArgs e)
        {
            //controller.model.AutoExposureMode();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            controller.model.GetImage();
            controller.model.HoughAOI();
            string AppPath = System.IO.Directory.GetCurrentDirectory(); //获取当前应用的文件路径
            string FilePath;
            FilePath = AppPath + @"\test2.bmp";
            pictureBox1.ImageLocation = FilePath;


        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            controller.model.GetImage();
            controller.model.Detect();
            string AppPath = System.IO.Directory.GetCurrentDirectory(); //获取当前应用的文件路径
            string FilePath;
            FilePath = AppPath + @"\test2.bmp";
            pictureBox1.ImageLocation = FilePath;
        }

        private void TempTest_Click(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }

        private void label3_Click(object sender, EventArgs e)
        {

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
            textBox2.Visible = true;
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
            textBox2.Visible = false;
            timer1.Enabled = flag;
        }

        private void TempTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.model.DisposeCamera();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
            textBox3.Text = e.X.ToString();
            textBox4.Text = e.Y.ToString();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
