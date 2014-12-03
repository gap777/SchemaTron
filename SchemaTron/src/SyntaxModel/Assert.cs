using System.Xml.XPath;
using XmlPrime;

namespace XRouter.SchemaTron.SyntaxModel
{
    /// <summary>
    /// Represents an assertion.
    /// </summary>
    internal sealed class Assert
    {
        public string Id { get; set; }

        public bool IsReport { get; set; }

        public string Test { get; set; }

        public XPath CompiledTest { get; set; }

        public string Message { get; set; }
       
        public bool[] DiagnosticsIsValueOf { get; set; }

        public string[] Diagnostics { get; set; }

        public XPath[] CompiledDiagnostics { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Id))
            {
                return string.Format("{0}", Test);
            }
            else
            {
                return string.Format("{0} ({1})", Id, Test);                
            }
        }
    }
}
