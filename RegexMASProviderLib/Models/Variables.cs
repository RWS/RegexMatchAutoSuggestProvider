using RegexMASProviderLib.Common;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace RegexMASProviderLib.DataAccess
{
    public class Variables : ModelBase
    {
        private readonly IVariableDataStore<BindingList<Variable>> _variableDataStore;
        private readonly string _variablesFilePath;

        public Variables(string variablesFilePath)
        {
            _variablesFilePath = variablesFilePath;
            _variableDataStore = new VariableDataStore<BindingList<Variable>>();
            Entries = _variableDataStore.Load(variablesFilePath);
        }

        private BindingList<Variable> _entries;

        public BindingList<Variable> Entries
        {
            get { return _entries; }
            set
            {
                _entries = value;
                OnPropertyChanged("Entries");
            }
        }

        public override string Error
        {
            get
            {
                var total = Entries.Count;
                if (total != Entries.Select(e => e.Name).Distinct().Count())
                {
                    return "Duplicate names.";
                }
                return null;
            }
        }

        public void Save()
        {
            _variableDataStore.Save(_variablesFilePath, Entries);
        }
    }
}