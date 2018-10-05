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
using GlassDetctionSystem.Model.FunctionClass;
using GlassDetctionSystem.Model.FunctionClass.Detect;
using GlassDetctionSystem.Model.InteractiveData;
using System.Threading;
using System.IO;



namespace GlassDetctionSystem
{
    public partial class MainDetect : Form
    {
        Thread thread;
        Thread threadstate;
        CameraStrategy maindetectsrategy ;
        private MainDetectController _controller;
        public MainDetectController controller
        {
            get { return _controller; }
            set
            {
                this._controller = value;
                this.textBox2.DataBindings.Add("Text", controller.model, "Radius");
                this.textBox3.DataBindings.Add("Text", controller.model, "Height");
                this.textBox4.DataBindings.Add("Text", controller.model, "DeltAy");
                this.textBox5.DataBindings.Add("Text", controller.model, "DeltAx");
                this.textBox6.DataBindings.Add("Text", controller.model, "DeltB");
                this.textBox1.DataBindings.Add("Text", controller.model, "BMaxNum");
                this.textBox8.DataBindings.Add("Text", controller.model, "AMaxNum");
                this.textBox9.DataBindings.Add("Text", controller.model, "AMinSize");
                this.textBox7.DataBindings.Add("Text", controller.model, "BMinSize");
                this.textBox10.DataBindings.Add("Text", controller.model, "Aradius");
                this.label27.DataBindings.Add("Text", controller.model, "Abadnum");
                this.label28.DataBindings.Add("Text", controller.model, "Bbadnum");
                this.label32.DataBindings.Add("Text", controller.model, "all");
                this.label31.DataBindings.Add("Text", controller.model, "good");
                this.label33.DataBindings.Add("Text", controller.model, "bad");


            }


        }
        public MainDetect()
        {
            InitializeComponent();
            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(MaindetectException);

           

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(MaindetectUnhandled);




        }

        #region ResultBuffer组件
        public string truestring = "是";
        public string falsestring = "否";

        private int LENGTH = 8;//缓存个数减去一，用于循环

       
        public void AddResultSet(string time, int flawA, int flawB, bool IsGood, Image image)
        {
            Label[] TimeLabels = { label44, label48, label52, label56, label60, label64, label68, label72, label76 };
            Label[] FlawALabels = { label43, label47, label51, label55, label59, label63, label67, label71, label75 };
            Label[] FlawBLabels = { label42, label46, label50, label54, label58, label62, label66, label70, label74};
            Label[] QualityLabels = { label41, label45, label49, label53, label57, label61, label65, label69, label73 };
            PictureBox[] Images = { pictureBox23, pictureBox25, pictureBox26, pictureBox27, pictureBox28, pictureBox29, pictureBox30, pictureBox31, pictureBox32 };
            //结果下移
            for (int i = 0; i < LENGTH; i++)
            {
                TimeLabels[LENGTH - i].Text = TimeLabels[LENGTH - i - 1].Text;
                FlawALabels[LENGTH - i].Text = FlawALabels[LENGTH - i - 1].Text;
                FlawBLabels[LENGTH - i].Text = FlawBLabels[LENGTH - i - 1].Text;
                QualityLabels[LENGTH - i].Text = QualityLabels[LENGTH - i - 1].Text;
                Images[LENGTH - i].Image = Images[LENGTH - i - 1].Image;
            }
            //第一条加入更新值
            TimeLabels[0].Text = time;
            FlawALabels[0].Text = "" + flawA;
            FlawBLabels[0].Text = "" + flawB;
            QualityLabels[0].Text = IsGood ? truestring : falsestring;
            Images[0].Image = image;



        }
        //双击缩略图进行预览
        
        private void PicturesDoubleClick(object sender, EventArgs e)
        {
            Look look = new Look(((PictureBox)sender).Image);
            look.Show();
        }


        #endregion


