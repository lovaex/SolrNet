using System;

namespace SolrNet {
    /// <summary>
    /// Starting row or cursor mark for pagination
    /// </summary>
    [Serializable]
    public abstract class StartOrCursor : IEquatable<StartOrCursor> {
        private StartOrCursor() {}

        public bool Equals(StartOrCursor other) {
            if (other == null)
                return false;
            return other.Switch(
                startOther => Switch(startThis => startThis.Equals(startOther), _ => false),
                cursorOther => Switch(_ => false, cursorThis => cursorThis.Equals(cursorOther)));
        }

        public abstract T Switch<T>(Func<Start, T> start, Func<Cursor, T> cursor);

        /// <summary>
        /// Starting row for pagination
        /// </summary>
        [Serializable]
        public sealed class Start : StartOrCursor, IEquatable<Start> {
            private readonly int row;

            /// <summary>
            /// Starting row for pagination
            /// </summary>
            /// <param name="row"></param>
            public Start(int row) {
                this.row = row;
            }

            public int Row
            {
                get { return row; }
            }

            public bool Equals(Start other) {
                if (other == null)
                    return false;
                return other.Row == Row;
            }

            public override T Switch<T>(Func<Start, T> start, Func<Cursor, T> cursor) {
                return start(this);
            }

            public override bool Equals(object obj) {
                return Equals(obj as Start);
            }

            public override int GetHashCode() {
                return row.GetHashCode();
            }
        }

        /// <summary>
        /// Cursor mark for pagination.
        /// <see cref="SolrNet.Commands.Parameters.QueryOptions.OrderBy" /> must contain at least one unique field.
        /// Requires Solr 4.7+
        /// </summary>
        [Serializable]
        public sealed class Cursor : StartOrCursor, IEquatable<Cursor> {
            /// <summary>
            /// Starting point cursor.
            /// </summary>
            public static readonly Cursor Start = new Cursor("*");

            private readonly string mark;

            /// <summary>
            /// Cursor mark for pagination.
            /// <see cref="SolrNet.Commands.Parameters.QueryOptions.OrderBy" /> must contain at least one unique field.
            /// Requires Solr 4.7+
            /// </summary>
            /// <param name="mark"></param>
            public Cursor(string mark) {
                if (mark == null)
                    throw new ArgumentNullException("mark");
                this.mark = mark;
            }

            public string Mark
            {
                get { return mark; }
            }

            public bool Equals(Cursor other) {
                if (other == null)
                    return false;
                return other.mark.Equals(mark);
            }

            public override T Switch<T>(Func<Start, T> start, Func<Cursor, T> cursor) {
                return cursor(this);
            }

            public override bool Equals(object obj) {
                return Equals(obj as Cursor);
            }

            public override int GetHashCode() {
                return mark.GetHashCode();
            }

            public override string ToString() {
                return mark;
            }
        }
    }
}