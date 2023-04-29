using System.IO;
using System.Text;
using System.Windows.Markup;

namespace Aki32Utilities.WPFAppUtilities.General;
public class ControlTextWriter : TextWriter
{

    // ★★★★★★★★★★★★★★★ props

    private IAddChild control;


    // ★★★★★★★★★★★★★★★ inits

    public ControlTextWriter(IAddChild control)
    {
        this.control = control;
    }


    // ★★★★★★★★★★★★★★★ methods

    public override void Write(char value)
    {
        control.AddText(value.ToString());
    }

    public override void Write(string value)
    {
        control.AddText(value);
    }

    public override Encoding Encoding
    {
        get { return Encoding.ASCII; }
    }


    // ★★★★★★★★★★★★★★★

}
