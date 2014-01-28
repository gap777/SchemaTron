namespace SchemaTron.Test.Resources
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml.Linq;

    internal static class Provider
    {
        public static XDocument LoadXmlDocument(string name)
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Stream stream = currentAssembly.GetManifestResourceStream(String.Format("SchemaTron.Test.Resources.{0}", name));
            XDocument xDoc = XDocument.Load(stream, LoadOptions.SetLineInfo);
            return xDoc;
        }
    }
}
