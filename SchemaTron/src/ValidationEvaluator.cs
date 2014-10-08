using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using SchemaTron.SyntaxModel;

using XmlPrime;


namespace SchemaTron
{
    
    /// <summary>
    /// Implements the actual validation by a Schematron schema.
    /// </summary>
    /// <remarks>
    /// An instance of this class can be created by the constructor.
    /// The instance can be validated by calling the Evaluate() method.
    /// </remarks>
    internal sealed class ValidationEvaluator
    {
        private Schema schema = null;
        private bool fullValidation;
        private XPathNavigator xNavigator = null;
        private List<XPathNavigator> usedContext = new List<XPathNavigator>();
        private ValidatorResults results = new ValidatorResults();
        private DocumentSet cachedDocuments;
        private bool isCanceled = false;

        /// <summary>
        /// Creates a new instance of the ValidationEvaluator containing the
        /// the validation schema and a specific XML document to be validated.
        /// </summary>
        /// <param name="schema">Validation schema. Must not be null.</param>
        /// <param name="xInstance">An instance of an XML document to be validated.
        /// Must not be null.</param>
        /// <param name="fullValidation">Indicates whether to validate the
        /// whole document regardless of any assertion, or to stop validation at
        /// the first assertion.</param>
        public ValidationEvaluator(Schema schema, XPathNavigator aNavigator, bool fullValidation)
        {
            this.schema = schema;
            this.fullValidation = fullValidation;
            this.xNavigator = aNavigator;
            XmlNameTable nameTable = new NameTable();
            XmlReaderSettings readerSettings = new XmlReaderSettings();
            readerSettings.NameTable = nameTable;
            readerSettings.XmlResolver = new XmlUrlResolver();
            IDocumentResolver documentResolver = new XmlReaderDocumentResolver(readerSettings);
            cachedDocuments = new DocumentSet(nameTable, documentResolver, null, null);
        }

        /// <summary>
        /// Starts the validation of the XML document using the schema, both specified
        /// in the constructor of the class.
        /// </summary>
        /// <returns>Results of the validation</returns>
        /// <seealso cref="ValidatorResults"/>
        public ValidatorResults Evaluate()
        {
            foreach (Pattern pattern in this.schema.Patterns)
            {
                this.ValidatePattern(pattern);

                if (isCanceled)
                {
                    break;
                }
            }

            return this.results;
        }

        private void ValidatePattern(Pattern pattern)
        {
            this.usedContext.Clear();
            foreach (Rule rule in pattern.Rules)
            {
                this.ValidateRule(pattern, rule);

                if (isCanceled)
                {
                    break;
                }
            }
        }

        private void ValidateRule(Pattern pattern, Rule rule)
        {
            DynamicContextSettings settings = new DynamicContextSettings
            {
                ContextItem = xNavigator,
                DocumentSet = cachedDocuments
            };

            IEnumerable<XPathItem> contextSet = rule.CompiledContext.Evaluate(settings);
            foreach (XPathItem nextItem in contextSet)
            {
                // assume that context resolves to a node, not to a value or function. 
                XPathNavigator contextNode = nextItem as XPathNavigator;
                if (!this.IsContextUsed(contextNode))
                {
                    foreach (Assert assert in rule.Asserts)
                    {
                        this.ValidateAssert(pattern, rule, assert, contextNode);

                        if (this.isCanceled)
                        {
                            break;
                        }
                    }

                    this.usedContext.Add(contextNode.Clone());   
                }

                if (this.isCanceled)
                {
                    break;
                }
            }
        }

        private bool IsContextUsed(XPathNavigator contextNode)
        {           
            foreach (XPathNavigator nav in this.usedContext)
            {
                if (contextNode.ComparePosition(nav) == System.Xml.XmlNodeOrder.Same)
                {
                    return true;
                }
            }

            return false;
        }

        private void ValidateAssert(Pattern pattern, Rule rule, Assert assert, XPathNavigator context)
        {
            try
            {
                DynamicContextSettings settings = new DynamicContextSettings
                {
                    ContextItem = context,
                    DocumentSet = cachedDocuments
                };

                // resolve object result
                bool isViolated = false;
                if (assert.CompiledTest.StaticType.TypeCode.IsNumber())
                {
                    XPathItem objResult = assert.CompiledTest.EvaluateToItem(settings);
                    double value = objResult.ValueAsDouble;
                    isViolated = double.IsNaN(value);
                }
                else if (assert.CompiledTest.StaticType.TypeCode == XmlTypeCode.Boolean)
                {
                    XPathItem objResult = assert.CompiledTest.EvaluateToItem(settings);
                    isViolated = !objResult.ValueAsBoolean;
                }
                else if (assert.CompiledTest.StaticType.TypeCode == XmlTypeCode.Element ||
                         assert.CompiledTest.StaticType.TypeCode == XmlTypeCode.Node ||
                         assert.CompiledTest.StaticType.TypeCode == XmlTypeCode.Item ||
                         assert.CompiledTest.StaticType.TypeCode == XmlTypeCode.Attribute)
                {
                    IEnumerable<XPathItem> objResults = assert.CompiledTest.Evaluate(settings);
                    isViolated = !objResults.Any();
                }
                else
                {
                    throw new InvalidOperationException(String.Format("Assertion {0} results in unsupported output type of {1}", assert.Id, assert.CompiledTest.StaticType.TypeCode));
                }

                // results
                if (isViolated)
                {
                    if (!this.fullValidation)
                    {
                        this.isCanceled = true;
                    }

                    this.results.IsValid = false;

                    AssertionInfo info = new AssertionInfo();
                    info.IsReport = assert.IsReport;
                    info.PatternId = pattern.Id;
                    info.RuleId = rule.Id;
                    info.RuleContext = rule.Context;
                    info.AssertionId = assert.Id;
                    info.AssertionTest = assert.Test;
                    info.UserMessage = CreateUserMessage(assert, context);
                    info.Location = CreateLocation(context);

                    IXmlLineInfo lineInfo = (IXmlLineInfo)context;
                    info.LineNumber = lineInfo.LineNumber;
                    info.LinePosition = lineInfo.LinePosition;

                    this.results.ViolatedAssertionsList.Add(info);
                }
            }
            catch (XdmException e)
            {
                e.Data["pattern"] = pattern.ToString();
                e.Data["rule"] = rule.ToString();
                e.Data["assert"] = assert.ToString();
                throw;
            }
                
            
        }

