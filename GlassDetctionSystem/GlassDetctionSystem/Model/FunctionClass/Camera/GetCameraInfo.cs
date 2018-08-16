using System;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassDetctionSystem.Model.FunctionClass.Camera;
using Basler.Pylon;
using System.Collections.Generic;


namespace GlassDetctionSystem.Model.FunctionClass.Camera
{
    class GetCameraInfo
    {
        /// <summary>
        /// 创建连接，传入相机类型，使用DeviceType类进行有效赋值,检测所有可能的usb相机设备的连接，并返回连接相机数目，以及相机信息列表；
        /// 若存在连接返回true，若无连接返回false
        /// </summary>
        /// <param name="DeviceType"></param>
        /// <param name="DeviceNum"></param>
        /// <param name="AllCameraInfo"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public  bool TryGetConnect(string DeviceType, out int DeviceNum, out List<ICameraInfo> AllCameraInfo, out string ErrorMessage)
        {
            List<ICamera> cameras = new List<ICamera>();
            try
            {

                AllCameraInfo = CameraFinder.Enumerate(Basler.Pylon.DeviceType.GigE);
                DeviceNum = AllCameraInfo.Count;
                if (DeviceNum == 0)
                {
                    DeviceNum = 0;
                    AllCameraInfo = null;
                    ErrorMessage = "0个设备连接";
                    return false;
                }
                else
                {
                    ErrorMessage = null;
                    return true;
                }
            }
            catch (Exception exception)
            {

                ErrorMessage = exception.Message;
                DeviceNum = 0;
                AllCameraInfo = null;
                return false;
            }

        }
    }
}
