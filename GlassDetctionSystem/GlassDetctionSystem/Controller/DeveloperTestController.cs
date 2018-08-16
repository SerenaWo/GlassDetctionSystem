using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassDetctionSystem.Model.FormModel;

namespace GlassDetctionSystem.Controller
{
    public class DeveloperTestController
    {
        public DeveloperTest view;
        public DeveloperTestModell model;
        public DeveloperTestController(DeveloperTest view)
        {
            model = new DeveloperTestModell() { ModelInput = "initialinput", ModelOutput = "initialoutput" };
            this.view = view;
            this.view.controller = this;
        }
        public void UpdateDevloperTestController()
        {
            UpdateToDataBase(model);
        }
        private void UpdateToDataBase(DeveloperTestModell m)
        {
            System.Windows.Forms.MessageBox.Show("modeloutput:" + m.ModelInput + "modeloutput:" + m.ModelOutput);
        }
    }
}
