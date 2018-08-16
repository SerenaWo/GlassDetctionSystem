using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basler.Pylon;
using GlassDetctionSystem.Model.FunctionClass.Camera;
using GlassDetctionSystem.Model.FunctionClass.DataCard;
using GlassDetctionSystem.Model.InteractiveData;

namespace GlassDetctionSystem.Model.FormModel
{
    public class InitializeModel
    {
        //相机启动部分
        public  int ConnectCameraTime = 500;

        public  int CameraNum=0;
        public  bool CameraIsOpen=false;
        public  int SelectCameraIndex = 0;
        public  string ErrorMessage;
        public  List<ICameraInfo> AllCameraInfo;

        //public static ICameraInfo ChosenCameraInfo;

        public void GetCameraNum()
        {
            GetCameraInfo TestGet = new GetCameraInfo();
            TestGet.TryGetConnect(DeviceType.GigE, out CameraNum, out AllCameraInfo, out ErrorMessage);

        }
        public void TryInitializeCamera()
        {
             ChosenCamera.ChosenCameraInfo= AllCameraInfo[SelectCameraIndex];

            if (ICamera.getInstance(AllCameraInfo[SelectCameraIndex]).SoftwareTriggerSet(out ErrorMessage))
            {
                if (ICamera.getInstance(AllCameraInfo[SelectCameraIndex]).OpenCamera(out ErrorMessage))
                {
                    ICamera.getInstance(AllCameraInfo[SelectCameraIndex]).GrabReady(out ErrorMessage);

                }
            }
            CameraIsOpen = ICamera.getInstance(AllCameraInfo[SelectCameraIndex]).IsOpen;
        }
        public void DisposeCamera()
        {
            if (AllCameraInfo != null)
            {
                if (ICamera.getInstance(AllCameraInfo[SelectCameraIndex]) != null)
                {
                    ICamera.getInstance(AllCameraInfo[SelectCameraIndex]).Close();
                    ICamera.getInstance(AllCameraInfo[SelectCameraIndex]).Dispose();
                    //ICamera.getInstance(AllCameraInfo[SelectCameraIndex]) = null;
                }
                    
            }
            
        }

        //数据卡启动部分
        public  int ConnectDataCardTime = 500;
        public  bool DataCardIsOpen = false;
        public  string DataCardDiscription=null;

        public void TryInitializeDataCard()
        {
            DataCard.getinstance().Open();
            DataCardIsOpen = DataCard.getinstance().IsOpen();
            DataCardDiscription = DataCard.getinstance().GetInstanceDiscription();
            
        }

    }
}
