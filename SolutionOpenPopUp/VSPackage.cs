using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using SolutionOpenPopUp.Options;

namespace SolutionOpenPopUp
{
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration(productName: "#110", productDetails: "#112", productId: Vsix.Version, IconResourceID = 400)]
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
            //TODO get from file on disc

            DisplayPopUpMessage("xxxxxxxxxx", "yyyyyyyy");
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
