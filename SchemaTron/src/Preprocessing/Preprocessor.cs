using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SchemaTron.Preprocessing
{
    /// <summary>
    /// Preprocessor provides transformation steps which help to resolve the
    /// schema syntax into the minimal form.
    /// </summary>
    internal static class Preprocessor
    {
        /// <summary>
        /// Resolves all inclusions by replacing each <c>include</c> element by the
        /// resource it references.
        /// </summary>
        /// <remarks>
        /// Potentially infinite <c>include</c> recursion is terminated after
        /// a finite number of steps.
        /// </remarks>
        /// <param name="xSchema">Validation schema.</param>
        /// <param name="resolver">Resolver of included elements</param>
        /// <param name="nsManager">Namespace manager</param>
        /// <exception cref="InvalidOperationException">If the <c>include</c> 
        /// recursion exceeds a limit.</exception>
        /// <exception cref="ArgumentNullException" />
        public static void ResolveInclusions(XDocument xSchema, IInclusionResolver resolver, XmlNamespaceManager nsManager)
        {
            if (xSchema == null)
            {
                throw new ArgumentNullException("xSchema");
            }

            if (nsManager == null)
            {
                throw new ArgumentNullException("nsManager");
            }
            
            int maxSteps = 500; // recursion termination limit
            int i;
            for (i = 0; i < maxSteps; i++)
            {
                // select inclusions
                List<XElement> listIncludes = new List<XElement>();
                foreach (XElement xInclude in xSchema.XPathSelectElements("//sch:include", nsManager))
                {
                    listIncludes.Add(xInclude);
                }

                if (listIncludes.Count == 0)
                {
                    break;
                }

                // replace inclusions
                foreach (XElement xInclude in listIncludes)
                {
                    string href = xInclude.Attribute(XName.Get("href")).Value;
                    XDocument xHref = resolver.Resolve(href);
                    xInclude.ReplaceWith(xHref.Root);
                }
            }

            if (maxSteps == i)
            {
                throw new InvalidOperationException(String.Format(
                    "There is a possibility of infinite recursion of include elements. " +
                    "Terminated after the maximum of {0} steps.", maxSteps));
            }
        }

        /// <summary>
        /// Removes phases and non-active patterns.
        /// </summary>
        /// <param name="xSchema">Validation schema</param>
        /// <param name="nsManager">Namespace manager</param>   
        /// <param name="phase">Validation phase name</param>
        /// <exception cref="ArgumentNullException" />
        public static void ResolvePhase(XDocument xSchema, XmlNamespaceManager nsManager, string phase)
        {
            if (xSchema == null)
            {
                throw new ArgumentNullException("xSchema");
            }

            if (nsManager == null)
            {
                throw new ArgumentNullException("nsManager");
            }

            if (phase == null)
            {
                throw new ArgumentNullException("phase");
            }

            if (phase == "#ALL")
            {
                // remove all phases
                List<XElement> xPhasesGarbage = new List<XElement>();
                foreach (XElement xPhase in xSchema.XPathSelectElements("//sch:phase", nsManager))
                {
                    xPhasesGarbage.Add(xPhase);
                }

                foreach (XElement xPhase in xPhasesGarbage)
                {
                    xPhase.Remove();
                }
            }
            else
            {
                XElement xActivePhase = xSchema.XPathSelectElement(String.Format("//sch:phase[@id='{0}']", phase), nsManager);

                XName nameActive = XName.Get("active", Constants.ISONamespace);
                XName nameLet = XName.Get("let", Constants.ISONamespace);

                // select active and let elements
                List<XElement> xActivePatterns = new List<XElement>();
                List<XElement> xLets = new List<XElement>();
                foreach (XElement xEle in xActivePhase.Descendants())
                {
                    if (xEle.Name == nameActive)
                    {
                        string patternId = xEle.Attribute(XName.Get("pattern")).Value;
                        XElement xPattern = xSchema.XPathSelectElement(String.Format("//sch:pattern[@id='{0}']", patternId), nsManager);
                        xActivePatterns.Add(xPattern);
                    }
                    else if (xEle.Name == nameLet)
                    {
                        xLets.Add(xEle);
                    }
                }

                // remove non-active patterns
                List<XElement> xNonActivePatterns = new List<XElement>();
                foreach (XElement xPattern in xSchema.XPathSelectElements("//sch:pattern", nsManager))
                {
                    if (xActivePatterns.IndexOf(xPattern) < 0)
                    {
                        xNonActivePatterns.Add(xPattern);
                    }
                }

                foreach (XElement xPattern in xNonActivePatterns)
                {
                    xPattern.Remove();
                }

                // remove non-active phases
                List<XElement> xPhasesGarbage = new List<XElement>();
                foreach (XElement xPhase in xSchema.XPathSelectElements(String.Format("//sch:phase[@id!='{0}']", phase), nsManager))
                {
                    xPhasesGarbage.Add(xPhase);
                }

                foreach (XElement xPhase in xPhasesGarbage)
                {
                    xPhase.Remove();
                }

                // override schema lets using active phase lets
                List<XElement> xLetGarbage = new List<XElement>();
                foreach (XElement xLet in xSchema.XPathSelectElements("/sch:schema/sch:let", nsManager))
                {
                    string name = xLet.Attribute(XName.Get("name")).Value;
                    foreach (XElement xPhaseLet in xLets)
                    {
                        if (name == xPhaseLet.Attribute(XName.Get("name")).Value)
                        {
                            xLetGarbage.Add(xLet);
                        }
                    }
                }

                foreach (XElement xLet in xLetGarbage)
                {
                    xLet.Remove();
                }

                // remove active phase
                xActivePhase.ReplaceWith(xLets);
            }
        }

        /// <summary>
        /// Resolves all abstract patterns in the schema by replacing
        /// parameter references by actual parameter values in all enclosed
        /// attributes that contain queries.
        /// </summary>
        /// <param name="xSchema">Validation schema</param>
        /// <param name="nsManager">Namespace manager</param>
        /// <exception cref="ArgumentNullException" />
        public static void ResolveAbstractPatterns(XDocument xSchema, XmlNamespaceManager nsManager)
        {
            if (xSchema == null)
            {
                throw new ArgumentNullException("xSchema");
            }

            if (nsManager == null)
            {
                throw new ArgumentNullException("nsManager");
            }

            // select abstract patterns
            Dictionary<string, XElement> dicAPs = new Dictionary<string, XElement>();
            foreach (XElement xAbstractPattern in xSchema.XPathSelectElements("//sch:pattern[@abstract='true']", nsManager))
            {
                string id = xAbstractPattern.Attribute(XName.Get("id")).Value;
                dicAPs.Add(id, xAbstractPattern);
            }

            if (dicAPs.Count > 0)
            {
                // select instances
                List<XElement> listInstances = new List<XElement>();
                foreach (XElement xInstance in xSchema.XPathSelectElements("//sch:pattern[@is-a]", nsManager))
                {
                    listInstances.Add(xInstance);
                }

                // replace instances
                foreach (XElement xInstance in listInstances)
                {
                    ReplaceAbstractPatternInstance(dicAPs, xInstance, nsManager);
                }

                // remove abstract patterns
                foreach (KeyValuePair<string, XElement> item in dicAPs)
                {
                    item.Value.Remove();
                }
            }
        }

        private static void ReplaceAbstractPatternInstance(Dictionary<string, XElement> dicAPs, XElement xInstance, XmlNamespaceManager nsManager)
        {
            // select is-a
            string isa = xInstance.Attribute(XName.Get("is-a")).Value;

            // select id
            string id = null;
            XAttribute xAttId = xInstance.Attribute(XName.Get("id"));
            if (xAttId != null)
            {
                id = xAttId.Value;
            }

            // select params
            XName paramName = XName.Get("param", Constants.ISONamespace);
            Dictionary<string, Parameter> dicParams = new Dictionary<string, Parameter>();
            foreach (XElement xParam in xInstance.Descendants())
            {
                if (xParam.Name == paramName)
                {
                    Parameter param = new Parameter();
                    param.Name = String.Concat("$", xParam.Attribute(XName.Get("name")).Value);
                    param.Value = xParam.Attribute(XName.Get("value")).Value;
                    dicParams.Add(param.Name, param);
                }
            }

            XElement newPattern = new XElement(dicAPs[isa]);

            // remove abstract attribute
            newPattern.Attribute(XName.Get("abstract")).Remove();

            // alter id attribute
            if (id != null)
            {
                newPattern.SetAttributeValue(XName.Get("id"), id);
            }
            else
            {
                newPattern.Attribute(XName.Get("id")).Remove();
            }

            // transform rules
            foreach (XElement xRule in newPattern.XPathSelectElements("//sch:rule[@context]", nsManager))
            {
                XAttribute xContext = xRule.Attribute(XName.Get("context"));
                string context = xContext.Value;
                foreach (KeyValuePair<string, Parameter> item in dicParams)
                {
                    context = context.Replace(item.Value.Name, item.Value.Value);
                }

                xContext.Value = context;
            }

            // transform asserts
            foreach (XElement xAssert in newPattern.XPathSelectElements("//sch:assert|//sch:report ", nsManager))
            {
                XAttribute xTest = xAssert.Attribute(XName.Get("test"));
                string test = xTest.Value;
                foreach (KeyValuePair<string, Parameter> item in dicParams)
                {
                    test = test.Replace(item.Value.Name, item.Value.Value);
                }

                xTest.Value = test;
            }

            xInstance.ReplaceWith(newPattern);
        }

        /// <summary>
        /// Resolves all abstract rules in the schema by replacing the
        /// <c>extends</c> elements by the contents of the abstract rule
        /// identified.
        /// </summary>
        /// <param name="xSchema">Validation schema</param>
        /// <param name="nsManager">Namespace manager</param>
        /// <exception cref="ArgumentNullException" />
        public static void ResolveAbstractRules(XDocument xSchema, XmlNamespaceManager nsManager)
        {
            if (xSchema == null)
            {
                throw new ArgumentNullException("xSchema");
            }

            if (nsManager == null)
            {
                throw new ArgumentNullException("nsManager");
            }

            // select abstract rules
            List<AbstractRule> listAbstractRules = new List<AbstractRule>();
            foreach (XElement xAbstractRule in xSchema.XPathSelectElements("//sch:rule[@abstract='true' and @id]", nsManager))
            {
                AbstractRule rule = new AbstractRule();
                rule.Id = xAbstractRule.Attribute(XName.Get("id")).Value;
                rule.Element = xAbstractRule;

                listAbstractRules.Add(rule);
            }

            if (listAbstractRules.Count > 0)
            {
                // select extends
                List<XElement> listExtends = new List<XElement>();
                foreach (XElement xExtends in xSchema.XPathSelectElements("//sch:extends[@rule]", nsManager))
                {
                    listExtends.Add(xExtends);
                }

                // replace selected extends with abstract rule contents
                foreach (XElement xExtends in listExtends)
                {
                    string rule = xExtends.Attribute(XName.Get("rule")).Value;

                    // select abstract rule contents
                    List<XElement> content = new List<XElement>();
                    foreach (AbstractRule abstractRule in listAbstractRules)
                    {
                        if (abstractRule.Id == rule)
                        {
                            content.AddRange(abstractRule.Element.Descendants());
                        }
                    }

                    // replace with content
                    if (content.Count > 0)
                    {
                        xExtends.ReplaceWith(content.ToArray());
                    }
                }

                // remove selected abstract rules
                foreach (AbstractRule abstractRule in listAbstractRules)
                {
                    XElement xAbstractRule = abstractRule.Element;
                    XElement xPattern = xAbstractRule.Parent;
                    xAbstractRule.Remove();
                }
            }
        }

        /// <summary>
        /// Substitutes variables into expressions before the expressions are evaluated. 
        /// </summary>
        /// <param name="xSchema">Validation schema</param>
        /// <param name="nsManager">Namespace manager</param>
        /// <exception cref="ArgumentNullException" />
        public static void ResolveLets(XDocument xSchema, XmlNamespaceManager nsManager)
        {
            if (xSchema == null)
            {
                throw new ArgumentNullException("xSchema");
            }

            if (nsManager == null)
            {
                throw new ArgumentNullException("nsManager");
            }

            ResolveRuleLets(xSchema, nsManager);
            ResolvePatternLets(xSchema, nsManager);
            ResolveSchemaLets(xSchema, nsManager);
        }

        private static List<Let> GetElementLets(XElement xEle, List<XElement> garbage)
        {
            XName letName = XName.Get("let", Constants.ISONamespace);
            List<Let> listLets = new List<Let>();
            foreach (XElement x in xEle.Descendants())
            {
                if (x.Name == letName)
                {
                    Let let = new Let();
                    let.Name = String.Concat("$", x.Attribute(XName.Get("name")).Value);
                    let.Value = x.Attribute(XName.Get("value")).Value;
                    garbage.Add(x);
                    listLets.Add(let);
                }
            }

            return listLets;
        }

        private static void ResolveRuleLets(XDocument xSchema, XmlNamespaceManager nsManager)
        {
            List<XElement> garbage = new List<XElement>();

            foreach (XElement xRule in xSchema.XPathSelectElements("//sch:rule[sch:let[@name and @value]]", nsManager))
            {
                // select lets
                List<Let> listLets = GetElementLets(xRule, garbage);

                // select assertions
                XName assertName = XName.Get("assert", Constants.ISONamespace);
                XName reportName = XName.Get("report", Constants.ISONamespace);
                XName nameName = XName.Get("name", Constants.ISONamespace);
                XName valueofName = XName.Get("value-of", Constants.ISONamespace);
                foreach (XElement xEle in xRule.Descendants())
                {
                    // TODO:
                    // Variable substitution should be separated for each element type.
                    // The current code could potentially lead to incorrect behavior.
                    // Consider:
                    //   <assert select="...">
                    //   <name test="...">
                    //   <value-of path="...">
                    if (xEle.Name == assertName || xEle.Name == reportName || xEle.Name == nameName || xEle.Name == valueofName)
                    {
                        foreach (Let let in listLets)
                        {
                            XAttribute testAtt = xEle.Attribute(XName.Get("test"));
                            if (testAtt != null)
                            {
                                testAtt.Value = testAtt.Value.Replace(let.Name, let.Value);
                            }
                            else
                            {
                                XAttribute pathAtt = xEle.Attribute(XName.Get("path"));
                                if (pathAtt != null)
                                {
                                    pathAtt.Value = pathAtt.Value.Replace(let.Name, let.Value);
                                }
                                else
                                {
                                    XAttribute selectAtt = xEle.Attribute(XName.Get("select"));
                                    if (selectAtt != null)
                                    {
                                        selectAtt.Value = selectAtt.Value.Replace(let.Name, let.Value);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // remove lets
            foreach (XElement xEle in garbage)
            {
                xEle.Remove();
            }
        }

        private static void ResolvePatternLets(XDocument xSchema, XmlNamespaceManager nsManager)
        {
            List<XElement> garbage = new List<XElement>();

            XName ruleName = XName.Get("rule", Constants.ISONamespace);
            XName assertName = XName.Get("assert", Constants.ISONamespace);
            XName reportName = XName.Get("report", Constants.ISONamespace);
            XName nameName = XName.Get("name", Constants.ISONamespace);
            XName valueofName = XName.Get("value-of", Constants.ISONamespace);
            foreach (XElement xPattern in xSchema.XPathSelectElements("//sch:pattern[sch:let[@name and @value]]", nsManager))
            {
                List<Let> listLets = GetElementLets(xPattern, garbage);
                ResolveLets(xPattern.Descendants(), listLets, ruleName, assertName, reportName, nameName, valueofName);
            }

            // remove lets
            foreach (XElement xEle in garbage)
            {
                xEle.Remove();
            }
        }

        private static void ResolveSchemaLets(XDocument xSchema, XmlNamespaceManager nsManager)
        {
            List<XElement> garbage = new List<XElement>();

            // resolve schema lets 
            XName ruleName = XName.Get("rule", Constants.ISONamespace);
            XName assertName = XName.Get("assert", Constants.ISONamespace);
            XName reportName = XName.Get("report", Constants.ISONamespace);
            XName nameName = XName.Get("name", Constants.ISONamespace);
            XName valueofName = XName.Get("value-of", Constants.ISONamespace);
            IEnumerable<Let> schemaLets = GetElementLets(xSchema.Root, garbage);

            IEnumerable<XElement> elements = xSchema.XPathSelectElements("//sch:rule|//sch:assert|//sch:report|//sch:name[@path]|//sch:value-of", nsManager);
            ResolveLets(elements, schemaLets, ruleName, assertName, reportName, nameName, valueofName);

            foreach (XElement xEle in garbage)
            {
                xEle.Remove();
            }
        }

        private static void ResolveLets(IEnumerable<XElement> elements, IEnumerable<Let> lets, XName ruleName, XName assertName,
            XName reportName, XName nameName, XName valueofName)
        {
            foreach (XElement xEle in elements)
            {
                foreach (Let let in lets)
                {
                    if (xEle.Name == ruleName)
                    {
                        XAttribute xContext = xEle.Attribute(XName.Get("context"));
                        if (xContext != null)
                        {
                            xContext.Value = xContext.Value.Replace(let.Name, let.Value);
                        }
                    }
                    else if (xEle.Name == assertName || xEle.Name == reportName)
                    {
                        XAttribute xTest = xEle.Attribute(XName.Get("test"));
                        if (xTest != null)
                        {
                            xTest.Value = xTest.Value.Replace(let.Name, let.Value);
                        }
                    }
                    else if (xEle.Name == nameName)
                    {
                        XAttribute xPath = xEle.Attribute(XName.Get("path"));
                        if (xPath != null)
                        {
                            xPath.Value = xPath.Value.Replace(let.Name, let.Value);
                        }
                    }
                    else if (xEle.Name == valueofName)
                    {
                        XAttribute xSelect = xEle.Attribute(XName.Get("select"));
                        if (xSelect != null)
                        {
                            xSelect.Value = xSelect.Value.Replace(let.Name, let.Value);
                        }
                    }
                }
            }
        }

        public static void ResolveDiagnostics(XDocument xSchema, XmlNamespaceManager nsManager)
        {
            if (xSchema == null)
            {
                throw new ArgumentNullException("xSchema");
            }

            if (nsManager == null)
            {
                throw new ArgumentNullException("nsManager");
            }

            XElement xDiagnostics = xSchema.XPathSelectElement("//sch:diagnostics", nsManager);

            if (xDiagnostics != null)
            {
                Dictionary<string, Diagnostic> diagnostics = new Dictionary<string, Diagnostic>();
                foreach (XElement xDiagnostic in xDiagnostics.XPathSelectElements("./sch:diagnostic", nsManager))
                {
                    Diagnostic diag = new Diagnostic();
                    diag.Id = xDiagnostic.Attribute(XName.Get("id")).Value;
                    diag.Element = xDiagnostic;
                    diagnostics.Add(diag.Id, diag);
                }

                foreach (XElement xAssertion in xSchema.XPathSelectElements("//sch:assert[@diagnostics]|//sch:report[@diagnostics]", nsManager))
                {
                    String refDiag = xAssertion.Attribute(XName.Get("diagnostics")).Value;
                    Diagnostic diag = diagnostics[refDiag];
                    xAssertion.ReplaceNodes(xAssertion.DescendantNodes(), diag.Element.DescendantNodes());
                    xAssertion.Attribute(XName.Get("diagnostics")).Remove();
                }

                xDiagnostics.Remove();
            }
        }

        /// <summary>
        /// Removes elements used for documentation from the schema.
        /// </summary>
        /// <param name="xSchema">Validation schema</param>
        /// <param name="nsManager">Namespace manager</param>
        /// <exception cref="ArgumentNullException" />
        public static void ResolveAncillaryElements(XDocument xSchema, XmlNamespaceManager nsManager)
        {
            if (xSchema == null)
            {
                throw new ArgumentNullException("xSchema");
            }

            if (nsManager == null)
            {
                throw new ArgumentNullException("nsManager");
            }
            
            List<XElement> garbage = new List<XElement>();
            foreach (XElement xEle in xSchema.XPathSelectElements("//sch:dir|//sch:emph|//sch:p|//sch:span|//sch:title", nsManager))
            {
                garbage.Add(xEle);
            }
           
            // remove
            foreach (XElement xEle in garbage)
            {
                xEle.Remove();
            }
        }
    }
}
