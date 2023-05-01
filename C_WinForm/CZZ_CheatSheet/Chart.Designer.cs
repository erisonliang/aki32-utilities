namespace Aki32Utilities.WinFormAppUtilities.CheatSheet
{
    partial class Form_Chart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Chart_Main = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.Chart_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // Chart_Main
            // 
            chartArea1.Name = "ChartArea1";
            this.Chart_Main.ChartAreas.Add(chartArea1);
            this.Chart_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.Chart_Main.Legends.Add(legend1);
            this.Chart_Main.Location = new System.Drawing.Point(0, 0);
            this.Chart_Main.Name = "Chart_Main";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.Chart_Main.Series.Add(series1);
            this.Chart_Main.Size = new System.Drawing.Size(1209, 698);
            this.Chart_Main.TabIndex = 0;
            this.Chart_Main.Text = "chart1";
            // 
            // Form_Chart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1209, 698);
            this.Controls.Add(this.Chart_Main);
            this.Name = "Form_Chart";
            this.Text = "Chart";
            ((System.ComponentModel.ISupportInitialize)(this.Chart_Main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart Chart_Main;
    }
}