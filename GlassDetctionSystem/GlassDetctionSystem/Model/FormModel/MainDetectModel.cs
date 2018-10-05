using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Text.RegularExpressions;
using GlassDetctionSystem.Model.FunctionClass.XML;
using GlassDetctionSystem.Model.FunctionClass;
using Basler.Pylon;
using GlassDetctionSystem.Model.InteractiveData;
using GlassDetctionSystem.Model.FunctionClass.DataCard;



namespace GlassDetctionSystem.Model.FormModel
{
    public class MainDetectModel : INotifyPropertyChanged
    {
        private string _NameId;
        public string NameId
        {
            get { return _NameId; }
            set { _NameId = value; OnPropertyChanged("NameId"); }
        }
        public ICameraInfo ChosenCameraInfo;
        public MainDetectModel()
        {
            this.ChosenCameraInfo = ChosenCamera.ChosenCameraInfo;
        }
        private double _Radius;
        public double Radius
        {
            get { return _Radius; }
            set
            {
                if (IsNum) { _Radius = value; OnPropertyChanged("Radius");RefrashParameter("Radius",value.ToString()); }
                else IsNum = true;
            }
        }

        private double _Height;
        public double Height
        {
            get { return _Height; }
            set
            {
                if (IsNum) { _Height = value; OnPropertyChanged("Height"); if (value != 0) RefrashParameter("Height", value.ToString()); }
                else IsNum = true;
            }
        }

        public int CenterX;
        public int CenterY;
        public double PixPerMm;

        public bool LorR;

        private double _DeltAx;
        public double DeltAx
        {
            get { return _DeltAx; }
            set
            {
                if (IsNum) { _DeltAx = value; OnPropertyChanged("DeltAx"); if (value != 0) RefrashParameter("DeltAx",value.ToString()); }
                else IsNum = true;
            }
        }

        private double _DeltAy;
        public double DeltAy
        {
            get { return _DeltAy; }
            set
            {
                if (IsNum) { _DeltAy = value; OnPropertyChanged("DeltAy"); if (value != 0) RefrashParameter("DeltAy",value.ToString()); }
                else IsNum = true;
            }
        }

        private double _DeltB;
        public double DeltB
        {
            get { return _DeltB; }
            set
            {
                if (IsNum) { _DeltB = value; OnPropertyChanged("DeltB"); if (value != 0) RefrashParameter("DeltB",value.ToString()); }
                else IsNum = true;
            }
        }

