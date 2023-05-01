using System.Windows.Forms;

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Aki32Utilities.WinFormAppUtilities.CheatSheet
{
    public partial class Form_OxyPlot : Form
    {
        public Form_OxyPlot()
        {
            InitializeComponent();
        }

        private void Main(double[] result)
        {
            // ※ OxyPlot.WindowsFormsをNuGetで入れる。
            // ※ デザイナーでPlotViewを追加

            //OxyPlotの追加
            var model = new PlotModel
            {
                Background = OxyColors.White,
            };
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom
            });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -1, Maximum = 1 });
            PlotView_Main.Model = model;

            //Plotの更新
            var series = new LineSeries { };
            for (int i = 0; i < result.Length; i++)
                series.Points.Add(new DataPoint(i, result[i]));
            PlotView_Main.Model.Series.Clear();
            PlotView_Main.Model.Series.Add(series);
            PlotView_Main.Model.InvalidatePlot(true);
        }
    }
}
