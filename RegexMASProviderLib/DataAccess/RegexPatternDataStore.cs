using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RegexMASProviderLib.DataAccess
{
    public class RegexPatternDataStore<T> : IRegexPatternDataStore<T> 
        where T : IList<RegexPatternEntry>, new()
    {
        private readonly T _value = new T();

        public T Load(string path)
        {
            _value.Clear();
            if (!File.Exists(path))
            {
                return _value;
            }

            var doc = XDocument.Load(path);
            foreach (var entry in doc.Descendants("Entry"))
            {
                var isEnabledElem = entry.Element("IsEnabled");
                var descElem = entry.Element("Description");
                var regexPatternElem = entry.Element("RegexPattern");
                var replacePatternElem = entry.Element("ReplacePattern");

                _value.Add(new RegexPatternEntry()
                {
                    IsEnabled = isEnabledElem == null || bool.Parse(isEnabledElem.Value),
                    Description = descElem != null ? descElem.Value : string.Empty,
                    RegexPattern = regexPatternElem != null ? regexPatternElem.Value : string.Empty,
                    ReplacePattern = replacePatternElem != null ? replacePatternElem.Value : string.Empty
                });
            }

            return _value;
        }

        public void Save(string path, T entries)
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "true"),
                new XElement("RegexMatchAutoSuggestProvider",
                    new XElement("Settings",
                        new XElement("RegexPatternEntries",
                            entries.Select(e =>
                                new XElement("Entry",
                                    new XElement("IsEnabled", e.IsEnabled),
                                    new XElement("Description", e.Description),
                                    new XElement("RegexPattern", e.RegexPattern),
                                    new XElement("ReplacePattern", e.ReplacePattern)))
                            ))));
            doc.Save(path);
        }
    }
}
