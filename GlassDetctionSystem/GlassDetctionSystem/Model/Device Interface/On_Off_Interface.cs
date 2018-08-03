using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassDetctionSystem.Model.Device_Interface
{
    interface On_Off_Interface 
    {
        void Open();
        bool IsOpen();
        void  Close();
        //bool IsClose();
    }
}
