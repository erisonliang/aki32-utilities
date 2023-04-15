using System.Windows;

namespace Aki32Utilities.ViewModels.NodeViewModels;

public interface INodeViewModel
{
    Guid Guid { get; set; }
    Point Position { get; set; }
    bool IsSelected { get; set; }
}
