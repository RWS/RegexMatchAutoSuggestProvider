using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexMASProviderLib.DataAccess
{
    public interface IVariableDataStore<T> : IDataStore<T, Variable>
        where T : IList<Variable>, new()
    {
    }
}
