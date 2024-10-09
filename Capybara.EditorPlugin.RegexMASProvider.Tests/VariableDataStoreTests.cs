using RegexMASProviderLib.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capybara.EditorPlugin.RegexMASProvider.Tests
{
    public class VariableDataStoreTests
    {
        private readonly VariableDataStore<List<Variable>> _variableDataStore = new ();
        private readonly string _testingFilesPath = Path.Combine($"{Directory.GetCurrentDirectory()}", "TestFiles");

        [Theory]
        [InlineData("dummy")]
        [InlineData("inexistentFile")]
        public void LoadVariables_FileDoesNotExist_ReturnEmptyList(string filePath)
        {
            // Arrange
            var patternPath = filePath;

            // Act
            var variables = _variableDataStore.Load(patternPath);

            // Assert
            Assert.Empty(variables);
        }

        [Fact]
        public void LoadVariables_FileValid_ReturnsVariableList()
        {
            // Arrange
            var filePath = $"{_testingFilesPath}/variables.xml";
            var expectedVariableName = "month";

            // Act
            var variables = _variableDataStore.Load(filePath);
            var firstVariable = variables[0];

            // Assert
            Assert.NotEmpty(variables);
            Assert.Equal(expectedVariableName, firstVariable.Name);
        }

        [Fact]
        public void SaveVariables_AnyList_WritesTheNewFile()
        {
            // Arrange
            var variables = new List<Variable>();
            var filePath = $"{_testingFilesPath}/variablesOutput.xml";
            var fileExists = false;

            // Act
            _variableDataStore.Save(filePath, variables);
            fileExists = File.Exists(filePath);

            // Assert
            Assert.True(fileExists);
        }

        [Fact]
        public void SaveVariables_EmptyList_WritesTheXMLFile()
        {
            // Arrange
            var variables = new List<Variable>();
            var filePath = $"{_testingFilesPath}/variablesOutput.xml";
            var returnedVariables = new List<Variable>();

            // Act
            _variableDataStore.Save(filePath, variables);
            returnedVariables = _variableDataStore.Load(filePath);

            // Assert
            Assert.Empty(returnedVariables);
        }

        [Fact]
        public void SaveVariable_ValidVariableList_WritesTheXMLFile()
        {
            // Arrange
            var variables = new List<Variable>()
            {
                new Variable()
                {
                    Name = "foo",
                }
            };
            var filePath = $"{_testingFilesPath}/variablesOutput.xml";
            var returnedVariables = new List<Variable>();
            var firstVariable = new Variable();

            // Act 
            _variableDataStore.Save(filePath, variables);
            returnedVariables = _variableDataStore.Load(filePath);
            firstVariable = returnedVariables[0];

            // Assert
            Assert.NotEmpty(returnedVariables);
            Assert.Equal("foo", firstVariable.Name);
        }
    }
}
