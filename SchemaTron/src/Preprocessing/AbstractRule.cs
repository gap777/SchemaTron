using System.Xml.Linq;

namespace XRouter.SchemaTron.Preprocessing
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