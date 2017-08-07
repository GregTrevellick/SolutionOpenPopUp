using System;
using SolutionOpenPopUp.Helpers.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace SolutionOpenPopUp.Helpers
{
    public class PackageHelper
    {
        public static string[] GetTruncatedIndividualLines(string[] allLines, int lineLengthTruncationLimit)
        {
            var allTruncatedLines = new List<string>();

            foreach (var line in allLines)
            {
                if (line.Length > lineLengthTruncationLimit)
                {
                    allTruncatedLines.Add(line.Substring(0, lineLengthTruncationLimit) + "...");
                }
                else
                {
                    allTruncatedLines.Add(line);
                }
            }

            return allTruncatedLines.ToArray();
        }

        public static void CalculateOverallLinesToShow(IEnumerable<TextFileDto> textFileDtos, int overallLinesLimit)
        {
            var allLinesTotal = textFileDtos.Sum(x => x.AllLines.Length);

            foreach (var textFileDto in textFileDtos)
            {
                var a = textFileDto.AllLines.Length /(double) allLinesTotal;
                var b = a * overallLinesLimit;
                var c = (int) Math.Round(b);
                textFileDto.MaxLinesToShow = c;
            }
        }      
    }
}
