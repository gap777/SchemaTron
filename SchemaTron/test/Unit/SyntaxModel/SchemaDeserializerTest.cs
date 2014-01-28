namespace SchemaTron.SyntaxModel.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using SchemaTron;
    using SchemaTron.Test.Resources;
    using Xunit;

    public class SchemaDeserializerTest
    {
        // Note: SchemaDeserializer is internal and access to it
        // must have been allowed explicitly.

        private const string BASIC_SCHEMA = "basics_sch.xml";

        [Fact]
        public void DeserializeNullSchema()
        {
            XmlNamespaceManager nsManager = new XmlNamespaceManager(new NameTable());
            Assert.Throws<ArgumentNullException>(() => SyntaxModel.SchemaDeserializer.Deserialize(null, nsManager));
        }

        [Fact]
        public void DeserializeNullNsManager()
        {
            XDocument xSchema = Provider.LoadXmlDocument(BASIC_SCHEMA);
            Assert.Throws<ArgumentNullException>(() => SyntaxModel.SchemaDeserializer.Deserialize(xSchema, null));
        }
    }
}