        private double _AMinSize;
        public double AMinSize
        {
            get { return _AMinSize; }
            set
            {
                if (IsNum) { _AMinSize = value; OnPropertyChanged("AMinSize"); if (value != 0) RefrashParameter("AMinSize",value.ToString()); }
                else IsNum = true;
            }
        }
        private double _AMaxNum;
        public double AMaxNum
        {
            get { return _AMaxNum; }
            set
            {
                if (IsNum) { _AMaxNum = value; OnPropertyChanged("AMaxNum"); if (value != 0) RefrashParameter("AMaxNum",value.ToString()); }
                else IsNum = true;
            }
        }
        private double _BMinSize;
        public double BMinSize
        {
            get { return _BMinSize; }
            set
            {
                if (IsNum) { _BMinSize = value; OnPropertyChanged("BMinSize"); if (value != 0) RefrashParameter("BMinSize",value.ToString()); }
                else IsNum = true;
            }
        }
        private double _BMaxNum;
        public double BMaxNum
        {
            get { return _BMaxNum; }
            set
            {
                if (IsNum) { _BMaxNum = value; OnPropertyChanged("BMaxNum"); if (value != 0) RefrashParameter("BMaxNum",value.ToString()); }
                else IsNum = true;
            }
        }
        private double _Aradius;
        public double Aradius
        {
            get { return _Aradius; }
            set
            {
                if (IsNum) { _Aradius = value; OnPropertyChanged("Aradius"); if (value != 0) RefrashParameter("Aradius",value.ToString()); }
                else IsNum = true;
            }
        }
        /// <summary>
        /// a区瑕疵数目，更新自resultbuffer
        /// </summary>
        private double _Abadnum;
        public double Abadnum
        {
            get { return _Abadnum; }
            set
            {
                if (IsNum) { _Abadnum = value; OnPropertyChanged("Abadnum");  }
                else IsNum = true;
            }
        }
        /// <summary>
        /// B区瑕疵数目，更新自resultbuffer
        /// </summary>
        private double _Bbadnum;
        public double Bbadnum
        {
            get { return _Bbadnum; }
            set
            {
                if (IsNum) { _Bbadnum = value; OnPropertyChanged("Bbadnum"); }
                else IsNum = true;
            }
        }
        /// <summary>
        /// A区所有瑕疵数
        /// </summary>
        public double Anum;
        /// <summary>
        /// B区所有瑕疵数
        /// </summary>
        public double Bnum;
        public bool IsNum = true;
        public void CheckNum(string num)
        {
            IsNum = Regex.IsMatch(num, @"^\d+\.\d+$") || Regex.IsMatch(num, @"^\d+$");
        }
        public int Count=0;
        public Dictionary<int, string> AllChildNodes;
        public string lasttimestamp = null;
        /// <summary>
        /// 获得所有标准
        /// </summary>
        public void GetAllVersion()
        {
            XMLParser.getInstance().getAllNodes("//standards", out Count, out AllChildNodes);
        }
        /// <summary>
        /// 载入所选标准参数
        /// id的方式待测试
        /// </summary>
        public void DownLoadParameter()
        {
            PixPerMm = float.Parse(XMLParser.getInstance().get("//platform[@id='0']/pxPerMm"));
            CenterX = int.Parse(XMLParser.getInstance().get("//platform[@id='0']/CenterX"));
            CenterY = int.Parse(XMLParser.getInstance().get("//platform[@id='0']/CenterY"));
            Radius = float.Parse(XMLParser.getInstance().get("//standard[@id='" + NameId + "']/Radius"));
            Height = float.Parse(XMLParser.getInstance().get("//standard[@id='" + NameId + "']/Height"));
            DeltAx = float.Parse(XMLParser.getInstance().get("//standard[@id='" + NameId + "']/DeltAx"));
            DeltAy = float.Parse(XMLParser.getInstance().get("//standard[@id='" + NameId + "']/DeltAy"));
            DeltB = float.Parse(XMLParser.getInstance().get("//standard[@id='" + NameId + "']/DeltB"));
            AMinSize = float.Parse(XMLParser.getInstance().get("//standard[@id='" + NameId + "']/AMinSize"));
            AMaxNum = float.Parse(XMLParser.getInstance().get("//standard[@id='" + NameId + "']/AMaxNum"));
            BMinSize = float.Parse(XMLParser.getInstance().get("//standard[@id='" + NameId + "']/BMinSize"));
            BMaxNum = float.Parse(XMLParser.getInstance().get("//standard[@id='" + NameId + "']/BMaxNum"));
            Aradius = float.Parse(XMLParser.getInstance().get("//standard[@id='" + NameId + "']/Aradius"));
            LorR = XMLParser.getInstance().get("//standard[@id='" + NameId + "']/LorR") == "左";

        }
        /// <summary>
        /// 将更改更新到xml中
        /// </summary>
        public void RefrashParameter(string node,string value)
        {
            XMLParser.getInstance().set("//standard" + "[@id=" + NameId + "]/"+node, value);
           
        }
        public int recordcount;
        public Dictionary<int, string> AllRecordType;

        public int month;

        public int day;

        private int _good;
        public int good
        {
            get { return _good; }
            set
            {
                _good = value; OnPropertyChanged("good"); 

            }
        }
        private int _bad;
        public int bad
        {
            get { return _bad; }
            set
            {
                _bad = value; OnPropertyChanged("bad"); 

            }
        }
        private int _all;
        public int all
        {
            get { return _all; }
            set
            {
                _all = value; OnPropertyChanged("all");

            }
        }

  

