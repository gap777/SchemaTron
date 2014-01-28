namespace Schematron.XsltValidator.App.Resources
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
            string resourceName = string.Format("Schematron.XsltValidator.App.Resources.{0}", name);
            Stream stream = currentAssembly.GetManifestResourceStream(resourceName);
            return XDocument.Load(stream, LoadOptions.SetLineInfo);
        }
    }
}
