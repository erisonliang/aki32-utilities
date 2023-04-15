using System.Windows;

namespace Aki32Utilities.WPFAppUtilities.NodeController.Controls;
public interface ICanvasObject : IDisposable
{
    Guid Guid { get; set; }
    void UpdateOffset(Point offset);
}
