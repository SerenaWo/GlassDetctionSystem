using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassDetctionSystem.Model.FunctionClass.Detect
{
    class DetectObserver
    {
        private bool startDetectSignal;
        private DetectStrategy detectStrategy;
        private Port port;

        public DetectObserver(DetectStrategy detectStrategy, Port port)
        {
            startDetectSignal = detectStrategy.getSignal(port);
            this.port = port;
            this.detectStrategy = detectStrategy;
        }

        public void tick()
        {
            while (true)
            {
                if (startDetectSignal)
                {
                    startDetectSignal = detectStrategy.getSignal(port);
                    if (!startDetectSignal)
                    {//下降沿
                        detectStrategy.unReady(port);
                    }
                }
                else
                {
                    startDetectSignal = detectStrategy.getSignal(port);
                    if (startDetectSignal)
                    {//上升沿
                        detectStrategy.detect(port);
                    }
                }
            }
        }
    }
}
