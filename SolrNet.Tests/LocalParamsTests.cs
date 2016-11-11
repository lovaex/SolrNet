#region license

// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System.Collections.Generic;
using NUnit.Framework;
using SolrNet.Exceptions;
using SolrNet.Impl.FieldSerializers;
using SolrNet.Impl.QuerySerializers;
using SolrNet.Utils;

namespace SolrNet.Tests
{
    [TestFixture]
    public class LocalParamsTests
    {
        private static IEnumerable<TestCaseData> TestParams
        {
            get
            {
                yield return new TestCaseData(KV.Create(new Dictionary<string, string> { { "type", "spatial" }, }, "{!type=spatial}"));
                yield return new TestCaseData(KV.Create(new Dictionary<string, string> { { "type", "spatial" }, { "a", "b" }, }, "{!type=spatial a=b}"));
                yield return new TestCaseData(KV.Create(new Dictionary<string, string> { { "type", "spatial" }, { "a", "1 2 3" }, }, "{!type=spatial a='1 2 3'}"));
                yield return new TestCaseData(KV.Create(new Dictionary<string, string> { { "type", "spatial" }, { "a", "1 2 '3" }, }, "{!type=spatial a='1 2 \\'3'}"));
            }
        }

        [TestCaseSource(nameof(TestParams))]
        public void  Tests(KeyValuePair<Dictionary<string, string>, string> customParams)
        {
            var expectedResult = customParams.Value;
            var localParams = customParams.Key;
            Assert.AreEqual(expectedResult, new LocalParams(localParams).ToString());
        }

        public string SerializeQuery(object q)
        {
            var serializer = new DefaultQuerySerializer(new DefaultFieldSerializer());
            return serializer.Serialize(q);
        }

        [Test]
        public void NullValueThrows()
        {
            Assert.Throws<SolrNetException>(() =>
            {
                var p = new LocalParams {
                    {"a", null}
                };
                p.ToString();
            });
        }

        [Test]
        public void OperatorPlus()
        {
            var p = new LocalParams {
                {"type", "spatial"},
            };
            var q = new SolrQueryByField("field", "value");
            var qq = p + q;
            Assert.AreEqual("{!type=spatial}field:(value)", SerializeQuery(qq));
        }

        [Test]
        public void OperatorPlus_multiple_queries()
        {
            var p = new LocalParams {
                {"type", "spatial"},
            };
            var q = new SolrQueryByField("field", "value");
            var q2 = new SolrQueryByRange<decimal>("price", 100m, 200m);
            var qq = p + (q + q2);
            Assert.AreEqual("{!type=spatial}(field:(value)  price:[100 TO 200])", SerializeQuery(qq));
        }
    }
}