using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlassDetctionSystem.Model.FunctionClass.Camera;
using GlassDetctionSystem.Model.FunctionClass.DataCard;
using GlassDetctionSystem.Controller;
using Basler.Pylon;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;




namespace GlassDetctionSystem
{
    public partial class DeveloperTest : Form
    {
        private DeveloperTestController _controller;
         public DeveloperTestController controller
        {
            get { return _controller; }
            set
            {
                this._controller = value;
                this.textBox8.DataBindings.Add("Text", controller.model, "ModelInput");
                this.textBox7.DataBindings.Add("Text", controller.model, "ModelOutput");
            }
                
                 
        }




        [DllImport(@"OpencvProcess.dll", EntryPoint = "read")]
        extern static void read(IntPtr dd, int height, int wheight);
        public DeveloperTest()
        {
            InitializeComponent();

        }
        
        string ErrorMessage;
        int DeviceNum;
        ICamera testcamera=null;

        List<ICameraInfo> AllCameraInfo;
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetCameraInfo TestGet = new GetCameraInfo();
            TestGet.TryGetConnect(DeviceType.GigE, out DeviceNum, out  AllCameraInfo, out  ErrorMessage);
            textBox1.Text = DeviceNum.ToString();
            textBox2.Text = ErrorMessage;
            for (int i = 0; i < DeviceNum; i++)
            {
                comboBox1.Items.Insert(i, AllCameraInfo[i]. ToString());
            }
            if(DeviceNum>0)
            comboBox1.SelectedIndex = 0;
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int status;
            int ImageWidth;
            int ImageHeight;
            byte[] buffer;

            testcamera.Trigger(out status, out ImageWidth,out  ImageHeight, out buffer, out ErrorMessage);
            if (status == 0)
            {
                textBox3.Text = "status" + status.ToString() + ":第一个像素点灰度值为" + buffer[0].ToString();

                //将buffer的指针传递给opencv
                GCHandle hObject = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                IntPtr pObject = hObject.AddrOfPinnedObject();
                if (hObject.IsAllocated)
                    hObject.Free();
                //opencvtest中的read将图片写成test2
                read(pObject, ImageWidth, ImageHeight);
            }
            else
            {
                textBox3.Text = "status" + status.ToString() + ":" + ErrorMessage;

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            testcamera=ICamera.getInstance(AllCameraInfo[0]);
            if (testcamera.SoftwareTriggerSet(out ErrorMessage))
            {
                button3.BackColor = Color.Green;
                textBox2.Text = ErrorMessage;
                //if (testcamera.SetAutoExposure(out ErrorMessage))
                //{
                    //button4.BackColor = Color.Green;
                    //textBox2.Text = ErrorMessage;
                    if (testcamera.OpenCamera(out ErrorMessage))
                    {
                        button5.BackColor = Color.Green;
                        textBox2.Text = ErrorMessage;
                        if (testcamera.GrabReady(out ErrorMessage))
                        {
                            button7.BackColor = Color.Green;
                        }
                        textBox2.Text = ErrorMessage;
                    }
                    textBox2.Text = ErrorMessage;
               // }
                textBox2.Text = ErrorMessage;
            }
           
            
            
            textBox2.Text = ErrorMessage;
        }

        private void DeveloperTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (testcamera != null)
            {
                ICamera.getInstance(AllCameraInfo[0]).GrabStop();
                ICamera.getInstance(AllCameraInfo[0]).CloseCamera(out ErrorMessage);
                ICamera.getInstance(AllCameraInfo[0]).Close();
                ICamera.getInstance(AllCameraInfo[0]).Dispose();

            }
            testcamera = null;
        }



