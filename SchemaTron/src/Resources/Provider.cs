using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace SchemaTron.Resources
{
    internal static class Provider
    {
        private static XDocument schemaPhaseA = null;
        private static XDocument schemaPhaseB = null;
        private static XDocument schemaPhaseC = null;

        public static XDocument SchemaPhaseA
        {
            get
            {
                lock (typeof(Provider))
                {
                    if (schemaPhaseA == null)
                    {
                        schemaPhaseA = LoadXmlDocument("SchemaTron.Resources.schema_phaseA.xml");
                    }
                }

                return schemaPhaseA;
            }
        }

        public static XDocument SchemaPhaseB
        {
            get
            {
                lock (typeof(Provider))
                {
                    if (schemaPhaseB == null)
                    {
                        schemaPhaseB = LoadXmlDocument("SchemaTron.Resources.schema_phaseB.xml");
                    }
                }

                return schemaPhaseB;
            }
        }

        public static XDocument SchemaPhaseC
        {
            get
            {
                lock (typeof(Provider))
                {
                    if (schemaPhaseC == null)
                    {
                        schemaPhaseC = LoadXmlDocument("SchemaTron.Resources.schema_phaseC.xml");
                    }
                }

                return schemaPhaseC;
            }
        }

        private static XDocument LoadXmlDocument(string name)
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Stream stream = currentAssembly.GetManifestResourceStream(name);
            XDocument xDoc = XDocument.Load(stream);
            return xDoc;
        }
    }
}
