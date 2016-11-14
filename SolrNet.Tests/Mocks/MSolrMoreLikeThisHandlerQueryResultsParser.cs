using SolrNet.Impl;
using SolrNet.Tests.Utils;

namespace SolrNet.Tests.Mocks {
    public class MSolrMoreLikeThisHandlerQueryResultsParser<T> : ISolrMoreLikeThisHandlerQueryResultsParser<T> {
        public MFunc<string, SolrMoreLikeThisHandlerResults<T>> parse;

        public SolrMoreLikeThisHandlerResults<T> Parse(string r) {
            return parse.Invoke(r);
        }
    }
}