using System.IO;
using System.Text;
using System.Windows.Controls;

namespace Aki32Utilities.WPFAppUtilities.General;
public class ListBoxTextWriter : TextWriter
{

    // ★★★★★★★★★★★★★★★ props

    private ListBox listBox;


    // ★★★★★★★★★★★★★★★ inits

    public ListBoxTextWriter(ListBox listBox)
    {
        this.listBox = listBox;
    }


    // ★★★★★★★★★★★★★★★ methods

    public override void Write(char value)
    {
        Write(value.ToString());
    }

    public override void Write(string value)
    {
        try
        {
            var addingValue = value.Trim(new char[] { '\r', '\n' });
            if (string.IsNullOrEmpty(addingValue))
                return;
            listBox.Items.Add($"{listBox.Items.Count}\t{addingValue}");
        }
        catch (Exception)
        {
        }
    }

    public override Encoding Encoding
    {
        get { return Encoding.ASCII; }
    }


    // ★★★★★★★★★★★★★★★

}
