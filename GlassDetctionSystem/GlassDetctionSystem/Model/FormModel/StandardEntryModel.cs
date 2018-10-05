using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using GlassDetctionSystem.Model.FunctionClass.XML;
using System.Text.RegularExpressions;
using Basler.Pylon;
using GlassDetctionSystem.Model.InteractiveData;

namespace GlassDetctionSystem.Model.FormModel
{
    public class StandardEntryModel : INotifyPropertyChanged
    {
        public StandardEntryModel()
        {
            this.ChosenCameraInfo = ChosenCamera.ChosenCameraInfo;
        }
        private string _NameId;
        public string NameId
        {
            get { return _NameId; }
            set { _NameId = value; OnPropertyChanged("NameId"); }
        }

        private double _Radius;
        public double Radius
        {
            get { return _Radius; }
            set {
                if (IsNum) { _Radius = value; OnPropertyChanged("Radius"); }
                else IsNum = true;
                }
        }

        private double _Height;
        public double Height
        {
            get { return _Height; }
            set {
                if (IsNum) { _Height = value; OnPropertyChanged("Height"); }
                else IsNum = true;
            }
        }

        public int CenterX;
        public int CenterY;
        public double PixPerMm;
        public ICameraInfo ChosenCameraInfo;
        public bool LorR;

        private double _DeltAx;
        public double DeltAx
        {
            get { return _DeltAx; }
            set {
                if (IsNum) { _DeltAx = value; OnPropertyChanged("DeltAx"); }
                else IsNum = true;
            }
        }

        private double _DeltAy;
        public double DeltAy
        {
            get { return _DeltAy; }
            set {
                if (IsNum) { _DeltAy = value; OnPropertyChanged("DeltAy"); }
                else IsNum = true;
            }
        }

        private double _DeltB;
        public double DeltB
        {
            get { return _DeltB; }
            set {
                if (IsNum) { _DeltB = value; OnPropertyChanged("DeltB"); }
                else IsNum = true;
            }
        }

        private double _AMinSize;
        public double AMinSize
        {
            get { return _AMinSize; }
            set {
                if (IsNum) { _AMinSize = value; OnPropertyChanged("AMinSize"); }
                else IsNum = true;
            }
        }
        private double _AMaxNum;
        public double AMaxNum
        {
            get { return _AMaxNum; }
            set { if (IsNum) { _AMaxNum = value; OnPropertyChanged("AMaxNum"); }
                else IsNum = true;
            }
        }
        private double _BMinSize;
        public double BMinSize
        {
            get { return _BMinSize; }
            set { if (IsNum) { _BMinSize = value; OnPropertyChanged("BMinSize"); }
                else IsNum = true;
            }
        }
        private double _BMaxNum;
        public double BMaxNum
        {
            get { return _BMaxNum; }
            set { if (IsNum) { _BMaxNum = value; OnPropertyChanged("BMaxNum"); }
                else IsNum = true;
            }
        }
        private double _Aradius;
        public double Aradius
        {
            get { return _Aradius; }
            set
            {
                if (IsNum) { _Aradius = value; OnPropertyChanged("Aradius");  }
                else IsNum = true;
            }
        }
        int Count;
        Dictionary<int, string> AllChildNodes;

        public bool IsNum=true;
        public void CheckNum(string num)
        {
            IsNum= Regex.IsMatch(num, @"^\d+\.\d+$") || Regex.IsMatch(num, @"^\d+$");
        }

        public void DownloadParemeter()
        {
            PixPerMm = float.Parse(XMLParser.getInstance().get("//platform[@id='0']/pxPerMm"));
            CenterX = int.Parse(XMLParser.getInstance().get("//platform[@id='0']/CenterX"));
            CenterY = int.Parse(XMLParser.getInstance().get("//platform[@id='0']/CenterY"));
        }
        public void UpLoadParemeter()
        {
            XMLParser.getInstance().getAllNodes("//standards",out Count,out AllChildNodes);
            bool FondFlag = false;
            for (int i = 0; i < Count; i++)
            {
                if (NameId == AllChildNodes[i])
                {
                    UpLoad();
                    FondFlag = true;
                }
             
            }
            if (!FondFlag)
            {
                Creat();
            }
            
            

        }
        public void Creat()
        {
            XMLParser.getInstance().addNodeWithID("//standards", "standard", NameId);
            


            XMLParser.getInstance().addNodeWithText("//standard[@id=" + NameId + "]", "Radius", Radius.ToString());
            XMLParser.getInstance().addNodeWithText("//standard[@id=" + NameId + "]", "Height", Height.ToString());
            XMLParser.getInstance().addNodeWithText("//standard[@id=" + NameId + "]", "DeltAx", DeltAx.ToString());
            XMLParser.getInstance().addNodeWithText("//standard[@id=" + NameId + "]", "DeltAy", DeltAy.ToString());
            XMLParser.getInstance().addNodeWithText("//standard[@id=" + NameId + "]", "DeltB", DeltB.ToString());
            XMLParser.getInstance().addNodeWithText("//standard[@id=" + NameId + "]", "AMinSize", AMinSize.ToString());
            XMLParser.getInstance().addNodeWithText("//standard[@id=" + NameId + "]", "AMaxNum", AMaxNum.ToString());
            XMLParser.getInstance().addNodeWithText("//standard[@id=" + NameId + "]", "BMinSize", BMinSize.ToString());
            XMLParser.getInstance().addNodeWithText("//standard[@id=" + NameId + "]", "BMaxNum", BMinSize.ToString());
            XMLParser.getInstance().addNodeWithText("//standard[@id=" + NameId + "]", "Aradius", Aradius.ToString());
            XMLParser.getInstance().addNodeWithText("//standard[@id=" + NameId + "]", "LorR", LorR.ToString());


        }
        public void UpLoad()
        {
            XMLParser.getInstance().set("//standard" + "[@id=" + NameId + "]/LorR", LorR? "左" : "右");
            XMLParser.getInstance().set("//standard" + "[@id=" + NameId + "]/Radius", Radius.ToString());
            XMLParser.getInstance().set("//standard" + "[@id=" + NameId + "]/Height", Height.ToString());
            XMLParser.getInstance().set("//standard" + "[@id=" + NameId + "]/DeltAx", DeltAx.ToString());
            XMLParser.getInstance().set("//standard" + "[@id=" + NameId + "]/DeltAy", DeltAy.ToString());
            XMLParser.getInstance().set("//standard" + "[@id=" + NameId + "]/DeltB", DeltB.ToString());
            XMLParser.getInstance().set("//standard" + "[@id=" + NameId + "]/AMinSize", AMinSize.ToString());
            XMLParser.getInstance().set("//standard" + "[@id=" + NameId + "]/AMaxNum", AMaxNum.ToString());
            XMLParser.getInstance().set("//standard" + "[@id=" + NameId + "]/BMinSize", BMinSize.ToString());
            XMLParser.getInstance().set("//standard" + "[@id=" + NameId + "]/BMaxNum", BMaxNum.ToString());
            XMLParser.getInstance().set("//standard" + "[@id=" + NameId + "]/Aradius", Aradius.ToString());
        }
        public void DisposeCamera()
        {


            if (ICamera.getInstance(ChosenCameraInfo) != null)
            {
                ICamera.getInstance(ChosenCameraInfo).Close();
                ICamera.getInstance(ChosenCameraInfo).Dispose();
                //ICamera.getInstance(AllCameraInfo[SelectCameraIndex]) = null;
            }



        }
        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        #endregion
    }
}
