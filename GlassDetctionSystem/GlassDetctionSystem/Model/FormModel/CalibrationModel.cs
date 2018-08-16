using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basler.Pylon;
using GlassDetctionSystem.Controller;
using GlassDetctionSystem.Model.InteractiveData;
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
        [DllImport(@"OpencvProcess.dll", EntryPoint = "read")]
        extern static void read(IntPtr dd, int height, int wheight);
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
            ICamera.getInstance(ChosenCameraInfo).GrabReady(out ErrorMessage);

        }
        public void SetExposureTime()
        {
            ICamera.getInstance(ChosenCameraInfo).GrabStop();
            ICamera.getInstance(ChosenCameraInfo).SetExposureTime(ExposureTime,out ErrorMessage);
            ICamera.getInstance(ChosenCameraInfo).GrabReady(out ErrorMessage);
        }

        public void UploadParemeter()
        {

        }
        public void DownloadParemeter()
        {

        }

        public void CalibratCamera()
        {
            //获取标定参数，存在model里
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
