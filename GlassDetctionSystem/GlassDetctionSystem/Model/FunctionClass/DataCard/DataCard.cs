using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.BDaq;
using GlassDetctionSystem.Model;

namespace GlassDetctionSystem.Model.FunctionClass.DataCard
{
    class DataCard : DataCard_Interface
    {
        /// <summary>
        /// 数据缓存，存储当前状态（解决数据卡写入后，继电器转换延迟，立即读取数据不准的问题）
        /// </summary>
        private byte doBuffer;
        /// <summary>
        /// 只有这一个设备，直接用它进行初始化
        /// 这个字符串需要设备管理器里的设备属性值来填.
        /// 暂时使用默认值"PCI-1761_BID#0".
        /// </summary>
        private string DeviceDiscription = "PCI-1761,BID#0";

        /// <summary>
        /// 实例创建所用的Description
        /// </summary>
        private String DeviceDiscriptionOfInstance = "";

        /// <summary>
        /// 封装的实例
        /// </summary>
        private InstantDiCtrl DiInstance = null;
        private InstantDoCtrl DoInstance = null;

        /// <summary>
        /// 标识实例是否过期
        /// </summary>
        private bool isOutDate()
        {
            return DeviceDiscription != DeviceDiscriptionOfInstance;
        }
        /// <summary>
        /// 尝试启动
        /// </summary>
        public void Open()
        {
            if (!IsOpen())
            {
                DiInstance = new InstantDiCtrl();
                DiInstance.SelectedDevice = new DeviceInformation(DeviceDiscription);

                DoInstance = new InstantDoCtrl();
                DoInstance.SelectedDevice = new DeviceInformation(DeviceDiscription);

                DeviceDiscriptionOfInstance = DeviceDiscription;
            }
            doBuffer = Read_do();
        }

        /// <summary>
        /// 是否启动
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            if (DiInstance == null || DoInstance == null)
            {//两个实例都不能是空
                return false;
            }
            if (isOutDate())
            {//实例不能过期
                return false;
            }

            //如果两个实例都初始化了,那就认为启动好了
            return DiInstance.Initialized && DoInstance.Initialized;

        }
        /// <summary>
        /// 设置描述字符串
        /// </summary>
        /// <param name="Discription"></param>
        public void SetDeviceDiscription(String Discription)
        {
            DeviceDiscription = Discription;
        }

        /// <summary>
        /// 获得描述字符串
        /// </summary>
        /// <returns></returns>
        public String GetDeviceDiscription()
        {
            return DeviceDiscription;
        }
        /// <summary>
        /// 获取已存在实例的描述符
        /// </summary>
        /// <returns></returns>
        public String GetInstanceDiscription()
        {
            return DeviceDiscriptionOfInstance;
        }

        /// 读取整个8位数据
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            byte data;
            DiInstance.Read(0, out data);
            return data;
        }

        public virtual byte Read_do()
        {
            byte data;
            DoInstance.Read(0, out data);
            return data;
        }
        /// <summary>
        /// 读取特定位上的数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool ReadInBits(int index)
        {
            return (ReadByte() & (1 << index)) > 0;
        }
        /// <summary>
        /// 写入整个8位数据
        /// </summary>
        /// <param name="data"></param>
        public virtual void WriteByte(byte data)
        {
            DoInstance.Write(0, data);
            doBuffer = data;
        }

        public void WriteByte(int data)
        {
            data = data & 0xff;
            WriteByte((byte)data);
        }

        /// <summary>
        /// 将某一位置一
        /// </summary>
        /// <param name="index"></param>
        /// <param name="data"></param>
        public void WriteInBits(int  BitIndex, int data)
        {
            if (data > 0)
            {
                set(BitIndex);
            }
            else
            {
                reset(BitIndex);
            }
        }

        private void set(int index)
        {
            byte before = doBuffer;
            int mask = 1 << index;
            if ((before & mask) == 0)
            {
                WriteByte(before | mask);
            }
        }

        private void reset(int index)
        {
            byte before = doBuffer;
            int mask = 1 << index;
            if ((before & mask) != 0)
            {
                WriteByte(before & (~mask));
            }
        }
        /// <summary>
        /// 尝试释放资源
        /// </summary>
        public void Close()
        {
            DeviceDiscriptionOfInstance = "";

            if (DoInstance != null)
            {
                if (DoInstance.Initialized)
                {
                    DoInstance.Dispose();
                }
                DoInstance = null;
            }
            if (DiInstance != null)
            {
                if (DiInstance.Initialized)
                {
                    DiInstance.Dispose();
                }
                DiInstance = null;
            }
        }
    }
}
