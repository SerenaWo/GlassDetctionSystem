using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlassDetctionSystem.Controller;
namespace GlassDetctionSystem
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //DeveloperTestController controller = new DeveloperTestController(new DeveloperTest());
            //Application.Run(controller.view);
            //Application.Run(new Initialize());

            //TempTestController controller = new TempTestController(new TempTest());
            //Application.Run(controller.view);
            //Application.Run(new Initialize());
            // Set the unhandled exception mode to force all Windows Forms errors to go through
            // our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            InitializeController initializecontroller = new InitializeController(new Initialize());
            Application.Run(initializecontroller.view);
        }

    }
}
