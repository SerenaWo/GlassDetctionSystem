using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GlassDetctionSystem.Model.FunctionClass.ImageProcess
{
    public class Transport
    {
        ICamera icamera;
        private static Transport transport;
        private Transport(ICamera camera)
        { this.icamera = camera; }
        public static Transport getinstance(ICamera ccamera)
        {
            if (transport == null)
            {
                transport = new Transport(ccamera);
            }
            return transport;
        }

        public int status;
        public int ImageWidth;
        public int ImageHeight;
        public byte[] buffer;
        string ErrorMessage;
        [DllImport(@"OpencvProcess.dll", EntryPoint = "read")]
        extern static void read(IntPtr dd, int height, int wheight);
        public void GetImage()
        {

            icamera.Trigger
                 (out status,
                 out ImageWidth,
                 out ImageHeight,
                 out buffer,
                 out ErrorMessage);
            //将buffer的指针传递给opencv
            GCHandle hObject = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr pObject = hObject.AddrOfPinnedObject();
            if (hObject.IsAllocated)
                hObject.Free();
            //opencvtest中的read将图片写成test2
            read(pObject, ImageWidth, ImageHeight);

        }
    }
}
