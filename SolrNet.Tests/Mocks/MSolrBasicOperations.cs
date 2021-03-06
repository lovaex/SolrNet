﻿using System;
using System.Collections.Generic;
using SolrNet.Commands.Parameters;
using SolrNet.Impl;
using SolrNet.Schema;
using SolrNet.Tests.Utils;

namespace SolrNet.Tests.Mocks {
    public class MSolrBasicOperations<T> : ISolrBasicOperations<T> {
        public MFunc<IEnumerable<KeyValuePair<T, double?>>, AddParameters, ResponseHeader> addWithBoost;

        public Func<CommitOptions, ResponseHeader> commit;

        public MFunc<IEnumerable<string>, ISolrQuery, DeleteParameters, ResponseHeader> delete;

        public MFunc<SolrSchema> getSchema;

        public Func<ResponseHeader> ping;

        public MFunc<ResponseHeader> rollback;

        public SolrQueryResults<T> Query(ISolrQuery query, QueryOptions options) {
            throw new NotImplementedException();
        }

        public SolrMoreLikeThisHandlerResults<T> MoreLikeThis(SolrMLTQuery query, MoreLikeThisHandlerQueryOptions options) {
            throw new NotImplementedException();
        }

        public ResponseHeader Ping() {
            return ping();
        }

        public SolrSchema GetSchema(string schemaFileName) {
            return getSchema.Invoke();
        }

        public SolrDIHStatus GetDIHStatus(KeyValuePair<string, string> options) {
            throw new NotImplementedException();
        }

        public ResponseHeader Commit(CommitOptions options) {
            return commit(options);
        }

        public ResponseHeader Optimize(CommitOptions options) {
            throw new NotImplementedException();
        }

        public ResponseHeader Rollback() {
            return rollback.Invoke();
        }

        public ResponseHeader AddWithBoost(IEnumerable<KeyValuePair<T, double?>> docs, AddParameters parameters) {
            return addWithBoost.Invoke(docs, parameters);
        }

        public ExtractResponse Extract(ExtractParameters parameters) {
            throw new NotImplementedException();
        }

        public string Send(ISolrCommand cmd) {
            throw new NotImplementedException();
        }

        public ResponseHeader SendAndParseHeader(ISolrCommand cmd) {
            throw new NotImplementedException();
        }

        public ExtractResponse SendAndParseExtract(ISolrCommand cmd) {
            throw new NotImplementedException();
        }

        public ResponseHeader Delete(IEnumerable<string> ids, ISolrQuery q, DeleteParameters parameters) {
            return delete.Invoke(ids, q, parameters);
        }

        public ResponseHeader Delete(IEnumerable<string> ids, ISolrQuery q) {
            return delete.Invoke(ids, q, null);
        }
    }
}