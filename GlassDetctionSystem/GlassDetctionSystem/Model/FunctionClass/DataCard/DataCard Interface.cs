using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassDetctionSystem.Model.Device_Interface;

namespace GlassDetctionSystem.Model.FunctionClass.DataCard
{
    interface DataCard_Interface : On_Off_Interface
    {
        new void  Open();
        new bool IsOpen();
        void SetDeviceDiscription(string DeviceDiscription);
        string GetInstanceDiscription();
        string GetDeviceDiscription();
        byte ReadByte();
        bool ReadInBits(int BitIndex);
        void WriteByte(byte Data);
        void WriteByte(int Data);
        void WriteInBits(int BitIndex, int  Data);
        new void Close();
        //new bool IsClose();
    }
}
