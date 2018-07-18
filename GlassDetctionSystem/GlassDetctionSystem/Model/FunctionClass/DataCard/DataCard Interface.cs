using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassDetctionSystem.Model.FunctionClass.DataCard
{
    interface DataCard_Interface
    {
        void open();
        bool IsOpen();
        bool SetDeviceDiscription(string DeviceDiscription);
        string GetDeviceDiscription();
        byte ReadByte();
        bool ReadInBits(int BitIndex);
        bool WriteByte(byte Data);
        bool WriteByte(int Data);
        bool WriteByte(int BitIndex, bool Data);
        bool Close();
        bool IsClose();
    }
}
