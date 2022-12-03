using Livet;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;

public class NodeLinkViewModel : ViewModel
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Guid InputConnectorGuid { get; set; } = Guid.NewGuid();
    public Guid OutputConnectorGuid { get; set; } = Guid.NewGuid();
    public Guid InputConnectorNodeGuid { get; set; } = Guid.NewGuid();
    public Guid OutputConnectorNodeGuid { get; set; } = Guid.NewGuid();

    public bool IsLocked { get; set; } = false;
    public bool IsSelected { get; set; } = false;

}
