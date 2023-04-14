using Livet;
using Livet.Commands;
using Aki32Utilities.WPFAppUtilities.NodeController.Operation;
using NodeGraph.Utilities;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Aki32Utilities.ViewModels.NodeViewModels;
using Aki32Utilities.ConsoleAppUtilities.Research;
using System.IO;
using Aki32Utilities.UsageExamples.ResearchArticlesNodeController.Views;
using PropertyChanged;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;
[AddINotifyPropertyChangedInterface]
public partial class MainWindowViewModel : ViewModel
{

    // ★★★★★★★★★★★★★★★ felds

    const int NODE_MARGIN_LEFT = 0;
    const int NODE_MARGIN_TOP = 0;
    const int NODE_VERTICAL_SPAN = 200; //250;
    const int NODE_HORIZONTAL_SPAN = 555; //500

    public static DirectoryInfo databaseDir = new($@"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}\ResearchArticleDB");

    internal MainWindow ParentView;


    // ★★★★★★★★★★★★★★★ props

    public double Scale { get; set; } = 1d;
    public Point CanvasOffset { get; set; } = new Point(0, 0);

    public static ResearchArticlesManager ResearchArticlesManager { get; set; }


    // ★★★★★★★★★★★★★★★ ヘッダー内

    public bool IsSaveBusy { get; set; } = false;
    public bool IsSaveDone { get; set; } = false;
    public ViewModelCommand SaveCommand => _SaveCommand.Get(async () => await Save());
    ViewModelCommandHandler _SaveCommand = new();

    public ViewModelCommand TestCommand => _TestCommand.Get(async () => await Test());
    ViewModelCommandHandler _TestCommand = new();

    public RangeSelectionMode[] RangeSelectionModes { get; } = Enum.GetValues(typeof(RangeSelectionMode)).OfType<RangeSelectionMode>().ToArray();
    public RangeSelectionMode SelectedRangeSelectionMode { get; set; } = RangeSelectionMode.包含;

    public bool ExecuteDynamicNodeRepulsions { get; set; } = false;
    public bool ExecuteDynamicLinkAttractions { get; set; } = false;
    public bool ExecuteDynamicOriginalPointAttractions { get; set; } = false;

    public bool IsLockedAllNodeLinks
    {
        get => _IsLockedAllNodeLinks;
        set
        {
            _IsLockedAllNodeLinks = value;

            foreach (var nodeLink in _NodeLinkViewModels)
                nodeLink.IsLocked = _IsLockedAllNodeLinks;

            RaisePropertyChanged(nameof(IsLockedAllNodeLinks));
        }
    }
    bool _IsLockedAllNodeLinks = false;

    public bool IsEnableAllNodeConnectors
    {
        get => _IsEnableAllNodeConnectors;
        set
        {
            _IsEnableAllNodeConnectors = value;

            foreach (var node in _NodeViewModels)
            {
                foreach (var input in node.Inputs)
                    input.IsEnable = _IsEnableAllNodeConnectors;
                foreach (var output in node.Outputs)
                    output.IsEnable = _IsEnableAllNodeConnectors;
            }

            RaisePropertyChanged(nameof(IsEnableAllNodeConnectors));
        }
    }
    bool _IsEnableAllNodeConnectors = false;


    // ★★★★★★★★★★★★★★★ 右クリック

    public ViewModelCommand AddNodeCommand => _AddNodeCommand.Get(AddNewArticleNode);
    ViewModelCommandHandler _AddNodeCommand = new();

    public ViewModelCommand RemoveSelectingArticleNodesCommand => _RemoveSelectingArticleNodesCommand.Get(RemoveSelectingArticleNodes);
    ViewModelCommandHandler _RemoveSelectingArticleNodesCommand = new();

    public ViewModelCommand RemoveTempArticleNodesCommand => _RemoveTempArticleNodesCommand.Get(RemoveTempArticleNodes);
    ViewModelCommandHandler _RemoveTempArticleNodesCommand = new();

    public ViewModelCommand MergeIfMergeableForAllCommand => _MergeIfMergeableForAllCommand.Get(MergeIfMergeableForAll);
    ViewModelCommandHandler _MergeIfMergeableForAllCommand = new();

    public ViewModelCommand AcceptSelectingArticleNodesCommand => _AcceptSelectingArticleNodesCommand.Get(AcceptSelectingArticleNodes);
    ViewModelCommandHandler _AcceptSelectingArticleNodesCommand = new();

