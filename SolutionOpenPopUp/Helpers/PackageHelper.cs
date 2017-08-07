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
            //textFileDtos.1.lines = 200
            //textFileDtos.2.lines = 300
            //textFileDtos.3.lines = 500
            //              TOTAL   1000
            //
            //overallLinesLimit = 100
            //should be
            // 1 = (200/1000) * 100
            // 2 = (300/1000) * 100
            // 3 = (500/1000) * 100

            var allLinesTotal = textFileDtos.Sum(x => x.AllLines.Length);

            foreach (var textFileDto in textFileDtos)
            {
                //if (textFileDto.AllLines.Length > overallLinesLimit)
                //{
                //    textFileDto.MaxLinesToShow = 3;//TODO calculater this properly as a %
                //}
                //else
                //{
                //    textFileDto.MaxLinesToShow = textFileDto.AllLines.Length;
                //}    
                textFileDto.MaxLinesToShow = (textFileDto.AllLines.Length / allLinesTotal) * overallLinesLimit;
                //                                                   200            1000                 100
            }
        }      
    }
}
