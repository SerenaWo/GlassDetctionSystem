using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GlassDetctionSystem.Model.FormModel;



namespace GlassDetctionSystem.Controller
{
    public class StandardEntryController
    {
        public StandardEntry view;
        public StandardEntryModel model;
        public StandardEntryController(StandardEntry view)
        {
            model = new StandardEntryModel();
            this.view = view;
            this.view.controller = this;
        }
    }
}
