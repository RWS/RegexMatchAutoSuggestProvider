using Sdl.TellMe.ProviderApi;
using System.Diagnostics;
using System.Drawing;

namespace Capybara.EditorPlugin.RegexMASProvider.TellMeProvider
{
    internal class RegexMatchContactAction : AbstractTellMeAction
    {
        public RegexMatchContactAction()
        {
            Name = "Regex Match AutoSuggest Provider - Trados Documentation";
        }

        public override bool IsAvailable => true;

        public override string Category => $"{PluginResources.Plugin_Name} results";

        public override Icon Icon => PluginResources.Regex_Documentation;

        public override void Execute()
        {
            Process.Start("https://appstore.rws.com/Plugin/275?tab=documentation");
        }
    }
}
