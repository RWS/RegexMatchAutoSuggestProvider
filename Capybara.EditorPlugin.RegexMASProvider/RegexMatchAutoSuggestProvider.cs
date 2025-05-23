﻿using RegexMASProviderLib.DataAccess;
using RegexMASProviderLib.Services;
using Sdl.FileTypeSupport.Framework.BilingualApi;
using Sdl.FileTypeSupport.Framework.IntegrationApi;
using Sdl.TranslationStudioAutomation.IntegrationApi;
using Sdl.TranslationStudioAutomation.IntegrationApi.AutoSuggest;
using Sdl.TranslationStudioAutomation.IntegrationApi.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Capybara.EditorPlugin.RegexMASProvider
{
    [AutoSuggestProvider(Id = "RegexMatchAutoSuggestProvider", Name = "RegexMatchAutoSuggestProvider",
        Description = "AutoSuggest provider for copying the source words that match the specified regular expressions",
        Icon = "regex_Logo1")]
    public class RegexMatchAutoSuggestProvider : AbstractAutoSuggestProvider
    {
        private List<string> _candidates;
        private RegexPatternEntries _regexPatternEntries;
        private IAutoSuggestService _autoSuggestService = new AutoSuggestService();
        private Variables _variables;
        private ListChangeNotifier _listChangeNotifier;
        private RegexMatchAutoSuggestProviderViewPartController _viewPartController;

        public RegexMatchAutoSuggestProvider()
        {
            _viewPartController =
                SdlTradosStudio.Application.GetController<RegexMatchAutoSuggestProviderViewPartController>();

            _viewPartController.InitializeController();

            if (_viewPartController != null)
            {
                _listChangeNotifier = _viewPartController.ListChangeNotifier;
                _listChangeNotifier.Changed += _listChangeNotifier_Changed;
            }

            Icon = PluginResources.regex_Logo1;
        }

        private void _listChangeNotifier_Changed(object sender, EventArgs e)
        {
            Debug.WriteLine("List Changed");
            _viewPartController.Variables.Save();
            _viewPartController.RegexPatternEntries.Save();
            InitializeCandidates();
        }

        protected override void OnActiveDocumentChanged()
        {
            if (_viewPartController == null)
            {
                _viewPartController =
                    SdlTradosStudio.Application.GetController<RegexMatchAutoSuggestProviderViewPartController>();
            }
            if (_viewPartController != null)
            {
                InitializeCandidates();
            }
        }

        private void InitializeCandidates()
        {
            if (_candidates == null)
            {
                _candidates = new List<string>();
            }
            else
            {
                _candidates.Clear();
            }
            var segmentPair = ActiveSegmentPair;
            if (segmentPair != null)
            {
                var text = string.Join("",
                    segmentPair.Source.AllSubItems.OfType<IText>().Select(txt => txt.Properties.Text));
                var autoSuggestEntries = _autoSuggestService.GetAutoSuggestEntries(
                    text, _viewPartController.RegexPatternEntries.Entries, 
                    _viewPartController.Variables.Entries);
                _candidates.AddRange(autoSuggestEntries.Select(e => e.AutoSuggestString).Distinct());
            }
        }


        protected override void OnActiveSegmentChanged()
        {
            if (Settings.Enabled && _viewPartController != null)
            {
                InitializeCandidates();
            }
            else
            {
                _candidates = null;
            }
        }

        protected override void OnSettingsChanged()
        {
            if (Settings.Enabled && _viewPartController != null)
            {
                InitializeCandidates();
            }
            else
            {
                _candidates = null;
            }
        }

        protected override IEnumerable<AbstractAutoSuggestResult> GetSuggestions(AbstractEditingContext context)
        {
            if (Settings.Enabled)
            {
                string prefix = context.GetAllPrefixes().FirstOrDefault();
                if (!string.IsNullOrEmpty(prefix) && _candidates != null && _candidates.Count > 0)
                {
                    return _candidates.Where(
                        item =>
                            item.StartsWith(prefix,
                                Settings.CaseSensitive
                                    ? StringComparison.InvariantCulture
                                    : StringComparison.InvariantCultureIgnoreCase)).Select(
                                        item =>
                                        {
                                            var result = new AutoSuggestTextResult(context, item)
                                            {
                                                Priority = Settings.Priority,
                                                Icon = Icon
                                            };
                                            return result;
                                        });
                }
            }
            return null;
        }
    }
}