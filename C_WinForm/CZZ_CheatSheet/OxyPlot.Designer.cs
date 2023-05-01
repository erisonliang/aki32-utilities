namespace Aki32Utilities.WinFormAppUtilities.CheatSheet
{
    partial class Form_OxyPlot
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
            this.PlotView_Main = new OxyPlot.WindowsForms.PlotView();
            this.SuspendLayout();
            // 
            // PlotView_Main
            // 
            this.PlotView_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlotView_Main.Location = new System.Drawing.Point(0, 0);
            this.PlotView_Main.Name = "PlotView_Main";
            this.PlotView_Main.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.PlotView_Main.Size = new System.Drawing.Size(850, 674);
            this.PlotView_Main.TabIndex = 0;
            this.PlotView_Main.Text = "PlotView_Main";
            this.PlotView_Main.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.PlotView_Main.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.PlotView_Main.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // Form_OxyPlot
            // 
            this.ClientSize = new System.Drawing.Size(850, 674);
            this.Controls.Add(this.PlotView_Main);
            this.Name = "Form_OxyPlot";
            this.Text = "Form_OxyPlot";
            this.ResumeLayout(false);

        }

        #endregion

        private OxyPlot.WindowsForms.PlotView PlotView_Main;
    }
}