using Aki32Utilities.UsageExamples.SampleNodeController.ViewModels;

using Livet;

using NodeGraph.Utilities;

using System.Windows;
using System.Windows.Input;

namespace Aki32_Utilities.ViewModels.NodeViewModels;

public abstract class DefaultNodeViewModel : ViewModel, INodeViewModel
{
    public double Width { get; set; } = 0;
    public double Height { get; set; } = 0;
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Point Position { get; set; } = new Point(0, 0);
    public bool IsSelected { get; set; } = false;

    public ICommand SizeChangedCommand => _SizeChangedCommand.Get(SizeChanged);
    ViewModelCommandHandler<Size> _SizeChangedCommand = new();

    public abstract IEnumerable<NodeConnectorViewModel> Inputs { get; }
    public abstract IEnumerable<NodeConnectorViewModel> Outputs { get; }

    public abstract NodeConnectorViewModel FindConnector(Guid guid);

    void SizeChanged(Size newSize)
    {
        Width = newSize.Width;
        Height = newSize.Height;
    }
}
