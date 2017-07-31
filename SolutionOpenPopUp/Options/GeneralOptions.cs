using Microsoft.VisualStudio.Shell;
using SolutionOpenPopUp.Helpers;
using System.ComponentModel;

namespace SolutionOpenPopUp.Options
{
    public class GeneralOptions : DialogPage
    {
        [Category("General")]
        [DisplayName("Personal pop-up text file")]
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
        [DisplayName("Team pop-up text file")]
        [Description("The name of file that will appear in pop-up for anyone who opens this sln. This would typically be a file under source control.")]
        public string PopUpTextFileNameTeam
        {
            get
            {
                if (string.IsNullOrEmpty(popUpTextFileNameTeam))
                {
                    return @"..\SolutionOpenPopUp.txt";
                }
                else
                {
                    return popUpTextFileNameTeam;
                }
            }
            set
            {
                popUpTextFileNameTeam = value;
            }
        }

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();
        }

        private string popUpTextFileFullPathSelf;
        private string popUpTextFileNameTeam;

        private string previousPopUpTextFile { get; set; }

        protected override void OnApply(PageApplyEventArgs e)
        {
            ApplyOptions(e, PopUpTextFileFullPathSelf, true);
            ApplyOptions(e, PopUpTextFileNameTeam, false);

            base.OnApply(e);
        }

        private void ApplyOptions(PageApplyEventArgs e, string filename, bool self)
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
                        if (self)
                        {
                            PersistPopUpTextFileFullPathSelf(persistOptionsDto.ValueToPersist);
                        }
                        else
                        {
                            PersistPopUpTextFileNameTeam(persistOptionsDto.ValueToPersist);
                        }
                    }
                }
            }
        }

        public void PersistPopUpTextFileFullPathSelf(string fileName)
        {
            VSPackage.Options.PopUpTextFileFullPathSelf = fileName;
            VSPackage.Options.SaveSettingsToStorage();
        }

        public void PersistPopUpTextFileNameTeam(string fileName)
        {
            VSPackage.Options.PopUpTextFileNameTeam = fileName;
            VSPackage.Options.SaveSettingsToStorage();
        }
    }
}
