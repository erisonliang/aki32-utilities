using System;
using System.IO;
using System.Windows.Forms;

namespace Aki32Utilities.UsageExamples.WinFormControlExample
{
    public partial class MainForm : Form
    {

        // ★★★★★★★★★★★★★★★ init

        public MainForm()
        {
            InitializeComponent();
        }


        // ★★★★★★★★★★★★★★★ ZoomableImage

        private void ZoomableImage_Main_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
                ZoomableImage_Main.ImageLocation = new FileInfo(files[0]);
            }
            catch (Exception ex)
            {
                ShowError("画像ファイルのみドロップ可能です。", ex);
            }
        }

        private void ZoomableImage_Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.All;
            else
                e.Effect = DragDropEffects.None;
        }

        // ★★★★★★★★★★★★★★★ methods

        private void ShowError(string manualMessage, Exception ex)
            => MessageBox.Show($"{manualMessage}\r\n\r\nｴﾗｰｺｰﾄﾞ:{ex.Message}", "失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // ★★★★★★★★★★★★★★★ 

    }

}
