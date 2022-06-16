using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Linq;
using TemplateGenerator.Models;
using TemplateGenerator.Helpers;
using TemplateGenerator.Extensions;
using System.Collections.Generic;

namespace TemplateGenerator
{
    class Program
    {
        public static string TemplatePath { get; set; }
        public static string OutputPath { get; set; }
        public static List<TextMutationMigrations> TextMutationMigrations { get; set;}
        public static string[] MigrationExclusions { get; set; }

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot config = builder.Build();
            TemplatePath = config["Paths:TemplatePath"].ToNonTralingSlashString();
            OutputPath = config["Paths:OutputPath"].ToNonTralingSlashString();
            MigrationExclusions = config.GetSection("Settings:MigrationExclusions").Get<string[]>();
            TextMutationMigrations = config.GetSection("Settings:TextMutationMigrations").Get<List<TextMutationMigrations>>();

            TextMutationMigrations = TextMutationMigrations;

            try
            {

                var templateDirectoryInfo = new DirectoryInfo(TemplatePath);
                var newSubDirectoryFullPath = $"{OutputPath}";
                MakeSureDirectoryPathExists(new DirectoryInfo(TemplatePath), TextMutationMigrations, newSubDirectoryFullPath);
                MergeTemplates(templateDirectoryInfo, TextMutationMigrations, newSubDirectoryFullPath);
                
                ScreenRenderer.Write("Template Generation Complete", $"Please check your configured output path {OutputPath} for the files which were generated.");
                Console.ReadKey();
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

        static void MergeTemplates(DirectoryInfo root, List<TextMutationMigrations> textFileMutations, string outputPath)
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
                    if (MigrationExclusions != null && !MigrationExclusions.Any(x => sourceFile.FullName.EndsWith(x, StringComparison.Ordinal)))
                    {
                        GenerateTargetFile(sourceFile, textFileMutations, outputPath);
                    }
                }
            }

            subDirs = root.GetDirectories();

            if (subDirs != null)
            {
                foreach (DirectoryInfo dirInfo in subDirs)
                {
                    if (MigrationExclusions != null && !MigrationExclusions.Any(x => dirInfo.FullName.EndsWith(x, StringComparison.Ordinal)))
                    {
                        MakeSureDirectoryPathExists(dirInfo, textFileMutations, outputPath);
                        MergeTemplates(dirInfo, textFileMutations, outputPath);
                    }
                }
            }
        }
        
        static void MakeSureDirectoryPathExists(DirectoryInfo dirInfo, List<TextMutationMigrations> textFileMutations, string outputPath)
        {
            var relativeTargetDirPath = dirInfo.FullName.Replace(new DirectoryInfo(TemplatePath).FullName, string.Empty);
            foreach (var item in textFileMutations)
            {
                relativeTargetDirPath = relativeTargetDirPath.Replace(item.Source, item.Target, StringComparison.Ordinal);
            }
            
            try
            {
                Directory.CreateDirectory(outputPath + relativeTargetDirPath);
            }
            catch (UnauthorizedAccessException e)
            {
                ScreenRenderer.WriteError("Unauthorized: Unable to create directory", e.Message);

            }
            catch (Exception e)
            {
                ScreenRenderer.WriteError("Unknown Error: Unable to create directory", e.Message);
            }

        }
        
        static void GenerateTargetFile(FileInfo sourceFile, List<TextMutationMigrations> textFileMutations, string outputPath)
        {
            var targetFileName = sourceFile.FullName.Replace(new DirectoryInfo(TemplatePath).FullName, outputPath, StringComparison.Ordinal);
            //if (BinaryMigrations != null && BinaryMigrations.Any(x => targetFileName.EndsWith(x, StringComparison.Ordinal)))
            //{
            //    GenerateFile(File.ReadAllBytes(sourceFile.FullName), targetFileName);
            //}
            //else
            //{
                foreach (var item in textFileMutations) {
                    targetFileName = targetFileName.Replace(item.Source, item.Target, StringComparison.Ordinal);
                }

                GenerateFile(File.ReadAllText(sourceFile.FullName), targetFileName, textFileMutations);
            //}
        }

        static void GenerateFile(string sourceFileText, string outputFileName, List<TextMutationMigrations> textFileMutations)
        {
            var targetFileText = sourceFileText;
            foreach (var item in textFileMutations)
            {
                while (targetFileText.IndexOf(item.Source, StringComparison.Ordinal) > -1)
                {
                    targetFileText = targetFileText.Replace(item.Source, item.Target, StringComparison.Ordinal);
                }
            }
          
            File.WriteAllText(outputFileName, targetFileText);


        }

        static void GenerateFile(byte[] sourceBytes, string outputFileName)
        {
            File.WriteAllBytes(outputFileName, sourceBytes);
        }
    }
}
