using Sdl.Desktop.IntegrationApi;
using Sdl.TellMe.ProviderApi;
using Sdl.TranslationStudioAutomation.IntegrationApi;
using Sdl.TranslationStudioAutomation.IntegrationApi.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capybara.EditorPlugin.RegexMASProvider.TellMeProvider
{
    internal class RegexMatchEditorViewAction : AbstractTellMeAction
    {
        public RegexMatchEditorViewAction()
        {
            Name = "Regex Match AutoSuggest Provider - Trados Settings";
        }

        public override bool IsAvailable => SdlTradosStudio.Application.GetController<EditorController>().ActiveDocument != null;

        public override string Category => $"{PluginResources.Plugin_Name} results";

        public override Icon Icon => PluginResources.Regex_Settings;

        public override void Execute()
        {
            SdlTradosStudio.Application.GetController<EditorController>().Activate();
            SdlTradosStudio.Application.GetController<RegexMatchAutoSuggestProviderViewPartController>().Activate();
        }
    }
}
