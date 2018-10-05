using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace GlassDetctionSystem.Model.FunctionClass.ImageProcess
{
    public class Detection
    {
        ParameterPackage parameters;
        private static Detection detection;
        private Detection(ParameterPackage parameters)
        {
            this.parameters = parameters;
        }
        public static Detection getinstance(ParameterPackage parameters)
        {
            
            
                detection = new Detection(parameters);
            
                return detection;
        }

        [DllImport(@"OpencvProcess-cal.dll", EntryPoint = "Detect")]
        extern static void Detect(int centerx, int centery, double height,
    double radius, double BAeraPrameter, double AAreaDistancex, double AAreaDistancey,
    double AAreaRedius, double minAradius, double minBradius, double maxAnum, double maxBnum, bool LorR, double PixPerMm,
    ref bool quality, ref int anum, ref int bnum,ref int sizea,ref int sizeb);

        public void DetectImage(out bool quality,out int numa,out int numb,out int sizea,out int sizeb)
        {
            bool q = false;
            int a = 0;
            int b = 0;
            int sa = 0;
            int sb = 0;
            Detect(parameters.CenterX, parameters.CenterY, parameters.Height, parameters.Radius, parameters.DeltB, parameters.DeltAx, parameters.DeltAy, parameters.Aradius, parameters.AMinSize, parameters.BMinSize, parameters.AMaxNum, parameters.BMaxNum, parameters.LorR, parameters.PixPerMm, ref q, ref a, ref b,ref sa,ref sb);
            quality = q;
            numa = a;
            numb = b;
            sizea = sa;
            sizeb = sb;


        }
    }

}
