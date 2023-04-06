using System.Windows;
using Aki32Utilities.UsageExamples.ResearchArticlesNodeController.ViewModels;
using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.WPFAppUtilities.General;
using System.Collections.Specialized;
using XPlot.Plotly;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.Views;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        //
        var childViewModel = (MainWindowViewModel)DataContext;
        Loaded += (_, _) => childViewModel.ParentView = this;
        Button_ManuallyAddPDF.Drop += childViewModel.Button_ManuallyAddPDF_Drop;

        // message
        Console.SetOut(new ListBoxTextWriter(ListView_Console));
        UtilConfig.ConsoleOutput_Preprocess = false;
        ((INotifyCollectionChanged)ListView_Console.Items).CollectionChanged += async (_, _) =>
        {
            ListView_Console.SelectedIndex = ListView_Console.Items.Count - 1;
            ListView_Console.ScrollIntoView(ListView_Console.SelectedItem);
        };

        // ensure node links are drawn correctly
        Loaded += async (_, _) =>
        {
            try
            {
                await Task.Delay(10);
                NodeGraph.Offset = new Point(1, 1);
            }
            catch (Exception)
            {
            }
        };


        //


        //

    }


}
