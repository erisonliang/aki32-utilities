using Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;

using System.Collections.ObjectModel;

namespace Aki32_Utilities.ViewModels.NodeViewModels
{
    public class Test1DefaultNodeViewModel : DefaultNodeViewModel
    {
        public string Name { get; set; }
        public string Memo { get; set; }

        public override IEnumerable<NodeConnectorViewModel> Inputs => _Inputs;
        readonly ObservableCollection<NodeInputViewModel> _Inputs = new();

        public override IEnumerable<NodeConnectorViewModel> Outputs => _Outputs;
        readonly ObservableCollection<NodeOutputViewModel> _Outputs = new();

        public Test1DefaultNodeViewModel()
        {
            for (int i = 0; i < 4; ++i)
            {
                if (i % 2 == 0)
                {
                    var label = $"Input{i}";
                    if (i > 1)
                    {
                        label += " Allow to connect multiple";
                    }
                    _Inputs.Add(new NodeInputViewModel(label, i > 1));
                }
                else
                {
                    _Inputs.Add(new NodeInputViewModel($"Limited Input", false));
                }
            }

            for (int i = 0; i < 5; ++i)
            {
                _Outputs.Add(new NodeOutputViewModel($"Output{i}"));
            }
        }

        public override NodeConnectorViewModel FindConnector(Guid guid)
        {
            var input = Inputs.FirstOrDefault(arg => arg.Guid == guid);
            if (input != null)
                return input;

            var output = Outputs.FirstOrDefault(arg => arg.Guid == guid);
            return output;
        }
    }
}
