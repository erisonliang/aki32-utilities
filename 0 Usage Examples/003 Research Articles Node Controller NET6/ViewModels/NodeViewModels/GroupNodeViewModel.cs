using Livet;

using NodeGraph.Utilities;

using System.Windows;
using System.Windows.Input;

namespace Aki32_Utilities.ViewModels.NodeViewModels
{
    public class GroupNodeViewModel : ViewModel, INodeViewModel
    {
        public Guid Guid
        {
            get => _Guid;
            set => RaisePropertyChangedIfSet(ref _Guid, value);
        }
        Guid _Guid = Guid.NewGuid();

        public string Name
        {
            get => _Name;
            set => RaisePropertyChangedIfSet(ref _Name, value);
        }
        string _Name = string.Empty;

        public Point Position
        {
            get => _Position;
            set => RaisePropertyChangedIfSet(ref _Position, value, nameof(Comment));
        }
        Point _Position = new Point(0, 0);

        public Point InterlockPosition
        {
            get => _InterlockPosition;
            set => RaisePropertyChangedIfSet(ref _InterlockPosition, value);
        }
        Point _InterlockPosition = new Point(0, 0);

        public Point InnerPosition
        {
            get => _InnerPosition;
            set => RaisePropertyChangedIfSet(ref _InnerPosition, value, nameof(Comment));
        }
        Point _InnerPosition = new Point(0, 0);

        public double InnerWidth
        {
            get => _InnerWidth;
            set => RaisePropertyChangedIfSet(ref _InnerWidth, value, nameof(Comment));
        }
        double _InnerWidth = 100;

        public double InnerHeight
        {
            get => _InnerHeight;
            set => RaisePropertyChangedIfSet(ref _InnerHeight, value, nameof(Comment));
        }
        double _InnerHeight = 100;

        public string Comment
        {
            get => $"InnerWidth = {InnerWidth:F2}, InnerHeight = {InnerHeight:F2},\n Position = {Position:F2}, InnerPosition = {InnerPosition:F2}";
        }

        public bool IsSelected
        {
            get => _IsSelected;
            set => RaisePropertyChangedIfSet(ref _IsSelected, value);
        }
        bool _IsSelected = false;

        public ICommand SizeChangedCommand => _SizeChangedCommand.Get(SizeChanged);
        ViewModelCommandHandler<Size> _SizeChangedCommand = new ViewModelCommandHandler<Size>();

        void SizeChanged(Size newSize)
        {

        }
    }
}