        #region Parameter组件
        private void ParameterTextChanged(object sender, EventArgs e)
        {
            controller.model.CheckNum(((TextBox)sender).Text);
            if (!controller.model.IsNum)
            {
                ((TextBox)sender).BackColor = Color.FromArgb(255, 0, 255, 255);
            }
            else
            {
                ((TextBox)sender).BackColor = Color.FromArgb(255, 210, 230, 234);
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                controller.model.LorR = true;
            else
                controller.model.LorR = false;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                controller.model.LorR = false;
            else
                controller.model.LorR = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            controller.model.NameId = controller.model.AllChildNodes[comboBox1.SelectedIndex];
            controller.model.DownLoadParameter();
            controller.model.DownLoadRecord();
            label35.Text = (double.Parse(label31.Text) / double.Parse(label32.Text) * 100).ToString("f2") + "%";



        }


        #endregion


        #region ProductionRecord组件

        private void timer1_Tick(object sender, EventArgs e)
        {
            label30.Text= DateTime.Now.ToString("yyyy-MM-dd");
            this.DoubleBuffered = true;  //设置本窗体
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            this.Refresh();
        }

        private void label30_TextChanged(object sender, EventArgs e)
        {
            controller.model.month = int.Parse(label30.Text.Substring(5, 2));
            controller.model.day= int.Parse(label30.Text.Substring(8, 2));
        }
        private void label32_TextChanged(object sender, EventArgs e)
        {
            if (double.Parse(label32.Text ) != 0)
                label35.Text = (double.Parse(label31.Text) / double.Parse(label32.Text) * 100).ToString("f2") + "%";
            else
                label35.Text = "00%";
        }

        public void CountRecord(bool isgood)
        {
            controller.model.all += 1;
            if (isgood) controller.model.good += 1;
            else controller.model.bad += 1;
            controller.model.UpdateRecords();
        }


        #endregion

        #region Detailed组件
        /// <summary>
        /// 重绘listBox1，使得item能根据指定颜色显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            MyListBoxItem it = listBox1.Items[e.Index] as MyListBoxItem;
            e.DrawBackground();
            e.Graphics.DrawString(it.Message, e.Font, new SolidBrush(it.ItemColor), e.Bounds, null);
            e.DrawFocusRectangle();

        }

        /// <summary>
        /// 重绘listBox2，使得item能根据指定颜色显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            MyListBoxItem it = listBox2.Items[e.Index] as MyListBoxItem;
            e.DrawBackground();
            e.Graphics.DrawString(it.Message, e.Font, new SolidBrush(it.ItemColor), e.Bounds, null);
            e.DrawFocusRectangle();
        }
        /// <summary>
        /// 添加item，重复item不添加
        /// </summary>
        /// <param name="cbox"></param>
        /// <param name="item"></param>
        private void AddComboBoxItem(ComboBox cbox, string item)
        {
            if (item == null || "".Equals(item) || cbox.Items.Contains(item))
            {
                return;
            }
            cbox.Items.Add(item);
        }
        /// <summary>
        /// 添加标题条目
        /// </summary>
        /// <param name="cbox"></param>
        private void AddFirstItem(ListBox box)
        {
            string fields = "坐标x" + "\t" + "坐标y" + "\t" + "直径" + "\t" ;
            box.Items.Add(new MyListBoxItem(Color.Black, fields));
        }

        /// <summary>
        /// 向指定listBox中添加条目
        /// </summary>
        /// <param name="lb"></param>
        /// <param name="flaws"></param>
        /// <param name="itemcolor"></param>
        /// <param name="pxPerMm"></param>
        /// <param name="groups"></param>
        private void addItem(ListBox lb, List<FlawPoint> flaws, Color itemcolor)
        {
            foreach (FlawPoint f in flaws)
            {
                string res = "" + (int)f.x + "\t" + (int)f.y + "\t" + (f.diameter).ToString("f2") ;
                lb.Items.Add(new MyListBoxItem(itemcolor, res));
            }
        }

        #endregion



        private void pictureBox8_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
            panel6.Visible = true;
            if(thread!=null)
            thread.Abort();
            timer2.Enabled = false;
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;
            panel6.Visible = false;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (controller.model.lasttimestamp != ResulBuffer.time)
            {
                Updateresult(); 

                //将时间戳更新
                controller.model.lasttimestamp = ResulBuffer.time;
            }
            
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            //创建检测线程
            controller.model.PackageParameter();
            maindetectsrategy = new CameraStrategy(ChosenCamera.ChosenCameraInfo, controller.model.Parameters);
            DetectObserver observer = new DetectObserver(maindetectsrategy, Port.getInstance(0));
            thread = new Thread(new ThreadStart(observer.tick));
            thread.Start();
            thread.IsBackground = true;
            timer2.Enabled = true;

