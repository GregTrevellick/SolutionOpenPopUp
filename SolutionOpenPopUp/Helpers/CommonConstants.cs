using System;

namespace SolutionOpenPopUp.Helpers
{
    public static class CommonConstants
    {
        public static string InformUserMissingFile(string missingFileName)
        {
            return $"The file {missingFileName} does not exist.";
        }

        public static string PromptForActualFile(string missingFileName)
        {
            return InformUserMissingFile(missingFileName)
                   + Environment.NewLine + Environment.NewLine
                   + "Do you want to browse the for the file ?"
                   + Environment.NewLine + Environment.NewLine
                   + "Click YES to locate the file, NO to save anyway.";
        }

        public const string CategorySubLevel = "General";
        public const string ReadMeDotTxt = "ReadMe.txt";
        public const string SolutionOpenPopUpDotTxt = "SolutionOpenPopUp.txt";
    }
}




//public const string ActualPathToExeOptionDetailedDescription = "Specify the absolute install path for the application.";
//public const string ActualPathToExeOptionLabelPrefix = "Application path to ";
//public static string ContinueAnyway = "Click OK to open anyway, or CANCEL to return to Visual Studio.";
//public const string DefaultFileQuantityWarningLimit = "10";
//public static string FileQuantityWarningLimitInvalid = "Invalid integer value specified for:" + Environment.NewLine + Environment.NewLine + FileQuantityWarningLimitOptionLabel;
//public const string FileQuantityWarningLimitOptionLabel = "Simultaneous artefacts opening count warning limit";
//public const string ToolsOptionsNotice = "(You can change suppress this notice in Tools | Options)";
//public static string UnexpectedError =
//    "An unexpected error has occured. Please restart Visual Studio and re-try." + Environment.NewLine + Environment.NewLine +
//    "If the error persists please log a bug for this extension via the Visual Studio Marketplace at https://marketplace.visualstudio.com" + Environment.NewLine + Environment.NewLine +
//    "Press OK to return to Visual Studio.";