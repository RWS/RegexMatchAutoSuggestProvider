using RegexMASProviderLib.DataAccess;
using RegexMASProviderLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capybara.EditorPlugin.RegexMASProvider.Tests
{
    public class AutoSuggestServiceTests
    {
        private readonly RegexPatternDataStore<List<RegexPatternEntry>> _reggexDataStore = new();
        private readonly VariableDataStore<List<Variable>> _variableDataStore = new ();
        private readonly IAutoSuggestService _autoSuggestService = new AutoSuggestService ();
        private readonly string _testingFilesPath = Path.Combine($"{Directory.GetCurrentDirectory()}", "TestFiles");

        private List<RegexPatternEntry> RegexEntries => _reggexDataStore.Load($"{_testingFilesPath}/regex.xml");

        private List<Variable> VariableEntries => _variableDataStore.Load($"{_testingFilesPath}/variables.xml");

        [Theory]
        [InlineData("January 1, 2023 (Monday)", "Montag-2023-1-Januar")]
        [InlineData("March 24, 2024 (Tuesday)", "Dienstag-2024-24-März")]
        public void GetAutosuggesEntries_TextValid_ReturnAutoSuggestEntries(string text, string expectedText)
        {
            // Arrange
            var regexText = text;
            var regexEntries = RegexEntries;
            var variableEntries = VariableEntries;

            // Act
            var autoSuggestEntries = _autoSuggestService.GetAutoSuggestEntries(regexText, regexEntries, variableEntries);
            var autoSuggestEntry = autoSuggestEntries[0];

            // Assert
            Assert.NotNull(autoSuggestEntries);
            Assert.Equal(expectedText, autoSuggestEntry.ToString());
        }

        [Theory]
        [InlineData("foo")]
        [InlineData("sampleText")]
        public void GetAutosuggesEntries_TextInvalid_ReturnEmptyList(string text) 
        {
            // Arrange
            var regexText = text;
            var regexEntries = RegexEntries;
            var variableEntries = VariableEntries;

            // Act
            var autoSuggestEntries = _autoSuggestService.GetAutoSuggestEntries(regexText, regexEntries, variableEntries);

            // Assert
            Assert.Empty(autoSuggestEntries);
        }
    }
}
