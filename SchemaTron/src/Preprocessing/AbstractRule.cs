using System.Xml.Linq;

namespace SchemaTron.Preprocessing
{
    /// <summary>
    /// Represents an abstract rule.
    /// </summary>  
    internal sealed class AbstractRule
    {
        public string Id { get; set; }

        public XElement Element { get; set; }
    }
}