using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassDetctionSystem.Model.FormModel;

namespace GlassDetctionSystem.Controller
{
    public class TempTestController
    {
        public TempTest view;
        public TempTestModel model;
        public TempTestController(TempTest view)
        {
            model = new TempTestModel();
            this.view = view;
            this.view.controller = this;
        }

    }
}
