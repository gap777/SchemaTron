namespace SchemaTron.Test.Functional
{
    using System.Xml.Linq;
    using SchemaTron;
    using Xunit;

    public class Basics
    {
        [Fact]
        public void SimpleValidation()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("basics_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("basics_xml.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.True(results.IsValid);
        }

        [Fact]
        public void SimpleValidation_InvalidInstance()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("basics_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("basics_xml_invalid.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.False(results.IsValid);
        }
    }
}
