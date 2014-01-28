using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SchemaTron
{
    /// <summary>
    /// A default implementation of resolving included elements.
    /// </summary>
    /// <remarks>
    /// Internally it caches the loaded documents.
    /// </remarks>
    internal sealed class FileInclusionResolver : IInclusionResolver
    {
        /// <summary>
        /// A cache of previously loaded documents.
        /// </summary>
        private Dictionary<string, XDocument> loadedDocs = new Dictionary<string, XDocument>();

        /// <summary>
        /// Loads an external XML document from a file specified in the
        /// <c>href</c> parameter.
        /// </summary>
        /// <param name="href">Absolute path to the external XML document.
        /// Also can a web URI. Must not be null.</param>
        /// <returns>The loaded external XML document.</returns>    
        public XDocument Resolve(string href)
        {
            if (href == null)
            {
                throw new ArgumentNullException("href");
            }

            // each external XML document is loaded only once
            XDocument doc;
            if (!this.loadedDocs.TryGetValue(href, out doc))
            {
                doc = XDocument.Load(href, LoadOptions.SetLineInfo);
                this.loadedDocs.Add(href, doc);
            }

            return doc;
        }
    }
}
