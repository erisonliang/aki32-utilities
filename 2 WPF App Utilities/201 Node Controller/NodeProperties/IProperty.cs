

namespace Aki32Utilities.WPFAppUtilities.NodeController
{
    public delegate void VoidHandler();

    public interface IProperty
    {
        string label { get; set; }
        object value { get; set; }
        event VoidHandler valueChanged;
    }
}
