using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassDetctionSystem.Model.FormModel;

namespace GlassDetctionSystem.Controller
{
    public  class MainDetectController
    {
        public MainDetect view;
        public MainDetectModel model;
        public MainDetectController(MainDetect view)
        {
            model = new MainDetectModel();
            this.view = view;
            this.view.controller = this;
        }
    }
}
