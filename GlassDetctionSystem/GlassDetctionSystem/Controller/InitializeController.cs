using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassDetctionSystem.Model.FormModel;

namespace GlassDetctionSystem.Controller
{
    public class InitializeController
    {
        public Initialize view;
        public InitializeModel model;
        public InitializeController(Initialize view)
        {
            model = new InitializeModel() ;
            this.view = view;
            this.view.controller = this;
        }
    }
}
