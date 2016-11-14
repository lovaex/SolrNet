using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolrNet.Impl {
    /// <summary>
    /// Debug results model
    /// </summary>
    public abstract class DebugResults {
        private readonly IDictionary<string, string> explanation;
        private readonly string parsedQuery;
        private readonly string parsedQueryString;
        private readonly TimingResults timing;

        private DebugResults(TimingResults timing, string parsedQuery, string parsedQueryString, IDictionary<string, string> explanation) {
            this.timing = timing;
            this.parsedQuery = parsedQuery;
            this.parsedQueryString = parsedQueryString;
            this.explanation = explanation;
        }

        /// <summary>
        /// Timing results
        /// </summary>
        public TimingResults Timing
        {
            get { return timing; }
        }

        /// <summary>
        /// Explanation results (plain or structured)
        /// </summary>
        public IDictionary<string, string> Explanation
        {
            get { return explanation; }
        }

        /// <summary>
        /// Parsed query
        /// </summary>
        public string ParsedQuery
        {
            get { return parsedQuery; }
        }

        /// <summary>
        /// Parsed query string
        /// </summary>
        public string ParsedQueryString
        {
            get { return parsedQueryString; }
        }

        /// <summary>
        /// Plain debug results model
        /// </summary>
        public sealed class PlainDebugResults : DebugResults {
            /// <summary>
            /// Plain debug results initializer
            /// </summary>
            public PlainDebugResults(TimingResults timing, string parsedQuery, string parsedQueryString, IDictionary<string, string> explanation) : base(timing, parsedQuery, parsedQueryString, explanation) {}
        }

        /// <summary>
        /// Structured debug results model
        /// </summary>
        public sealed class StructuredDebugResults : DebugResults {
            private readonly IDictionary<string, ExplanationModel> structuredExplanation;

            /// <summary>
            /// Structured debug results initializer
            /// </summary>
            public StructuredDebugResults(TimingResults timing, string parsedQuery, string parsedQueryString, IDictionary<string, ExplanationModel> structuredExplanation) :
                base(timing, parsedQuery, parsedQueryString, structuredExplanation.ToDictionary(x => x.Key, y => y.Value.ToString())) {
                this.structuredExplanation = structuredExplanation;
            }

            /// <summary>
            /// Structured explanation
            /// </summary>
            public IDictionary<string, ExplanationModel> ExplanationStructured
            {
                get { return structuredExplanation; }
            }
        }
    }

    /// <summary>
    /// Timing results model
    /// </summary>
    public class TimingResults {
        private readonly IDictionary<string, double> prepare;
        private readonly IDictionary<string, double> process;
        private readonly double totalTime;

        /// <summary>
        /// TimingResults initializer
        /// </summary>
        public TimingResults(double totalTime, IDictionary<string, double> prepare, IDictionary<string, double> process) {
            this.totalTime = totalTime;
            this.prepare = prepare;
            this.process = process;
        }

        /// <summary>
        /// Elapsed time
        /// </summary>
        public double TotalTime
        {
            get { return totalTime; }
        }

        /// <summary>
        /// Time results for preparing stage
        /// </summary>
        public IDictionary<string, double> Prepare
        {
            get { return prepare; }
        }

        /// <summary>
        /// Time results for processing stage
        /// </summary>
        public IDictionary<string, double> Process
        {
            get { return process; }
        }
    }

    /// <summary>
    /// Explanation details model
    /// </summary>
    public class ExplanationModel {
        private readonly string description;
        private readonly ICollection<ExplanationModel> details;
        private readonly bool match;
        private readonly double value;

        /// <summary>
        /// ExplanationModel initializer
        /// </summary>
        public ExplanationModel(bool match, double value, string description, ICollection<ExplanationModel> details) {
            this.match = match;
            this.value = value;
            this.description = description;
            this.details = details;
        }

        /// <summary>
        /// Explanation "match" flag
        /// </summary>
        public bool Match
        {
            get { return match; }
        }

        /// <summary>
        /// Explanation "value" field
        /// </summary>
        public double Value
        {
            get { return value; }
        }

        /// <summary>
        /// Explanation description
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Explanation details collection
        /// </summary>
        public ICollection<ExplanationModel> Details
        {
            get { return details; }
        }

        public override string ToString() {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("Match:{0}", Match));
            sb.AppendLine(string.Format("Value:{0}", Value));
            sb.AppendLine(string.Format("Description:{0}", Description));
            sb.AppendLine(string.Format("Details:"));

            foreach (var explanationModel in Details) {
                sb.AppendLine(explanationModel.ToString());
            }

            return sb.ToString();
        }
    }
}