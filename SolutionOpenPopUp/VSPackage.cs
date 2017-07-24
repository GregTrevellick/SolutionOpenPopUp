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
    //[Guid(PackageGuids.guidOpenInAppPackageString)]
    [Guid("5e45aa4e-1a24-4edf-b10a-228b63448f70")]//153788b5-1eff-4709-b5f3-8bb0a92c0799
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
            //var fileName = Options.PopUpTextFileSelf;
            
            var slnPath = dte.Solution.FullName;
            var slnFolder = Path.GetDirectoryName(slnPath);
            var fileName = Path.Combine(slnFolder, "PopUpTextFileSelfTeam.txt");
            var popUpMessage = GetPopUpMessage(fileName);

            DisplayPopUpMessage("a.n.title", popUpMessage);

        }

        private string GetPopUpMessage(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            else
            {
                return null;
            }
        }

        private void DisplayPopUpMessage(string title, string text)
        {
            IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
            Guid clsid = Guid.Empty;
            int result;

            uiShell.ShowMessageBox(
                0,
                ref clsid,
                title.ToUpper(),
                text,
                string.Empty,
                0,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                OLEMSGICON.OLEMSGICON_INFO,
                0,
                out result);
        }
    }
}
