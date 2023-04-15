using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Aki32Utilities.WPFAppUtilities.General;
public class MultiTextWriter : TextWriter
{

    // ★★★★★★★★★★★★★★★ props

    private IEnumerable<TextWriter> writers;


    // ★★★★★★★★★★★★★★★ inits

    public MultiTextWriter(IEnumerable<TextWriter> writers)
    {
        this.writers = writers.ToList();
    }
    public MultiTextWriter(params TextWriter[] writers)
    {
        this.writers = writers;
    }


    // ★★★★★★★★★★★★★★★ methods

    public override void Write(char value)
    {
        foreach (var writer in writers)
            writer.Write(value);
    }

    public override void Write(string value)
    {
        foreach (var writer in writers)
            writer.Write(value);
    }

    public override void Flush()
    {
        foreach (var writer in writers)
            writer.Flush();
    }

    public override void Close()
    {
        foreach (var writer in writers)
            writer.Close();
    }

    public override Encoding Encoding
    {
        get { return Encoding.ASCII; }
    }


    // ★★★★★★★★★★★★★★★

}
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
