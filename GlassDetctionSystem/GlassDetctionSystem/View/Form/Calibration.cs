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
using GlassDetctionSystem.Model.InteractiveData;
using System.Threading;


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
                this.textBox2.DataBindings.Add("Text", controller.model, "Cornerw");
                this.textBox3.DataBindings.Add("Text", controller.model, "Cornerh");
                this.textBox4.DataBindings.Add("Text", controller.model, "Length");
                this.textBox5.DataBindings.Add("Text", controller.model, "CenterX");
                this.textBox6.DataBindings.Add("Text", controller.model, "CenterY");



            }


        }
        public bool normalclose=false;//标记是否为正常关闭
        public Calibration()
        {
            InitializeComponent();
            //controller.model.UploadParemeterPPM();
            //controller.model.UploadParemeterCenter();
            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(CalibrationException);

            //// Set the unhandled exception mode to force all Windows Forms errors to go through
            //// our handler.
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(CalibrationUnhandled);
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

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            pictureBox11.Visible = true;
            pictureBox5.Visible = false;
            label5.Visible = false;
            pictureBox7.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label15.Visible = false;
            controller.model.GetImage();
            controller.model.CalibratCamera();
            pictureBox11.Visible = false;
            if (controller.model.PixPerMm != -1)
            {
                pictureBox5.Visible = true;
                label5.Visible = true;
                pictureBox7.Visible = false;
                label7.Visible = false;
                label8.Text = "标定参数为：" + controller.model.PixPerMm.ToString("f3") + "pixel/mm";
                label8.Visible = true;
                label15.Visible = false;
            }
            else
            {
                pictureBox5.Visible = false;
                label5.Visible = false;
                pictureBox7.Visible = true;
                label7.Visible = true;
                label8.Visible = false;
                label15.Text ="Error:" +controller.model.ErrorMessage;
                label15.Visible = true;
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            normalclose = true;
            controller.model.DownloadParemeter();
            this.Close();

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            normalclose = true;
            this.Close();
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }
        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)

        {

            if (e.Delta >= 0)

            {

                pictureBox1.Width = (int)(pictureBox1.Width * 1.1);//因为Widthh和Height都是int类型，所以要强制转换一下-_-||

                pictureBox1.Height = (int)(pictureBox1.Height * 1.1);

            }

            else

            {

                pictureBox1.Width = (int)(pictureBox1.Width * 0.9);

                pictureBox1.Height = (int)(pictureBox1.Height * 0.9);

            }

        }

        private void Calibration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!normalclose)
            { controller.model.DisposeCamera(); }
            
            FormInteraction.IsCalibrationShow = false;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            timer1.Enabled = false;
            controller.model.GetImage();
            label20.Text = e.X.ToString();
            label21.Text = e.Y.ToString();
            controller.model.CenterX = (int)((double)e.X / (double)controller.model.imagew* 2448);
            controller.model.CenterY = (int)((double )e.Y / (double)controller.model.imageh * 2048);
            controller.model.DrawCenterPoint();
            string AppPath = System.IO.Directory.GetCurrentDirectory(); //获取当前应用的文件路径
            string FilePath;
            FilePath = AppPath + @"result.txt";
            pictureBox1.Image= Image.FromFile(AppPath + @"/test2.bmp");
            for (int i = 0; i < 12000; i++)
            {
                int r = 0;
            }
            timer1.Enabled = true;
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                controller.model.UploadParemeterPPM();
                pictureBox11.Visible = false;
                pictureBox5.Visible = false;
                label5.Visible = false;
                pictureBox7.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label15.Visible = false;
                label19.Text = "上次标定参数为：" + controller.model.PixPerMm + "Pix/Mm";
                label19.Visible = true;
            }
            else
            {
                label19.Visible = false;
            }
        

        }

        public void CalibrationException(object sender, ThreadExceptionEventArgs t)
        {

            controller.model.DisposeCamera();
        }

        public void CalibrationUnhandled(object sender, UnhandledExceptionEventArgs e)
        {

            controller.model.DisposeCamera();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            controller.model.UploadParemeterCenter();
        }
    }
}
