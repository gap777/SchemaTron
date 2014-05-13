﻿using System;
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
        private XDocument xInstance = null;
        private bool fullValidation;
        private XPathNavigator xNavigator = null;
        private List<XPathNavigator> usedContext = new List<XPathNavigator>();
        private ValidatorResults results = new ValidatorResults();
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
        public ValidationEvaluator(Schema schema, XDocument xInstance, bool fullValidation)
        {
            this.schema = schema;
            this.xInstance = xInstance;
            this.fullValidation = fullValidation;
            this.xNavigator = xInstance.CreateNavigator();
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
            IEnumerable<XPathItem> contextSet = rule.CompiledContext.Evaluate(xNavigator);
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
            // evaluate test
            IEnumerable<int> dummy = new[] {1, 2, 3};
            List<int> dummyList = dummy.ToList();

            XPathItem objResult = assert.CompiledTest.EvaluateToItem(context);

            // resolve object result
            bool isViolated = false;
            if (assert.CompiledTest.StaticType.TypeCode.IsNumber())
            {
                double value = objResult.ValueAsDouble;
                isViolated = double.IsNaN(value);
            }
            else if (assert.CompiledTest.StaticType.TypeCode == XmlTypeCode.Boolean)
            {
                isViolated = !objResult.ValueAsBoolean;
            }
            else if (objResult is XPathNavigator)
            {
                //assert.CompiledTest.StaticType == XdmType.Empty ?
                isViolated = (objResult as XPathNavigator).IsEmptyElement;
            }                              
            else
            {
                throw new InvalidOperationException(String.Format("'{0}'.", assert.Test));
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

        private string CreateUserMessage(Assert assert, XPathNavigator context)
        {
            if (assert.Diagnostics.Length == 0)
            {
                return assert.Message.Trim();
            }
            else
            {
                List<string> diagValues = new List<string>();

                foreach (XPath xpeDiag in assert.CompiledDiagnostics)
                {
                    IEnumerable<XPathItem> objDiagResults = xpeDiag.Evaluate(context);
                                        
                    if (xpeDiag.StaticType.TypeCode == XmlTypeCode.String ||
                        xpeDiag.StaticType.TypeCode.IsNumber())
                    {
                        diagValues.Add(objDiagResults.First().Value);
                    }
                    else if (xpeDiag.StaticType.TypeCode == XmlTypeCode.Boolean)
                    {
                        diagValues.Add(objDiagResults.First().Value.ToLower());
                        break;
                    }
                    else if (!objDiagResults.Any())
                    {
                        diagValues.Add(string.Empty);
                    }
                    else if (objDiagResults.First() is XPathNavigator)
                    {
                        foreach (XPathNavigator navResult in objDiagResults)
                        {
                            diagValues.Add(navResult.Value);
                        }
                        break;
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
                    nsManager.AddNamespace(current.Prefix, current.NamespaceURI);

                    // resolve name + position
                    string name = current.Name;
                    int position = 1 + ancestors.Current.Select(String.Format("preceding-sibling::{0}", name), nsManager).Count;
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