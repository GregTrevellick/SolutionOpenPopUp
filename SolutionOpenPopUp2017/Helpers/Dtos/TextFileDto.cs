namespace SolutionOpenPopUp.Helpers.Dtos
{
    public class TextFileDto
    {
        public string FileName { get; set; }
        public int MaxLinesToShow { get; set; }
        public string[] AllLines { get; set; }
        public bool SourceControlStatus;
        public bool FileExists;
    }
}
