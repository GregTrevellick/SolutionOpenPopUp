using Microsoft.VisualStudio.Shell;
using System.ComponentModel;
using SolutionOpenPopUp.Helpers;

namespace SolutionOpenPopUp.Options
{
    public class GeneralOptions : DialogPage
    {
        [Category("General")]
        [DisplayName("A N Label 2")]
        [Description("Description self")]
        public string PopUpTextFileSelf
        {
            get
            {
                if (string.IsNullOrEmpty(popUpTextFileSelf))
                {
                    return @"c:\temp\ADefaultPopUpTextFile_SELF_FileName.txt";
                }
                else
                {
                    return popUpTextFileSelf;
                }
            }
            set
            {
                popUpTextFileSelf = value;
            }
        }


        [Category("General")]
        [DisplayName("A N Label 1")]
        [Description("Description team")]
        public string PopUpTextFileTeam
        {
            get
            {
                if (string.IsNullOrEmpty(popUpTextFileTeam))
                {
                    return @"..\ADefaultPopUpTextFile_TEAM_FileName.txt";
                }
                else
                {
                    return popUpTextFileTeam;
                }
            }
            set
            {
                popUpTextFileTeam = value;
            }
        }

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();
        }

        private string popUpTextFileSelf;
        private string popUpTextFileTeam;
        private string previousPopUpTextFile { get; set; }

        protected override void OnApply(PageApplyEventArgs e)
        {
            ApplyOptions(e, PopUpTextFileSelf);
            ApplyOptions(e, PopUpTextFileTeam);

            base.OnApply(e);
        }

        private void ApplyOptions(PageApplyEventArgs e, string filename)
        {
            var previousFileChanged = false;

            if (filename != previousPopUpTextFile)
            {
                previousFileChanged = true;
                previousPopUpTextFile = filename;
            }

            if (previousFileChanged)
            {
                if (!ArtefactsHelper.DoesFileExist(filename))
                {
                    e.ApplyBehavior = ApplyKind.Cancel;

                    var caption = Vsix.Name + " " + Vsix.Version;

                    var filePrompterHelper = new FilePrompterHelper(caption, "something.exe");

                    var persistOptionsDto = filePrompterHelper.PromptForActualExeFile(filename);

                    if (persistOptionsDto.Persist)
                    {
                        PersistVSToolOptions(persistOptionsDto.ValueToPersist);
                    }
                }
            }
        }

        public void PersistVSToolOptions(string fileName)
        {
            VSPackage.Options.PopUpTextFileTeam = fileName;
            VSPackage.Options.SaveSettingsToStorage();
        }
    }
}
