using Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;

using System.Collections.ObjectModel;

namespace Aki32_Utilities.ViewModels.NodeViewModels
{
    public class Test3DefaultNodeViewModel : DefaultNodeViewModel
    {
        public string Name
        {
            get => _Name;
            set => RaisePropertyChangedIfSet(ref _Name, value);
        }
        string _Name = string.Empty;

        public string Memo
        {
            get => _Memo;
            set => RaisePropertyChangedIfSet(ref _Memo, value);
        }
        string _Memo = string.Empty;

        public override IEnumerable<NodeConnectorViewModel> Inputs => _Inputs;
        readonly ObservableCollection<NodeInputViewModel> _Inputs = new ObservableCollection<NodeInputViewModel>();

        public override IEnumerable<NodeConnectorViewModel> Outputs => _Outputs;
        readonly ObservableCollection<NodeOutputViewModel> _Outputs = new ObservableCollection<NodeOutputViewModel>();

        public Test3DefaultNodeViewModel()
        {
            for (int i = 0; i < 2; ++i)
            {
                _Outputs.Add(new NodeOutputViewModel($"Output{i}"));
            }
        }

        public override NodeConnectorViewModel FindConnector(Guid guid)
        {
            return Outputs.FirstOrDefault(arg => arg.Guid == guid);
        }
    }
}
