using System.Xml.Linq;

namespace XRouter.SchemaTron.Preprocessing
{
    internal sealed class Diagnostic
    {
        public string Id { get; set; }
      
        public XElement Element { get; set; }
    }
}
