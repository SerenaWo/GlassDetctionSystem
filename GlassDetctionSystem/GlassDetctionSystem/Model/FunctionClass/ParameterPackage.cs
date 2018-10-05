using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassDetctionSystem.Model.FunctionClass
{
    public struct ParameterPackage
    {

        public string NameId;
        public double Radius;
        public double Height;
        public int CenterX;
        public int CenterY;
        public double PixPerMm;
        public bool LorR;
        public double DeltAx;
        public double DeltAy;
        public double DeltB;
        public double AMinSize;
        public double AMaxNum;
        public double BMinSize;
        public double BMaxNum;
        public double Aradius;
        public ParameterPackage(string nameid, double radius, double height, int centerx, int centery, double pixpermm,
            bool lorr, double deltax, double deltay, double deltb, double aminsize, double amaxnum, double bminsize, double bmaxnum,double aradius)
        {
            this.NameId = nameid;
            this.Radius = radius;
            this.Height = height;
            this.CenterX = centerx;
            this.CenterY = centery;
            this.PixPerMm = pixpermm;
            this.LorR = lorr;
            this.DeltAx = deltax;
            this.DeltAy = deltay;
            this.DeltB = deltb;
            this.AMinSize = aminsize;
            this.AMaxNum = amaxnum;
            this.BMinSize = bminsize;
            this.BMaxNum = bmaxnum;
            this.Aradius = aradius;
        }
       
    }
}
