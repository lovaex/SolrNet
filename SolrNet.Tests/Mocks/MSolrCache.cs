using SolrNet.Impl;
using SolrNet.Tests.Utils;

namespace SolrNet.Tests.Mocks {
    public class MSolrCache : ISolrCache {
        public MFunc<SolrCacheEntity, Unit> add;
        public MFunc<string, SolrCacheEntity> get;

        public SolrCacheEntity this[string url]
        {
            get { return get.Invoke(url); }
        }

        public void Add(SolrCacheEntity e) {
            add.Invoke(e);
        }
    }
}