using System.IO;
using TemplateGenerator.Models;
using System.Linq;
using System.Collections.Generic;

namespace TemplateGenerator.Helpers
{
    public static class FileChecker
    {
        public static int[] ExceptionList { get; set; }

        public static int[] OffenderList { get; set; }

        static FileChecker()
        {
            ExceptionList = new int[] { 239, 187, 191 };

            OffenderList = new int[] { 13 };
        }

        public static ResultData TestByteCount(int maxByteCode, string filePath)
        {
            var result = new ResultData();
            byte[] bytes = File.ReadAllBytes(filePath);
            for (int i = 0; i < bytes.Length; i++)
            {
                var val = bytes[i];
                if (ExceptionList.ToList().Exists(x => x == val))
                {
                    result.ExceptionByteCount += 1;
                }
                else
                {
                    if (val > maxByteCode)
                    {
                        result.HighCodeByteCount += 1;
                    }

                    if (val <= maxByteCode)
                    {
                        result.StandardByteCount += 1;
                    }
                }
            }

            return result;
        }

        public static List<ByteData> GetOffenderData(int maxCount, string filePath)
        {
            var byteDataList = new List<ByteData>();
            byte[] bytes = File.ReadAllBytes(filePath);
            for (int i = 0; i < bytes.Length; i++)
            {
                var val = bytes[i];
                if (val > maxCount && !ExceptionList.ToList().Exists(x => x == val) || OffenderList.ToList().Exists(x => x == val))
                {
                    var result = new ByteData();
                    result.Byte = val;
                    result.Index = i;
                    byteDataList.Add(result);
                }
            }

            return byteDataList;
        }

    }
}
