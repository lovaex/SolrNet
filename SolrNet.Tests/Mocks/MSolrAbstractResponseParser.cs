using System;
using System.Xml.Linq;
using SolrNet.Impl;
using SolrNet.Tests.Utils;

namespace SolrNet.Tests.Mocks {
    public class MSolrAbstractResponseParser<T> : ISolrAbstractResponseParser<T> {
        public MFunc<XDocument, AbstractSolrQueryResults<T>, Unit> parse;

        public void Parse(XDocument xml, AbstractSolrQueryResults<T> results) {
            parse.Invoke(xml, results);
        }
    }
}