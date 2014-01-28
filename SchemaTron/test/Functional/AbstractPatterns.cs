namespace SchemaTron.Test.Functional
{
    using System.Xml.Linq;
    using SchemaTron;
    using Xunit;

    public class AbsPatterns
    {
        [Fact]
        public void SimpleValidation()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("abspatterns_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("abspatterns_xml.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.True(results.IsValid);
        }

        [Fact]
        public void SimpleValidation_InvalidInstance()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("abspatterns_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("abspatterns_xml_invalid.xml");

            Validator validator = Validator.Create(xSch);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.False(results.IsValid);
        }
    }
}
