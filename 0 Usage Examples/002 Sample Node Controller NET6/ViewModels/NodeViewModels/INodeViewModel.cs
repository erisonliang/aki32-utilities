using System.Windows;

namespace Aki32_Utilities.ViewModels.NodeViewModels;

public interface INodeViewModel
{
    Guid Guid { get; set; }
    Point Position { get; set; }
    bool IsSelected { get; set; }
}
