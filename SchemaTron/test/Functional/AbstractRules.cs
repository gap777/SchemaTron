namespace SchemaTron.Test.Functional
{
    using System.Xml.Linq;
    using SchemaTron;
    using Xunit;

    public class AbsRules
    {
        [Fact]
        public void SimpleValidation()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("absrules_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("absrules_xml.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.True(results.IsValid);
        }

        [Fact]
        public void SimpleValidation_InvalidInstance()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("absrules_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("absrules_xml_invalid.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.False(results.IsValid);
        }

        [Fact]
        public void RuleExtendsRuleFromAnotherPattern_SchematronIsValid()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("absrules_crosspattern_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("absrules_xml.xml");

            // this throws if schematron is not valid
            Validator validator = Validator.Create(xSch);
        }

        [Fact]
        public void RuleExtendsRuleFromAnotherPattern_InvalidXmlIsFlagged()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("absrules_crosspattern_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("absrules_xml_invalid2.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.False(results.IsValid);
        }
    }
}
