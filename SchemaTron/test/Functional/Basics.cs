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
            XDocument xSch = Resources.Provider.LoadXmlDocument("basics_xpath_sch.xml");
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
    }
}