    public ViewModelCommand MergeSelectingTwoArticleNodesCommand => _MergeSelectingTwoArticleNodesCommand.Get(MergeSelectingTwoArticleNodes);
    ViewModelCommandHandler _MergeSelectingTwoArticleNodesCommand = new();

    public ViewModelCommand DuplicateSelectingArticleNodeCommand => _DuplicateSelectingArticleNodeCommand.Get(DuplicateSelectingArticleNode);
    ViewModelCommandHandler _DuplicateSelectingArticleNodeCommand = new();


    public ViewModelCommand RearrangeNodesAlignLeftCommand => _RearrangeNodesAlignLeftCommand.Get(RearrangeNodesAlignLeft);
    ViewModelCommandHandler _RearrangeNodesAlignLeftCommand = new();

    public ViewModelCommand RearrangeNodesAlignRightCommand => _RearrangeNodesAlignRightCommand.Get(RearrangeNodesAlignRight);
    ViewModelCommandHandler _RearrangeNodesAlignRightCommand = new();

    public ViewModelCommand RearrangeNodesChronologicallyAlignLeftCommand => _RearrangeNodesChronologicallyAlignLeftCommand.Get(RearrangeNodesChronologicallyAlignLeft);
    ViewModelCommandHandler _RearrangeNodesChronologicallyAlignLeftCommand = new();


    // ★★★★★★★★★★★★★★★ Node Controller 内

    public ResearchArticleNodeViewModel? SelectedNodeViewModel { get; set; } = null;

    public ListenerCommand<PreviewConnectLinkOperationEventArgs> PreviewConnectLinkCommand => _PreviewConnectLinkCommand.Get(PreviewConnect);
    ViewModelCommandHandler<PreviewConnectLinkOperationEventArgs> _PreviewConnectLinkCommand = new();

    public ListenerCommand<ConnectedLinkOperationEventArgs> ConnectedLinkCommand => _ConnectedLinkCommand.Get(NodeConnected);
    ViewModelCommandHandler<ConnectedLinkOperationEventArgs> _ConnectedLinkCommand = new();

    public ListenerCommand<DisconnectedLinkOperationEventArgs> DisconnectedLinkCommand => _DisconnectedLinkCommand.Get(NodeDisconnected);
    ViewModelCommandHandler<DisconnectedLinkOperationEventArgs> _DisconnectedLinkCommand = new();

    public ListenerCommand<EndMoveNodesOperationEventArgs> EndMoveNodesCommand => _EndMoveNodesCommand.Get(NodesMoved);
    ViewModelCommandHandler<EndMoveNodesOperationEventArgs> _EndMoveNodesCommand = new();

    public ListenerCommand<IList> SelectionChangedCommand => _SelectionChangedCommand.Get(SelectionChanged);
    ViewModelCommandHandler<IList> _SelectionChangedCommand = new();


    public IEnumerable<DefaultNodeViewModel> NodeViewModels => _NodeViewModels;
    ObservableCollection<DefaultNodeViewModel> _NodeViewModels = new();

    public IEnumerable<NodeLinkViewModel> NodeLinkViewModels => _NodeLinkViewModels;
    ObservableCollection<NodeLinkViewModel> _NodeLinkViewModels = new();

    public IEnumerable<GroupNodeViewModel> GroupNodeViewModels => _GroupNodeViewModels;
    ObservableCollection<GroupNodeViewModel> _GroupNodeViewModels = new();

    public EmphasizePropertyItems[] EmphasizePropertyItems { get; } = Enum.GetValues(typeof(EmphasizePropertyItems)).OfType<EmphasizePropertyItems>().ToArray();
    public static EmphasizePropertyItems _SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.デフォルト;
    public EmphasizePropertyItems SelectedEmphasizePropertyItem
    {
        get
        {
            return _SelectedEmphasizePropertyItem;
        }
        set
        {
            if (RaisePropertyChangedIfSet(ref _SelectedEmphasizePropertyItem, value))
            {
                NotifyResearchArticlesPropertiesChanged();
                _SelectedEmphasizePropertyItem = value;
            }
        }
    }


    // ★★★★★★★★★★★★★★★ 右パネル内 → 選択中の文献

    public bool IsPullReferenceInfoBusy { get; set; } = false;
    public bool IsPullReferenceInfoDone { get; set; } = false;
    public ViewModelCommand PullReferenceInfoCommand => _PullReferenceInfoCommand.Get(async () => await PullReferenceInfo());
    ViewModelCommandHandler _PullReferenceInfoCommand = new();

