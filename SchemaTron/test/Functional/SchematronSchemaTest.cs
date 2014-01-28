namespace SchemaTron.Test.Functional
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using System.Collections.Generic;
    using SchemaTron;
    using Xunit;

    /// <summary>
    /// Functional tests of Schematron schemas.
    /// </summary>
    public class SchematronSchemaTest
    {
        // Resource prefixes correspond to directories but are separated by
        // '.' character instead of '/' or '\'
        private static readonly string BAD_SCHEMAS = "BadSchemas";
        private static readonly string GOOD_SCHEMAS = "GoodSchemas";

        private ValidatorSettings goodSchemaSettings;
        private ValidatorSettings badSchemaSettings;

        public SchematronSchemaTest()
        {
            goodSchemaSettings = new ValidatorSettings()
            {
                InclusionsResolver = new CustomInclusionResolver()
                {
                    Prefix = GOOD_SCHEMAS
                }
            };
            badSchemaSettings = new ValidatorSettings()
            {
                InclusionsResolver = new CustomInclusionResolver()
                {
                    Prefix = BAD_SCHEMAS
                }
            };
        }

        #region Bad-day test methods

        [Fact]
        public void BadRootElement()
        {
            TestBadSchema(GetBadFile("bad_root.xml"), new string[] {
                "Only 'schema' can be used as the root element.",
            });
        }

        [Fact]
        public void BadSchema()
        {
            // TODO:
            // - test missing schema.@xmlns attribute
            TestBadSchema(GetBadFile("bad_schema_element.xml"), new string[] {
                "The value of the 'queryBinding' attribute ('fooBinding') is not valid.",
                "The 'schema' element must contain at most one 'diagnostics' element.",
                "The 'fooElement' element is not allowed inside the 'schema' element.",
            });
        }

        [Fact]
        public void BadNamespace()
        {
            TestBadSchema(GetBadFile("bad_ns_element.xml"), new string[] {
                "The 'ns' element must contain the 'prefix' attribute.",
                "The 'ns' element must contain the 'uri' attribute.",
                "The 'ns' element must contain the 'prefix' attribute.",
                "The 'ns' element must contain the 'uri' attribute.",
                "The 'fooElement' element is not allowed inside the 'ns' element.",
            });
        }

        [Fact]
        public void BadPhase()
        {
            TestBadSchema(GetBadFile("bad_phase_element.xml"), new string[] {
                "The 'phase' element must contain the 'id' attribute.",
                "The value of the 'id' attribute ('#ALL') is not valid.",
                "The value of the 'id' attribute ('#DEFAULT') is not valid.",
                "The 'fooElement' element is not allowed inside the 'phase' element.",
            });
        }

        [Fact]
        public void BadActive()
        {
            TestBadSchema(GetBadFile("bad_active_element.xml"), new string[] {
                "The 'active' element must contain the 'pattern' attribute.",
                "The 'fooElement' element is not allowed inside the 'active' element.",
            });
        }

        [Fact]
        public void BadPattern()
        {
            TestBadSchema(GetBadFile("bad_pattern_element.xml"), new string[] {
                "The value of the 'abstract' attribute ('foo') is not valid.",
                "The value of the 'abstract' attribute ('True') is not valid.",
                "The value of the 'abstract' attribute ('False') is not valid.",
                "The value of the 'abstract' attribute ('TRUE') is not valid.",
                "An abstract 'pattern' element must contain the 'id' attribute.",
                "The value of the 'abstract' attribute ('true') is not valid. " + 
                "The pattern cannot be simultaneously abstract and instance " +
                "of an abstract pattern.",
                "The 'let' element is not allowed inside the instance 'pattern' element.",
                "The 'rule' element is not allowed inside the instance 'pattern' element.",
                "The 'param' element is not allowed inside the non-instance 'pattern' element.",
            });
        }

        [Fact]
        public void BadParam()
        {
            TestBadSchema(GetBadFile("bad_param_element.xml"), new string[] {
                "The 'fooElement' element is not allowed inside the 'param' element.",
                "The 'param' element must contain the 'name' attribute.",
                "The 'param' element must contain the 'value' attribute.",
            });
        }

        [Fact]
        public void BadDiagnostic()
        {
            TestBadSchema(GetBadFile("bad_diagnostic_element.xml"), new string[] {
                "The 'diagnostic' element must contain the 'id' attribute.",
                "The 'fooElement' element is not allowed inside the 'diagnostic' element.",
            });
        }

        [Fact]
        public void BadDiagnostics()
        {
            TestBadSchema(GetBadFile("bad_diagnostics_element.xml"), new string[] {
                "The 'fooElement' element is not allowed inside the 'diagnostics' element.",
            });
        }

        [Fact]
        public void BadRule()
        {
            TestBadSchema(GetBadFile("bad_rule_element.xml"), new string[] {
                "The value of the 'abstract' attribute ('foo') is not valid.",
                "The value of the 'abstract' attribute ('True') is not valid.",
                "The value of the 'abstract' attribute ('False') is not valid.",
                "The value of the 'abstract' attribute ('TRUE') is not valid.",
                "The non-abstract 'rule' element must contain the 'context' attribute.",
                "The abstract 'rule' element must contain the 'id' attribute.",
                "The abstract 'rule' element must not contain the 'context' attribute.",
                "The 'fooElement' element is not allowed inside the 'rule' element.",
            });
        }

        [Fact]
        public void BadExtends()
        {
            TestBadSchema(GetBadFile("bad_extends_element.xml"), new string[] {
                "The 'extends' element must contain the 'rule' attribute.",
                "The 'fooElement' element is not allowed inside the 'extends' element."
            });
        }

        [Fact]
        public void BadAssertOrReport()
        {
            TestBadSchema(GetBadFile("bad_assert_or_report_element.xml"), new string[] {
                "The 'assert' element must contain the 'test' attribute.",
                "The 'fooElement' element is not allowed inside the 'assert' or 'report' element.",    
                "The 'report' element must contain the 'test' attribute.",
                "The 'barElement' element is not allowed inside the 'assert' or 'report' element.",
            });
        }

        [Fact]
        public void BadLet()
        {
            TestBadSchema(GetBadFile("bad_let_element.xml"), new string[] {
                "The 'let' element must contain the 'name' attribute.",
                "The 'let' element must contain the 'value' attribute.",
                "The 'fooElement' element is not allowed inside the 'let' element.",
            });
        }

        [Fact]
        public void BadName()
        {
            TestBadSchema(GetBadFile("bad_name_element.xml"), new string[] {
                "The 'barElement' element is not allowed inside the 'name' element.",
                "The 'fooElement' element is not allowed inside the 'name' element.",
            });
        }

        [Fact]
        public void BadValueOf()
        {
            TestBadSchema(GetBadFile("bad_value-of_element.xml"), new string[] {
                "The 'value-of' element must contain the 'select' attribute.",
                "The 'fooElement' element is not allowed inside the 'value-of' element.",
                "The 'value-of' element must contain the 'select' attribute.",
                "The 'barElement' element is not allowed inside the 'value-of' element.",
            });
        }

        [Fact]
        public void BadEmph()
        {
            TestBadSchema(GetBadFile("bad_emph_element.xml"), new string[] {
                "The 'fooElement' element is not allowed inside the 'emph' element.",
            });
        }

        [Fact]
        public void BadParagraph()
        {
            TestBadSchema(GetBadFile("bad_p_element.xml"), new string[] {
                "The 'fooElement' element is not allowed inside the 'p' element.",
            });
        }

        [Fact]
        public void BadSpan()
        {
            TestBadSchema(GetBadFile("bad_span_element.xml"), new string[] {
                "The 'span' element must contain the 'class' attribute.",
                "The 'fooElement' element is not allowed inside the 'span' element.",
            });
        }

        [Fact]
        public void BadTitle()
        {
            TestBadSchema(GetBadFile("bad_title_element.xml"), new string[] {
                "The 'fooElement' element is not allowed inside the 'title' element.",
            });
        }

        [Fact]
        public void BadIdReferences()
        {
            TestBadSchema(GetBadFile("bad_id_reference.xml"), new string[] {
                "The value of the 'defaultPhase' attribute ('fooPhase') does not reference any phase.",
                "The value of the 'id' attribute ('nonUniquePattern') must be unique.",
                "The value of the 'id' attribute ('nonUniquePhase') must be unique.",
                "The value of the 'id' attribute ('nonUniqueRule') must be unique.",
                "The value of the 'is-a' attribute ('nonAbstractPattern') does not reference any abstract pattern.",
                "The value of the 'is-a' attribute ('nonExistentAbstractPattern') does not reference any abstract pattern.",
                "The value of the 'name' attribute ('nonUniqueParam') must be unique.",
                "The value of the 'pattern' attribute ('nonExistentPattern') does not reference any non-abstract pattern.",
                "The value of the 'prefix' attribute ('nonUniqueNS') must be unique.",
                "The value of the 'diagnostics' attribute ('nonExistentAssertDiagnostics') does not reference any diagnostic.",
                "The value of the 'diagnostics' attribute ('nonExistentReportDiagnostics') does not reference any diagnostic.",
                "The value of the 'id' attribute ('nonUniqueAssert') must be unique.",
                "The value of the 'id' attribute ('nonUniqueDiagnostic') must be unique.",
                "The value of the 'id' attribute ('nonUniqueReport') must be unique.",
                "The value of the 'name' attribute ('nonUniqueLet') must be unique.",
            });
        }

        [Fact]
        public void BadInclude()
        {
            TestBadSchema(GetBadFile("bad_include_element.xml"), new string[] {
                "The 'include' element must contain the 'href' attribute.",
                "The 'include' element is only allowed to be inside the following elements: 'schema', 'phase', 'pattern', 'diagnostics' and 'rule'.",
                "The 'fooElement' element is not allowed inside the 'include' element.",
            });
        }

        [Fact]
        public void BadIncludeInfiniteRecursion()
        {
            XDocument xSchema = Resources.Provider.LoadXmlDocument(GetBadFile("infinite_recursion_include.xml"));

            Assert.Throws<InvalidOperationException>(
                () => Validator.Create(xSchema, badSchemaSettings));
        }

        [Fact]
        public void BadXPathExpressions()
        {
            // bad rule.@context
            // bad assert.@test or report.@test
            // bad value-of.@select
            // bad name.@path
            TestBadSchema(GetBadFile("bad_xpath_expressions.xml"), new string[] {
                "Invalid XPath 1.0 context='/invalidXPath/rule/context[@': Expression must evaluate to a node-set.",
                "Invalid XPath 1.0 test='/invalidXPath/assert/test[@': Expression must evaluate to a node-set.",
                "Invalid XPath 1.0 path='name(/invalidXPath/name/path[@)']: Expression must evaluate to a node-set.",
                "Invalid XPath 1.0 select='/invalidXPath/value-of/select[@': Expression must evaluate to a node-set.",
                "Invalid XPath 1.0 test='not(/invalidXPath/report/test[@)': Expression must evaluate to a node-set.",
            });
        }

        /// <summary>
        /// Note that references to variables in expressions are
        /// resolved by the query language binding, here XPath.
        /// </summary>
        [Fact]
        public void ReferenceToUndefinedLet()
        {
            TestBadSchema(GetBadFile("undefined_let.xml"), new string[] {
                // NOTE: Notice the odd XPath message for using an undefined variable.
                "Invalid XPath 1.0 test='$nonExistentLet': XsltContext is needed for this query because of an unknown function.",
            });
        }

        #endregion

        #region Happy-day test methods

        [Fact]
        public void GoodAssertOrReport()
        {
            TestGoodSchema(GetGoodFile("good_assert_or_report.xml"));
        }

        [Fact]
        public void GoodLet()
        {
            TestGoodSchema(GetGoodFile("good_let.xml"));
        }

        [Fact]
        public void GoodDiagnostics()
        {
            TestGoodSchema(GetGoodFile("good_diagnostics.xml"));
        }

        [Fact]
        public void GoodAncillaryElements()
        {
            TestGoodSchema(GetGoodFile("good_ancillary_elements.xml"));
        }

        #endregion

        #region Test helpers

        /// <summary>
        /// Test validation of a schematron schema assuming it is bad.
        /// </summary>
        /// <remarks>
        /// A SyntaxException is excepted with one or more user massages in
        /// arbitrary order.
        /// </remarks>
        /// <param name="schemaFileName">File name of a testing schema</param>
        /// <param name="expectedMessages">A collection of expected messages
        /// in expected order</param>
        public void TestBadSchema(string schemaFileName, IEnumerable<string> expectedMessages)
        {
            XDocument xSchema = Resources.Provider.LoadXmlDocument(schemaFileName);

            try
            {
                Validator.Create(xSchema, badSchemaSettings);
            }
            catch (SyntaxException ex)
            {
                List<string> expectedList = new List<string>(expectedMessages);
                List<string> actualList = new List<string>(ex.UserMessages);
                expectedList.Sort();
                actualList.Sort();

                // TODO: there is a problem that duplicate messages get squashed
                // into a single message

                List<string> missingExpected = expectedList.Except(actualList).ToList();
                List<string> unexpectedActual = actualList.Except(expectedList).ToList();

                string assertMessage = string.Empty;
                if (missingExpected.Count > 0)
                {
                    assertMessage = string.Format("\nMissing expected messages:\n{0}",
                        string.Join("\n", missingExpected));
                }
                if (unexpectedActual.Count > 0)
                {
                    assertMessage += string.Format("\nUnexpected actual messages:\n{0}",
                        string.Join("\n", unexpectedActual));
                }
                Assert.True((missingExpected.Count + unexpectedActual.Count) == 0, assertMessage);
                return;
            }

            Assert.True(false, "A SyntaxException should be thrown.");
        }

        public void TestGoodSchema(string schemaFileName)
        {
            XDocument xSchema = Resources.Provider.LoadXmlDocument(schemaFileName);
            Validator validator = null;
            try
            {
                validator = Validator.Create(xSchema, goodSchemaSettings);
            }
            catch (SyntaxException ex)
            {
                Assert.True(false, string.Format(
                    "Unexpected SyntaxException with messages:\n{0}",
                    string.Join("\n", ex.UserMessages)));
            }
            Assert.NotNull(validator);
        }

        private static string GetBadFile(string fileName)
        {
            return string.Format("{0}.{1}", BAD_SCHEMAS, fileName);
        }

        private static string GetGoodFile(string fileName)
        {
            return string.Format("{0}.{1}", GOOD_SCHEMAS, fileName);
        }

        public class CustomInclusionResolver : SchemaTron.IInclusionResolver
        {
            /// <summary>
            /// Resource prefix.
            /// </summary>
            public string Prefix { get; set; }

            public XDocument Resolve(string href)
            {
                string absoluteHref = href;
                if (!string.IsNullOrEmpty(Prefix))
                {
                    absoluteHref = Prefix + "." + href;
                }
                return Resources.Provider.LoadXmlDocument(absoluteHref);
            }
        }

        #endregion
    }
}
