using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aki32Utilities.WFAAppUtilities.NodeController;
using System.Drawing;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController.NumberNode
{
    /// <summary>
    /// Number节点基类 用于确定节点风格 标题颜色 以及 数据类型颜色
    /// </summary>
    public abstract class NumberNodeBase : STNode
    {
        protected override void OnCreate()
        {
            base.OnCreate();
            this.TitleColor = Color.FromArgb(200, Color.CornflowerBlue);
        }
        protected override void OnOwnerChanged()
        {
            base.OnOwnerChanged();
            if (this.Owner != null) this.Owner.SetTypeColor(typeof(int), Color.CornflowerBlue);
        }
    }
}
