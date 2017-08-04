using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SolutionOpenPopUp.Helpers;
using SolutionOpenPopUp.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SolutionOpenPopUp.Helpers.Dtos;

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
        private int textLimit = 2000;//gregt put into options
        private int lineLengthTruncationLimit = 20;//gregt put into options
        private string solutionFolder;
        private List<TextFileDto> textFileDtos = new List<TextFileDto>();

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

            SolutionOpenPopUpDotTxtHandler();

            ShowReadMeDotTxtHandler();

            var popUpBody = GetPopUpBody(textFileDtos);

            DisplayPopUpMessage(string.Empty, popUpBody);
        }

        private void ShowReadMeDotTxtHandler()
        {
            if (GeneralOptionsDto.ShowReadMeDotTxt)
            {
                var textFile = Path.Combine(SolutionFolder, CommonConstants.ReadMeDotTxt);
                textFileDtos.Add(new TextFileDto {FileName = textFile});
            }
        }

        private void SolutionOpenPopUpDotTxtHandler()
        {
            if (GeneralOptionsDto.ShowSolutionOpenPopUpDotTxt)
            {
                var textFile = Path.Combine(SolutionFolder, CommonConstants.SolutionOpenPopUpDotTxt);
                textFileDtos.Add(new TextFileDto {FileName = textFile});
            }
        }

        private string GetPopUpBody(IEnumerable<TextFileDto> textFileDtos)
        {
            var result = string.Empty;
            
            foreach (var textFileDto in textFileDtos)
            {
                var textFileIsUnderSourceControl = dte.SourceControl.IsItemUnderSCC(textFileDto.FileName);
                result += GetPopUpMessage(textFileDto.FileName, textFileIsUnderSourceControl);
            }

            result = SetPopUpFooter(result);            

            return result;
        }

        private string GetPopUpMessage(string textFile, bool fileIsUnderSourceControl)
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(textFile))
            {

                if (File.Exists(textFile))
                {
                    //var text = File.ReadAllText(textFile);

                    //if (text.Length > textLimit)
                    //{
                    //    result += text.Substring(0, textLimit);
                    //}
                    //else
                    //{
                    //    result += text;
                    //}

                    var text = File.ReadAllLines(textFile);
                    var linesLimit = 3;

                    if (text.Length > linesLimit)
                    {
                        result += text.Take(linesLimit);
                    }
                    else
                    {
                        result += text;
                    }


                    var sourceControlStatus = fileIsUnderSourceControl ? "is" : "is not";
                    popUpFooter += bulletPoint + textFile + " (file " + sourceControlStatus + " under source control)";
                    popUpFooter += Environment.NewLine;
                }
                else
                {
                    popUpFooter += bulletPoint + textFile + " not found.";
                    popUpFooter += Environment.NewLine;
                }

                result += Environment.NewLine;
            }

            return result;
        }

        private string SetPopUpFooter(string result)
        {
            if (!string.IsNullOrEmpty(popUpFooter))
            {
                result += Environment.NewLine;
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

        private GeneralOptionsDto GeneralOptionsDto
        {
            get
            {
                var generalOptions = (GeneralOptions)GetDialogPage(typeof(GeneralOptions));
                return new GeneralOptionsDto
                {
                    ShowSolutionOpenPopUpDotTxt = generalOptions.ShowSolutionOpenPopUpDotTxt,
                    ShowReadMeDotTxt = generalOptions.ShowReadMeDotTxt
                };
            }
        }

        private string SolutionFolder
        {
            get
            {
                if (string.IsNullOrEmpty(solutionFolder))
                {
                    var solutionPath = dte.Solution.FullName;
                    solutionFolder = Path.GetDirectoryName(solutionPath);
                }
                return solutionFolder;
            }
        } 
    }
}
