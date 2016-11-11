using System;
using System.IO;
using System.Reflection;

namespace SolrNet.Tests.Integration
{
    public static class DirectoryUtility
    {
        private const string TestFilePath = @"..\..\FileForTests\test.pdf";
        private const string XmlFilespath = @"..\..\FileForTests\exampledocs";
        private const string IstanceDir = @"..\..\..\solr_5.5.3\server\solr";
        private const string ConfigFilePath = @"..\..\..\solr_5.5.3\core_template";
        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static string GetDirectoryTestFile()
        {
            return Path.Combine(AssemblyDirectory, TestFilePath);
        }

        public static string GetDirectoryXmlFile()
        {
            return Path.Combine(AssemblyDirectory, XmlFilespath);
        }

        public static string GetConfigurationFiles()
        {
            return Path.Combine(AssemblyDirectory, ConfigFilePath);
        }

        public static void CreateCoreFolder(string coreName)
        {
            var coreFolder = Path.Combine(GetIstanceDirectory(), coreName);
            var configFilePath = Path.Combine(AssemblyDirectory, ConfigFilePath);
            if (!Directory.Exists(coreFolder))
                CopyDirectory(configFilePath, coreFolder);
        }

        public static string GetCoreIstanceDirectory(string coreName)
        {
            return Path.Combine(GetIstanceDirectory(), coreName);
        }

        public static void DeleteCoreFolder(string coreName)
        {
            var coreFolder = Path.Combine(GetIstanceDirectory(), coreName);
            if (Directory.Exists(coreFolder))
            {
                Directory.Delete(coreFolder, true);
            }
        }

        private static string GetIstanceDirectory()
        {
            return Path.Combine(AssemblyDirectory, IstanceDir);
        }

        private static void CopyDirectory(string folderName, string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }

            foreach (string file in Directory.GetFiles(folderName))
            {
                string dest = Path.Combine(destination, Path.GetFileName(file));
                File.Copy(file, dest);
            }

            foreach (string folder in Directory.GetDirectories(folderName))
            {
                string dest = Path.Combine(destination, Path.GetFileName(folder));
                CopyDirectory(folder, dest);
            }
        }
    }
}
