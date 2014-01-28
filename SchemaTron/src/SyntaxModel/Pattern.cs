using System.Collections.Generic;

namespace SchemaTron.SyntaxModel
{
    /// <summary>
    /// Represents a pattern.
    /// </summary>
    internal sealed class Pattern
    {
        public string Id { get; set; }

        public IEnumerable<Rule> Rules { get; set; }
    }
}
