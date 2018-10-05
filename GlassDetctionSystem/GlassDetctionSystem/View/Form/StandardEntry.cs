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
    public partial class StandardEntry : Form
    {
        private StandardEntryController _controller;
        public StandardEntryController controller
        {
            get { return _controller; }
            set
            {
                this._controller = value;
                this.textBox1.DataBindings.Add("Text", controller.model, "NameId");
                this.textBox2.DataBindings.Add("Text", controller.model, "Radius");
                this.textBox3.DataBindings.Add("Text", controller.model, "Height");
                this.textBox4.DataBindings.Add("Text", controller.model, "DeltAy");
                this.textBox5.DataBindings.Add("Text", controller.model, "DeltAx");
                this.textBox6.DataBindings.Add("Text", controller.model, "DeltB");
                this.textBox7.DataBindings.Add("Text", controller.model, "BMaxNum");
                this.textBox8.DataBindings.Add("Text", controller.model, "AMaxNum");
                this.textBox9.DataBindings.Add("Text", controller.model, "AMinSize");
                this.textBox10.DataBindings.Add("Text", controller.model, "BMinSize");
                this.textBox11.DataBindings.Add("Text", controller.model, "Aradius");
            }
        }
        public StandardEntry()
        {

            InitializeComponent();
            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new ThreadExceptionEventHandler(StandardEntryException);

            //// Set the unhandled exception mode to force all Windows Forms errors to go through
            //// our handler.
            //Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(StandardEntryUnhandled);
        }
        public bool nomalclose = false;
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            controller.model.CheckNum(textBox2.Text);
            if (!controller.model.IsNum)
            {
                textBox2.BackColor = Color.FromArgb(255, 0, 255, 255);
            }
            else
            {
                textBox2.BackColor= Color.FromArgb(255, 210, 230, 234);
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            controller.model.CheckNum(textBox3.Text);
            if (!controller.model.IsNum)
            {
                textBox3.BackColor = Color.FromArgb(255, 0, 255, 255);
            }
            else
            {
                textBox3.BackColor = Color.FromArgb(255, 210, 230, 234);
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            controller.model.CheckNum(textBox5.Text);
            if (!controller.model.IsNum)
            {
                textBox5.BackColor = Color.FromArgb(255, 0, 255, 255);
            }
            else
            {
                textBox5.BackColor = Color.FromArgb(255, 210, 230, 234);
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            controller.model.CheckNum(textBox4.Text);
            if (!controller.model.IsNum)
            {
                textBox4.BackColor = Color.FromArgb(255, 0, 255, 255);
            }
            else
            {
                textBox4.BackColor = Color.FromArgb(255, 210, 230, 234);
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            controller.model.CheckNum(textBox6.Text);
            if (!controller.model.IsNum)
            {
                textBox6.BackColor = Color.FromArgb(255, 0, 255, 255);
            }
            else
            {
                textBox6.BackColor = Color.FromArgb(255, 210, 230, 234);
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            controller.model.CheckNum(textBox9.Text);
            if (!controller.model.IsNum)
            {
                textBox9.BackColor = Color.FromArgb(255, 0, 255, 255);
            }
            else
            {
                textBox9.BackColor = Color.FromArgb(255, 210, 230, 234);
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            controller.model.CheckNum(textBox8.Text);
            if (!controller.model.IsNum)
            {
                textBox8.BackColor = Color.FromArgb(255, 0, 255, 255);
            }
            else
            {
                textBox8.BackColor = Color.FromArgb(255, 210, 230, 234);
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            controller.model.CheckNum(textBox10.Text);
            if (!controller.model.IsNum)
            {
                textBox10.BackColor = Color.FromArgb(255, 0, 255, 255);
            }
            else
            {
                textBox10.BackColor = Color.FromArgb(255, 210, 230, 234);
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            controller.model.CheckNum(textBox7.Text);
            if (!controller.model.IsNum)
            {
                textBox7.BackColor = Color.FromArgb(255, 0, 255, 255);
            }
            else
            {
                textBox7.BackColor = Color.FromArgb(255, 210, 230, 234);
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
            if (radioButton1.Checked)
                controller.model.LorR = false;
            else
                controller.model.LorR = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                controller.model.LorR = true;
            else
                controller.model.LorR = false;
            controller.model.UpLoadParemeter();
            nomalclose = true;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            nomalclose = true;
            this.Close();
        }

        private void StandardEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!nomalclose)
            {
                controller.model.DisposeCamera();
            }
            FormInteraction.IsStandardEntryShow = false;
        }

        public void StandardEntryException(object sender, ThreadExceptionEventArgs t)
        {

            controller.model.DisposeCamera();
        }

        public void StandardEntryUnhandled(object sender, UnhandledExceptionEventArgs e)
        {

            controller.model.DisposeCamera();
        }
    }
}
