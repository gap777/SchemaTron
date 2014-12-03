using System.Collections.Generic;
using System.Linq;
using XmlPrime;

namespace XRouter.SchemaTron.SyntaxModel
{
    /// <summary>
    /// Represents a rule.
    /// </summary>  
    internal sealed class Rule
    {
        public string Id { get; set; }

        public string Context { get; set; }

        public XPath CompiledContext { get; set; }

        public IEnumerable<Assert> Asserts { get; set; }

        public override string ToString()
        {
            return string.Format("{0} asserts about {1}", Asserts.Count(), Context);
        }
    }
}