        private string CreateUserMessage(Assert assert, XPathNavigator context)
        {
            if (assert.Diagnostics.Length == 0)
            {
                return assert.Message.Trim();
            }
            else
            {
                List<string> diagValues = new List<string>();
                DynamicContextSettings settings = new DynamicContextSettings
                {
                    ContextItem = context,
                    DocumentSet = cachedDocuments
                };

                foreach (XPath xpeDiag in assert.CompiledDiagnostics)
                {
                    if (xpeDiag.StaticType.TypeCode == XmlTypeCode.String ||
                        xpeDiag.StaticType.TypeCode.IsNumber())
                    {
                        XPathItem objDiagResult = xpeDiag.EvaluateToItem(settings);
                        diagValues.Add(objDiagResult.Value);
                    }
                    else if (xpeDiag.StaticType.TypeCode == XmlTypeCode.Boolean)
                    {
                        XPathItem objDiagResult = xpeDiag.EvaluateToItem(settings);
                        diagValues.Add(objDiagResult.Value.ToLower());
                        break;
                    }
                    else if (xpeDiag.StaticType.TypeCode == XmlTypeCode.Element ||
                             xpeDiag.StaticType.TypeCode == XmlTypeCode.Node ||
                             xpeDiag.StaticType.TypeCode == XmlTypeCode.Item ||
                             xpeDiag.StaticType.TypeCode == XmlTypeCode.Attribute)
                    {
                        IEnumerable<XPathItem> objDiagResults = xpeDiag.Evaluate(settings);
                        if (objDiagResults.Any())
                        {
                            foreach (XPathNavigator navResult in objDiagResults)
                            {
                                diagValues.Add(navResult.Value);
                            }
                        }
                        else
                        {
                            diagValues.Add(string.Empty);
                        }
                    }
                    else
                    {
                        diagValues.Add(string.Empty);
                    }
                }

                return String.Format(assert.Message, diagValues.ToArray()).Trim();
            }
        }

        private string CreateLocation(XPathNavigator context)
        {
            Stack<string> steps = new Stack<string>();

            if (context.NodeType == XPathNodeType.Attribute)
            {
                steps.Push(String.Format("@{0}", context.Name));
            }
            else if (context.NodeType == XPathNodeType.Text)
            {
                steps.Push("text()");
            }

            XPathNodeIterator ancestors = context.SelectAncestors(XPathNodeType.Element, true);
            while (ancestors.MoveNext())
            {
                XPathNavigator current = ancestors.Current;
                if (current.NodeType == XPathNodeType.Element)
                {
                    // resolve namespace
                    XmlNamespaceManager nsManager = new XmlNamespaceManager(new NameTable());
                    string prefix = current.Prefix;
                    if (prefix.Length == 0)
                    {
                        // microsoft bug whereby unprefixed name is assumed to be in no namespace
                        prefix = "unprefixed";
                    }
                    nsManager.AddNamespace(prefix, current.NamespaceURI);

                    // resolve name + position
                    string name = current.LocalName;
                    int position = 1 + 
                        ancestors.Current.Select(
                            String.Format("preceding-sibling::{0}:{1}", 
                                          prefix, 
                                          name), 
                            nsManager)
                        .Count;

                    steps.Push(String.Format("{0}[{1}]", name, position));
                }
            }

            // results
            StringBuilder sb = new StringBuilder();
            while (steps.Count > 0)
            {
                sb.Append("/");
                sb.Append(steps.Pop());
            }

            return sb.ToString();
        }
    }

    public static class SchematronExtensions
    {
        public static bool IsNumber(this XmlTypeCode typeCode)
        {
            bool rval = false;
            switch (typeCode)
            {
                case XmlTypeCode.Decimal:
                case XmlTypeCode.Double:
                case XmlTypeCode.Float:
                case XmlTypeCode.Int:
                case XmlTypeCode.Integer:
                case XmlTypeCode.Long:
                case XmlTypeCode.NegativeInteger:
                case XmlTypeCode.NonNegativeInteger:
                case XmlTypeCode.NonPositiveInteger:
                case XmlTypeCode.PositiveInteger:
                case XmlTypeCode.Short:
                case XmlTypeCode.UnsignedInt:
                case XmlTypeCode.UnsignedLong:
                case XmlTypeCode.UnsignedShort:
                    rval = true;
                    break;
                default:
                    rval = false;
                    break;
            }
            return rval;
        }
    }
}
