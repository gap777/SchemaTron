using System;
using System.Xml.Linq;

namespace Schematron.Benchmark
{
    class ValidatorBenchmark
    {
        public static void MeasureValidation(
            string schemaFileName,
            string validatedDocument,
            string description)
        {
            Console.WriteLine("===== Benchmark of validating =====");
            Console.WriteLine();

            Console.WriteLine("Description: {0}", description);
            Console.WriteLine();
            Console.WriteLine("Schema: {0}", schemaFileName);
            Console.WriteLine("Validated document: {0}", validatedDocument);
            Console.WriteLine();

            XDocument xSchema = XDocument.Load(schemaFileName);
            XDocument xDocument = XDocument.Load(validatedDocument);

            Console.WriteLine("==== XSLT ISO Schematron validator ====");
            Console.WriteLine();

            Schematron.XsltValidator.Validator xsltValidator = Schematron.XsltValidator.Validator.Create(xSchema);

            Meter.MeasureActionByTime(() =>
            {
                xsltValidator.Validate(xDocument);
            }, "Validate - full validation");

            Console.WriteLine("==== SchemaTron - native C# ISO Schematron validator ====");
            Console.WriteLine();

            SchemaTron.Validator nativeValidator = SchemaTron.Validator.Create(xSchema);

            Meter.MeasureActionByTime(() =>
            {
                nativeValidator.Validate(xDocument, true);
            }, "Validate - full validation");


            Meter.MeasureActionByTime(() =>
            {
                nativeValidator.Validate(xDocument, false);
            }, "Validate - partial validation");

            Console.WriteLine();
        }

        public static void MeasureCreatingValidator(string schemaFileName, string description)
        {
            Console.WriteLine("===== Benchmark of creating validator =====");
            Console.WriteLine();

            Console.WriteLine("Description: {0}", description);
            Console.WriteLine();
            Console.WriteLine("Schema: {0}", schemaFileName);
            Console.WriteLine();

            XDocument xSchema = XDocument.Load(schemaFileName);

            Console.WriteLine("==== XSLT ISO Schematron validator ====");
            Console.WriteLine();

            Meter.MeasureActionByTime(() =>
            {
                Schematron.XsltValidator.Validator.Create(xSchema);
            }, "Create a validator");

            Console.WriteLine("==== SchemaTron - native C# ISO Schematron validator ====");
            Console.WriteLine();

            Meter.MeasureActionByTime(() =>
            {
                SchemaTron.Validator.Create(xSchema);
            }, "Create a validator");

            Console.WriteLine();
        }
    }
}
