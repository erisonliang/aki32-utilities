using Livet;

using NodeGraph.Utilities;

using PropertyChanged;

using System.Windows;
using System.Windows.Input;

namespace Aki32_Utilities.ViewModels.NodeViewModels
{
    public class GroupNodeViewModel : ViewModel, INodeViewModel
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";

        [AlsoNotifyFor(nameof(Comment))]
        public Point Position { get; set; } = new Point(0, 0);

        public Point InterlockPosition { get; set; } = new Point(0, 0);

        [AlsoNotifyFor(nameof(Comment))]
        public Point InnerPosition { get; set; } = new Point(0, 0);

        [AlsoNotifyFor(nameof(Comment))]
        public double InnerWidth { get; set; } = 100;

        [AlsoNotifyFor(nameof(Comment))]
        public double InnerHeight { get; set; } = 100;

        public string Comment => $"InnerWidth = {InnerWidth:F2}, InnerHeight = {InnerHeight:F2},\n Position = {Position:F2}, InnerPosition = {InnerPosition:F2}";

        public bool IsSelected { get; set; }

        public ICommand SizeChangedCommand => _SizeChangedCommand.Get(SizeChanged);
        ViewModelCommandHandler<Size> _SizeChangedCommand = new();

        void SizeChanged(Size newSize)
        {

        }
    }
}
