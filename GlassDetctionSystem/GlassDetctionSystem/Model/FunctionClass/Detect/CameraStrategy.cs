using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basler.Pylon;
using GlassDetctionSystem.Model.InteractiveData;
using System.Runtime.InteropServices;
using GlassDetctionSystem.Model.FunctionClass.ImageProcess;
using GlassDetctionSystem.Model.FunctionClass.DataCard;

namespace GlassDetctionSystem.Model.FunctionClass.Detect
{
    class CameraStrategy:AbstractCameraStrategy
    {
        string ErrorMessage;
        ICameraInfo cameraInfo;
        ICamera Icamera;
        ParameterPackage parameters;
        string TimeStamp;
        bool Quality;
        int Anum;
        int Bnum;
        int SizeA;
        int SizeB;



        //构造时初始化相机和端口
        public CameraStrategy(ICameraInfo cameraInfo, ParameterPackage parameters) :base(cameraInfo,parameters)
        {
            this.cameraInfo = cameraInfo;
            this.parameters = parameters;
            //getCameraInfo.TryGetConnect(DeviceType.GigE, out DeviceNum, out AllCameraInfo, out ErrorMessage);
             Icamera = ICamera.getInstance(cameraInfo);
        }
        public override void detect(Port port)
        {
            reset(port);//开始检测前首先复位品质信息位，再将检测完成信号拉低
            Transport.getinstance(Icamera).GetImage();
            TimeStamp = System.DateTime.Now.ToString("T");
            Detection.getinstance(parameters).DetectImage(out Quality,out Anum,out Bnum,out SizeA,out SizeB);
            ResulBuffer.Quality = Quality;
            ResulBuffer.Anum = Anum;
            ResulBuffer.Bnum = Bnum;
            ResulBuffer.SizeA = SizeA;
            ResulBuffer.SizeB = SizeB;
            ResulBuffer.time = TimeStamp;
            //至此界面能检测到更新，写数据卡与更新界面将同时操作
            if (Quality)
            {
                buhege(port);
            
            }
            else
            {
                hege(port);
            }

           
            
        }
        //对应的数据卡品质信息位：
        //do_quality0  do_quality1
        //     1            0       合格
        //     0            1       不合格
        protected void hege(Port port)
        {
            DataCard.DataCard.getinstance().WriteInBits (port.do_quality0, 1);
            DataCard.DataCard.getinstance().WriteInBits(port.do_quality1, 0);//合格
            DataCard.DataCard.getinstance().WriteInBits(port.do_detected, 1);//检测完成
        }
        protected void buhege(Port port)
        {
            DataCard.DataCard.getinstance().WriteInBits(port.do_quality0, 0);
            DataCard.DataCard.getinstance().WriteInBits(port.do_quality1, 1);//不合格
            DataCard.DataCard.getinstance().WriteInBits(port.do_detected, 1);//检测完成
        }
        protected void reset(Port port)
        {
            //复位
            DataCard.DataCard.getinstance().WriteInBits(port.do_quality0, 0);
            DataCard.DataCard.getinstance().WriteInBits(port.do_quality1, 0);
            DataCard.DataCard.getinstance().WriteInBits(port.do_detected, 0);
        }

    }
}
