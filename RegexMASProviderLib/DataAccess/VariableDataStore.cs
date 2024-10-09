using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace RegexMASProviderLib.DataAccess
{
    public class VariableDataStore<T> : IVariableDataStore<T>
        where T : IList<Variable>, new()
    {
        private readonly T _variables = new T();

        public T Load(string path) 
        {
            _variables.Clear();
            if (!File.Exists(path))
            {
                return _variables;
            }

            var doc = XDocument.Load(path);
            foreach (var varElem in doc.Descendants("Variable"))
            {
                var variable = new Variable
                {
                    IsEnabled = (bool)varElem.Element("IsEnabled"),
                    Name = (string)varElem.Element("Name")
                };
                foreach (var pairElem in varElem.Descendants("TranslationPair"))
                {
                    variable.TranslationPairs.Add(new TranslationPair
                    {
                        Source = (string)pairElem.Element("Source"),
                        Target = (string)pairElem.Element("Target")
                    });
                }

                _variables.Add(variable);
            }

            return _variables;
        }

        public void Save(string path, T variables)
        {
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "true"),
                new XElement("RegexMatchAutoSuggestProvider",
                    new XElement("Variables",
                        variables.Select(e =>
                            new XElement("Variable",
                                new XElement("IsEnabled", e.IsEnabled),
                                new XElement("Name", e.Name),
                                new XElement("TranslationPairs",
                                    e.TranslationPairs.Select(p =>
                                        new XElement("TranslationPair",
                                            new XElement("Source", p.Source),
                                            new XElement("Target", p.Target)))))))));
            doc.Save(path);
        }
    }
}
