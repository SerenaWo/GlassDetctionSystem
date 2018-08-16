using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basler.Pylon;

namespace GlassDetctionSystem.Model.FunctionClass.Detect
{
    class CameraStrategy:AbstractCameraStrategy
    {
        string ErrorMessage;
        ICameraInfo cameraInfo;
        ICamera Icamera;


        //构造时初始化相机和端口
        public CameraStrategy(ICameraInfo cameraInfo):base(cameraInfo)
        {
            this.cameraInfo = cameraInfo;
            //getCameraInfo.TryGetConnect(DeviceType.GigE, out DeviceNum, out AllCameraInfo, out ErrorMessage);
             Icamera = ICamera.getInstance(cameraInfo);
        }
        public override void detect(Port port)
        {
            //获得收据库所有参数
            //用相机获得照片
            //传入opencv，传出处理结果和检测后图片
            //判断AB区是否合格
            //数据卡写品质位
        }
    }
}
