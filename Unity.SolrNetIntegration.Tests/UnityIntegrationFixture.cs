// 

using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using SolrNet;
using Unity.SolrNetIntegration.Config;

namespace Unity.SolrNetIntegration.Tests {
    [TestFixture]
    public class UnityIntegrationFixture {
        internal static readonly SolrServers TestServers = new SolrServers {
            new SolrServerElement {
                Id = "entity",
                DocumentType = typeof(Entity).AssemblyQualifiedName,
                Url = "http://localhost:8983/solr/core0",
            },
            new SolrServerElement {
                Id = "entity2Dict",
                DocumentType = typeof(Dictionary<string, object>).AssemblyQualifiedName,
                Url = "http://localhost:8983/solr/core1",
            },
            new SolrServerElement {
                Id = "entity2",
                DocumentType = typeof(Entity2).AssemblyQualifiedName,
                Url = "http://localhost:8983/solr/core1",
            },
        };


        private void DictionaryDocumentAdd() {
            using (var container = new UnityContainer()) {
                new SolrNetContainerConfiguration().ConfigureContainer(TestServers, container);

                var solr = container.Resolve<ISolrOperations<Dictionary<string, object>>>();

                solr.Add(new Dictionary<string, object> {
                    {"id", "5"},
                    {"manu", "who knows"},
                    {"popularity", 55},
                    {"timestamp", DateTime.UtcNow},
                });
                solr.Commit();
            }
        }

        [Test]
        public void DictionaryDocument() {
            DictionaryDocumentAdd();
            using (var container = new UnityContainer()) {
                new SolrNetContainerConfiguration().ConfigureContainer(TestServers, container);
                var solr = container.Resolve<ISolrOperations<Entity2>>();
                var results = solr.Query(SolrQuery.All);
                Assert.That(results.Count, Is.GreaterThan(0));
            }
        }

        [Test]
        public void Ping_And_Query() {
            using (var container = UnityFixture.SetupContainer()) {
                var solr = container.Resolve<ISolrOperations<Entity>>();
                solr.Ping();
                var count = solr.Query(SolrQuery.All).Count;
                Assert.That(count, Is.GreaterThan(0));
            }
        }
    }
}