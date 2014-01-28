using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SchemaTron.SyntaxModel
{
    /// <summary>
    /// Provides a facility for converting a schema from the XML form
    /// (XDocument) into the internal representation (Schema).
    /// </summary>
    /// <seealso cref="XDocument"/>
    /// <seealso cref="Schema"/>
    internal static class SchemaDeserializer
    {
        /// <summary>
        /// Converts a schema from the XML form (XDocument) to the internal
        /// representation (Schema).
        /// </summary>
        /// <param name="xSchema">Schema in serialized XML form. Must not be null.</param>
        /// <param name="nsManager">Namespace manager. Must not be null.</param>
        /// <returns>Schema in internal representation (Schema)</returns>
        /// <exception cref="ArgumentNullException" />
        public static Schema Deserialize(XDocument xSchema, XmlNamespaceManager nsManager)
        {
            if (xSchema == null)
            {
                throw new ArgumentNullException("xSchema");
            }

            if (nsManager == null)
            {
                throw new ArgumentNullException("nsManager");
            }

            Schema schema = new Schema();
            schema.Namespaces = DeserializeNamespaces(xSchema.Root, nsManager);
            schema.Patterns = DeserializePatterns(xSchema.Root, nsManager);
            return schema;
        }

        private static IEnumerable<Namespace> DeserializeNamespaces(XElement xRoot, XmlNamespaceManager nsManager)
        {
            List<Namespace> listNs = new List<Namespace>();
            foreach (XElement xNs in xRoot.XPathSelectElements("sch:ns", nsManager))
            {
                Namespace ns = new Namespace();

                // @prefix
                ns.Prefix = xNs.Attribute(XName.Get("prefix")).Value;

                // @uri
                ns.Uri = xNs.Attribute(XName.Get("uri")).Value;

                listNs.Add(ns);
            }

            return listNs;
        }

        private static IEnumerable<Pattern> DeserializePatterns(XElement xRoot, XmlNamespaceManager nsManager)
        {
            List<Pattern> listPattern = new List<Pattern>();
            foreach (XElement xPattern in xRoot.XPathSelectElements("sch:pattern", nsManager))
            {
                Pattern pattern = new Pattern();

                // @id
                XAttribute xId = xPattern.Attribute(XName.Get("id"));
                if (xId != null)
                {
                    pattern.Id = xId.Value;
                }

                // rules 
                pattern.Rules = DeserializeRules(xPattern, nsManager);

                listPattern.Add(pattern);
            }

            return listPattern;
        }

        private static IEnumerable<Rule> DeserializeRules(XElement xPattern, XmlNamespaceManager nsManager)
        {
            List<Rule> listRule = new List<Rule>();
            foreach (XElement xRule in xPattern.XPathSelectElements("sch:rule", nsManager))
            {
                Rule rule = new Rule();

                // @id
                XAttribute xId = xRule.Attribute(XName.Get("id"));
                if (xId != null)
                {
                    rule.Id = xId.Value;
                }

                // @context
                rule.Context = xRule.Attribute(XName.Get("context")).Value;

                // asserts
                rule.Asserts = DeserializeAsserts(xRule, nsManager);

                listRule.Add(rule);
            }

            return listRule;
        }

        private static IEnumerable<Assert> DeserializeAsserts(XElement xRule, XmlNamespaceManager nsManager)
        {
            List<Assert> listAssert = new List<Assert>();
            foreach (XElement xAssert in xRule.XPathSelectElements("sch:assert|sch:report", nsManager))
            {
                Assert assert = new Assert();

                // @id
                XAttribute xId = xAssert.Attribute(XName.Get("id"));
                if (xId != null)
                {
                    assert.Id = xId.Value;
                }

                // @test
                assert.Test = xAssert.Attribute(XName.Get("test")).Value;

                // assert vs. report
                if (xAssert.Name.LocalName == "report")
                {
                    assert.IsReport = true;
                    assert.Test = String.Format("not({0})", assert.Test);
                }
                else
                {
                    assert.IsReport = false;
                }

                // resolve content
                if (xAssert.HasElements)
                {
                    ResolveAssertContent(xAssert, assert, nsManager);
                }
                else
                {
                    assert.Diagnostics = new string[0];
                    assert.Message = xAssert.Value;
                }

                listAssert.Add(assert);
            }

            return listAssert;
        }

        private static void ResolveAssertContent(XElement xAssert, Assert assert, XmlNamespaceManager nsManager)
        {
            List<string> diagnostics = new List<string>();
            List<bool> diagnosticsIsValueOf = new List<bool>();

            XName nameElement = XName.Get("name", Constants.ISONamespace);
            XName valueofElement = XName.Get("value-of", Constants.ISONamespace);

            StringBuilder sbMessage = new StringBuilder();
            foreach (XNode node in xAssert.DescendantNodes())
            {
                if (!(node is XElement))
                {
                    sbMessage.Append(node.ToString());
                }
                else
                {
                    XElement xEle = (XElement)node;

                    // resolve name, value-of
                    string xpathDiagnostic = null;
                    if (xEle.Name == nameElement)
                    {
                        diagnosticsIsValueOf.Add(false);
                        xpathDiagnostic = "name()";
                        XAttribute xPath = xEle.Attribute(XName.Get("path"));
                        if (xPath != null)
                        {
                            xpathDiagnostic = String.Format(String.Format("name({0})", xPath.Value));
                        }
                    }
                    else if (xEle.Name == valueofElement)
                    {
                        diagnosticsIsValueOf.Add(true);
                        xpathDiagnostic = xEle.Attribute(XName.Get("select")).Value;
                    }

                    if (xpathDiagnostic != null)
                    {
                        // get collection index 
                        int index = diagnostics.IndexOf(xpathDiagnostic);
                        if (index < 0)
                        {
                            diagnostics.Add(xpathDiagnostic);
                            index = diagnostics.Count - 1;
                        }

                        sbMessage.Append("{");
                        sbMessage.Append(index);
                        sbMessage.Append("}");
                        index++;
                    }
                }
            }

            assert.Message = sbMessage.ToString();
            assert.Diagnostics = diagnostics.ToArray();
            assert.DiagnosticsIsValueOf = diagnosticsIsValueOf.ToArray();
        }
    }
}
