using SolutionOpenPopUp.Helpers.Dtos;
using System.Collections.Generic;

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
            foreach (var textFileDto in textFileDtos)
            {
                if (textFileDto.AllLines.Length > overallLinesLimit)
                {
                    textFileDto.MaxLinesToShow = 3;//TODO calculater this properly as a %
                }
                else
                {
                    textFileDto.MaxLinesToShow = textFileDto.AllLines.Length;
                }    
            }
        }      
    }
}
