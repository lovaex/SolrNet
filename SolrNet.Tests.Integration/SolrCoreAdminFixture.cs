using System;
using System.Configuration;
using NUnit.Framework;
using SolrNet.Commands.Cores;
using SolrNet.Impl;
using SolrNet.Impl.ResponseParsers;

namespace SolrNet.Tests.Integration
{
    [TestFixture]
    public class SolrCoreAdminFixture
    {
        #region Action
        /*
        CREATE
        http://localhost:8983/solr/admin/cores?action=CREATE&name=core0&instanceDir=path_to_instance_directory

        RENAME
        http://localhost:8983/solr/admin/cores?action=RENAME&core=core0&other=core5

        RELOAD
        http://localhost:8983/solr/admin/cores?action=RELOAD&core=core0

        ALIAS
        http://localhost:8983/solr/admin/cores?action=ALIAS&core=core0&other=corefoo

        SWAP
        http://localhost:8983/solr/admin/cores?action=SWAP&core=core1&other=core0
         
        STATUS
        http://localhost:8983/solr/admin/cores?action=STATUS&core=core0
        
        UNLOAD
        http://localhost:8983/solr/admin/cores?action=UNLOAD&core=core0
        */
        #endregion

        private static readonly string SolrUrl = ConfigurationManager.AppSettings["solrBase"];
        private static readonly string CoreNew = "core-new";
        private static readonly string CoreSwap = "core-swap";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            DirectoryUtility.CreateCoreFolder(CoreNew);
            DirectoryUtility.CreateCoreFolder(CoreSwap);
        }

        [Test]
        public void GetStatusForAllCores()
        {
            var solrCoreAdmin = new SolrCoreAdmin(new SolrConnection(SolrUrl), GetHeaderParser(), GetStatusResponseParser());

            var results = solrCoreAdmin.Status();
            Assert.IsNotEmpty(results);
        }

        [Test]
        public void Create()
        {

            var solrCoreAdmin = new SolrCoreAdmin(new SolrConnection(SolrUrl), GetHeaderParser(), GetStatusResponseParser());
            var status = solrCoreAdmin.Status(CoreNew);
            if (!string.IsNullOrEmpty(status.Name))
                solrCoreAdmin.Unload(CoreNew);
            try
            {
                var createResponseHeader = solrCoreAdmin.Create(CoreNew, null, null, null, null);
            }
            catch (ArgumentException)
            {
                // Should get an Exception here because instance directory was not specified.
                var istanceDir = DirectoryUtility.GetCoreIstanceDirectory(CoreNew);
                var createResponseHeader = solrCoreAdmin.Create(CoreNew, istanceDir);
                Assert.AreEqual(createResponseHeader.Status, 0);
            }

            var result = solrCoreAdmin.Status(CoreNew);
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Name);
            Assert.AreEqual(CoreNew, result.Name);
        }

        [Test]
        public void GetStatusForNamedCore()
        {
            var solrCoreAdmin = new SolrCoreAdmin(new SolrConnection(SolrUrl), GetHeaderParser(), GetStatusResponseParser());

            var result = solrCoreAdmin.Status(CoreNew);
            Assert.IsNotEmpty(result.Name);
            Assert.AreEqual(CoreNew, result.Name);
        }

        [Test]
        public void ReloadCore()
        {
            var solrCoreAdmin = new SolrCoreAdmin(new SolrConnection(SolrUrl), GetHeaderParser(), GetStatusResponseParser());

            var reloadResponseHeader = solrCoreAdmin.Reload(CoreNew);
            Assert.AreEqual(reloadResponseHeader.Status, 0);
        }

        [Test, Ignore("Obsolete method")]
        public void Alias()
        {
            var istanceDir = DirectoryUtility.GetCoreIstanceDirectory(CoreNew);
            var solrCoreAdmin = new SolrCoreAdmin(new SolrConnection(SolrUrl), GetHeaderParser(), GetStatusResponseParser());
            var status = solrCoreAdmin.Status(CoreNew);
            if (string.IsNullOrEmpty(status.Name))
            {
                solrCoreAdmin.Create(CoreNew, istanceDir);
            }


            var aliasResponseHeader = solrCoreAdmin.Alias(CoreNew, "corefoo");
            Assert.AreEqual(aliasResponseHeader.Status, 0);
        }

        [Test]
        public void CreateSwapCore()
        {
            var istanceDir = DirectoryUtility.GetCoreIstanceDirectory(CoreSwap);
            
            var solrCoreAdmin = new SolrCoreAdmin(new SolrConnection(SolrUrl), GetHeaderParser(), GetStatusResponseParser());

            var createResponseHeader = solrCoreAdmin.Create(CoreSwap, istanceDir);
            Assert.AreEqual(createResponseHeader.Status, 0);
            var result = solrCoreAdmin.Status(CoreSwap);
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.Name);
            Assert.AreEqual(CoreSwap, result.Name);
        }

        [Test]
        public void SwapCores()
        {
            var solrCoreAdmin = new SolrCoreAdmin(new SolrConnection(SolrUrl), GetHeaderParser(), GetStatusResponseParser());

            var swapResponseHeader = solrCoreAdmin.Swap(CoreNew, CoreSwap);
            Assert.AreEqual(swapResponseHeader.Status, 0);
        }

        [Test]
        public void Unload()
        {
            var solrCoreAdmin = new SolrCoreAdmin(new SolrConnection(SolrUrl), GetHeaderParser(), GetStatusResponseParser());

            var swapUnloadResponseHeader = solrCoreAdmin.Unload(CoreSwap, UnloadCommand.Delete.Index);
            Assert.AreEqual(swapUnloadResponseHeader.Status, 0);

            var newUnloadResponseHeader = solrCoreAdmin.Unload(CoreNew, UnloadCommand.Delete.Index);
            Assert.AreEqual(newUnloadResponseHeader.Status, 0);
        }

        private HeaderResponseParser<string> GetHeaderParser()
        {
            return new HeaderResponseParser<string>();
        }

        private SolrStatusResponseParser GetStatusResponseParser()
        {
            return new SolrStatusResponseParser();
        }
    }
}