        /// <summary>
        /// 刷新生产记录
        /// </summary>
        public void UpdateRecords()
        {
            XMLParser.getInstance().getAllNodes("//records",out recordcount,out AllRecordType);
            bool Fond = false;
            for (int i = 0; i < Count; i++)
            {

                if (day.ToString() == AllRecordType[i])
                {
                    Fond = true;
                    RefreshRecord();
                }
            }
            if (!Fond)
            {
                all = 0;
                good = 0;
                bad = 0;
                CreatRecord();
            }
        }
        public int typecont;
        public Dictionary<int, string> AllType;
        public void RefreshRecord()
        {
            //XMLParser.getInstance().set("//record" + "[@id=" + day.ToString()+ "]/month", month.ToString());

            XMLParser.getInstance().getAllNodes("//types",out typecont,out AllType);
            bool Fond = false;
            for (int i = 0; i < typecont; i++)
            {
                if (NameId == AllType[i])
                {
                    Fond = true;
                    XMLParser.getInstance().set("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]/all", all.ToString());
                    XMLParser.getInstance().set("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]/bad", bad.ToString());
                    XMLParser.getInstance().set("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]/good", good.ToString());
                }
            }
            if (!Fond)
            {
                XMLParser.getInstance().addNodeWithID("//types", "type", NameId);
                XMLParser.getInstance().addNodeWithText("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]", "bad", bad.ToString());
                XMLParser.getInstance().addNodeWithText("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]", "all", all.ToString());
                XMLParser.getInstance().addNodeWithText("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]", "good", good.ToString());

            }
        }
        public void CreatRecord()
        {
            XMLParser.getInstance().addNodeWithID("//records", "record", day.ToString());
            //
            XMLParser.getInstance().addNodeWithText("//record[@id=" + day.ToString() + "]", "month", month.ToString());
            XMLParser.getInstance().addNode("//record[@id=" + day.ToString() + "]", "types");
            XMLParser.getInstance().addNodeWithID("//record[@id=" + day.ToString() + "]"+"/types", "type", NameId);
            XMLParser.getInstance().addNodeWithText("//record[@id=" + day.ToString() + "]" + "/types"+"/type[@id=" + NameId + "]", "bad", bad.ToString());
            XMLParser.getInstance().addNodeWithText("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]", "all", all.ToString());
            XMLParser.getInstance().addNodeWithText("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]", "good", good.ToString());


        }
        /// <summary>
        /// 下载生产记录
        /// </summary>
        public void DownLoadRecord()
        {
            int cont;
            Dictionary<int, string> AllType;
            bool flag = false;

            XMLParser.getInstance().getAllNodes("//record[@id=" + day.ToString() + "]" + "/types", out cont, out AllType);
            for (int i = 0; i < cont; i++)
            {
                if (AllType[i] == NameId)
                {
                    flag = true;
                    all = int.Parse(XMLParser.getInstance().get("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]/all"));
                    good = int.Parse(XMLParser.getInstance().get("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]/good"));
                    bad = int.Parse(XMLParser.getInstance().get("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]/bad"));
                }
            }
            if (!flag)
            {
                all = 0;
                good = 0;
                bad = 0;
                XMLParser.getInstance().addNodeWithID("//record[@id=" + day.ToString() + "]" + "/types", "type" ,NameId);
                XMLParser.getInstance().addNodeWithText("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]", "bad", bad.ToString());
                XMLParser.getInstance().addNodeWithText("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]", "all", all.ToString());
                XMLParser.getInstance().addNodeWithText("//record[@id=" + day.ToString() + "]" + "/types" + "/type[@id=" + NameId + "]", "good", good.ToString());
            }
           
        }
        /// <summary>
        /// 参数结构体用于存储打包的参数
        /// </summary>
        public ParameterPackage Parameters;
        /// <summary>
        /// 打包参数
        /// </summary>
        public void PackageParameter()
        {
            Parameters = new ParameterPackage(NameId, Radius, Height, CenterX, CenterY, PixPerMm, LorR,
                DeltAx, DeltAy, DeltB, AMinSize, AMaxNum, BMinSize, BMaxNum,Aradius);
        }
        /// <summary>
        /// 划分瑕疵
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <param name="f"></param>
        /// <param name="flawgood"></param>
        /// <param name="flawbad"></param>
        public void DivideFlaws(double[] x, double[] y, double[] r, bool[] f,out List<FlawPoint>flawgood,out List<FlawPoint>flawbad)
        {
            flawbad = null;
            flawgood = null;
            for (int i = 0; i < x.Length; i++)
            {
                if (f[i])
                {
                    flawgood.Add(new FlawPoint(x[i], y[i], r[i]));
                }
                else
                {
                    flawbad.Add(new FlawPoint(x[i], y[i], r[i]));
                }
            }
        
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
