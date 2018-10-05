using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlassDetctionSystem.Controller;
using GlassDetctionSystem.Model.InteractiveData;


namespace GlassDetctionSystem
{
    public partial class Initialize : Form
    {
        private InitializeController _controller;
        public InitializeController controller
        {
            //get { return _controller; }
            get { return _controller; }
            set
            {
                this._controller = value;
               
            }


        }
        public int connectcameratime = 0;
        //public int opencameratime = 0;
        public int opendatacardtime = 0;
        public Initialize()
        {
            InitializeComponent();
            pictureBox2.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
            pictureBox3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;

            label7.Text=("正在连接所有相机...");
            timer1.Enabled = true;
            connectcameratime = 0;
            //opencameratime = 0;
            opendatacardtime = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            if (connectcameratime >300)
            {
                controller.model.GetCameraNum();
                for (int i = 0; i < controller.model.CameraNum; i++)
                {
                    comboBox1.Items.Insert(i, (i+1).ToString());
                }
                if (controller.model.CameraNum > 0)
                {
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    label7.Text = ("无相机连接...");
                    if (connectcameratime ==800)
                    {
                        timer1.Enabled = false;
                        Thread.Sleep(3000);
                        this.Close();
                    }


                }
            }
            if (connectcameratime == 1000)
            {
                controller.model.SelectCameraIndex = comboBox1.SelectedIndex;
                controller.model.TryInitializeCamera();
                label7.Text = ("正在初始化所选相机...");
                
            }
            if (connectcameratime > 1000)
            {
                
                if (controller.model.CameraIsOpen)
                {
                    label3.Text = "已连接！";
                    label7.Text = ("相机连接成功");
                    timer1.Enabled = false;
                    timer2.Enabled = true;
                    label7.Text = ("正在初始化数据卡...");
                    //label8.Visible = true;
                    //pictureBox4.Visible = true;

                }
                else
                {
                    label7.Text = ("连接相机失败！错误原因：" + controller.model.ErrorMessage);
                    if (connectcameratime > 2500)
                    {
                        timer1.Enabled = false;
                        //Thread.Sleep(2000);
                        this.Close();
                    }

                }
            }
            connectcameratime += 100;
            timer1.Enabled = true;

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            if (opendatacardtime > 100)
            {
                try
                {
                    controller.model.TryInitializeDataCard();
                }
                catch (Exception exception)
                {
                    label7.Text = ("数据卡异常："+ exception.Message);
                }
                //controller.model.TryInitializeDataCard();
                if (controller.model.DataCardIsOpen)
                {

                    label7.Text = ("数据卡" + controller.model.DataCardDiscription + "连接成功！");
                    label5.Text = ("已连接");
                    label8.Visible = true;
                    pictureBox4.Visible = true;
                    label9.Visible = true;
                    pictureBox5.Visible = true;
                    if (opendatacardtime == 1500)
                    {
                        timer2.Enabled = false;
                        timer1.Enabled = false;
                        MainDetectController mainDetectController = new MainDetectController(new MainDetect());
                        timer1.Enabled = false;
                        timer2.Enabled = false;
                        this.Hide();
                        mainDetectController.view.Show();
                    }
                }
                else
                {
                    //label7.Text = ("连接数据卡失败！");
                    if (opendatacardtime > 800)
                    {
                        label7.Text = ("连接数据卡失败！");
                        if (opendatacardtime > 1100)
                        {
                            timer2.Enabled = false;
                            timer1.Enabled = false;
                            //Thread.Sleep(2000);
                            this.Close();
                        }
                       
                    }
                }


            }
            opendatacardtime += 100;
            timer2.Enabled = true;
        }

        private void Initialize_FormClosed(object sender, FormClosedEventArgs e)
        {
            controller.model.DisposeCamera();
        }

        private void Initialize_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.model.DisposeCamera();
        }



        private void Initialize_DoubleClick(object sender, EventArgs e)
        {
            DeveloperTestController developertestcontroller = new DeveloperTestController(new DeveloperTest());
            timer1.Enabled = false;
            timer2.Enabled = false;
            controller.model.DisposeCamera();
            this.Hide();
            developertestcontroller.view.Show();
   
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            CalibrationController calibrationController = new CalibrationController(new Calibration());
            timer1.Enabled = false;
            timer2.Enabled = false;
            this.Hide();
            calibrationController.view.Show();
            FormInteraction.IsCalibrationShow = true;
            timer3.Enabled=true;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            StandardEntryController standardEntryController = new StandardEntryController(new StandardEntry());
            timer1.Enabled = false;
            timer2.Enabled = false;
            this.Hide();
            standardEntryController.view.Show();
            FormInteraction.IsStandardEntryShow = true;
            timer3.Enabled = true;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (!FormInteraction.IsCalibrationShow && !FormInteraction.IsStandardEntryShow)
            {
                timer1.Enabled = true;
                timer2.Enabled = true;
                timer3.Enabled = false;
                
            }
        }
    }
}
