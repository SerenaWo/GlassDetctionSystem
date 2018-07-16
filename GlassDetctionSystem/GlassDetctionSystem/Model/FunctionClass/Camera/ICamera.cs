using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basler.Pylon;

namespace GlassDetctionSystem.Model.FunctionClass.Camera
{
    class ICamera : Basler.Pylon.Camera
    {
        /// <summary>
        /// 构造函数;
        /// 传入相机信息
        /// </summary>
        public ICamera(ICameraInfo cameraInfo) : base(cameraInfo)
        {
        }
        /// <summary>
        /// 创建连接，传入相机类型，使用DeviceType类进行有效赋值,检测所有可能的usb相机设备的连接，并返回连接相机数目，以及相机信息列表；
        /// 若存在连接返回true，若无连接返回false
        /// </summary>
        /// <param name="DeviceType"></param> 
        /// <param name="DeviceNum"></param>
        /// <param name="AllCameraInfo"></param>
        /// <returns></returns>
        public static  bool TryGetConnect(string DeviceType,out int DeviceNum,out List<ICameraInfo> AllCameraInfo)
        {
            try
            {

                AllCameraInfo = CameraFinder.Enumerate(DeviceType);
                DeviceNum = AllCameraInfo.Count;
                if (DeviceNum == 0)
                {
                    DeviceNum = 0;
                    AllCameraInfo = null;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception exception)
            {
                //此处应该加入显示错误信息
                DeviceNum = 0;
                AllCameraInfo = null;
                return false;
            }

        }
        /// <summary>
        /// 内部事件，当相机掉线时触发的处理事件(考虑使用外部事件)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void OnConnectionLost(Object sender, EventArgs e)
        {
            //输出相机掉线正在重新连接。。。
            

        }
        /// <summary>
        /// 配置触发模式为软件触发
        /// </summary>
        /// <returns></returns>
        public bool SoftwareTriggerSet()
        {
            try
            {
                this.CameraOpened += Configuration.SoftwareTrigger;
                this.ConnectionLost += OnConnectionLost;
                return true;
            }
            catch (Exception exception)
            {
                //此处应该加入显示错误信息
                return false;
                
            }
        }
        /// <summary>
        /// 配置曝光模式为连续自动曝光
        /// </summary>
        /// <returns></returns>
        public bool SetAutoExposure()
        {
            try
            {
                this.Parameters[PLUsbCamera.ExposureAuto].TrySetValue(PLUsbCamera.ExposureAuto.Continuous);
                //this.Parameters[PLUsbCamera.ExposureMode].TrySetValue(PLUsbCamera.ExposureMode.Timed);????
                return true;
            }
            catch (Exception exception)
            {
                //此处应该加入显示错误信息
                return false;

            }

        }
        /// <summary>
        /// 获取当前曝光时间
        /// </summary>
        /// <returns></returns>
        public double  GetCurrentExposureTime()
        {
            return this.Parameters[PLUsbCamera.ExposureTime].GetValue();
        }

        /// <summary>
        /// 配置曝光模式为按时间曝光
        /// </summary>
        /// <returns></returns>
        public bool CloseAutoExposure()
        {
            try
            {
                this.Parameters[PLUsbCamera.ExposureAuto].TrySetValue(PLUsbCamera.ExposureAuto.Off);
                this.Parameters[PLUsbCamera.ExposureMode].TrySetValue(PLUsbCamera.ExposureMode.Timed);                

                return true;
            }
            catch (Exception exception)
            {
                //此处应该加入显示错误信息
                return false;
            }

        }
        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="ExposureTime"></param>
        /// <returns></returns>
        public bool SetExposureTime(double ExposureTime)
        {
            try
            {
                this.Parameters[PLUsbCamera.ExposureTime].TrySetValue(ExposureTime);
                return true;
            }
            catch (Exception exception)
            {
                //此处应该加入显示错误信息
                return false;
            }
        }
        /// <summary>
        /// 打开相机，最大等待一秒后返回结果
        /// </summary>
        /// <returns></returns>
        public bool OpenCamera()
        {
            try
            {
                return this.Open(1000, TimeoutHandling.ThrowException);
       
            }
            catch (Exception exception)
            {
                //此处应该加入显示错误信息
                return false;
            }

        }
        /// <summary>
        /// 关闭并注销相机，在该函数执行后应将函数类置空，camera=null
        /// </summary>
        /// <returns></returns>
        public bool CloseCamera()
        {
            try
            {               
                    this.Close();
                    this.Dispose();
                    return true;
            }
            catch
            {
                //此处应该加入显示错误信息Console.Error.WriteLine("Exception: {0}", e.Message);
                return false;
            }

        }
        /// <summary>
        /// 获得相机AOI尺寸即图片尺寸,成功返回ture，且返回宽高，否则返回false，宽高为0
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public bool GetAOISize(out long Width, out long  Height)
        {
            try
            {
                Width = this.Parameters[PLCamera.Width].GetValue();
                Height = this.Parameters[PLCamera.Height].GetValue();
                return true;
               // PLCamera.PixelFormat.
            }
            catch
            {
                Width = 0;
                Height = 0;
                //此处应该加入显示错误信息Console.Error.WriteLine("Exception: {0}", e.Message);
                return false;
            }
        }

    }
}
