using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using GlassDetctionSystem.Model.InteractiveData;
using GlassDetctionSystem.Model.FunctionClass.XML;
using System.Runtime.InteropServices;
using Basler.Pylon;


namespace GlassDetctionSystem.Model.FormModel
{
    public class TempTestModel : INotifyPropertyChanged
    {
        private int _AOIp;
        public int AOIp
        {
            get { return _AOIp; }
            set { _AOIp = value; OnPropertyChanged("AOIp");  }
        }
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

        [DllImport(@"OpencvProcess-cal.dll", EntryPoint = "read")]
        extern static void read(IntPtr dd, int height, int wheight);
        [DllImport(@"OpencvProcess-cal.dll", EntryPoint = "calibration")]
        extern static double calibration(IntPtr dd, int height,
                                        int wheight, int cornerw,
                                        int cornerh, double length);
        [DllImport(@"OpencvProcess-cal.dll", EntryPoint = "AOI")]
        extern static void AOI(int houghroi);
        [DllImport(@"OpencvProcess-cal.dll", EntryPoint = "Detect")]
        extern static void Detect(int houghroi);

        public TempTestModel()
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
        public void HoughAOI()
        {
           
            AOI(AOIp);

        }
        public void Detect()
        {
            ICamera.getInstance(ChosenCameraInfo).Trigger
               (out status,
               out ImageWidth,
               out ImageHeight,
               out buffer,
               out ErrorMessage);
            Detect(AOIp);

        }
        public void AutoExposureMode()
        {
            ICamera.getInstance(ChosenCameraInfo).GrabStop();
            ICamera.getInstance(ChosenCameraInfo).SetAutoExposure(out ErrorMessage);
            //ExposureTime = ICamera.getInstance(ChosenCameraInfo).GetCurrentExposureTime();
            ICamera.getInstance(ChosenCameraInfo).GrabReady(out ErrorMessage);

        }
        public void ChangeExposureMode()
        {
            ICamera.getInstance(ChosenCameraInfo).GrabStop();
            ICamera.getInstance(ChosenCameraInfo).CloseAutoExposure(out ErrorMessage);
            ICamera.getInstance(ChosenCameraInfo).GrabReady(out ErrorMessage);

        }
        public void SetExposureTime()
        {
            ICamera.getInstance(ChosenCameraInfo).GrabStop();
            ICamera.getInstance(ChosenCameraInfo).SetExposureTime(ExposureTime, out ErrorMessage);
            ICamera.getInstance(ChosenCameraInfo).GrabReady(out ErrorMessage);
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
