using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Aki32Utilities.WinFormAppUtilities.CheatSheet
{
    public partial class Form_Chart : Form
    {
        public Form_Chart()
        {
            InitializeComponent();
        }

        private async Task Main()
        {
            // WinForm用
            // .NET Core 非対応…

            // Clear
            Chart_Main.Series.Clear();
            Chart_Main.ChartAreas.Clear();
            Chart_Main.Titles.Clear();

            // Chart Area
            ChartArea area1 = new ChartArea("Area1");
            area1.AxisX.Title = "Title1-XAxis";
            area1.AxisY.Title = "Title1-YAxis";

            ChartArea area2 = new ChartArea("Area2");
            area2.AxisX.Title = "Title2-XAxis";
            area2.AxisY.Title = "Title2-YAxis";

            Title title = new Title("Title");
            Title title1 = new Title("Title1");
            Title title2 = new Title("Title2");
            title1.DockedToChartArea = "Area1"; // ChartAreaとの紐付
            title2.DockedToChartArea = "Area2"; // ChartAreaとの紐付

            // Series
            Random rdm = new Random();
            Series seriesLine = new Series();
            seriesLine.ChartType = SeriesChartType.Line;
            seriesLine.LegendText = "Legend:Line";
            seriesLine.BorderWidth = 2;
            seriesLine.MarkerStyle = MarkerStyle.Circle;
            seriesLine.MarkerSize = 12;
            for (int i = 0; i < 10; i++)
            {
                seriesLine.Points.Add(new DataPoint(i, rdm.Next(0, 210)));
            }
            seriesLine.ChartArea = "Area1"; // ChartAreaとの紐付

            Series seriesColumn = new Series();
            seriesColumn.LegendText = "Legend:Column";
            seriesColumn.ChartType = SeriesChartType.Column;
            for (int i = 0; i < 10; i++)
            {
                seriesColumn.Points.Add(new DataPoint(i, rdm.Next(0, 210)));
            }
            seriesColumn.ChartArea = "Area2"; // ChartAreaとの紐付

            Chart_Main.Titles.Add(title);
            Chart_Main.Titles.Add(title1);
            Chart_Main.Titles.Add(title2);
            Chart_Main.ChartAreas.Add(area1);
            Chart_Main.ChartAreas.Add(area2);
            Chart_Main.Series.Add(seriesColumn);
            Chart_Main.Series.Add(seriesLine);
        }
    }
}
