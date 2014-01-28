namespace SchemaTron.Test.Functional
{
    using System.Xml.Linq;
    using SchemaTron;
    using Xunit;

    public class Inclusions
    {
        [Fact]
        public void SimpleValidation()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("inclusions_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("inclusions_xml.xml");

            ValidatorSettings settings = new ValidatorSettings();
            settings.InclusionsResolver = new InclusionsEmbededResourceResolver();
            Validator validator = Validator.Create(xSch, settings);
            ValidatorResults results = validator.Validate(xIn, true);

            Assert.True(results.IsValid);
        }

        [Fact]
        public void SimpleValidation_InvalidInstance()
        {
            XDocument xSch = Resources.Provider.LoadXmlDocument("inclusions_sch.xml");
            XDocument xIn = Resources.Provider.LoadXmlDocument("inclusions_xml_invalid.xml");

            ValidatorSettings settings = new ValidatorSettings();
            settings.InclusionsResolver = new InclusionsEmbededResourceResolver();
            Validator validator = Validator.Create(xSch, settings);

            // full validation
            ValidatorResults results1 = validator.Validate(xIn, true);

            // partial validation
            ValidatorResults results2 = validator.Validate(xIn, false);

            Assert.False(results1.IsValid);

            Assert.False(results2.IsValid);
        }

        class InclusionsEmbededResourceResolver : SchemaTron.IInclusionResolver
        {
            public XDocument Resolve(string href)
            {
                return Resources.Provider.LoadXmlDocument(href);
            }
        }
    }
}
