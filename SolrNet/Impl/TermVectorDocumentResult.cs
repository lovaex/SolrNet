using System;
using System.Collections.Generic;

namespace SolrNet.Impl {
    /// <summary>
    /// Terms Results
    /// </summary>
    public class TermVectorDocumentResult {
        /// <summary>
        /// Term Vectors
        /// </summary>
        public readonly ICollection<TermVectorResult> TermVector;

        /// <summary>
        /// Unique key of document
        /// </summary>
        public readonly string UniqueKey;

        public TermVectorDocumentResult(string uniqueKey, ICollection<TermVectorResult> termVector) {
            if (termVector == null)
                throw new ArgumentNullException("termVector");
            UniqueKey = uniqueKey;
            TermVector = termVector;
        }
    }


    public class TermVectorResult {
        /// <summary>
        /// Document frequency
        /// </summary>
        public readonly int? Df;

        /// <summary>
        /// Field name
        /// </summary>
        public readonly string Field;

        /// <summary>
        /// Term offsets
        /// </summary>
        public readonly ICollection<Offset> Offsets;

        /// <summary>
        /// Term offsets
        /// </summary>
        public readonly ICollection<int> Positions;

        /// <summary>
        /// Term value
        /// </summary>
        public readonly string Term;

        /// <summary>
        /// Term frequency
        /// </summary>
        public readonly int? Tf;

        /// <summary>
        /// TF*IDF weight
        /// </summary>
        public readonly double? Tf_Idf;

        public TermVectorResult(string field, string term, int? tf, int? df, double? tfIdf, ICollection<Offset> offsets, ICollection<int> positions) {
            Field = field;
            Term = term;
            Tf = tf;
            Df = df;
            Tf_Idf = tfIdf;
            Offsets = offsets;
            Positions = positions;
        }
    }

    public class Offset {
        public readonly int End;
        public readonly int Start;

        public Offset(int start, int end) {
            Start = start;
            End = end;
        }
    }
}