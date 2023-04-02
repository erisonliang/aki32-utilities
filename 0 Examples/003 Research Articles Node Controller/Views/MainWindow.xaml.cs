using System.Windows;

using Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.Views;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var childViewModel = (MainWindowViewModel)DataContext;
        Loaded += (_, _) => childViewModel.ParentView = this;
        Button_ManuallyAddPDF.Drop += childViewModel.Button_ManuallyAddPDF_Drop;
    }
}
