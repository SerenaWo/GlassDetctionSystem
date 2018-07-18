using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.BDaq;

namespace GlassDetctionSystem.Model.FunctionClass.DataCard
{
    class DataCard
    {
        /// <summary>
        /// 只有这一个设备，直接用它进行初始化
        /// 这个字符串需要设备管理器里的设备属性值来填.
        /// 暂时使用默认值"PCI-1761_BID#0".
        /// </summary>
        private string DeviceDiscription = "PCI-1761,BID#0";

        /// <summary>
        /// 实例创建所用的Description
        /// </summary>
        private String deviceDescriptionOfInstance = "";

        /// <summary>
        /// 封装的实例
        /// </summary>
        private InstantDiCtrl diInstance = null;
        private InstantDoCtrl doInstance = null;

        /// <summary>
        /// 标识实例是否过期
        /// </summary>
        private bool isOutDate()
        {
            return DeviceDiscription != deviceDescriptionOfInstance;
        }
    }
}
