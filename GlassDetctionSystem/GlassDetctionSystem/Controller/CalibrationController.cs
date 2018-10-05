using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassDetctionSystem.Model.FormModel;

namespace GlassDetctionSystem.Controller
{
    public class CalibrationController
    {
        public Calibration view;
        public CalibrationModel model;
        public CalibrationController(Calibration view)
        {
            model = new CalibrationModel();
            this.view = view;
            this.view.controller = this;
        }

    }
}