            //显示正在运行
            pictureBox14.Visible = true;
            label23.Visible = true;
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            if (thread.IsAlive)
            {
                thread.Abort();
                timer2.Enabled = false;
                //不显示正在运行
                pictureBox14.Visible = false;
                label23.Visible = false;
            }
           
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            if (thread.IsAlive)
            {
                thread.Abort();
                timer2.Enabled = false;
                //不显示正常运行
                pictureBox14.Visible = false;
                label23.Visible = false;
            }
;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            controller.model.PackageParameter();
            maindetectsrategy = new CameraStrategy(ChosenCamera.ChosenCameraInfo, controller.model.Parameters);
            maindetectsrategy.detect(Port.getInstance(0));
            Updateresult();
        }
        public void Updateresult()
        {
            controller.model.Abadnum = ResulBuffer.Anum;
            controller.model.Bbadnum = ResulBuffer.Bnum;
            controller.model.Anum = ResulBuffer.SizeA;
            controller.model.Bnum = ResulBuffer.SizeB;
            if (ResulBuffer.Quality)
            {
                label26.Visible = true;
                label24.Visible = false;
            }
            else
            {
                label26.Visible = false;
                label24.Visible = true;
            }
            string AppPath = System.IO.Directory.GetCurrentDirectory(); //获取当前应用的文件路径
            string FilePath;
            FilePath = AppPath + @"\result.txt";
            FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            StreamReader FileReader = new StreamReader(fs, System.Text.Encoding.Default);
            List<FlawPoint> Abad = new List<FlawPoint> { };//存储A区的不合格瑕疵
            List<FlawPoint> Agood = new List<FlawPoint> { };//存储A区的合格瑕疵

            List<FlawPoint> Bbad = new List<FlawPoint> { };//存储B区的不合格瑕疵
            List<FlawPoint> Bgood = new List<FlawPoint> { };//存储B区的合格瑕疵

            double x;
            double y;
            double r;
            bool f;
            //循环读取A区瑕疵分组成list
            for (int i = 0; i < controller.model.Anum; i++)
            {


                x = double.Parse(FileReader.ReadLine());
                string yyy = FileReader.ReadLine();
               // yyy = FileReader.ReadLine();
                y = double.Parse(FileReader.ReadLine());
                yyy = FileReader.ReadLine();
                r = double.Parse(FileReader.ReadLine());
                yyy = FileReader.ReadLine();
                f = int.Parse(FileReader.ReadLine())==1?true:false;
                yyy = FileReader.ReadLine();
                FlawPoint add = new FlawPoint(x, y, r);
                if (f)
                {
                    Abad.Add(add);
                }
                else
                {
                    Agood.Add(add);
                }


            }
            //循环读取b区瑕疵，并分类到list
            for (int i = 0; i < controller.model.Bnum; i++)
            {


                x = double.Parse(FileReader.ReadLine());
                string yyy = FileReader.ReadLine();
                y = double.Parse(FileReader.ReadLine());
                yyy = FileReader.ReadLine();
                r = double.Parse(FileReader.ReadLine());
                yyy = FileReader.ReadLine();
                f = int.Parse(FileReader.ReadLine())==1?true:false;
                yyy = FileReader.ReadLine();
                FlawPoint add = new FlawPoint(x, y, r);
                if (f)
                {
                    Bbad.Add(add);
                }
                else
                {
                    Bgood.Add(add);
                }


            }
            controller.model.Abadnum = ResulBuffer.Anum;
            controller.model.Bbadnum = ResulBuffer.Bnum;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            AddFirstItem(listBox1);
            AddFirstItem(listBox2);

            //additem，分别更新Alist 和Blist
            addItem(listBox1, Abad, Color.Red);
            addItem(listBox1, Agood, Color.Green);
            addItem(listBox2, Bbad, Color.Red);
            addItem(listBox2, Bgood, Color.Green);

            //缓存结果，包括图片
            //读取文件流
            string AppPath1 = System.IO.Directory.GetCurrentDirectory(); //获取当前应用的文件路径
            string FilePath1;
            FilePath1 = AppPath + @"\结果.bmp";

            System.IO.FileStream fs1 = new System.IO.FileStream(FilePath1, System. IO.FileMode.Open, System. IO.FileAccess.Read);
            Image result = System.Drawing.Image.FromStream(fs1);
            fs1.Close();


            //Image result = Image.FromFile(AppPath + @"\src1.bmp");
            pictureBox16.Image = result;
            AddResultSet(ResulBuffer.time, ResulBuffer.Anum, ResulBuffer.Bnum, !ResulBuffer.Quality, result);
            //显示结果图
            pictureBox16.Image = result;

            //更新生产记录
            CountRecord(!ResulBuffer.Quality);
        }

        private void MainDetect_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            controller.model.GetAllVersion();
            for (int i = 0; i < controller.model.Count; i++)
            {
                comboBox1.Items.Add(controller.model.AllChildNodes[i]);
            }
            timer1.Enabled = true;
            label30.Text = DateTime.Now.ToString("yyyy-MM-dd");
            if (controller.model.LorR)
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
            }
            else
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
            }
            label35.Text = (double.Parse(label31.Text) / double.Parse(label32.Text) * 100).ToString("f2") + "%";
        }


        private void pictureBox16_Click(object sender, EventArgs e)
        {

        }

        private void MainDetect_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.model.DisposeCamera();
        }
       
        public  void MaindetectException(object sender, ThreadExceptionEventArgs t)
        {

           controller.model.DisposeCamera();
        }

        public void MaindetectUnhandled(object sender, UnhandledExceptionEventArgs e)
        {

            controller.model.DisposeCamera();
        }
    }
}
