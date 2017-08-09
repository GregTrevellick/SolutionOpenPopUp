using Microsoft.VisualStudio.Shell;
using SolutionOpenPopUp.Helpers;
using System.ComponentModel;

namespace SolutionOpenPopUp.Options
{
    public class GeneralOptions : DialogPage
    {
        [Category(CommonConstants.CategorySubLevel)]
        [DisplayName("Show " + CommonConstants.ReadMeDotTxt + " (from solution root) when opening solution")]
        [Description("Set to true so that the content of a file named '" + CommonConstants.ReadMeDotTxt + "' (case insensitive), located in the root folder of the solution, is displayed in a pop-up dialog when the solution is opened, provided such a file exists.")]
        public bool ShowReadMeDotTxt { get; set; } = true;

        [Category(CommonConstants.CategorySubLevel)]
        [DisplayName("Show " + CommonConstants.SolutionOpenPopUpDotTxt + " (from solution root) when opening solution")]
        [Description("Set to true so that the content of a file named '" + CommonConstants.SolutionOpenPopUpDotTxt + "' (case insensitive), located in the root folder of the solution, is displayed in a pop-up dialog when the solution is opened, provided such a file exists.")]
        public bool ShowSolutionOpenPopUpDotTxt { get; set; } = true;

        //[Category("General")]
        //[DisplayName("Custom solution open pop-up file")]
        //[Description("The name of the file on your PC that will appear in pop-up. This would typically not be a file under source control.")]
        //public string CustomFileDotTxt
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(customFileDotTxt))
        //        {
        //            return string.Empty;
        //        }
        //        else
        //        {
        //            return customFileDotTxt;
        //        }
        //    }
        //    set
        //    {
        //        customFileDotTxt = value;
        //    }
        //}

        [Category(CommonConstants.CategorySubLevel)]
        [DisplayName("Maxiumum lines to,display")]
        [Description("The overall maxiumum number of lines to show in the pop-up for all text files combined. When more than one text file is displayed, the contents of each file are shown on a pro-rata basis.")]
        public int OverallLinesLimit { get; set; } = 35;

        [Category(CommonConstants.CategorySubLevel)]
        [DisplayName("Line truncation limit")]
        [Description("The truncation point at which individual lines of text in the source file will be truncated.")]
        public int LineLengthTruncationLimit { get; set; } = 100;

        [Category(CommonConstants.CategorySubLevel)]
        [DisplayName("Show source file names")]
        [Description("Show or hide the file names of the files containing content that appears in the pop-up window.")]
        public bool ShowFileNamesInPopUp { get; set; } = true;

        ////public override void LoadSettingsFromStorage()
        ////{
        ////    base.LoadSettingsFromStorage();
        ////}

        //private string customFileDotTxt { get; set; }
        //private string previousCustomFileDotTxt { get; set; }

        //protected override void OnApply(PageApplyEventArgs e)
        //{
        //    ApplyOptions(e, CustomFileDotTxt);

        //    base.OnApply(e);
        //}

        //private void ApplyOptions(PageApplyEventArgs e, string customFileDotTxt)
        //{
        //    var previousFileChanged = false;

        //    if (!string.IsNullOrEmpty(customFileDotTxt))
        //    {
        //        previousFileChanged = true;
        //        previousCustomFileDotTxt = customFileDotTxt;
        //    }

        //    if (previousFileChanged)
        //    {
        //        if (!ArtefactsHelper.DoesFileExist(customFileDotTxt))
        //        {
        //            e.ApplyBehavior = ApplyKind.Cancel;

        //            var caption = Vsix.Name + " " + Vsix.Version;

        //            var filePrompterHelper = new FilePrompterHelper(caption, customFileDotTxt);

        //            var persistOptionsDto = filePrompterHelper.PromptForActualFile(customFileDotTxt);

        //            if (persistOptionsDto.Persist)
        //            {
        //                PersistCustomFileDotTxt(persistOptionsDto.ValueToPersist);
        //            }
        //        }
        //    }
        //}

        //public void PersistCustomFileDotTxt(string fileName)
        //{
        //    VSPackage.Options.CustomFileDotTxt = fileName;
        //    VSPackage.Options.SaveSettingsToStorage();
        //}
    }
}
