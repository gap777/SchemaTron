using System.Xml.Linq;

namespace SchemaTron
{
    /// <summary>
    /// Represents a resolver for delivering included Schematron elements.
    /// </summary>
    public interface IInclusionResolver
    {
        /// <summary>
        /// Resolved an included Schematron element specified by the
        /// <c>href</c> parameter.
        /// </summary>
        /// <remarks>
        /// This method is called by the preprocessor at each <c>include</c>
        /// element occurence.
        /// </remarks>
        /// <param name="href">A reference to an external well-formed XML
        /// document.</param>
        /// <returns>The referenced external XML document</returns>
        XDocument Resolve(string href);
    }
}
