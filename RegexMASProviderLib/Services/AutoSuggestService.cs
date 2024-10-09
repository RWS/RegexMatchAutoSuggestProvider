using RegexMASProviderLib.Common;
using RegexMASProviderLib.DataAccess;
using RegexMASProviderLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexMASProviderLib.Services
{
    public class AutoSuggestService : IAutoSuggestService
    {
        public List<AutoSuggestEntry> GetAutoSuggestEntries(string text, IList<RegexPatternEntry> regexEntries, IList<Variable> variablesEntries)
        {
            var results = new List<string>();
            var autoSuggestEntries = new List<AutoSuggestEntry>();
            if (!string.IsNullOrEmpty(text))
            {
                var validVariables = variablesEntries.ToDistinct();
                foreach (var pattern in regexEntries.Where(e => e.IsEnabled && !e.HasErrors))
                {
                    var intermediateRegex = ConstructIntermediateRegex(validVariables, pattern.RegexPattern);
                    var r = new Regex(intermediateRegex.ConcatenatedFindPattern);
                   
                    //foreach (Match initialMatch in Regex.Matches(text, intermediateRegex.ConcatenatedFindPattern, RegexOptions.IgnoreCase))
                    foreach (Match initialMatch in r.Matches(text))
                    {
                        var finalRegex = ConstructFinalRegex(r, initialMatch, intermediateRegex);
                        foreach (Match finalMatch in Regex.Matches(finalRegex.NewSourceText, finalRegex.EvaluatedFindPattern))
                        {
                            var autoSuggestString = finalMatch.Result(pattern.ReplacePattern).WideToNarrow();
                            results.Add(autoSuggestString);
                            var autoSuggestEntry = new AutoSuggestEntry
                            {
                                OriginalSourceText = text,
                                OriginalFindPattern = pattern.RegexPattern,
                                OriginalReplacePattern = pattern.ReplacePattern,
                                NumberedFindPattern = intermediateRegex.NumberedFindPattern,
                                ConcatenatedFindPattern = intermediateRegex.ConcatenatedFindPattern,
                                ConcatenatedFindPatternMatch = initialMatch,
                                EvaluatedFindPattern = finalRegex.EvaluatedFindPattern,
                                EvaluatedFindPatternMatch = finalMatch,
                                NewSourceText = finalRegex.NewSourceText,
                                AutoSuggestString = autoSuggestString,
                            };
                            autoSuggestEntries.Add(autoSuggestEntry);
                        }
                    }

                    //results.AddRange(from Match match in Regex.Matches(text, pattern.RegexPattern)
                    //                     select match.Result(pattern.ReplacePattern).WideToNarrow());

                }
            }
            //return results.Select(e => e).Distinct().ToList();
            return autoSuggestEntries;
        }

        private class FinalRegex
        {
            public string EvaluatedFindPattern { get; set; }
            public string NewSourceText { get; set; }
        }

        private FinalRegex ConstructFinalRegex(Regex sourceRegex, Match match, IntermediateRegex intermediateRegex)
        {
            var entireValue = match.Value;
            var finalMatchPattern = intermediateRegex.NumberedFindPattern;
            var groupNames = sourceRegex.GetGroupNames();
            var groups = match.Groups;
            var diff = 0;
            for (int i = 1; i < groupNames.Length; i++)
            {
                var groupName = groupNames[i];
                var group = groups[groupName];
                if (groupName.IsNumber())
                {
                    continue;
                }
                if (!group.Success || !intermediateRegex.VariableMap.ContainsKey(groupName))
                {
                    continue;
                }

                var variable = intermediateRegex.VariableMap[groupName];
                var pair = variable.TranslationPairs.FirstOrDefault(x =>
                    !x.HasErrors && x.Source == group.Value);
                if (pair == null)
                {
                    continue;
                }
                finalMatchPattern = finalMatchPattern.ReplaceFirst($"#{groupName}#", Regex.Escape(pair.Target));

                var beforeLen = entireValue.Length;
                entireValue = entireValue.Remove(group.Index - match.Index - diff, group.Length);
                entireValue = entireValue.Insert(group.Index - match.Index - diff, $"{pair.Target}");
                var afterLen = entireValue.Length;
                diff += beforeLen - afterLen;
            }


            return new FinalRegex
            {
                EvaluatedFindPattern = finalMatchPattern,
                NewSourceText = entireValue
            };

        }

        private class IntermediateRegex
        {
            public string ConcatenatedFindPattern { get; set; }

            /// <summary>
            /// Used to construct a final regex pattern.
            /// </summary>
            public string NumberedFindPattern { get; set; }

            /// <summary>
            /// Name to Variable Mapping. Name is suffixed with a unique index.
            /// </summary>
            public Dictionary<string, Variable> VariableMap { get; set; }
        }


        private IntermediateRegex ConstructIntermediateRegex(IEnumerable<Variable> variables, string basedPattern)
        {
            var intermediateRegex = new IntermediateRegex
            {
                ConcatenatedFindPattern = basedPattern,
                NumberedFindPattern = basedPattern,
                VariableMap = new Dictionary<string, Variable>()
            };

            foreach (var variable in variables.Where(p => !p.HasErrors && p.IsEnabled))
            {

                var idx = 0;
                while (intermediateRegex.ConcatenatedFindPattern.Contains($"#{variable.Name}#"))
                {
                    var groupName = $"{variable.Name}{idx}";
                    var concatenatedSources = string.Join("|", variable.TranslationPairs.Where(p => !p.HasErrors).Select(p => Regex.Escape(p.Source)));
                    intermediateRegex.ConcatenatedFindPattern = intermediateRegex.ConcatenatedFindPattern.ReplaceFirst($"#{variable.Name}#", $"(?<{groupName}>{concatenatedSources})");
                    intermediateRegex.NumberedFindPattern =
                        intermediateRegex.NumberedFindPattern.ReplaceFirst($"#{variable.Name}#", $"#{groupName}#");
                    intermediateRegex.VariableMap.Add(groupName, variable);
                    idx++;
                }
            }

            return intermediateRegex;
        }
    }
}
