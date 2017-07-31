using System;
using System.Windows.Forms;
using SolutionOpenPopUp.Helpers.Dtos;

namespace SolutionOpenPopUp.Helpers
{
    public class BrowseFileHelper
    {
        public static FileBrowseOutcomeDto BrowseToFileLocation(string executableFileToBrowseFor)
        {
            var dialog = new OpenFileDialog
            {
                DefaultExt = ".exe",
                FileName = executableFileToBrowseFor,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                CheckFileExists = true
            };

            var dialogResult = dialog.ShowDialog();

            return new FileBrowseOutcomeDto
            {
                FileNameChosen = dialog.FileName,
                DialogResult = dialogResult
            };
        }

    }
}
