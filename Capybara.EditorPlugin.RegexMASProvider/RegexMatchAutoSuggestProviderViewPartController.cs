﻿using RegexMASProviderLib.Common;
using RegexMASProviderLib.DataAccess;
using RegexMASProviderLib.View;
using Sdl.Desktop.IntegrationApi;
using Sdl.Desktop.IntegrationApi.Extensions;
using Sdl.Desktop.IntegrationApi.Interfaces;
using Sdl.TranslationStudioAutomation.IntegrationApi;
using System;
using System.Reflection;

namespace Capybara.EditorPlugin.RegexMASProvider
{
    [ViewPart(
        Id = "RegexMatchAutoSuggestProviderViewPart",
        Name = "Plugin_Name",
        Description = "Plugin_Description",
        Icon = "regex_Logo1"
    )]
    [ViewPartLayout(typeof(EditorController), Dock = DockType.Bottom)]
    class RegexMatchAutoSuggestProviderViewPartController : AbstractViewPartController
    {
        protected override IUIControl GetContentControl()
        {
            return Control.Value;
        }

        public void InitializeController()
        {
            Initialize();
        }

        protected override void Initialize()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();

            // Load regex entries
            var regexFileExtension = ".settings.xml";
            var regexFilePath = Utils.GetSettingsPath(regexFileExtension, executingAssembly);
            _regexPatternEntries = new RegexPatternEntries(regexFilePath);

            // Load variable entries
            var variableFileExtension = ".variables.xml";
            var variableFilePath = Utils.GetSettingsPath(variableFileExtension, executingAssembly);
            _variables = new Variables(variableFilePath);
            _listChangeNotifier = new ListChangeNotifier();
            Control.Value.Initialize(_regexPatternEntries, _variables, _listChangeNotifier);


            var editorController = SdlTradosStudio.Application.GetController<EditorController>();
            editorController.Closed += (sender, args) =>
            {
                _regexPatternEntries.Save();
                _variables.Save();
            };

        }


        private RegexPatternEntries _regexPatternEntries;

        public RegexPatternEntries RegexPatternEntries
        {
            get { return _regexPatternEntries; }
        }

        private Variables _variables;

        public Variables Variables
        {
            get { return _variables; }
        }

        private ListChangeNotifier _listChangeNotifier;

        public ListChangeNotifier ListChangeNotifier
        {
            get { return _listChangeNotifier; }
        }


        private static readonly Lazy<RegexDataGridView> Control =
            new Lazy<RegexDataGridView>(() =>
        {
            return new RegexDataGridView();
        });
    }
}
