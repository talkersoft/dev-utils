using System;
namespace TemplateGenerator.Models
{
    public class SourceFileData
    {
        public Guid FileId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public string DirectoryPath { get; set; }

        public Int64 Size { get; set; }

        public ResultData ByteCounts { get; set; }
    }
}
