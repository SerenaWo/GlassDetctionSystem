using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace GlassDetctionSystem.Model.FormModel
{
    public class DeveloperTestModell:INotifyPropertyChanged
    {
        private string _ModelInput;
        public string ModelInput
        {
            get { return _ModelInput; }
            set { _ModelInput = value;OnPropertyChanged("ModelInput"); }
        }

        private string _ModelOutput;
        public string ModelOutput
        {
            get { return _ModelOutput; }
            set { _ModelOutput = value; OnPropertyChanged("ModelOutput"); }
        }

        public void plus()
        {
            this.ModelOutput += "plus";
        }
        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        #endregion

    }

}
