using System.Xml.Linq;
using SolrNet.Impl;
using SolrNet.Tests.Utils;

namespace SolrNet.Tests.Mocks {
    public class MSolrHeaderResponseParser : ISolrHeaderResponseParser {
        public MFunc<XDocument, ResponseHeader> parse;

        public ResponseHeader Parse(XDocument response) {
            return parse.Invoke(response);
        }
    }
}