using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassDetctionSystem.Model.FunctionClass
{
    public struct FlawPoint
    {
        /// <summary>
        /// 瑕疵的横坐标，单位是像素
        /// </summary>
        public double x;

        /// <summary>
        /// 瑕疵的纵坐标，单位是像素
        /// </summary>
        public double y;

        /// <summary>
        /// 瑕疵的直径，单位是毫米
        /// </summary>
        public double diameter;

        public FlawPoint(double x, double y, double diameter)
        {
            this.x = x;
            this.y = y;
            this.diameter = diameter;
        }
    }
}
