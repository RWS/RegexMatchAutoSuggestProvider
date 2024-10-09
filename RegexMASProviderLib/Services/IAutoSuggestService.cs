using RegexMASProviderLib.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexMASProviderLib.Services
{
    public interface IAutoSuggestService
    {
        List<AutoSuggestEntry> GetAutoSuggestEntries(
            string text, IList<RegexPatternEntry> regexEntries, IList<Variable> variablesEntries);
    }
}
