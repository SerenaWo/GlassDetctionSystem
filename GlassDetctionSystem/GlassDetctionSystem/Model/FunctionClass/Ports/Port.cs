using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassDetctionSystem.Model.FunctionClass
{
    public class Port
    {
        public int do_ready;        //软件准备好
        public int do_detected;     //检测完成
        public int do_quality0;     //品质信息位
        public int do_quality1;     //品质信息位
        public int di_detect;       //检测信号
        public int di_productA;     //产品A
        public int di_productB;     //产品B

        private const int maxPortIndex = 2;
        private static Port[] portInstances = new Port[maxPortIndex];

        private Port(
            int do_detected,
            int do_ready,
            int do_quality0,
            int do_quality1,
            int di_detect,
            int di_productA,
            int di_productB
            )
        {
            this.do_detected = do_detected;
            this.do_quality0 = do_quality0;
            this.do_quality1 = do_quality1;
            this.do_ready = do_ready;
            this.di_detect = di_detect;
            this.di_productA = di_productA;
            this.di_productB = di_productB;
        }

        public static Port getInstance(int index)
        {
            if (index < 0 || index > 1)
            {
                throw new Exception("端口数目Out of range");
            }

            Port port = portInstances[index];

            if (port == null)
            {
                //一个数据卡分两个端口，所以这里的index的可能值为0和1
                port = index == 0 ? new Port(0, 1, 2, 3, 0, 1, 2) : new Port(4, 5, 6, 7, 4, 5, 6);
            }
            return port;
        }
    
    }
}
