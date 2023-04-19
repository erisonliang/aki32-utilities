using System.Windows;

namespace Aki32Utilities.WPFAppUtilities.NodeController.Controls;
internal interface ISelectableObject
{
    bool IsSelected { get; set; }
    object DataContext { get; set; }

    bool Contains(Rect rect);
    bool IntersectsWith(Rect rect);
}
