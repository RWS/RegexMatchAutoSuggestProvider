using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexMASProviderLib.DataAccess
{
    public interface IRegexPatternDataStore<T> : IDataStore<T, RegexPatternEntry>
        where T : IList<RegexPatternEntry>, new()
    {
    }
}
