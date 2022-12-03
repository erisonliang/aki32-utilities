using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using Aki32Utilities.WFAAppUtilities.NodeController;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.Blender
{
    /// <summary>
    /// 此类仅演示 作为MixRGB节点的颜色选择按钮
    /// </summary>
    public class STNodeColorButton : STNodeControl
    {
        public event EventHandler ValueChanged;
        protected virtual void OnValueChanged(EventArgs e)
        {
            if (this.ValueChanged != null) this.ValueChanged(this, e);
        }

        protected override void OnMouseClick(System.Windows.Forms.MouseEventArgs e)
        {
            base.OnMouseClick(e);
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() != DialogResult.OK) return;
            //this._Color = cd.Color;
            this.BackColor = cd.Color;
            this.OnValueChanged(new EventArgs());
        }
    }
}
