namespace Aki32Utilities.UsageExamples.WinFormControlExample
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.TabControl_Main = new System.Windows.Forms.TabControl();
            this.TabPage_Main = new System.Windows.Forms.TabPage();
            this.TabPage_ZoomableImage = new System.Windows.Forms.TabPage();
            this.ZoomableImage_Main = new Aki32Utilities.WinFormAppUtilities.Control.ZoomableImage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItem_Instruction1 = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.TabControl_Main.SuspendLayout();
            this.TabPage_Main.SuspendLayout();
            this.TabPage_ZoomableImage.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabControl_Main
            // 
            this.TabControl_Main.Controls.Add(this.TabPage_Main);
            this.TabControl_Main.Controls.Add(this.TabPage_ZoomableImage);
            this.TabControl_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl_Main.Location = new System.Drawing.Point(0, 0);
            this.TabControl_Main.Name = "TabControl_Main";
            this.TabControl_Main.SelectedIndex = 0;
            this.TabControl_Main.Size = new System.Drawing.Size(818, 563);
            this.TabControl_Main.TabIndex = 0;
            // 
            // TabPage_Main
            // 
            this.TabPage_Main.Controls.Add(this.button1);
            this.TabPage_Main.Location = new System.Drawing.Point(8, 39);
            this.TabPage_Main.Name = "TabPage_Main";
            this.TabPage_Main.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_Main.Size = new System.Drawing.Size(802, 516);
            this.TabPage_Main.TabIndex = 1;
            this.TabPage_Main.Text = "Main";
            this.TabPage_Main.UseVisualStyleBackColor = true;
            // 
            // TabPage_ZoomableImage
            // 
            this.TabPage_ZoomableImage.Controls.Add(this.ZoomableImage_Main);
            this.TabPage_ZoomableImage.Controls.Add(this.menuStrip1);
            this.TabPage_ZoomableImage.Location = new System.Drawing.Point(8, 39);
            this.TabPage_ZoomableImage.Name = "TabPage_ZoomableImage";
            this.TabPage_ZoomableImage.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage_ZoomableImage.Size = new System.Drawing.Size(817, 569);
            this.TabPage_ZoomableImage.TabIndex = 0;
            this.TabPage_ZoomableImage.Text = "ZoomableImage";
            this.TabPage_ZoomableImage.UseVisualStyleBackColor = true;
            // 
            // ZoomableImage_Main
            // 
            this.ZoomableImage_Main.AllowDrop = true;
            this.ZoomableImage_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ZoomableImage_Main.Image = null;
            this.ZoomableImage_Main.Location = new System.Drawing.Point(3, 43);
            this.ZoomableImage_Main.Name = "ZoomableImage_Main";
            this.ZoomableImage_Main.Size = new System.Drawing.Size(811, 523);
            this.ZoomableImage_Main.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItem_Instruction1});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(811, 40);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItem_Instruction1
            // 
            this.ToolStripMenuItem_Instruction1.Name = "ToolStripMenuItem_Instruction1";
            this.ToolStripMenuItem_Instruction1.Size = new System.Drawing.Size(275, 36);
            this.ToolStripMenuItem_Instruction1.Text = "↓画像をドロップして追加";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(796, 510);
            this.button1.TabIndex = 1;
            this.button1.Text = "Hello WinFormControlExample!";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 563);
            this.Controls.Add(this.TabControl_Main);
            this.Name = "MainForm";
            this.Text = "WinFormControlExample";
            this.TabControl_Main.ResumeLayout(false);
            this.TabPage_Main.ResumeLayout(false);
            this.TabPage_ZoomableImage.ResumeLayout(false);
            this.TabPage_ZoomableImage.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabControl_Main;
        private System.Windows.Forms.TabPage TabPage_Main;
        private System.Windows.Forms.TabPage TabPage_ZoomableImage;
        private WinFormAppUtilities.Control.ZoomableImage ZoomableImage_Main;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem_Instruction1;
        private System.Windows.Forms.Button button1;
    }
}

