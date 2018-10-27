using EnvDTE;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SolutionOpenPopUp.Helpers;
using SolutionOpenPopUp.Helpers.Dtos;
using SolutionOpenPopUp.Options;
using SolutionOpenPopUp.Rating;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using VsixRatingChaser.Interfaces;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Events;
using Task = System.Threading.Tasks.Task;

namespace SolutionOpenPopUp
{
    [ProvideOptionPage(typeof(GeneralOptions), Vsix.Name, "General", 0, 0, true)]
    //[ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionOpening_string, PackageAutoLoadFlags.BackgroundLoad)]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration(productName: "#110", productDetails: "#112", productId: Vsix.Version, IconResourceID = 400)]
    [Guid("65dcb9bb-a90a-458b-9a89-a12fe85b9077")]
    public sealed class VSPackage : AsyncPackage
    {
//#pragma warning disable S1075 // URIs should not be hardcoded
        private const string marketplaceUrl = "https://goo.gl/aGVjJ8";
//#pragma warning restore S1075 // URIs should not be hardcoded
        private DTE dte;
        public static GeneralOptions Options { get; private set; }
        private string popUpFooter;
        private readonly string bulletPoint = " - ";
        private string solutionFolder;
        private List<TextFileDto> textFileDtos;
        private bool popUpBodyIsPopulated;

        public VSPackage()
        {
        }

//        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
//        {
//            await JoinableTaskFactory.SwitchToMainThreadAsync();

//            IServiceContainer serviceContainer = this as IServiceContainer;
//            dte = serviceContainer.GetService(typeof(SDTE)) as DTE;
//            Assumes.Present(dte);
//             var solutionEvents = dte.Events.SolutionEvents;
//            solutionEvents.Opened += OnSolutionOpened;
//#pragma warning disable S125 // Sections of code should not be commented out
//                            //bool a = dte.Solution.IsOpen();
//        }
//#pragma warning restore S125 // Sections of code should not be commented out

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // runs in the background thread and doesn't affect the responsiveness of the UI thread.
            //await Task.Delay(5000);

            bool isSolutionLoaded = await IsSolutionLoadedAsync();

            if (isSolutionLoaded)
            {
                HandleOpenSolution();
            }

            // Listen for subsequent solution events
            Microsoft.VisualStudio.Shell.Events.SolutionEvents.OnAfterOpenSolution += HandleOpenSolution;
        }

        private async Task<bool> IsSolutionLoadedAsync()
        {
            // Since this package might not be initialized until after a solution has finished loading, we need to check if a solution has already been loaded and then handle it.
            await JoinableTaskFactory.SwitchToMainThreadAsync();
            var solService = await GetServiceAsync(typeof(SVsSolution)) as IVsSolution;

            ErrorHandler.ThrowOnFailure(solService.GetProperty((int)__VSPROPID.VSPROPID_IsSolutionOpen, out object value));

            return value is bool isSolOpen && isSolOpen;
        }

        private void HandleOpenSolution(object sender = null, EventArgs e = null)
        {
            ///////////////////////ThreadHelper.ThrowIfNotOnUIThread();



            IServiceContainer serviceContainer = this as IServiceContainer;
            dte = serviceContainer.GetService(typeof(SDTE)) as DTE;



            popUpBodyIsPopulated = false;
            popUpFooter = string.Empty;
            textFileDtos = new List<TextFileDto>();

            SolutionOpenPopUpDotTxtHandler();
            ReadMeDotTxtHandler();

            var popUpBody = GetPopUpBody(textFileDtos);

            if (popUpBodyIsPopulated)
            {
                if (GeneralOptionsDto.ShowFileNamesInPopUp)
                {
                    popUpBody += GetPopUpFooter();
                }

                DisplayPopUpMessage(string.Empty, popUpBody);
            }
        }

        private void ReadMeDotTxtHandler()
        {
            //ThreadHelper.ThrowIfNotOnUIThread();

            if (GeneralOptionsDto.ShowReadMeDotTxt)
            {
                var textFile = Path.Combine(SolutionFolder, CommonConstants.ReadMeDotTxt);
                textFileDtos.Add(new TextFileDto {FileName = textFile});
            }
        }

        private void SolutionOpenPopUpDotTxtHandler()
        {
            //ThreadHelper.ThrowIfNotOnUIThread();

            if (GeneralOptionsDto.ShowSolutionOpenPopUpDotTxt)
            {
                var textFile = Path.Combine(SolutionFolder, CommonConstants.SolutionOpenPopUpDotTxt);
                textFileDtos.Add(new TextFileDto {FileName = textFile});
            }
        }

