using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Aki32Utilities.WFAAppUtilities.NodeController;

namespace Aki32Utilities.UsageExamples.ResearchArticlesNodeController
{
    [STNode("/SimpleNode/")]
    public class EmptyOptionTestNode : STNode
    {
        protected override void OnCreate() {
            base.OnCreate();
            this.Title = "EmptyTest";
            this.InputOptions.Add(STNodeOption.Empty);
            this.InputOptions.Add("string", typeof(string), false);
        }
    }
}
