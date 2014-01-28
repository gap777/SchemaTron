using System.Xml.Linq;

namespace SchemaTron.Preprocessing
{
    internal sealed class Diagnostic
    {
        public string Id { get; set; }
      
        public XElement Element { get; set; }
    }
}
