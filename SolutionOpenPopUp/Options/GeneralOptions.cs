using Microsoft.VisualStudio.Shell;
using SolutionOpenPopUp.Helpers;
using System.ComponentModel;

namespace SolutionOpenPopUp.Options
{
    public class GeneralOptions : DialogPage
    {
        //[Category("General")]
        //[DisplayName("Personal solution open pop-up file")]
        //[Description("The name of the file on your PC that will appear in pop-up. This would typically not be a file under source control.")]
        //public string PopUpTextFileFullPathSelf
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(popUpTextFileFullPathSelf))
        //        {
        //            return string.Empty;
        //        }
        //        else
        //        {
        //            return popUpTextFileFullPathSelf;
        //        }
        //    }
        //    set
        //    {
        //        popUpTextFileFullPathSelf = value;
        //    }
        //}

        //public string ReadMeDotTxt
        //{
        //    get { return @"ReadMe.txt"; }
        //}

        ////[Category("General")]
        ////[DisplayName("Shared solution open pop-up file")]
        ////[Description("The fixed name of file. located in the root of your solution folder, that will appear in pop-up for anyone who opens this sln. You should typically place this file under source control.")]
        //public string SolutionOpenPopUpDotTxt
        //{
        //    get
        //    {
        //        return CommonConstants.CategorySubLevel;// @"SolutionOpenPopUp.txt"; 
        //    }
        //}

        [Category(CommonConstants.CategorySubLevel)]
        [DisplayName("Show " + CommonConstants.ReadMeDotTxt + " from root of solution when opening solution")]
        [Description("Set to true so that the content of a file named '" + CommonConstants.ReadMeDotTxt + "' (case insensitive), located in the root folder of the solution, are displayed in the pop-up message when the solution is opened, provided such a file exists.")]
        public bool ShowReadMeDotTxt { get; set; } = true;

        [Category(CommonConstants.CategorySubLevel)]
        [DisplayName("Show " + CommonConstants.SolutionOpenPopUpDotTxt + " from root of solution when opening solution")]
        [Description("Set to true so that the content of a file named '" + CommonConstants.SolutionOpenPopUpDotTxt + "' (case insensitive), located in the root folder of the solution, are displayed in the pop-up message when the solution is opened, provided such a file exists.")]
        public bool ShowSolutionOpenPopUpDotTxt { get; set; } = true;

        ////public override void LoadSettingsFromStorage()
        ////{
        ////    base.LoadSettingsFromStorage();
        ////}

        //private string popUpTextFileFullPathSelf;

        //private string previousPopUpTextFile { get; set; }

        //protected override void OnApply(PageApplyEventArgs e)
        //{
        //    ApplyOptions(e, PopUpTextFileFullPathSelf);

        //    base.OnApply(e);
        //}

        //private void ApplyOptions(PageApplyEventArgs e, string selfFilename)
        //{
        //    var previousFileChanged = false;

        //    if (!string.IsNullOrEmpty(selfFilename))
        //    {
        //        previousFileChanged = true;
        //        previousPopUpTextFile = selfFilename;
        //    }

        //    if (previousFileChanged)
        //    {
        //        if (!ArtefactsHelper.DoesFileExist(selfFilename))
        //        {
        //            e.ApplyBehavior = ApplyKind.Cancel;

        //            var caption = Vsix.Name + " " + Vsix.Version;

        //            var filePrompterHelper = new FilePrompterHelper(caption, "something.exe");

        //            var persistOptionsDto = filePrompterHelper.PromptForActualFile(selfFilename);

        //            if (persistOptionsDto.Persist)
        //            {
        //                PersistPopUpTextFileFullPathSelf(persistOptionsDto.ValueToPersist);
        //            }
        //        }
        //    }
        //}

        //public void PersistPopUpTextFileFullPathSelf(string fileName)
        //{
        //    VSPackage.Options.PopUpTextFileFullPathSelf = fileName;
        //    VSPackage.Options.SaveSettingsToStorage();
        //}
    }
}
