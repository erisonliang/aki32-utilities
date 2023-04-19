using System.Windows;
using Aki32Utilities.Apps.ResearchArticlesNodeController.ViewModels;
using Aki32Utilities.ConsoleAppUtilities.General;
using Aki32Utilities.WPFAppUtilities.General;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace Aki32Utilities.Apps.ResearchArticlesNodeController.Views;
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
        ((INotifyCollectionChanged)ListView_Console.Items).CollectionChanged += (_, _) =>
        {
            ListView_Console.SelectedIndex = ListView_Console.Items.Count - 1;
            ListView_Console.ScrollIntoView(ListView_Console.SelectedItem);
        };

        // ensure node links are drawn correctly
        Loaded += async (_, _) =>
        {
            try
            {
                childViewModel.InitResearchArticlesManager();
                await Task.Delay(10);
                NodeGraph.Offset = new Point(1, 1);
            }
            catch (Exception)
            {
            }
        };

        // click to copy Text prop
        TextBlock_ReferenceString.MouseDown += TextBlock_MouseDown_ToCopyText;
        TextBlock_CrossRef_UnstructuredRefString.MouseDown += TextBlock_MouseDown_ToCopyText;
        TextBlock_JStage_UnstructuredRefString.MouseDown += TextBlock_MouseDown_ToCopyText;
        StackPanel_AOI.MouseDown += StackPanel_MouseDown_ToCopyTagText;

        //

    }

    private void TextBlock_MouseDown_ToCopyText(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        try
        {
            var senderTextBlock = (TextBlock)sender;
            var text = senderTextBlock.Tag is string tagText ? tagText : senderTextBlock.Text;
            Clipboard.SetText(text);
            Console.WriteLine("✓ コピーしました");
        }
        catch (Exception)
        {
        }
    }

    private void StackPanel_MouseDown_ToCopyTagText(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        try
        {
            Clipboard.SetText(((StackPanel)sender).Tag.ToString());
            Console.WriteLine("✓ コピーしました");
        }
        catch (Exception)
        {
        }
    }
}
