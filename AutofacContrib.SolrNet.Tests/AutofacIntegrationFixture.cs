﻿// 

using System;
using System.Collections.Generic;
using Autofac;
using NUnit.Framework;
using SolrNet;

namespace AutofacContrib.SolrNet.Tests {
    [TestFixture]
    public class AutofacIntegrationFixture {
        [SetUp]
        public void SetUp() {
            containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new SolrNetModule("http://localhost:8983/solr/core0"));
            container = containerBuilder.Build();
        }

        private ContainerBuilder containerBuilder;
        private IContainer container;

        [Test]
        public void DictionaryDocument() {
            var solr = container.Resolve<ISolrOperations<Dictionary<string, object>>>();
            var results = solr.Query(SolrQuery.All);
            Assert.Greater(results.Count, 0);
            foreach (var d in results) {
                Assert.Greater(d.Count, 0);
                foreach (var kv in d)
                    Console.WriteLine("{0}: {1}", kv.Key, kv.Value);
            }
        }

        [Test]
        public void DictionaryDocument_add() {
            var solr = container.Resolve<ISolrOperations<Dictionary<string, object>>>();
            solr.Add(new Dictionary<string, object> {
                {"id", "ababa"},
                {"manu", "who knows"},
                {"popularity", 55},
                {"timestamp", DateTime.UtcNow},
            });
        }

        [Test]
        public void Ping_And_Query() {
            var solr = container.Resolve<ISolrOperations<AutofacFixture.Entity>>();
            solr.Ping();
            Console.WriteLine(solr.Query(SolrQuery.All).Count);
        }
    }
}