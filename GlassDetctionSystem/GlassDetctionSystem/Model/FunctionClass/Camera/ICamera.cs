using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basler.Pylon;

namespace GlassDetctionSystem
{
    public class ICamera : Basler.Pylon.Camera
    {
        private static ICamera Icamera;
        //string ErrorMessage;
        /// <summary>
        /// 构造函数;
        /// 传入相机信息
        /// </summary>
        private ICamera(ICameraInfo cameraInfo) : base(cameraInfo)
        {
        }
        public static ICamera getInstance(ICameraInfo cameraInfo)
        {
            //ICamera Icamera;
            if (Icamera == null)
            {
                Icamera = new ICamera(cameraInfo);
            }
            
            
            return Icamera;
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
        ///配置触发模式为软件触发
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public bool SoftwareTriggerSet(out string ErrorMessage)
        {
            try
            {
                this.CameraOpened += Configuration.SoftwareTrigger;
                this.ConnectionLost += OnConnectionLost;
                ErrorMessage = null;
                return true;
            }
            catch (Exception exception)
            {
                ErrorMessage = exception.Message;
                return false;                
            }
        }

        /// <summary>
        /// 配置曝光模式为连续自动曝光
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public bool SetAutoExposure(out string ErrorMessage)
        {
            try
            {
                this.Parameters[PLGigECamera.ExposureAuto].TrySetValue(PLGigECamera.ExposureAuto.Continuous);
                //this.Parameters[PLUsbCamera.ExposureMode].TrySetValue(PLUsbCamera.ExposureMode.Timed);????
                ErrorMessage = null;
                return true;
            }
            catch (Exception exception)
            {
                ErrorMessage = exception.Message;
                return false;
            }

        }
        /// <summary>
        /// 获取当前曝光时间
        /// </summary>
        /// <returns></returns>
        public double  GetCurrentExposureTime()
        {
            return this.Parameters[PLGigECamera.ExposureTimeAbs].GetValue();
        }

        /// <summary>
        /// 配置曝光模式为按时间曝光
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public bool CloseAutoExposure(out string ErrorMessage)
        {
            try
            {
                this.Parameters[PLGigECamera.ExposureAuto].TrySetValue(PLUsbCamera.ExposureAuto.Off);
                this.Parameters[PLGigECamera.ExposureMode].TrySetValue(PLUsbCamera.ExposureMode.Timed);
                
                ErrorMessage = null;
                return true;
            }
            catch (Exception exception)
            {
                ErrorMessage = exception.Message;
                return false;
            }

        }
        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="ExposureTime"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public bool SetExposureTime(double ExposureTime, out string ErrorMessage)
        {
            try
            {
                this.Parameters[PLGigECamera.ExposureTimeAbs].TrySetValue(ExposureTime);
                ErrorMessage = null;
                return true;
            }
            catch (Exception exception)
            {
                ErrorMessage = exception.Message;
                return false;
            }
        }
        /// <summary>
        /// 打开相机，最大等待一秒后返回结果
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public bool OpenCamera(out string ErrorMessage)
        {
            try
            {
                ErrorMessage = null;
                return this.Open(1000, TimeoutHandling.ThrowException);
       
            }
            catch (Exception exception)
            {
                ErrorMessage = exception.Message;
                return false;
            }

        }
        /// <summary>
        /// 关闭并注销相机，在该函数执行后应将函数类置空，camera=null
        /// </summary>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public bool CloseCamera(out string ErrorMessage)
        {
            try
            {               
                    this.Close();
                    this.Dispose();
                    ErrorMessage = null;
                    return true;
            }
            catch(Exception exception)
            {
                ErrorMessage = exception.Message;
                return false;
            }

        }
        /// <summary>
        /// 获得相机AOI尺寸即图片尺寸,成功返回ture，且返回宽高，否则返回false，宽高为0
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public bool GetAOISize(out long Width, out long  Height, out string ErrorMessage)
        {
            try
            {
                Width = this.Parameters[PLCamera.Width].GetValue();
                Height = this.Parameters[PLCamera.Height].GetValue();
                ErrorMessage = null;
                return true;
               // PLCamera.PixelFormat.
            }
            catch (Exception exception)
            {
                Width = 0;
                Height = 0;
                ErrorMessage = exception.Message;
                return false;
            }
        }
        /// <summary>
        /// 使相机处于可以抓取状态
        /// </summary>
        /// <returns></returns>
        public bool GrabReady(out string ErrorMessage)
        {
            if (this.IsOpen && this.IsConnected)
            {
                try
                {
                    this.StreamGrabber.Start();
                    if (this.StreamGrabber.IsGrabbing)
                    {
                        ErrorMessage = null;
                        return true;
                    }
                    else
                    {
                        ErrorMessage = "相机不在可抓取状态！";
                        return false;
                    }

                }
                catch (Exception exception)
                {
                   
                    ErrorMessage = exception.Message;
                    return false;
                }

            }
            else
            {
                ErrorMessage = "相机未打开或者未连接！";
                return false;
            }            
        }
        /// <summary>
        /// 使相机停止可以抓取状态
        /// </summary>
        /// <returns></returns>
        public bool GrabStop()
        {
            this.StreamGrabber.Stop();
            if (!this.StreamGrabber.IsGrabbing)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 触发一次拍照，返回图片宽高和图片数组
        /// </summary>
        /// <param name="status"></param>
        /// <param name="ImageWidth"></param>
        /// <param name="ImageHeight"></param>
        /// <param name="buffer"></param>
        /// <param name="ErrorMessage"></param>
        /// <returns></returns>
        public bool Trigger(out int status,out int ImageWidth,out int ImageHeight,out byte[] buffer, out string ErrorMessage)
        {
            const int isDone = 0;
            const int noGrabing = 1;
            const int isTriggering = 2;
            const int ResultError = 3;
            if (WaitForFrameTriggerReady(1000, TimeoutHandling.ThrowException))
            {
                if (this.StreamGrabber.IsGrabbing)
                {
                    this.ExecuteSoftwareTrigger();
                    IGrabResult grabResult = StreamGrabber.RetrieveResult(5000, TimeoutHandling.ThrowException);
                    using (grabResult)
                    {
                        if (grabResult.GrabSucceeded)
                        {
                            ImageWidth = grabResult.Width;
                            ImageHeight = grabResult.Height;
                            
                            buffer = grabResult.PixelData as byte[];
                            ImagePersistence.Save(ImageFileFormat.Bmp, "test.bmp", grabResult);
                            status = isDone;
                            ErrorMessage = null;
                            return true;
                        }
                        else
                        {
                            status = ResultError;
                            ImageWidth = 0;
                            ImageHeight =0;
                            buffer = null;
                            ErrorMessage = grabResult.ErrorCode+grabResult.ErrorDescription;
                            return false;
                        }

                    }
                                  
                }
                else
                {
                    status = noGrabing;
                    ErrorMessage = "相机不在抓取状态下。";
                    ImageWidth = 0;
                    ImageHeight = 0;
                    buffer = null;
                    return false;

                }

            }
            else
            {
                ImageWidth = 0;
                ImageHeight = 0;
                buffer = null;
                status = isTriggering;
                ErrorMessage = "正在等待前一次触发完成。";
                return false;
            }

        }
        



    }
}
