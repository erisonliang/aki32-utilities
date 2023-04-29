using System.IO;
using System.Text;

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