    public bool IsPullNormalMetaInfoBusy { get; set; } = false;
    public bool IsPullNormalMetaInfoDone { get; set; } = false;
    public ViewModelCommand PullNormalMetaInfoCommand => _PullNormalMetaInfoCommand.Get(async () => await PullNormalMetaInfo());
    ViewModelCommandHandler _PullNormalMetaInfoCommand = new();

    public bool IsPullAIMetaInfoBusy { get; set; } = false;
    public bool IsPullAIMetaInfoDone { get; set; } = false;
    public ViewModelCommand PullAIMetaInfoCommand => _PullAIMetaInfoCommand.Get(async () => await PullAIMetaInfo());
    ViewModelCommandHandler _PullAIMetaInfoCommand = new();

    public bool IsOpenPDFBusy { get; set; } = false;
    public bool IsOpenPDFDone { get; set; } = false;
    public ViewModelCommand OpenPDFCommand => _OpenPDFCommand.Get(async () => await OpenPDF());
    ViewModelCommandHandler _OpenPDFCommand = new();

    public bool IsOpenWebSiteBusy { get; set; } = false;
    public bool IsOpenWebSiteDone { get; set; } = false;
    public ViewModelCommand OpenWebSiteCommand => _OpenWebSiteCommand.Get(OpenWebSite);
    ViewModelCommandHandler _OpenWebSiteCommand = new();

    public bool IsExecuteAllBusy { get; set; } = false;
    public bool IsExecuteAllDone { get; set; } = false;
    public ViewModelCommand ExecuteAllCommand => _ExecuteAllCommand.Get(ExecuteAll);
    ViewModelCommandHandler _ExecuteAllCommand = new();

    public bool IsAISummaryBusy { get; set; } = false;
    public bool IsAISummaryDone { get; set; } = false;
    public ViewModelCommand AISummaryCommand => _AISummaryCommand.Get(AISummary);
    ViewModelCommandHandler _AISummaryCommand = new();

    public bool IsManuallyAddPDFBusy { get; set; } = false;
    public bool IsManuallyAddPDFDone { get; set; } = false;
    public ViewModelCommand ManuallyAddPDFCommand => _ManuallyAddPDFCommand.Get(ManuallyAddPDF);
    ViewModelCommandHandler _ManuallyAddPDFCommand = new();

    public bool IsUndefinedButton1Busy { get; set; } = false;
    public bool IsUndefinedButton1Done { get; set; } = false;
    public ViewModelCommand UndefinedButton1Command => _UndefinedButton1Command.Get(async () => await UndefinedButton1());
    ViewModelCommandHandler _UndefinedButton1Command = new();

    public ViewModelCommand RefreshSelectedArticleInfoCommand => _RefreshSelectedArticleInfoCommand.Get(RefreshSelectedArticleInfo);
    ViewModelCommandHandler _RefreshSelectedArticleInfoCommand = new();

    public string MemoTemplateString => @"★★★★★ 読んだメモ


【貢献を一行で】
・■■は，■■という課題解決のために■■を行い，■■がわかった。


【問題提起】
・解消したい既知の問題点，既往研究で不足しているところ。


【手法】
・どんな仮定・設備・アルゴリズムで，実験・解析を行ったか。なぜ？


【結果・知見】
・結果。上手くいくときと行かないときの条件。


【要注意事項】
・こういう仮定のもとのみで成り立つことに注意，など。


【今後】
・どんなことがまだ知られていないか，あるいは解決していないか？


【考察】
・あれば。自分の研究に生かせそう？


";

    public ViewModelCommand AddMemoTemplateStringCommand => _AddMemoTemplateStringCommand.Get(AddMemoTemplateString);
    ViewModelCommandHandler _AddMemoTemplateStringCommand = new();


    // ★★★★★★★★★★★★★★★ 右パネル内 → 検索

    public ViewModelCommand OpenSearchTabCommand => _OpenSearchTabCommand.Get(async () => await OpenLocalSearch());
    ViewModelCommandHandler _OpenSearchTabCommand = new();

    public static string _LocalSearchString;
    public string LocalSearchString
    {
        get => _LocalSearchString;
        set
        {
            if (RaisePropertyChangedIfSet(ref _LocalSearchString, value))
            {
                NotifyResearchArticlesPropertiesChanged();
                SelectedEmphasizePropertyItem = ViewModels.EmphasizePropertyItems.検索結果;
                _LocalSearchString = value;
            }
        }
    }

    public ViewModelCommand ResetLocalSearchStringCommand => _ResetLocalSearchStringCommand.Get(() => LocalSearchString = "");
    ViewModelCommandHandler _ResetLocalSearchStringCommand = new();

