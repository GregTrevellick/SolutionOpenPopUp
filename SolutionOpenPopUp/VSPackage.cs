using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SolutionOpenPopUp.Helpers;
using SolutionOpenPopUp.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.InteropServices;

namespace SolutionOpenPopUp
{
    [ProvideOptionPage(typeof(GeneralOptions), Vsix.Name, "General", 0, 0, true)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration(productName: "#110", productDetails: "#112", productId: Vsix.Version, IconResourceID = 400)]
    [Guid("65dcb9bb-a90a-458b-9a89-a12fe85b9077")]
    public sealed class VSPackage : Package
    {
        private DTE dte;
        public static GeneralOptions Options { get; private set; }

        public VSPackage()
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            IServiceContainer serviceContainer = this as IServiceContainer;
            dte = serviceContainer.GetService(typeof(SDTE)) as DTE;
            var solutionEvents = dte.Events.SolutionEvents;
            solutionEvents.Opened += OnSolutionOpened;
        }

        private void OnSolutionOpened()
        {
            var textFiles = new List<string>();

            var solutionPath = dte.Solution.FullName;
            var solutionFolder = Path.GetDirectoryName(solutionPath);

            var solutionOpenPopUpDotTxt = CommonConstants.SolutionOpenPopUpDotTxt; 
            var textFile = Path.Combine(solutionFolder, solutionOpenPopUpDotTxt);
            textFiles.Add(textFile);

            var readMeDotTxt = CommonConstants.ReadMeDotTxt; 
            textFile = Path.Combine(solutionFolder, readMeDotTxt);
            textFiles.Add(textFile);
            
            var popUpBody = GetPopUpMessage(textFiles);
            var popUpTitle = Vsix.Name + " " + Vsix.Version;

            DisplayPopUpMessage(popUpTitle, popUpBody);
        }

        private string GetPopUpMessage(IEnumerable<string> textFiles)
        {
            var result = string.Empty;

            foreach (var textFile in textFiles)
            {
                var textFileIsUnderSourceControl = dte.SourceControl.IsItemUnderSCC(textFile);
                result += GetPopUpMessage(textFile, textFileIsUnderSourceControl);
            }

            return result;
        }

        private string GetPopUpMessage(string textFile, bool fileIsUnderSourceControl)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(textFile))
            {
                var textLimit = 2000;//gregt put into options

                if (File.Exists(textFile))
                {
                    var sourceControlStatus = fileIsUnderSourceControl ? "this file IS under source control" : "this file is NOT under source control";

                    var text = File.ReadAllText(textFile);

                    if (text.Length > textLimit)
                    {
                        result += text.Substring(0, textLimit);
                    }
                    else
                    {
                        result += text;
                    }

                    result += Environment.NewLine;
                    result += Environment.NewLine;
                    result += "Source: " + textFile + " (" + sourceControlStatus + ")";
                }
                else
                {
                    result += "The file " + textFile + " cannnot be found.";
                }

                result += Environment.NewLine;
                result += Environment.NewLine;
            }

            return result;
        }

        private void DisplayPopUpMessage(string popUpTitle, string popUpBody)
        {
            if (!string.IsNullOrEmpty(popUpBody))
            {
                IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
                Guid clsid = Guid.Empty;
                int result;

                uiShell.ShowMessageBox(
                    0,
                    ref clsid,
                    popUpTitle,
                    popUpBody,
                    string.Empty,
                    0,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                    OLEMSGICON.OLEMSGICON_NOICON,
                    0,
                    out result);
            }
        }

        //private string GetCustomFileDotTxt()
        //{
        //    var generalOptions = (GeneralOptions)GetDialogPage(typeof(GeneralOptions));
        //    return generalOptions.CustomFileDotTxt;
        //}
    }
}
