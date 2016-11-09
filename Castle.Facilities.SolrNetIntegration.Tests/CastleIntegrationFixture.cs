using System;
using System.Collections.Generic;
using Castle.Core.Configuration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NUnit.Framework;
using SolrNet;

namespace Castle.Facilities.SolrNetIntegration.Tests {
    [TestFixture]
    public class CastleIntegrationFixture {
        private string solrServiceUrl = "http://localhost:8983/solr/core0";

        [Test]
        public void Ping_Query()
        {
            var configStore = new DefaultConfigurationStore();
            var configuration = new MutableConfiguration("facility");
            configuration.Attribute("type", typeof(SolrNetFacility).AssemblyQualifiedName);
            configuration.CreateChild("solrURL", solrServiceUrl);
            configStore.AddFacilityConfiguration(typeof(SolrNetFacility).FullName, configuration);
            var container = new WindsorContainer(configStore);

            var solr = container.Resolve<ISolrOperations<CastleFixture.Document>>();
            solr.Ping();
            Console.WriteLine(solr.Query(SolrQuery.All).Count);
        }

        [Test]
        public void DictionaryDocument()
        {
            var solrFacility = new SolrNetFacility(solrServiceUrl);
            var container = new WindsorContainer();
            container.AddFacility("solr", solrFacility);
            var solr = container.Resolve<ISolrOperations<Dictionary<string, object>>>();
            var results = solr.Query(SolrQuery.All);
            Assert.Greater(results.Count, 0);
            foreach (var d in results)
            {
                Assert.Greater(d.Count, 0);
                foreach (var kv in d)
                    Console.WriteLine("{0}: {1}", kv.Key, kv.Value);
            }
        }

        [Test]
        public void DictionaryDocument_add()
        {
            var solrFacility = new SolrNetFacility(solrServiceUrl);
            var container = new WindsorContainer();
            container.AddFacility(solrFacility);
            var solr = container.Resolve<ISolrOperations<Dictionary<string, object>>>();
            solr.Add(new Dictionary<string, object> {
                {"id", "ababa"},
                {"manu", "who knows"},
                {"popularity", 55},
                {"timestamp", DateTime.UtcNow},
            });
        }
    }
}