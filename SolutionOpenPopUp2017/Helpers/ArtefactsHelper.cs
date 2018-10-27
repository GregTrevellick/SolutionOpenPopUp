using System.IO;

namespace SolutionOpenPopUp.Helpers
{
    public static class ArtefactsHelper
    {
        public static bool DoesFileExist(string fullArtefactName)
        {
            var result = true;
           
            if (string.IsNullOrEmpty(fullArtefactName))
            {
                result = false;
            }
            else
            {
                    if (!File.Exists(fullArtefactName))
                    {
                        result = false;
                    }
            }

            return result;
        }
    }
}
