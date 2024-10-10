using Sdl.TellMe.ProviderApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capybara.EditorPlugin.RegexMASProvider.TellMeProvider
{
    internal class RegexMatchCommunitySupportAction : AbstractTellMeAction
    {
        public RegexMatchCommunitySupportAction()
        {
            Name = "RWS Community AppStore Forum";
        }

        public override string Category => $"{PluginResources.Plugin_Name} results";

        public override Icon Icon => PluginResources.Regex_Question;

        public override bool IsAvailable => true;

        public override void Execute()
        {
            Process.Start("https://community.rws.com/product-groups/trados-portfolio/rws-appstore/f");
        }
    }
}
