
namespace SchemaTron
{
    /// <summary>
    /// Contains detailed information related to a violated assertion
    /// (assert or report).
    /// </summary>
    /// <remarks>
    /// This class is a data-transfer object (DTO).
    /// </remarks>
    public sealed class AssertionInfo
    {
        /// <summary>
        /// Prevent creating instances outside this assembly.
        /// </summary>
        internal AssertionInfo()
        {
        }

        /// <summary>
        /// Indicates the type of assertion: report or assert.
        /// </summary>
        public bool IsReport { get; internal set; }

        /// <summary>
        /// Represents the assertion pattern identifier.
        /// </summary>
        public string PatternId { get; internal set; }

        /// <summary>
        /// Represents the assertion rule identifier.
        /// </summary>
        public string RuleId { get; internal set; }

        /// <summary>
        /// Represents the assertion rule context.
        /// </summary>
        public string RuleContext { get; internal set; }

        /// <summary>
        /// Represents the assertion identifier.
        /// </summary>
        public string AssertionId { get; internal set; }

        /// <summary>
        /// Represents the assertion test.
        /// </summary>
        public string AssertionTest { get; internal set; }

        /// <summary>
        /// Represents the number of the line where the node was located.
        /// </summary>
        /// <remarks>
        /// Available only in case the validated XML document instance
        /// contained the line information.
        /// </remarks>
        public int LineNumber { get; internal set; }

        /// <summary>
        /// Represents the position of the node on a line.
        /// </summary>
        /// <remarks>
        /// Available only in case the validated XML document instance
        /// contained the line information.
        /// </remarks>
        public int LinePosition { get; internal set; }

        /// <summary>
        /// Represents the XPath location of the node.
        /// </summary>
        public string Location { get; internal set; }

        /// <summary>
        /// Represents the assertion user message with the <c>name</c> and
        /// <c>value-of</c> elements substituted with selected values.
        /// </summary>
        public string UserMessage { get; internal set; }

        /// <summary>
        /// Returns a System.String which represent the current AssertionInfo
        /// instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.UserMessage;
        }
    }
}
