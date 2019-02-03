﻿using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Capybara.EditorPlugin.RegexMASProvider.Common;

namespace Capybara.EditorPlugin.RegexMASProvider.Models
{
    public class Variables : ModelBase
    {
        private static readonly string FileNameSuffix = ".variables.xml";

        public Variables()
        {
            Entries = new BindingList<Variable>();
            Load();
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

        public void Load()
        {
            var path = Utils.GetSettingsPath(FileNameSuffix);
            if (!File.Exists(path))
            {
                return;
            }
            var doc = XDocument.Load(path);
            foreach (var varElem in doc.Descendants("Variable"))
            {
                var variable = new Variable
                {
                    IsEnabled = (bool) varElem.Element("IsEnabled"),
                    Name = (string) varElem.Element("Name")
                };
                foreach (var pairElem in varElem.Descendants("TranslationPair"))
                {
                    variable.TranslationPairs.Add(new TranslationPair
                    {
                        Source = (string) pairElem.Element("Source"),
                        Target = (string) pairElem.Element("Target")
                    });
                }
                Entries.Add(variable);
            }
        }

        public void Save()
        {
            var path = Utils.GetSettingsPath(FileNameSuffix);
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "true"),
                new XElement("RegexMatchAutoSuggestProvider",
                    new XElement("Variables",
                        Entries.Select(e =>
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