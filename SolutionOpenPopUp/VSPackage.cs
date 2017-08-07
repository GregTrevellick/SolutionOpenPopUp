﻿using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using SolutionOpenPopUp.Helpers;
using SolutionOpenPopUp.Helpers.Dtos;
using SolutionOpenPopUp.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
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
        private int overallLinesLimit = 35;//gregt put into options
        private int lineLengthTruncationLimit = 100;//gregt put into options
        private string solutionFolder;
        private List<TextFileDto> textFileDtos;

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
            textFileDtos = new List<TextFileDto>();
            popUpFooter = string.Empty;

            SolutionOpenPopUpDotTxtHandler();

            ShowReadMeDotTxtHandler();

            var popUpBody = GetPopUpBody(textFileDtos);

            popUpBody += GetPopUpFooter();

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
            var popUpBody = string.Empty;

            foreach (var textFileDto in textFileDtos)
            {
                ReadAllLines(textFileDto);
                textFileDto.AllLines = PackageHelper.GetTruncatedIndividualLines(textFileDto.AllLines, lineLengthTruncationLimit);
            }

            PackageHelper.CalculateOverallLinesToShow(textFileDtos, overallLinesLimit);

            foreach (var textFileDto in textFileDtos)
            {
                popUpBody += GetPopUpMessage(textFileDto);
            }

            return popUpBody;
        }
    
        private void ReadAllLines(TextFileDto textFileDto)
        {
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
                    var linesToUseJoined = string.Join(Environment.NewLine, linesToUse);
                    linesToUseJoined.TrimPrefix(Environment.NewLine);
                    result += linesToUseJoined;

                    var sourceControlStatus = textFileDto.SourceControlStatus ? "is" : "is not";
                    popUpFooter += bulletPoint + textFileDto.FileName + " (file " + sourceControlStatus + " under source control)";
                    popUpFooter += Environment.NewLine;
                }
                else
                {
                    popUpFooter += bulletPoint + textFileDto + " not found.";
                    popUpFooter += Environment.NewLine;
                }

                result += Environment.NewLine;
                result += Environment.NewLine;
            }

            return result;
        }

        private string GetPopUpFooter()
        {
            string result = null;

            if (!string.IsNullOrEmpty(popUpFooter))
            {
                var shortUrl = "https://goo.gl/aGVjJ8";
                result +=  Vsix.Name + "   " + Vsix.Version + "   " + shortUrl;
                result += Environment.NewLine;
                //var url = "https://marketplace.visualstudio.com/items?itemName=GregTrevellick.SolutionOpenPopUp";
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
