using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexMASProviderLib.DataAccess
{
    public interface IDataStore<TList, TValue> 
        where TList : IList<TValue>, new()
        where TValue : class
    {
        TList Load(string path);

        void Save(string path, TList entries);
    }
}
