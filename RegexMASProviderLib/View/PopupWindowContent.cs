using RegexMASProviderLib.DataAccess;
using System.Collections.Generic;

namespace RegexMASProviderLib.View
{
    public class PopupWindowContent
    {
        public PopupWindowContent()
        {
            AutoSuggestEntries = new List<AutoSuggestEntry>();
        }

        public PopupWindowContent(IEnumerable<AutoSuggestEntry> autoSuggestEntries)
        {
            AutoSuggestEntries = new List<AutoSuggestEntry>(autoSuggestEntries);
        }

        public List<AutoSuggestEntry> AutoSuggestEntries { get; set; }
    }
}
