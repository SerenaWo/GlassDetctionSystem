using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassDetctionSystem.Model.FunctionClass.Camera;
using GlassDetctionSystem.Model.FunctionClass.DataCard;
using Basler.Pylon;

namespace GlassDetctionSystem.Model.FunctionClass.Detect
{
    abstract class AbstractCameraStrategy
    {
        //GetCameraInfo getCameraInfo = new GetCameraInfo();
        //int cameraIndex = 0;
        //int DeviceNum;
        //List<ICameraInfo> AllCameraInfo;
        string ErrorMessage;
        ICameraInfo cameraInfo;


        //构造时初始化相机和端口
        public AbstractCameraStrategy(ICameraInfo cameraInfo)
        {
            this.cameraInfo = cameraInfo;
            //getCameraInfo.TryGetConnect(DeviceType.GigE, out DeviceNum, out AllCameraInfo, out ErrorMessage);
            ICamera Icamera = ICamera.getInstance(cameraInfo);
        }

        //第0位的上升沿是相机开始检测的信号
        public bool getSignal(Port port)
        {
            return DataCard.DataCard.getinstance().ReadInBits (port.di_detect);
        }

        public abstract void detect(Port port);

        public void unReady(Port port)
        {
            DataCard.DataCard.getinstance().WriteInBits(port.do_detected, 0);//检测未完成
        }
    }
}
