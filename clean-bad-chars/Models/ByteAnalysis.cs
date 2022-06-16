using System;
using System.Collections.Generic;

namespace TemplateGenerator.Models
{
    public class ByteAnalysis
    {
        public Guid FileId { get; set; }

        public List<ByteData> ByteData { get; set; }
    }
}
