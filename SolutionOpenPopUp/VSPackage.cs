using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SolutionOpenPopUp.Options;
using System;
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
            var solutionPath = dte.Solution.FullName;
            var solutionFolder = Path.GetDirectoryName(solutionPath);
            var popUpTextFileNameTeam = GetPopUpTextFileNameTeam();

            var popUpFileTeam = Path.Combine(solutionFolder, popUpTextFileNameTeam);
            var popUpTextFileFullPathSelf = GetPopUpTextFileFullPathSelf();

            var popUpBody = GetPopUpMessage(popUpTextFileFullPathSelf, popUpFileTeam);
            var popUpTitle = Vsix.Name + " " + Vsix.Version;

            DisplayPopUpMessage(popUpTitle, popUpBody);
        }

        private string GetPopUpMessage(string popUpFileSelf, string popUpFileTeam)
        {
            var popUpFileSelfIsUnderSourceControl = dte.SourceControl.IsItemUnderSCC(popUpFileSelf);
            var popUpFileTeamIsUnderSourceControl = dte.SourceControl.IsItemUnderSCC(popUpFileTeam);

            var result = string.Empty;

            result += GetPopUpMessage(popUpFileSelf, popUpFileSelfIsUnderSourceControl);
            result += GetPopUpMessage(popUpFileTeam, popUpFileTeamIsUnderSourceControl);

            return result;
        }

        private string GetPopUpMessage(string fileName, bool fileIsUnderSourceControl)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(fileName))
            {
                var textLimit = 2000;//gregt put in options

                var sourceControlStatus = fileIsUnderSourceControl ? "this file IS under source control" : "this file is NOT under source control";

                if (File.Exists(fileName))
                {
                    result += File.ReadAllText(fileName);
                    result = result.Substring(0, textLimit);
                    result += Environment.NewLine;
                    result += Environment.NewLine;
                    result += "Source: " + fileName + " (" + sourceControlStatus + ")";
                }
                else
                {
                    result += "The file " + fileName + " cannnot be found.";
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
                    OLEMSGICON.OLEMSGICON_INFO,
                    0,
                    out result);
            }
        }

        private string GetPopUpTextFileFullPathSelf()
        {
            var generalOptions = (GeneralOptions)GetDialogPage(typeof(GeneralOptions));
            return generalOptions.PopUpTextFileFullPathSelf;
        }

        private string GetPopUpTextFileNameTeam()
        {
            var generalOptions = (GeneralOptions)GetDialogPage(typeof(GeneralOptions));
            return generalOptions.PopUpTextFileNameTeam;
        }
    }
}
