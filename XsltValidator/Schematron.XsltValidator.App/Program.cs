using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Schematron.XsltValidator;
using Schematron.XsltValidator.App.Resources;

namespace Schematron.XsltValidator.App
{
    class Program
    {
        // sample

        static void Main(string[] args)
        {
            XDocument xSchema = XDocument.Load(args[0]);
            Validator validator = Validator.Create(xSchema);

            XDocument xDocument = XDocument.Load(args[1]);
            XDocument xResult = validator.Validate(xDocument);
            Console.WriteLine(xResult.ToString());
        }
    }
}
