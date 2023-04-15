using System.Windows;

namespace Aki32Utilities.WPFAppUtilities.NodeController.Controls;
public class CanvasMouseEventArgs : EventArgs
{
    // This position has taken scale and offset into account.
    public Point TransformedPosition { get; }

    public CanvasMouseEventArgs(Point transformedPosition)
    {
        TransformedPosition = transformedPosition;
    }
}
