﻿using System;
using SolrNet.Impl;
using SolrNet.Tests.Utils;

namespace SolrNet.Tests.Mocks {
    public class MSolrQuerySerializer : ISolrQuerySerializer {
        public Func<Type, bool> canHandleType;
        public MFunc<object, string> serialize;

        public bool CanHandleType(Type t) {
            return canHandleType(t);
        }

        public string Serialize(object q) {
            return serialize.Invoke(q);
        }
    }
}