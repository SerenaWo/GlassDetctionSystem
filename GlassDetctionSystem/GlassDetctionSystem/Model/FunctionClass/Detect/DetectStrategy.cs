using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GlassDetctionSystem.Model.FunctionClass.Detect
{
    interface DetectStrategy
    {
        bool getSignal(Port port);
        void detect(Port port);
        void unReady(Port port);
    }
}
