using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using CBR_prototype;
using System.Diagnostics;

namespace Schematron.Benchmark
{
    /// <summary>
    /// Measures the performance of Schematron validators.
    /// </summary>
    /// <remarks>
    /// Measures the average time to create a validator from the schema,
    /// to validate an XML document and to match to document to a schema
    /// within a content-based router.
    /// </remarks>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Date: {0}", DateTime.Now);
            Console.WriteLine();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Reset();
            stopwatch.Start();

            MeasureCreatingValidator();

            MeasureValidation();

            MeasureRouting();

            stopwatch.Stop();
            Console.WriteLine("Total elapsed time: {0:0.###} sec", stopwatch.ElapsedMilliseconds / 1000.0);
        }

        private static void MeasureCreatingValidator()
        {
            ValidatorBenchmark.MeasureCreatingValidator(
                @"XML\schema_envelope.xml",
                "Simple schema describing a SOAP envelope.");
        }

        private static void MeasureValidation()
        {
            ValidatorBenchmark.MeasureValidation(
                @"XML\schema_envelope.xml",
                @"XML\doc_envelope_valid.xml",
                "Simple schema and valid document."
                + " Both validator must perform full validation.");

            ValidatorBenchmark.MeasureValidation(
                @"XML\schema_envelope.xml",
                @"XML\doc_envelope_invalid_further.xml",
                "Simple schema and invalid document with violations further inside the document."
                + " SchemaTron with partial validation can stop at the first assert.");

            ValidatorBenchmark.MeasureValidation(
               @"XML\schema_envelope.xml",
               @"XML\doc_envelope_bad_root.xml",
               "Simple schema and invalid document with a violation at the first element."
               + " SchemaTron with partial validation can stop right at the first element.");
        }

        private static void MeasureRouting()
        {
            RouterBenchmark.MeasureRouting(
                @"XML\CBR\CBR_prototype.xml",
                @"XML\CBR\message_1.xml",
                "Simple router, schema and message. Matching a price within an interval."
                + " The first of four predicates should be matched.");

            RouterBenchmark.MeasureRouting(
                @"XML\CBR\CBR_prototype.xml",
                @"XML\CBR\message_2.xml",
                "Simple router, schema and message. Matching a price within an interval."
                + " The second of four predicates should be matched.");

            RouterBenchmark.MeasureRouting(
                @"XML\CBR\CBR_prototype.xml",
                @"XML\CBR\message_3.xml",
                "Simple router, schema and message. Matching a price within an interval."
                + " The third of four predicates should be matched.");

            RouterBenchmark.MeasureRouting(
                @"XML\CBR\CBR_prototype.xml",
                @"XML\CBR\message_4.xml",
                "Simple router, schema and message. Matching a price within an interval."
                + " The last of four predicates should be matched.");

            RouterBenchmark.MeasureRouting(
                @"XML\CBR\CBR_prototype.xml",
                @"XML\CBR\message_invalid.xml",
                "Message has invalid root element.");
        }

        //private static void MeasureCreatingXSLTValidator()
        //{
        //    XDocument xSchema = XDocument.Load(@"XML\schema_envelope.xml");
        //    Schematron.XsltValidator.Validator xsltValidator = Schematron.XsltValidator.Validator.CreateWithMeasurement(xSchema);
        //}
    }
}
