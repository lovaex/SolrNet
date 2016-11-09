using System.Collections.Generic;
using System.Configuration;
using SolrNet;
using SolrNet.Exceptions;
using SolrNet.Impl;
using NUnit.Framework;
using StructureMap.SolrNetIntegration.Config;

namespace StructureMap.SolrNetIntegration.Tests
{
    [TestFixture]
    public class StructureMapFixture
    {                
        [Test]
        public void ResolveSolrOperations()
        {
            SetupContainer();
           var m = ObjectFactory.GetInstance<ISolrOperations<Entity>>();
            Assert.IsNotNull(m);            
        }

        [Test]
        public void RegistersSolrConnectionWithAppConfigServerUrl() {
            SetupContainer();
            var instanceKey = "entity" + typeof(SolrConnection);

            var solrConnection = (SolrConnection)ObjectFactory.Container.GetInstance<ISolrConnection>(instanceKey);

            Assert.AreEqual("http://localhost:8983/solr/entity", solrConnection.ServerURL);
        }

        [Test]
        public void Should_throw_exception_for_invalid_protocol_on_url()
        {
            var solrServers = new SolrServers {
                new SolrServerElement {
                    Id = "test",
                    Url = "htp://localhost:8893",
                    DocumentType = typeof(Entity2).AssemblyQualifiedName,
                }                
            };
            Assert.Throws<InvalidURLException>(() => {
                ObjectFactory.Initialize(c => c.IncludeRegistry(new SolrNetRegistry(solrServers)));
                ObjectFactory.GetInstance<SolrConnection>();
            });
        }

        [Test]
        public void Should_throw_exception_for_invalid_url()
        {
            var solrServers = new SolrServers {
                new SolrServerElement {
                    Id = "test",
                    Url = "http:/localhost:8893",
                    DocumentType = typeof(Entity2).AssemblyQualifiedName,
                }
            };
            Assert.Throws<InvalidURLException>(() => {
                ObjectFactory.Initialize(c => c.IncludeRegistry(new SolrNetRegistry(solrServers)));
                ObjectFactory.GetInstance<SolrConnection>();
            });
        }

        [Test]
        public void Container_has_ISolrFieldParser()
        {
            SetupContainer();
            var parser = ObjectFactory.GetInstance<ISolrFieldParser>();
            Assert.IsNotNull(parser);
        }

        [Test]
        public void Container_has_ISolrFieldSerializer()
        {
           SetupContainer();
            ObjectFactory.GetInstance<ISolrFieldSerializer>();
        }

        [Test]
        public void Container_has_ISolrDocumentPropertyVisitor()
        {
            SetupContainer();
            ObjectFactory.GetInstance<ISolrDocumentPropertyVisitor>();
        }

        [Test]
        public void DictionaryDocument_and_multi_core() {
            var cores = new SolrServers {
                new SolrServerElement {
                    Id = "default",
                    DocumentType = typeof(Entity).AssemblyQualifiedName,
                    Url = "http://localhost:8983/solr/entity1",
                },
                new SolrServerElement {
                    Id = "entity1dict",
                    DocumentType = typeof(Dictionary<string, object>).AssemblyQualifiedName,
                    Url = "http://localhost:8983/solr/entity1",
                },
                new SolrServerElement {
                    Id = "another",
                    DocumentType = typeof(Entity2).AssemblyQualifiedName,
                    Url = "http://localhost:8983/solr/entity2",
                },
            };
            ObjectFactory.Initialize(c => c.IncludeRegistry(new SolrNetRegistry(cores)));
            var solr1 = ObjectFactory.GetInstance<ISolrOperations<Entity>>();
            var solr2 = ObjectFactory.GetInstance<ISolrOperations<Entity2>>();
            var solrDict = ObjectFactory.GetInstance<ISolrOperations<Dictionary<string, object>>>();
        }

        [Test]
        public void DictionaryDocument_ResponseParser()
        {
            SetupContainer();

            var parser = ObjectFactory.GetInstance<ISolrDocumentResponseParser<Dictionary<string, object>>>();
            Assert.IsInstanceOf<SolrDictionaryDocumentResponseParser>(parser);
        }

        [Test]
        public void DictionaryDocument_Serializer()
        {
            SetupContainer();
            var serializer = ObjectFactory.GetInstance<ISolrDocumentSerializer<Dictionary<string, object>>>();
            Assert.IsInstanceOf<SolrDictionarySerializer>(serializer);
        }

        [Test]
        public void Cache() {
            SetupContainer();
            ObjectFactory.Configure(cfg => cfg.For<ISolrCache>().Use<HttpRuntimeCache>());
            var connectionId = "entity" + typeof (SolrConnection);
            var connection = (SolrConnection) ObjectFactory.GetNamedInstance<ISolrConnection>(connectionId);
            Assert.IsNotNull(connection.Cache);
            Assert.IsInstanceOf<HttpRuntimeCache>(connection.Cache);
        }

        private static void SetupContainer()
        {
            var solrConfig = (SolrConfigurationSection)ConfigurationManager.GetSection("solr");
            ObjectFactory.Initialize(c => c.IncludeRegistry(new SolrNetRegistry(solrConfig.SolrServers)));
        }
    }
}
