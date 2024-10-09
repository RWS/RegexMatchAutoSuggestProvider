using RegexMASProviderLib.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace RegexMASProviderLib.DataAccess
{
    public class RegexPatternEntries : ModelBase
    {
        private readonly IRegexPatternDataStore<BindingList<RegexPatternEntry>> _regexPatternDataStore;
        private readonly string _regexFilePath;

        private BindingList<RegexPatternEntry> _entries;

        public BindingList<RegexPatternEntry> Entries
        {
            get { return _entries; }
            set
            {
                _entries = value;
                OnPropertyChanged("Entries");
            }
        }

        public RegexPatternEntries(string regexFilePath)
        {
            _regexPatternDataStore = new RegexPatternDataStore<BindingList<RegexPatternEntry>>();
            _regexFilePath = regexFilePath;
            Entries = _regexPatternDataStore.Load(regexFilePath); 
        }

        public void Save()
        {
            _regexPatternDataStore.Save(_regexFilePath, Entries);
        }

        public override string Error
        {
            get { return null; }
        }

        //[Obsolete("This method is obsolete. Use GetAutoSuggestEntries method, which handles multiple variables correctly.")]
        //public List<string> EvaluateMatches(string text, Variables variables)
        //{
        //    var results = new List<string>();
        //    if (!string.IsNullOrEmpty(text))
        //    {
        //        var validVariables = variables.ToDistinct();
        //        foreach (var pattern in Entries.Where(e => e.IsEnabled && !e.HasErrors))
        //        {
        //            var pairPatterns = GetTranslationPairPatterns(validVariables, pattern.RegexPattern);
        //            foreach (var pairPattern in pairPatterns)
        //            {
        //                var findWord = pairPattern.FindWord;
        //                var replaceWord = pairPattern.ReplaceWord;
        //                var newtext = Regex.Replace(text, pairPattern.OriginalPattern,
        //                    match => match.Value.Replace(findWord, replaceWord));
        //                results.AddRange(from Match match in Regex.Matches(newtext, pairPattern.ModifiedPattern)
        //                                 select Utils.WideToNarrow(match.Result(pattern.ReplacePattern)));
        //            }

        //            results.AddRange(from Match match in Regex.Matches(text, pattern.RegexPattern)
        //                             select Utils.WideToNarrow(match.Result(pattern.ReplacePattern)));
        //        }
        //    }
        //    return results.Select(e => e).Distinct().ToList();
        //}

        private List<TranslationPairPattern> GetTranslationPairPatterns(List<Variable> variables, string regexPattern)
        {
            var list = new List<TranslationPairPattern>();
            foreach (var variable in variables)
            {
                var varname = "#" + variable.Name + "#";
                if (!regexPattern.Contains(varname))
                {
                    continue;
                }
                foreach (var pair in variable.TranslationPairs.Where(p => !p.HasErrors))
                {
                    var findPattern = regexPattern.Replace(varname,
                        string.Format("(?:{0})", Regex.Escape(pair.Source)));
                    var replacePattern = regexPattern.Replace(varname,
                        string.Format("(?:{0})", Regex.Escape(pair.Target)));
                    list.Add(new TranslationPairPattern
                    {
                        OriginalPattern = findPattern,
                        FindWord = pair.Source,
                        ModifiedPattern = replacePattern,
                        ReplaceWord = pair.Target
                    });
                }
            }
            return list;
        }

        private class TranslationPairPattern
        {
            public string OriginalPattern { get; set; }
            public string FindWord { get; set; }
            public string ModifiedPattern { get; set; }
            public string ReplaceWord { get; set; }
        }
    }
}