using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basler.Pylon;
using GlassDetctionSystem.Controller;
using GlassDetctionSystem.Model.InteractiveData;
using GlassDetctionSystem.Model.FunctionClass.XML;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ComponentModel;


namespace GlassDetctionSystem.Model.FormModel
{
    public class CalibrationModel : INotifyPropertyChanged
    {
        private double _ExposureTime;
        public double ExposureTime
        {
            get { return _ExposureTime; }
            set { _ExposureTime = value; OnPropertyChanged("ExposureTime"); SetExposureTime(); }
        }
    
        public string ErrorMessage;
        public ICameraInfo ChosenCameraInfo;
        public int status;
        public int ImageWidth;
        public int ImageHeight;
        public byte[] buffer;

        private int _Cornerw;
        public int Cornerw
        {
            get { return _Cornerw; }
            set { _Cornerw = value; OnPropertyChanged("Cornerw"); }
        }

        private int _Cornerh;
        public int Cornerh
        {
            get { return _Cornerh; }
            set { _Cornerh = value; OnPropertyChanged("Cornerh"); }
        }

        private double _Length;
        public double Length
        {
            get { return _Length; }
            set { _Length = value; OnPropertyChanged("Length"); }
        }

        private int _CenterX;
        public int CenterX
        {
            get { return _CenterX; }
            set { _CenterX = value; OnPropertyChanged("CenterX"); }
        }
        private int _CenterY;
        public int CenterY
        {
            get { return _CenterY; }
            set { _CenterY = value; OnPropertyChanged("CenterY"); }
        }
        public int imagew = 407;//待测试
        public int imageh = 339;//待测试


        public double PixPerMm;
        
        [DllImport(@"OpencvProcess-cal.dll", EntryPoint = "read")]
        extern static void read(IntPtr dd, int height, int wheight);
        [DllImport(@"OpencvProcess-cal.dll", EntryPoint = "calibration")]
        extern static double   calibration(IntPtr dd, int height,
                                        int wheight, int cornerw,
                                        int cornerh, double length);
        [DllImport(@"OpencvProcess-cal.dll", EntryPoint = "DrawCenter")]
        extern static void DrawCenter(int centerx, int centery);

        public CalibrationModel()
        {
            this.ChosenCameraInfo = ChosenCamera.ChosenCameraInfo;
        }
        public void GetImage()
        {
            //ICamera.getInstance(ChosenCameraInfo).GrabStop();
            ICamera.getInstance(ChosenCameraInfo).Trigger
                (out status,
                out ImageWidth,
                out ImageHeight,
                out buffer,
                out ErrorMessage);
            //将buffer的指针传递给opencv
            GCHandle hObject = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr pObject = hObject.AddrOfPinnedObject();
            if (hObject.IsAllocated)
                hObject.Free();
            //opencvtest中的read将图片写成test2
            read(pObject, ImageWidth, ImageHeight);

        }

        public void ChangeExposureMode()
        {
            ICamera.getInstance(ChosenCameraInfo).GrabStop();
            ICamera.getInstance(ChosenCameraInfo).CloseAutoExposure(out ErrorMessage);
            ICamera.getInstance(ChosenCameraInfo).GrabReady(out ErrorMessage);

        }
        public void AutoExposureMode()
        {
            ICamera.getInstance(ChosenCameraInfo).GrabStop();
            ICamera.getInstance(ChosenCameraInfo).SetAutoExposure(out ErrorMessage);
            ExposureTime= ICamera.getInstance(ChosenCameraInfo).GetCurrentExposureTime();
            ICamera.getInstance(ChosenCameraInfo).GrabReady(out ErrorMessage);

        }
        public void SetExposureTime()
        {
            ICamera.getInstance(ChosenCameraInfo).GrabStop();
            ICamera.getInstance(ChosenCameraInfo).SetExposureTime(ExposureTime,out ErrorMessage);
            ICamera.getInstance(ChosenCameraInfo).GrabReady(out ErrorMessage);
        }

        public void UploadParemeterPPM()
        {
            PixPerMm = float.Parse(XMLParser.getInstance().get("//platform[@id='0']/pxPerMm"));
        }
        public void UploadParemeterCenter()
        {
            CenterX = int.Parse(XMLParser.getInstance().get("//platform[@id='0']/CenterX"));
            CenterY= int.Parse(XMLParser.getInstance().get("//platform[@id='0']/CenterY"));
        }
        public void DownloadParemeter()
        {
            XMLParser.getInstance().set("//platform[@id='0']/pxPerMm", PixPerMm.ToString());
            //XMLParser.getInstance().set("//standard[@id='0']/exposureTime", ExposureTime.ToString());
            XMLParser.getInstance().set("//platform[@id='0']/CenterX", CenterX.ToString());
            XMLParser.getInstance().set("//platform[@id='0']/CenterY", CenterY.ToString());
        }

        public void CalibratCamera()
        {
            GetImage();
            //获取标定参数，存在model里
            ICamera.getInstance(ChosenCameraInfo).Trigger
                (out status,
                out ImageWidth,
                out ImageHeight,
                out buffer,
                out ErrorMessage);
            //将buffer的指针传递给opencv
            GCHandle hObject = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr pObject = hObject.AddrOfPinnedObject();
            if (hObject.IsAllocated)
                hObject.Free();
            try { PixPerMm = calibration(pObject, ImageHeight, ImageWidth, Cornerw, Cornerh, Length); }
            catch (Exception e)
            {
                ErrorMessage = e.Message;
                PixPerMm = -1;
            }

        }
        public void DrawCenterPoint()
        {
            
            DrawCenter(CenterX, CenterY);
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
