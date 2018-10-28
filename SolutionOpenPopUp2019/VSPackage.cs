using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SolutionOpenPopUp.Helpers;
using SolutionOpenPopUp.Helpers.Dtos;
using SolutionOpenPopUp.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using SolutionEvents = Microsoft.VisualStudio.Shell.Events.SolutionEvents;
using Task = System.Threading.Tasks.Task;

namespace SolutionOpenPopUp
{
    [Guid("61eadc52-5677-4548-b273-08f1e6574f71")]
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("Solution Load Sample", "Demonstrates use of solution load events", "1.0")]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionOpening_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideOptionPage(typeof(DialogPageProvider.General), Vsix.Name, "General", 0, 0, true)]
    public sealed class VSPackage : AsyncPackage
    {
        private string bulletPoint = " - ";
        private bool popUpBodyIsPopulated;
        private string popUpFooter;
        private string solutionFolder;
        private List<TextFileDto> textFileDtos;

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            bool isSolutionLoaded = await IsSolutionLoadedAsync();

            if (isSolutionLoaded)
            {
                await HandleOpenSolutionAsync();
            }

            SolutionEvents.OnAfterOpenSolution += HandleOpenSolution;
        }

        private async Task<bool> IsSolutionLoadedAsync()
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();

            var solService = await GetServiceAsync(typeof(SVsSolution)) as IVsSolution;
            ErrorHandler.ThrowOnFailure(solService.GetProperty((int)__VSPROPID.VSPROPID_IsSolutionOpen, out object value));
            ErrorHandler.ThrowOnFailure(solService.GetProperty((int)__VSPROPID.VSPROPID_SolutionDirectory, out object solutionDirectoryObject));
            solutionFolder = (string)solutionDirectoryObject;

            return value is bool isSolOpen && isSolOpen;
        }

        private void HandleOpenSolution(object sender = null, EventArgs e = null)
        {
            Task.Run(async () => await HandleOpenSolutionAsync());
        }

        private async Task HandleOpenSolutionAsync(object sender = null, EventArgs e = null)
        {
            // Handle the open solution and try to do as much work on a background thread as possible
            popUpBodyIsPopulated = false;
            popUpFooter = string.Empty;
            textFileDtos = new List<TextFileDto>();

            var generalOptionsDto = await GetGeneralOptionsDtoAsync();//GetGeneralOptionsDto();

            SolutionOpenPopUpDotTxtHandler(generalOptionsDto);
            ReadMeDotTxtHandler(generalOptionsDto);

            var popUpBody = await GetPopUpBodyAsync(textFileDtos, generalOptionsDto);

            if (popUpBodyIsPopulated)
            {
                if (generalOptionsDto.ShowFileNamesInPopUp)
                {
                    popUpBody += GetPopUpFooter();
                }

                await DisplayPopUpMessageAsync(string.Empty, popUpBody);
            }
        }

        private void ReadMeDotTxtHandler(GeneralOptionsDto generalOptionsDto)
        {
            if (generalOptionsDto.ShowReadMeDotTxt)
            {
                var textFile = Path.Combine(solutionFolder, CommonConstants.ReadMeDotTxt);
                textFileDtos.Add(new TextFileDto { FileName = textFile });
            }
        }

        private void SolutionOpenPopUpDotTxtHandler(GeneralOptionsDto generalOptionsDto)
        {
            if (generalOptionsDto.ShowSolutionOpenPopUpDotTxt)
            {
                var textFile = Path.Combine(solutionFolder, CommonConstants.SolutionOpenPopUpDotTxt);
                textFileDtos.Add(new TextFileDto { FileName = textFile });
            }
        }

        private async Task<string> GetPopUpBodyAsync(IEnumerable<TextFileDto> textFileDtos, GeneralOptionsDto generalOptionsDto)
        {
            var popUpBody = string.Empty;

            foreach (var textFileDto in textFileDtos)
            {
                await ReadAllLinesAsync(textFileDto);
                textFileDto.AllLines = PackageHelper.GetTruncatedIndividualLines(textFileDto.AllLines, generalOptionsDto.LineLengthTruncationLimit);
            }

            PackageHelper.CalculateOverallLinesToShow(textFileDtos, generalOptionsDto.OverallLinesLimit);

            foreach (var textFileDto in textFileDtos)
            {
                popUpBody += GetPopUpMessage(textFileDto);
            }

            return popUpBody;
        }

        private async Task ReadAllLinesAsync(TextFileDto textFileDto)
        {
            if (!string.IsNullOrEmpty(textFileDto.FileName))
            {
                if (File.Exists(textFileDto.FileName))
                {
                    textFileDto.FileExists = true;
                    textFileDto.AllLines = File.ReadAllLines(textFileDto.FileName);
                    var dte = await GetServiceAsync(typeof(DTE)) as DTE2;
                    Assumes.Present(dte);
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
            if (!popUpBodyIsPopulated)
            {
                if (linesToUse.Any(x => !string.IsNullOrEmpty(x)) ||
                    linesToUse.Any(x => !string.IsNullOrWhiteSpace(x)))
                {
                    popUpBodyIsPopulated = true;
                }
            }
        }

        private string GetPopUpFooter()
        {
            string result = null;

            if (!string.IsNullOrEmpty(popUpFooter))
            {
                //var url = "https://marketplace.visualstudio.com/items?itemName=GregTrevellick.SolutionOpenPopUp";
                var shortUrl = "https://goo.gl/aGVjJ8";
                result += Vsix.Name + "   " + Vsix.Version + "   " + shortUrl;
                result += Environment.NewLine;
                result += popUpFooter;
            }

            return result;
        }

        //private GeneralOptionsDto GetGeneralOptionsDto()
        //{
        //    Task<GeneralOptionsDto> task = Task.Run<GeneralOptionsDto>(async () => await GetGeneralOptionsDtoAsync());
        //    return task.Result;
        //}

        private async Task<GeneralOptionsDto> GetGeneralOptionsDtoAsync()
        {
            // Call from a background thread to avoid blocking the UI thread
            //var generalOptions = GeneralOptions.Instance;

            // Call from a background thread to avoid blocking the UI thread
            var generalOptions = await GeneralOptions.GetLiveInstanceAsync();

            return new GeneralOptionsDto
            {
                LineLengthTruncationLimit = generalOptions.LineLengthTruncationLimit,
                OverallLinesLimit = generalOptions.OverallLinesLimit,
                ShowFileNamesInPopUp = generalOptions.ShowFileNamesInPopUp,
                ShowReadMeDotTxt = generalOptions.ShowReadMeDotTxt,
                ShowSolutionOpenPopUpDotTxt = generalOptions.ShowSolutionOpenPopUpDotTxt,
            };
        }

        private async Task DisplayPopUpMessageAsync(string popUpTitle, string popUpBody)
        {
            if (!string.IsNullOrEmpty(popUpBody))
            {
                IVsUIShell uiShell = await GetServiceAsync(typeof(SVsUIShell)) as IVsUIShell;
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
    }
}