        private void button8_Click(object sender, EventArgs e)
        {
            //加入计时
            Stopwatch watch = new Stopwatch();
            watch.Start();
            pictureBox1.ImageLocation = @" E:\镜片缺陷检测海沧\GlassDetectionSystem\GlassDetctionSystem\GlassDetctionSystem\bin\Release\test.bmp";
            watch.Stop();
            string time = watch.ElapsedMilliseconds.ToString();
            textBox2.Text = time;
            
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button25_Click(object sender, EventArgs e)
        {
            DataCard.getinstance().Open();
            if (DataCard.getinstance().IsOpen())
            {
                button27.BackColor = Color.Green;
                textBox5.Text = DataCard.getinstance().GetInstanceDiscription();
            }

        }

        private void button26_Click(object sender, EventArgs e)
        {
            DataCard.getinstance().SetDeviceDiscription(textBox4.Text);
            button27.BackColor = Color.Red;
        }

        private void button28_Click(object sender, EventArgs e)
        {
            int data;
            data=DataCard.getinstance().ReadByte();
            textBox6.Text = data.ToString();
            bool[] indexnum = new bool[8];
         
            for (int i = 0; i < 8; i++)
            {
               indexnum[i]= DataCard.getinstance().ReadInBits(i);
                
            }
            button9.BackColor = indexnum[0] ? Color.Green : Color.Red;
            button10.BackColor = indexnum[1] ? Color.Green : Color.Red;
            button11.BackColor = indexnum[2] ? Color.Green : Color.Red;
            button12.BackColor = indexnum[3] ? Color.Green : Color.Red;
            button13.BackColor = indexnum[4] ? Color.Green : Color.Red;
            button14.BackColor = indexnum[5] ? Color.Green : Color.Red;
            button15.BackColor = indexnum[6] ? Color.Green : Color.Red;
            button16.BackColor = indexnum[7] ? Color.Green : Color.Red;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (button24.BackColor == Color.Red)
            {
                DataCard.getinstance().WriteInBits(0, 1);
                button24.BackColor = Color.Green;
            }

            else
            {
                DataCard.getinstance().WriteInBits(0, 0);
                button24.BackColor = Color.Red;
            }

        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (button23.BackColor == Color.Red)
            {
                DataCard.getinstance().WriteInBits(1, 1);
                button23.BackColor = Color.Green;
            }

            else
            {
                DataCard.getinstance().WriteInBits(1, 0);
                button23.BackColor = Color.Red;
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (button22.BackColor == Color.Red)
            {
                DataCard.getinstance().WriteInBits(2, 1);
                button22.BackColor = Color.Green;
            }

            else
            {
                DataCard.getinstance().WriteInBits(2, 0);
                button22.BackColor = Color.Red;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (button21.BackColor == Color.Red)
            {
                DataCard.getinstance().WriteInBits(3, 1);
                button21.BackColor = Color.Green;
            }

            else
            {
                DataCard.getinstance().WriteInBits(3, 0);
                button21.BackColor = Color.Red;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (button20.BackColor == Color.Red)
            {
                DataCard.getinstance().WriteInBits(4, 1);
                button20.BackColor = Color.Green;
            }

            else
            {
                DataCard.getinstance().WriteInBits(4, 0);
                button20.BackColor = Color.Red;
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (button19.BackColor == Color.Red)
            {
                DataCard.getinstance().WriteInBits(5, 1);
                button19.BackColor = Color.Green;
            }

            else
            {
                DataCard.getinstance().WriteInBits(5, 0);
                button19.BackColor = Color.Red;
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (button18.BackColor == Color.Red)
            {
                DataCard.getinstance().WriteInBits(6, 1);
                button18.BackColor = Color.Green;
            }

            else
            {
                DataCard.getinstance().WriteInBits(6, 0);
                button18.BackColor = Color.Red;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (button17.BackColor == Color.Red)
            {
                DataCard.getinstance().WriteInBits(7, 1);
                button17.BackColor = Color.Green;
            }

            else
            {
                DataCard.getinstance().WriteInBits(7, 0);
                button17.BackColor = Color.Red;
            }
        }



        private void textBox8_Leave(object sender, EventArgs e)
        {
            controller.UpdateDevloperTestController(); 
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            controller.UpdateDevloperTestController();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            controller.model.plus();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
