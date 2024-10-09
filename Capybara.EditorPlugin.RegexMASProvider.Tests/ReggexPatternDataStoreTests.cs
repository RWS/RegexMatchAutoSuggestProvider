using RegexMASProviderLib.DataAccess;

namespace Capybara.EditorPlugin.RegexMASProvider.Tests
{
    public class ReggexPatternDataStoreTests
    {
        private readonly RegexPatternDataStore<List<RegexPatternEntry>> _reggexPatternDataStore;
        private readonly string _testingFilesPath = Path.Combine($"{Directory.GetCurrentDirectory()}", "TestFiles");

        public ReggexPatternDataStoreTests()
        {
            _reggexPatternDataStore = new RegexPatternDataStore<List<RegexPatternEntry>>();
        }

        [Theory]
        [InlineData("dummy")]
        [InlineData("inexistentFile")]
        public void LoadPatterns_FilesDoesNotExist_ReturnsEmptyRegexPatternEntriesList(string filePath)
        {
            // Arrange
            var patternPath = filePath;

            // Act
            var regexPatternEntries = _reggexPatternDataStore.Load(patternPath);

            // Assert
            Assert.Empty(regexPatternEntries);
        }

        [Fact]
        public void LoadPatterns_FileExist_RetrunsReggexPatternEntries()
        {
            // Arrange
            var filePath = $"{_testingFilesPath}/regex.xml";
            var expectedRegexPattern = @"(#month#) (\d{1,2}), (\d{4}) \((#dow#)\)";

            // Act
            var regexPatternEntries = _reggexPatternDataStore.Load(filePath);
            var firtReggexEntry = regexPatternEntries.First();

            // Assert
            Assert.NotEmpty(regexPatternEntries);
            Assert.Equal(expectedRegexPattern, firtReggexEntry.RegexPattern);
        }

        [Fact]
        public void SavePatterns_AnyList_CreatesNewXMLFile()
        {
            // Arrange
            var regexEntries = new List<RegexPatternEntry>();
            var filePath = $"{_testingFilesPath}/output.xml";
            var fileExist = false;

            // Act
            _reggexPatternDataStore.Save(filePath, regexEntries);
            fileExist = File.Exists(filePath);

            // Assert 
            Assert.True(fileExist);
        }

        [Fact]
        public void SavePatterns_EmptyRegexEntryList_WritesNewContent()
        {
            // Arrange
            var regexEntries = new List<RegexPatternEntry>();
            var filePath = $"{_testingFilesPath}/output.xml";
            var returnedRegexEntries = new List<RegexPatternEntry>();

            // Act
            _reggexPatternDataStore.Save(filePath, regexEntries);
            returnedRegexEntries = _reggexPatternDataStore.Load(filePath);

            // Assert
            Assert.Empty(returnedRegexEntries);
        }

        [Fact]
        public void SavePattenrs_ValidRegexEntryList_WritesNewContent()
        {
            // Arrange
            var regexEntries = new List<RegexPatternEntry>() {
                new RegexPatternEntry() {
                    RegexPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"
                } };
            var filePath = $"{_testingFilesPath}/output.xml";
            var returnedRegexEntries = new List<RegexPatternEntry>();

            // Act
            _reggexPatternDataStore.Save(filePath, regexEntries);
            returnedRegexEntries = _reggexPatternDataStore.Load(filePath);

            // Assert
            Assert.NotEmpty(returnedRegexEntries);
        }
    }
}