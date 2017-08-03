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
        private string popUpFooter;
        private string bulletPoint = " - ";

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
            popUpFooter = string.Empty;

            var textFiles = new List<string>();

            var solutionPath = dte.Solution.FullName;
            var solutionFolder = Path.GetDirectoryName(solutionPath);

            if (ShowSolutionOpenPopUpDotTxt)
            {
                var solutionOpenPopUpDotTxt = CommonConstants.SolutionOpenPopUpDotTxt;
                var textFile = Path.Combine(solutionFolder, solutionOpenPopUpDotTxt);
                textFiles.Add(textFile);
            }

            if (ShowReadMeDotTxt)
            {
                var readMeDotTxt = CommonConstants.ReadMeDotTxt;
                var textFile = Path.Combine(solutionFolder, readMeDotTxt);
                textFiles.Add(textFile);
            }

            var popUpBody = GetPopUpBody(textFiles);

            DisplayPopUpMessage(string.Empty, popUpBody);
        }

        private string GetPopUpBody(IEnumerable<string> textFiles)
        {
            var result = string.Empty;
            
            foreach (var textFile in textFiles)
            {
                var textFileIsUnderSourceControl = dte.SourceControl.IsItemUnderSCC(textFile);
                result += GetPopUpMessage(textFile, textFileIsUnderSourceControl);
            }

            if (!string.IsNullOrEmpty(popUpFooter))
            {
                result += "ABOUT";
                result += Environment.NewLine;
                result += bulletPoint + Vsix.Name + " " + Vsix.Version;
                result += Environment.NewLine;
                var url = "https://marketplace.visualstudio.com/items?itemName=GregTrevellick.SolutionOpenPopUp";
                result += bulletPoint + url;
                result += Environment.NewLine;
                result += popUpFooter;
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

                    var sourceControlStatus = fileIsUnderSourceControl ? "IS" : "is NOT";
                    popUpFooter += bulletPoint + textFile + " (file " + sourceControlStatus + " under source control)";
                    popUpFooter += Environment.NewLine;
                }
                else
                {
                    popUpFooter += bulletPoint + textFile + " not be found.";
                    popUpFooter += Environment.NewLine;
                }

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
            
        private bool ShowSolutionOpenPopUpDotTxt
        {
            get
            {
                var generalOptions = (GeneralOptions)GetDialogPage(typeof(GeneralOptions));
                return generalOptions.ShowSolutionOpenPopUpDotTxt;
            }
        }

        private bool ShowReadMeDotTxt
        {
            get
            {
                var generalOptions = (GeneralOptions) GetDialogPage(typeof(GeneralOptions));
                return generalOptions.ShowReadMeDotTxt;
            }
        }
    }
}
