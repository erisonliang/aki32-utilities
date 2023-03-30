using System.Windows;

using Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.Views;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => ((MainWindowViewModel)DataContext).ParentView = this;
    }
}
