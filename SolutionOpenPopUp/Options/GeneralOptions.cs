using Microsoft.VisualStudio.Shell;
using SolutionOpenPopUp.Helpers;
using System.ComponentModel;

namespace SolutionOpenPopUp.Options
{
    public class GeneralOptions : DialogPage
    {
        [Category("General")]
        [DisplayName("Personal solution open pop-up file")]
        [Description("The name of the file on your PC that will appear in pop-up. This would typically not be a file under source control.")]
        public string PopUpTextFileFullPathSelf
        {
            get
            {
                if (string.IsNullOrEmpty(popUpTextFileFullPathSelf))
                {
                    return string.Empty;
                }
                else
                {
                    return popUpTextFileFullPathSelf;
                }
            }
            set
            {
                popUpTextFileFullPathSelf = value;
            }
        }

        [Category("General")]
        [DisplayName("Shared solution open pop-up file")]
        [Description("The fixed name of file. located in the root of your solution folder, that will appear in pop-up for anyone who opens this sln. You should typically place this file under source control.")]
        public string PopUpTextFileNameTeam
        {
            get { return @"SolutionOpenPopUp.txt"; }
        }

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();
        }

        private string popUpTextFileFullPathSelf;

        private string previousPopUpTextFile { get; set; }

        protected override void OnApply(PageApplyEventArgs e)
        {
            ApplyOptions(e, PopUpTextFileFullPathSelf);

            base.OnApply(e);
        }

        private void ApplyOptions(PageApplyEventArgs e, string selfFilename)
        {
            var previousFileChanged = false;

            if (!string.IsNullOrEmpty(selfFilename))
            {
                previousFileChanged = true;
                previousPopUpTextFile = selfFilename;
            }

            if (previousFileChanged)
            {
                if (!ArtefactsHelper.DoesFileExist(selfFilename))
                {
                    e.ApplyBehavior = ApplyKind.Cancel;

                    var caption = Vsix.Name + " " + Vsix.Version;

                    var filePrompterHelper = new FilePrompterHelper(caption, "something.exe");

                    var persistOptionsDto = filePrompterHelper.PromptForActualExeFile(selfFilename);

                    if (persistOptionsDto.Persist)
                    {
                        PersistPopUpTextFileFullPathSelf(persistOptionsDto.ValueToPersist);
                    }
                }
            }
        }

        public void PersistPopUpTextFileFullPathSelf(string fileName)
        {
            VSPackage.Options.PopUpTextFileFullPathSelf = fileName;
            VSPackage.Options.SaveSettingsToStorage();
        }
    }
}
