using Sdl.TellMe.ProviderApi;

namespace Capybara.EditorPlugin.RegexMASProvider.TellMeProvider
{
    [TellMeProvider]
    public class RegexMatchTellMeProvider : ITellMeProvider
    {
        public string Name => "Regex Match AutoSuggest";

        public AbstractTellMeAction[] ProviderActions => new AbstractTellMeAction[]
        {
            new RegexMatchContactAction()
            {
                Keywords = new[]{
                    "regex", "regex match", "regex autosuggest",
                    "regex provider", "regex trados", "regex documentation"
                }
            },

            new RegexMatchCommunitySupportAction()
            {
                Keywords = new[]
                {
                    "regex", "regex match", "regex autosuggest",
                    "regex provider", "regex trados", "regex support",
                    "regex community", "regex forum"
                }
            },

            new RegexMatchSourceCodeAction()
            {
                Keywords = new[]
                {
                    "regex", "regex match", "regex autosuggest",
                    "regex provider", "regex trados", "regex code",
                    "regex source", "regex trados"
                }
            },

            new RegexMatchEditorViewAction()
            {
                Keywords = new[]
                {
                    "regex", "regex match", "regex autosuggest",
                    "regex provider", "regex trados", "regex settings"
                }
            }
        };
    }
}