    public string InternetSearch_FreeWord { get; set; }
    public string InternetSearch_PublishedFrom { get; set; }
    public string InternetSearch_PublishedUntil { get; set; }
    public string InternetSearch_DataMaxCount { get; set; } = "20";

    public string InternetSearch_Authors { get; set; }
    public string InternetSearch_ArticleTitle { get; set; }
    public string InternetSearch_MaterialTitle { get; set; }
    public string InternetSearch_Keywords { get; set; }
    public string InternetSearch_MaterialVolume { get; set; }

    public string InternetSearch_DOI { get; set; }

    public bool IsInternetSearch_CiNii_A_Busy { get; set; } = false;
    public bool IsInternetSearch_CiNii_A_Done { get; set; } = false;
    public ViewModelCommand InternetSearch_CiNii_A_Command => _InternetSearch_CiNii_A_Command.Get(async () => await InternetSearch_CiNii_A_());
    ViewModelCommandHandler _InternetSearch_CiNii_A_Command = new();

    public bool IsInternetSearch_NDLSearch_A_Busy { get; set; } = false;
    public bool IsInternetSearch_NDLSearch_A_Done { get; set; } = false;
    public ViewModelCommand InternetSearch_NDLSearch_A_Command => _InternetSearch_NDLSearch_A_Command.Get(async () => await InternetSearch_NDLSearch_A_());
    ViewModelCommandHandler _InternetSearch_NDLSearch_A_Command = new();

    public bool IsInternetSearch_JStage_B_Busy { get; set; } = false;
    public bool IsInternetSearch_JStage_B_Done { get; set; } = false;
    public ViewModelCommand InternetSearch_JStage_B_Command => _InternetSearch_JStage_B_Command.Get(async () => await InternetSearch_JStage_B_());
    ViewModelCommandHandler _InternetSearch_JStage_B_Command = new();

    public bool IsInternetSearch_CiNii_B_Busy { get; set; } = false;
    public bool IsInternetSearch_CiNii_B_Done { get; set; } = false;
    public ViewModelCommand InternetSearch_CiNii_B_Command => _InternetSearch_CiNii_B_Command.Get(async () => await InternetSearch_CiNii_B_());
    ViewModelCommandHandler _InternetSearch_CiNii_B_Command = new();

    public bool IsInternetSearch_NDLSearch_B_Busy { get; set; } = false;
    public bool IsInternetSearch_NDLSearch_B_Done { get; set; } = false;
    public ViewModelCommand InternetSearch_NDLSearch_B_Command => _InternetSearch_NDLSearch_B_Command.Get(async () => await InternetSearch_NDLSearch_B_());
    ViewModelCommandHandler _InternetSearch_NDLSearch_B_Command = new();

    public bool IsInternetSearch_JStage_C_Busy { get; set; } = false;
    public bool IsInternetSearch_JStage_C_Done { get; set; } = false;
    public ViewModelCommand InternetSearch_JStage_C_Command => _InternetSearch_JStage_C_Command.Get(async () => await InternetSearch_JStage_C_());
    ViewModelCommandHandler _InternetSearch_JStage_C_Command = new();

    public bool IsInternetSearch_CiNii_C_Busy { get; set; } = false;
    public bool IsInternetSearch_CiNii_C_Done { get; set; } = false;
    public ViewModelCommand InternetSearch_CiNii_C_Command => _InternetSearch_CiNii_C_Command.Get(async () => await InternetSearch_CiNii_C_());
    ViewModelCommandHandler _InternetSearch_CiNii_C_Command = new();

    public bool IsInternetSearch_CrossRef_C_Busy { get; set; } = false;
    public bool IsInternetSearch_CrossRef_C_Done { get; set; } = false;
    public ViewModelCommand InternetSearch_CrossRef_C_Command => _InternetSearch_CrossRef_C_Command.Get(async () => await InternetSearch_CrossRef_C_());
    ViewModelCommandHandler _InternetSearch_CrossRef_C_Command = new();


    // ★★★★★★★★★★★★★★★ 右パネル内 → 設定InsertToFormat

    public ListenerCommand<object> InsertToFormatCommand => _InsertToFormatCommand.Get(InsertToFormat);
    ViewModelCommandHandler<object> _InsertToFormatCommand = new();

    private string _SidePanelFontSize = "13";
    public string SidePanelFontSize
    {
        get
        {
            return _SidePanelFontSize;
        }
        set
        {
            _SidePanelFontSize = value.ToString();
            if (double.TryParse(value, out double valueDouble))
                ParentView.TabControl_RightPanel.FontSize = valueDouble;
        }
    }


    // ★★★★★★★★★★★★★★★ 

}
