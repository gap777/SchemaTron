using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using SchemaTron.SyntaxModel;
using XmlPrime;

namespace SchemaTron.Test.Functional
{
    using System.Xml.Linq;
    using SchemaTron;
    using Xunit;

    public class Basics
    {

        [Fact]
        public void SimpleValidation_InvalidInstance()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("basics_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("basics_xml_invalid.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.False(results.IsValid);
        }

        [Fact]
        public void Validatation_QueryBindingXPath()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("basics_xpath_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("basics_xml.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.True(results.IsValid);

        }

        [Fact]
        public void Validatation_QueryBindingXPath2()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("basics_xpath2_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("basics_xml.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.True(results.IsValid);
        }

        [Fact]
        public void Validatation_QueryBindingXSLT()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("basics_xslt_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("basics_xml.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.True(results.IsValid);
        }

        [Fact]
        public void Validatation_QueryBindingXSLT2()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("basics_xslt2_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("basics_xml.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.True(results.IsValid);
        }

        [Fact]
        public void QueryBinding_XQuery()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("basics_xquery_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("basics_xml.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.True(results.IsValid);
        }

        [Fact]
        public void StupidXmlPrimeBug()
        {
            NameTable theTable = new NameTable();
            XmlNamespaceManager nsManager = new XmlNamespaceManager(theTable);
            nsManager.AddNamespace("m", "http://www.w3schools.com/prices");
            nsManager.AddNamespace("soap", "http://www.w3.org/2001/12/soap-envelope");
            XPath compiledXpathFindingItem = XPath.Compile("//m:Item", theTable, nsManager);
            XPath compiledXpathComparingIdAttrValue = XPath.Compile("@id > 0", theTable, nsManager);

            XDocument xIn = Resources.Provider.LoadXmlDocument("basics_xml.xml");
            XPathNavigator navigator = xIn.CreateNavigator();
            IEnumerable<XPathItem> results = compiledXpathFindingItem.Evaluate(navigator);
            foreach (XPathItem item in results)
            {
                XPathNavigator itemNav = item as XPathNavigator;
                XPathItem comparisonResult = compiledXpathComparingIdAttrValue.EvaluateToItem(itemNav);
                Assert.True(comparisonResult.ValueAsBoolean);
            }

        }

        [Fact]
        public void StupidXmlPrimeBugWorkaround()
        {
            NameTable theTable = new NameTable();
            XmlNamespaceManager nsManager = new XmlNamespaceManager(theTable);
            nsManager.AddNamespace("m", "http://www.w3schools.com/prices");
            nsManager.AddNamespace("soap", "http://www.w3.org/2001/12/soap-envelope");
            XPath compiledXpathFindingItem = XPath.Compile("//m:Item", theTable, nsManager);
            XPath compiledXpathComparingIdAttrValue = XPath.Compile("@id > 0", theTable, nsManager);

            XDocument xIn = Resources.Provider.LoadXmlDocument("basics_xml.xml");

            XdmDocument betterIn = new XdmDocument(xIn.CreateReader());
            XPathNavigator navigator = betterIn.CreateNavigator();
            IEnumerable<XPathItem> results = compiledXpathFindingItem.Evaluate(navigator);
            foreach (XPathItem item in results)
            {
                XPathNavigator itemNav = item as XPathNavigator;
                XPathItem comparisonResult = compiledXpathComparingIdAttrValue.EvaluateToItem(itemNav);
                Assert.True(comparisonResult.ValueAsBoolean);
            }

        }
    }
}
