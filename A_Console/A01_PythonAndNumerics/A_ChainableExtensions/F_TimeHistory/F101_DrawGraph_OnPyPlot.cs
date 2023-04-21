using Aki32Utilities.ConsoleAppUtilities.General;

using static Aki32Utilities.ConsoleAppUtilities.General.TimeHistory;

namespace Aki32Utilities.ConsoleAppUtilities.PythonAndNumerics;
public static partial class ChainableExtensions
{

    /// <summary>
    /// Draw Line Graph on Python and return Image File
    /// </summary>
    /// <param name="yName">Vertical Axis</param>
    /// <param name="xName">Horizontal Axis</param>
    public static FileInfo DrawGraph_OnPyPlot(this TimeHistory th, FileInfo outputFile, string yName,
        ChartType type = ChartType.Line,
        string chartTitle = "",
        string? xLabel = null,
        string? yLabel = null,
        bool preview = false
        )
    {
        return th.DrawGraph_OnPyPlot(outputFile, "", yName: yName,
            type: type,
            chartTitle: chartTitle,
            xLabel: xLabel,
            yLabel: yLabel,
            preview: preview);
    }

    /// <summary>
    /// Draw Line Graph on Python and return Image File
    /// </summary>
    /// <param name="yName">Vertical Axis</param>
    /// <param name="xName">Horizontal Axis</param>
    public static FileInfo DrawGraph_OnPyPlot(this TimeHistory th, FileInfo outputFile, string xName, string yName,
        ChartType type = ChartType.Line,
        string chartTitle = "",
        string? xLabel = null,
        string? yLabel = null,
        bool preview = false
        )
    {
        return th.DrawGraph_OnPyPlot(outputFile, xName, new string[] { yName },
            type: type,
            chartTitle: chartTitle,
            xLabel: xLabel,
            yLabel: yLabel,
            preview: preview);
    }

    /// <summary>
    /// Draw Line Graph on Python and return Image File
    /// </summary>
    /// <param name="yName">Vertical Axis</param>
    /// <param name="xName">Horizontal Axis</param>
    public static FileInfo DrawGraph_OnPyPlot(this TimeHistory th, FileInfo outputFile, string xName, string[] yName,
        ChartType type = ChartType.Line,
        string chartTitle = "",
        string? xLabel = null,
        string? yLabel = null,
        bool preview = false
        )
    {
        try
        {
            var x = string.IsNullOrEmpty(xName) ? null : th[xName];
            var plots = new List<PyPlotWrapper.IPlot>();
            xLabel ??= xName;
            yLabel ??= yName.Length == 1 ? yName[0] : "";

            switch (type)
            {
                case ChartType.Scatter:
                    {
                        if (yName.Length == 1)
                        {
                            plots.Add(new PyPlotWrapper.ScatterPlot(x, th[yName[0]])
                            {
                                MarkerColor = "blue",
                            });
                        }
                        else
                        {
                            foreach (var name in yName)
                                plots.Add(new PyPlotWrapper.ScatterPlot(x, th[name])
                                {
                                    LegendLabel = name,
                                });
                        }

                        new PyPlotWrapper.Figure
                        {
                            IsTightLayout = true,
                            SubPlots = new List<PyPlotWrapper.SubPlot>()
                            {
                                new PyPlotWrapper.SubPlot()
                                {
                                    XLabel = xLabel,
                                    YLabel = yLabel,
                                    Title = chartTitle,
                                    Plots = plots,
                                }
                            }

                        }.Run(outputFile, preview);
                    }
                    break;

                case ChartType.Line:
                    {
                        if (yName.Length == 1)
                        {
                            plots.Add(new PyPlotWrapper.LinePlot(x, th[yName[0]])
                            {
                                LineColor = "blue",
                            });
                        }
                        else
                        {
                            foreach (var name in yName)
                                plots.Add(new PyPlotWrapper.LinePlot(x, th[name])
                                {
                                    LegendLabel = name,
                                    LineColor = null
                                });
                        }

                        new PyPlotWrapper.Figure
                        {
                            IsTightLayout = true,
                            SubPlots = new List<PyPlotWrapper.SubPlot>()
                            {
                                new PyPlotWrapper.SubPlot()
                                {
                                    XLabel = xLabel,
                                    YLabel = yLabel,
                                    Title = chartTitle,
                                    Plots = plots,
                                }
                            }

                        }.Run(outputFile, preview);
                    }
                    break;

                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            ConsoleExtension.WriteLineWithColor($"Failed to draw graph: {ex.Message}", ConsoleColor.Red);
        }

        return outputFile;
    }


}
