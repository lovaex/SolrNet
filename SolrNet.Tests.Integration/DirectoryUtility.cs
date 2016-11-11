using System;
using System.IO;
using System.Reflection;

namespace SolrNet.Tests.Integration
{
    public static class DirectoryUtility
    {
        private const string TestFilePath = @"..\..\FileForTests\test.pdf";
        private const string XmlFilespath = @"..\..\FileForTests\exampledocs";
        private const string IstanceDir = @"..\..\..\";
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
            var assemblyPath = AssemblyDirectory;

            return Path.Combine(assemblyPath, TestFilePath);
        }

        public static string GetDirectoryXmlFile()
        {
            var assemblyPath = AssemblyDirectory;

            return Path.Combine(assemblyPath, XmlFilespath);
        }

        public static string GetIstanceDirectory()
        {
            var assemplyPath = AssemblyDirectory;
            return string.Empty;
        }
    }
}
