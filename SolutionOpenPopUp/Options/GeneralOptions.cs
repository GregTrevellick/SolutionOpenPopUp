using Microsoft.VisualStudio.Shell;
using SolutionOpenPopUp.Helpers;
using System.ComponentModel;

namespace SolutionOpenPopUp.Options
{
    public class GeneralOptions : DialogPage
    {
        [Category("General")]
        [DisplayName("Personal pop-up text file")]
        [Description("The name of the file on your PC that will appear in pop-up. Typically not a file under source control.")]
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


        //[Category("General")]
        //[DisplayName("Shared pop-up text file")]
        //[Description("The name of file that will appear in pop-up for anyone who opens . Typically not a file under source control.  on your PC that is part of the source control 1")]
        //public string PopUpTextFileTeam
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(popUpTextFileTeam))
        //        {
        //            return @"..\ADefaultPopUpTextFile_TEAM_FileName.txt";
        //        }
        //        else
        //        {
        //            return popUpTextFileTeam;
        //        }
        //    }
        //    set
        //    {
        //        popUpTextFileTeam = value;
        //    }
        //}

        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();
        }

        private string popUpTextFileSelf;
        //private string popUpTextFileTeam;
        private string previousPopUpTextFile { get; set; }

        protected override void OnApply(PageApplyEventArgs e)
        {
            ApplyOptions(e, PopUpTextFileSelf);
            //ApplyOptions(e, PopUpTextFileTeam);

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
            VSPackage.Options.PopUpTextFileSelf = fileName;
            VSPackage.Options.SaveSettingsToStorage();
        }
    }
}
