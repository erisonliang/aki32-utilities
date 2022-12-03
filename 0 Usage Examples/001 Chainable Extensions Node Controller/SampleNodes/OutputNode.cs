using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Aki32Utilities.WPFAppUtilities.NodeController;

namespace Aki32Utilities.UsageExamples.ChainableExtensionNodeController
{

    public class ConsoleOutputNode : Node
    {

        public override void setConnections()
        {
            title = "Console";
            addInput(1);
        }

        public override object[] process(object[] ins, Dictionary<string, object> parameters)
        {
            //Console.WriteLine("Result : " + ins[0]);
            MessageBox.Show("Result : " + ins[0]);
            return null;
        }
    }
}
