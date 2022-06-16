using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Linq;
using TemplateGenerator.Models;
using TemplateGenerator.Helpers;
using System.Collections.Generic;

namespace TemplateGenerator
{
    class Program
    {
        public static string TargetPath { get; set; }

        public static int MaxByteCode { get; set; }

        public static List<SourceFileData> SourceFiles { get; set; }

        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    throw new Exception("Please specify path");
                }

                var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                IConfigurationRoot config = builder.Build();
                MaxByteCode = 127;
                TargetPath = args[0];
                SourceFiles = new List<SourceFileData>();
                ProcessDirectory(new DirectoryInfo(TargetPath));

                var binaryFiles = SourceFiles
                    .Where(x => x.ByteCounts.HighCodeByteCount > 0 && x.ByteCounts.StandardByteCount / x.ByteCounts.HighCodeByteCount > 0.01).ToList();
                var sourceFiles = SourceFiles
                    .Where(x => (x.ByteCounts.HighCodeByteCount == 0 ||
                        (x.ByteCounts.HighCodeByteCount > 0 && x.ByteCounts.StandardByteCount / x.ByteCounts.HighCodeByteCount <= 0.01)) && x.Size > 1024).ToList();

                var byteDataList = new List<ByteData>();
                var byteAnalysisList = new List<ByteAnalysis>();

                foreach (var item in sourceFiles)
                {
                    var byteAnalysis = new ByteAnalysis();
                    byteAnalysis.FileId = item.FileId;
                    byteAnalysis.ByteData
                        = FileChecker.GetOffenderData(MaxByteCode, item.FilePath);
                    byteAnalysisList.Add(byteAnalysis);
                }

                foreach (var item in sourceFiles.Where(x => byteAnalysisList.Where(y => y.FileId == x.FileId && y.ByteData.Any()).Any()))
                {
                    Console.WriteLine($"FileName: {item.FileName} Size: {(item.Size / 1024).ToString()} Kb --> " +
                        $"High# {item.ByteCounts.HighCodeByteCount}" +
                        $"Low# {item.ByteCounts.StandardByteCount}");
                    var itemAnalysis = byteAnalysisList.FirstOrDefault(x => item.FileId == x.FileId);
                    if (itemAnalysis != null)
                    {
                        foreach (var byteData in itemAnalysis.ByteData)
                        {
                            Console.WriteLine($"\tByteCode: {byteData.Byte} Index: {byteData.Index} -->");
                        }
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                ScreenRenderer.WriteError("Unauthorized", e.Message);
            }
            catch (FileNotFoundException e)
            {
                ScreenRenderer.WriteError("File Not Found", e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                ScreenRenderer.WriteError("Directory Not Found", e.Message);
            }
            catch (Exception e)
            {
                ScreenRenderer.WriteError("Unknown Error", e.Message);
            }
        }

        static void ProcessDirectory(DirectoryInfo root)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirs = null;

            try
            {
                files = root.GetFiles("*");
            }
            catch (UnauthorizedAccessException e)
            {
                ScreenRenderer.WriteError("Unauthorized: Unable to access template files", e.Message);

            }
            catch (DirectoryNotFoundException e)
            {
                ScreenRenderer.WriteError("Directory Not Found: Unable to access template files", e.Message);
            }
            catch (Exception e)
            {
                ScreenRenderer.WriteError("Unknown Error: Unable to access template files", e.Message);
            }

            if (files != null)
            {
                foreach (FileInfo sourceFile in files)
                {
                    SourceFileData sourceFileData = new SourceFileData();
                    sourceFileData.FileName = sourceFile.Name;
                    sourceFileData.FilePath = sourceFile.FullName;
                    sourceFileData.Size = sourceFile.Length;
                    sourceFileData.DirectoryPath = root.FullName;
                    sourceFileData.FileId = Guid.NewGuid();
                    sourceFileData.ByteCounts = FileChecker.TestByteCount(MaxByteCode, sourceFile.FullName);
                    SourceFiles.Add(sourceFileData);
                }
            }

            subDirs = root.GetDirectories();

            if (subDirs != null)
            {
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    ProcessDirectory(dirInfo);
                }
            }
        }
    }
}
