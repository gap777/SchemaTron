using System.Collections.Generic;
using System.Linq;

namespace XRouter.SchemaTron.SyntaxModel
{
    /// <summary>
    /// Represents a pattern.
    /// </summary>
    internal sealed class Pattern
    {
        public string Id { get; set; }

        public IEnumerable<Rule> Rules { get; set; }

        public override string ToString()
        {
            return string.Format("{0} ({1} rules)", Id, Rules.Count());
        }
    }
}