        private string GetPopUpBody(IEnumerable<TextFileDto> textFileDtos)
        {
            //ThreadHelper.ThrowIfNotOnUIThread();

            var popUpBody = string.Empty;

            foreach (var textFileDto in textFileDtos)
            {
                ReadAllLines(textFileDto);
                textFileDto.AllLines = PackageHelper.GetTruncatedIndividualLines(textFileDto.AllLines, GeneralOptionsDto.LineLengthTruncationLimit);
            }

            PackageHelper.CalculateOverallLinesToShow(textFileDtos, GeneralOptionsDto.OverallLinesLimit);

            foreach (var textFileDto in textFileDtos)
            {
                popUpBody += GetPopUpMessage(textFileDto);
            }

            return popUpBody;
        }
    
        private void ReadAllLines(TextFileDto textFileDto)
        {
            //ThreadHelper.ThrowIfNotOnUIThread();

            if (!string.IsNullOrEmpty(textFileDto.FileName))
            {
                if (File.Exists(textFileDto.FileName))
                {
                    textFileDto.FileExists = true;
                    textFileDto.AllLines = File.ReadAllLines(textFileDto.FileName);
                    textFileDto.SourceControlStatus = dte.SourceControl.IsItemUnderSCC(textFileDto.FileName);
                }
                else
                {
                    textFileDto.FileExists = false;
                }
            }
            else
            {
                textFileDto.FileExists = false;
            }
        }

        private string GetPopUpMessage(TextFileDto textFileDto)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(textFileDto.FileName))
            {
                if (textFileDto.FileExists)
                {
                    var linesToUse = textFileDto.AllLines.Take(textFileDto.MaxLinesToShow);
                    var toUse = linesToUse as IList<string> ?? linesToUse.ToList();
                    SetPopUpBodyIsPopulated(toUse);
                    var linesToUseJoined = string.Join(Environment.NewLine, toUse);
                    linesToUseJoined.TrimPrefix(Environment.NewLine);
                    result += linesToUseJoined;

                    var sourceControlStatus = textFileDto.SourceControlStatus ? "is" : "is not";
                    popUpFooter += bulletPoint + textFileDto.FileName + " (file " + sourceControlStatus + " under source control)";
                    popUpFooter += Environment.NewLine;
                }
                else
                {
                    popUpFooter += bulletPoint + textFileDto.FileName + " not found.";
                    popUpFooter += Environment.NewLine;
                }

                result += Environment.NewLine;
                result += Environment.NewLine;
            }

            return result;
        }

        private void SetPopUpBodyIsPopulated(IList<string> linesToUse)
        {
            if (!popUpBodyIsPopulated &&
                (linesToUse.Any(x => !string.IsNullOrEmpty(x)) ||
                linesToUse.Any(x => !string.IsNullOrWhiteSpace(x))))
            {
                popUpBodyIsPopulated = true;
            }
        }

        private string GetPopUpFooter()
        {
            string result = null;

            if (!string.IsNullOrEmpty(popUpFooter))
            {
                result +=  Vsix.Name + "   " + Vsix.Version + "   " + marketplaceUrl;
                result += Environment.NewLine;
                result += popUpFooter;
            }

            return result;
        }

        private void DisplayPopUpMessage(string popUpTitle, string popUpBody)
        {
            //ThreadHelper.ThrowIfNotOnUIThread();
            ChaseRating();

            if (!string.IsNullOrEmpty(popUpBody))
            {
                IVsUIShell uiShell = (IVsUIShell)GetService(typeof(SVsUIShell));
                Assumes.Present(uiShell);
                Guid clsid = Guid.Empty;

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
                    out int result);
            }
        }

        private GeneralOptionsDto GeneralOptionsDto
        {
            get
            {
                var generalOptions = (GeneralOptions)GetDialogPage(typeof(GeneralOptions));
                return new GeneralOptionsDto
                {
                    LineLengthTruncationLimit = generalOptions.LineLengthTruncationLimit,
                    OverallLinesLimit = generalOptions.OverallLinesLimit,
                    ShowFileNamesInPopUp = generalOptions.ShowFileNamesInPopUp,
                    ShowReadMeDotTxt = generalOptions.ShowReadMeDotTxt,
                    ShowSolutionOpenPopUpDotTxt = generalOptions.ShowSolutionOpenPopUpDotTxt,
                };
            }
        }

        private string SolutionFolder
        {
            get
            {
                //ThreadHelper.ThrowIfNotOnUIThread();

                if (string.IsNullOrEmpty(solutionFolder))
                {
                    var solutionPath = dte.Solution.FullName;
                    solutionFolder = Path.GetDirectoryName(solutionPath);
                }
                return solutionFolder;
            }
        }

        private void ChaseRating()
        {
            var hiddenChaserOptions = (IRatingDetailsDto)GetDialogPage(typeof(HiddenRatingDetailsDto));
            var packageRatingChaser = new PackageRatingChaser();
            packageRatingChaser.Hunt(hiddenChaserOptions);
        }
    }
}
