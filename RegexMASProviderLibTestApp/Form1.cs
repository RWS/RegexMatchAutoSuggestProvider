using RegexMASProviderLib.DataAccess;
using RegexMASProviderLib.Services;
using RegexMASProviderLib.View;
using System;
using System.Windows.Forms;

namespace RegexMASProviderLibTestApp
{
    public partial class Form1 : Form
    {
        private RegexPatternEntries _regexPatternEntries;
        private Variables _variables;
        private ListChangeNotifier _listChangeNotifier;
        private IAutoSuggestService _autoSuggestService = new AutoSuggestService();

        public string RegexFile
        {
            get { return @"..\..\TestFiles\regex.xml"; }
        }

        public string VariableFile
        {
            get { return @"..\..\TestFiles\variables.xml"; }
        }

        public Form1()
        {
            InitializeComponent();
            _regexPatternEntries = new RegexPatternEntries(RegexFile);
            _variables = new Variables(VariableFile);
            _listChangeNotifier = new ListChangeNotifier();
            regexDgv.Initialize(_regexPatternEntries, _variables, _listChangeNotifier);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _regexPatternEntries.Save();
            _variables.Save();
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            var sourceText = txtSource.Text;
            if (string.IsNullOrEmpty(sourceText))
            {
                return;
            }

            var autoSuggestEntries = _autoSuggestService.GetAutoSuggestEntries(
                sourceText, _regexPatternEntries.Entries, _variables.Entries);
            var popupContent = new PopupWindowContent(autoSuggestEntries);
            popupWindow.SetContent(popupContent);
        }

        private void btnShowRegexTester_Click(object sender, EventArgs e)
        {
            regexDgv.ShowRegexTester = !regexDgv.ShowRegexTester;
        }

        private void regexDgv_Load(object sender, EventArgs e)
        {

        }
    }
}
