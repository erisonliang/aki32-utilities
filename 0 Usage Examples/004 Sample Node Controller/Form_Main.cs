﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Aki32Utilities.WFAAppUtilities.NodeController;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController
{
    public partial class Form_Main : Form
    {
        public Form_Main()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            stNodePropertyGrid1.Text = "Node_Property";
            stNodeTreeView1.LoadAssembly(Application.ExecutablePath);
            stNodeEditor1.LoadAssembly(Application.ExecutablePath);

            stNodeEditor1.ActiveChanged += (s, ea) => stNodePropertyGrid1.SetNode(stNodeEditor1.ActiveNode);
            //stNodeEditor1.SelectedChanged += (s, ea) => stNodePropertyGrid1.SetSTNode(stNodeEditor1.ActiveNode);
            stNodeEditor1.OptionConnected += (s, ea) => stNodeEditor1.ShowAlert(ea.Status.ToString(), Color.White, ea.Status == ConnectionStatus.Connected ? Color.FromArgb(125, Color.Green) : Color.FromArgb(125, Color.Red));
            stNodeEditor1.CanvasScaled += (s, ea) => stNodeEditor1.ShowAlert(stNodeEditor1.CanvasScale.ToString("F2"), Color.White, Color.FromArgb(125, Color.Yellow));
            stNodeEditor1.NodeAdded += (s, ea) => ea.Node.ContextMenuStrip = contextMenuStrip1;

            stNodeEditor1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            contextMenuStrip1.ShowImageMargin = false;
            contextMenuStrip1.Renderer = new ToolStripRendererEx();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //int nLines = 0;
            //foreach (var v in Directory.GetFiles("../../../", "*.cs", SearchOption.AllDirectories)) {
            //    nLines += File.ReadAllLines(v).Length;
            //}
            //MessageBox.Show(nLines.ToString());
            //this.Resize += (s, ea) => this.Text = this.Size.ToString();
            //this.BeginInvoke(new MethodInvoker(() => {
            //    //this.Size = new Size(488, 306);
            //    this.Size = new Size(488, 246);
            //    stNodeTreeView1.Visible = false;
            //    stNodePropertyGrid1.Top = stNodeEditor1.Top;
            //    stNodePropertyGrid1.Height = stNodeEditor1.Height;
            //    stNodeTreeView1.Height = stNodeEditor1.Height;
            //}));
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.stn|*.stn";
            if (ofd.ShowDialog() != DialogResult.OK) return;
            stNodeEditor1.Nodes.Clear();
            stNodeEditor1.LoadCanvas(ofd.FileName);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog
            {
                Filter = "*.stn|*.stn"
            };
            if (sfd.ShowDialog() != DialogResult.OK) return;
            stNodeEditor1.SaveCanvas(sfd.FileName);
        }

        private void lockConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stNodeEditor1.ActiveNode.LockOption = !stNodeEditor1.ActiveNode.LockOption;
        }

        private void lockLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.ActiveNode == null) return;
            stNodeEditor1.ActiveNode.LockLocation = !stNodeEditor1.ActiveNode.LockLocation;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (stNodeEditor1.ActiveNode == null) return;
            stNodeEditor1.Nodes.Remove(stNodeEditor1.ActiveNode);
        }
    }
}
