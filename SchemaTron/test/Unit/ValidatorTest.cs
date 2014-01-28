namespace SchemaTron.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;
    using SchemaTron;
    using Xunit;

    // TODO:
    // - create and test some bad schemas:
    //
    // - create and test some good schemas:
    //   - try to override a <let> in the schema by a <let> in
    //     an active pattern

    /// <summary>
    /// SchemaTron.Validator unit tests.
    /// </summary>
    /// <remarks>
    /// These test should cover creating a validator and performing validation
    /// itself. Schemas supplied to validator are assumed to be correct. For
    /// validation of Schematron schemas see SchematronSchemaTest.
    /// </remarks>
    public class ValidatorTest
    {
        private const string BASIC_SCHEMA = "basics_sch.xml";
        private const string BASIC_DOCUMENT = "basics_xml.xml";
        private const string PHASES_SCHEMA = "phases_sch.xml";

        #region Creating validator

        [Fact]
        public void CreateValidatorNullSchemaNoSettings()
        {
            Assert.Throws<ArgumentNullException>(() => Validator.Create(null));
        }

        [Fact]
        public void CreateValidatorNullSchemaWithSettings()
        {
            Assert.Throws<ArgumentNullException>(() => Validator.Create(
                null, new ValidatorSettings()));
        }

        [Fact]
        public void CreateValidatorGoodSchemaWithoutSettings()
        {
            XDocument xSchema = Resources.Provider.LoadXmlDocument(BASIC_SCHEMA);
            Validator validator = Validator.Create(xSchema);
            Assert.NotNull(validator);
        }

        [Fact]
        public void CreateValidatorGoodSchemaWithSettings()
        {
            XDocument xSchema = Resources.Provider.LoadXmlDocument(BASIC_SCHEMA);
            Validator validator = Validator.Create(xSchema, new ValidatorSettings());
            Assert.NotNull(validator);
        }

        [Fact]
        public void CreateValidatorGoodSchemaWithCustomSettings()
        {
            XDocument xSchema = Resources.Provider.LoadXmlDocument(PHASES_SCHEMA);
            ValidatorSettings settings = new ValidatorSettings()
            {
                Phase = "#DEFAULT",
                InclusionsResolver = new CustomInclusionResolver()
            };
            Validator validator = Validator.Create(xSchema, settings);
            Assert.NotNull(validator);
        }

        [Fact]
        public void CreateValidatorGoodSchemaWithBadSettings()
        {
            XDocument xSchema = Resources.Provider.LoadXmlDocument(PHASES_SCHEMA);
            ValidatorSettings settings = new ValidatorSettings()
            {
                Phase = null
            };
            Assert.Throws<ArgumentNullException>(() => Validator.Create(xSchema, settings));
        }

        [Fact]
        public void CreateValidatorNoDefaultPhaseSpecified()
        {
            XDocument xSchema = Resources.Provider.LoadXmlDocument("GoodSchemas.no_default_phase.xml");
            ValidatorSettings settings = new ValidatorSettings()
            {
                Phase = "#DEFAULT"
            };
            Assert.Throws<ArgumentException>(() => Validator.Create(xSchema, settings));
        }

        [Fact]
        public void CreateValidatorNonExistentPhaseRequested()
        {
            XDocument xSchema = Resources.Provider.LoadXmlDocument("GoodSchemas.no_default_phase.xml");
            ValidatorSettings settings = new ValidatorSettings()
            {
                Phase = "nonExistentPhase"
            };
            Assert.Throws<ArgumentException>(() => Validator.Create(xSchema, settings));
        }

        #endregion

        #region Validation

        [Fact]
        public void ValidateNullDocument()
        {
            XDocument xSchema = Resources.Provider.LoadXmlDocument(BASIC_SCHEMA);
            Validator validator = Validator.Create(xSchema);
            Assert.Throws<ArgumentNullException>(() => validator.Validate(null, true));
        }

        [Fact]
        public void ValidateGoodDocument()
        {
            XDocument xSchema = Resources.Provider.LoadXmlDocument(BASIC_SCHEMA);
            XDocument xDocument = Resources.Provider.LoadXmlDocument(BASIC_DOCUMENT);

            Validator validator = Validator.Create(xSchema);
            Assert.NotNull(validator);

            ValidatorResults results = validator.Validate(xDocument, true);

            Assert.True(results.IsValid);
        }

        #endregion

        #region Helper methods and classes

        public class CustomInclusionResolver : SchemaTron.IInclusionResolver
        {
            public XDocument Resolve(string href)
            {
                return Resources.Provider.LoadXmlDocument(href);
            }
        }

        #endregion
    }
}
