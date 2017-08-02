using System.Windows.Forms;
using SolutionOpenPopUp.Helpers.Dtos;

namespace SolutionOpenPopUp.Helpers
{
    public class FilePrompterHelper
    {
        private string caption { get; set; }
        private string fileToBrowseFor { get; set; }

        public FilePrompterHelper(string caption, string fileToBrowseFor)
        {
            this.caption = caption;
            this.fileToBrowseFor = fileToBrowseFor;
        }

        public PersistOptionsDto PromptForActualFile(string originalPathToFile)
        {
            var saveSettingsDto = new PersistOptionsDto();

            var box = MessageBox.Show(
               CommonConstants.PromptForActualExeFile(originalPathToFile),
               caption,
               MessageBoxButtons.YesNo,
               MessageBoxIcon.Question);

            switch (box)
            {
                case DialogResult.Yes:
                    var resultAndNamePicked = BrowseFileHelper.BrowseToFileLocation(fileToBrowseFor);
                    if (resultAndNamePicked.DialogResult == DialogResult.OK)
                    {
                        SetSaveSettingsDto(saveSettingsDto, resultAndNamePicked.FileNameChosen);
                    }
                    break;
                case DialogResult.No:
                    SetSaveSettingsDto(saveSettingsDto, originalPathToFile);
                    break;
            }

            return saveSettingsDto;
        }

        private void SetSaveSettingsDto(PersistOptionsDto saveSettingsDto, string fileName)
        {
            saveSettingsDto.ValueToPersist = fileName;
            saveSettingsDto.Persist = true;
        }
    }
}
