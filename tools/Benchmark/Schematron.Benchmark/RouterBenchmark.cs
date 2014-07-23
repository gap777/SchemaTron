using System;
using System.Xml.Linq;
using CBR_prototype;
using CBR_prototype.Validator;

namespace Schematron.Benchmark
{
    class RouterBenchmark
    {
        /// <summary>
        /// Measure performance of selecting the first matching schema for
        /// a document.
        /// </summary>
        /// <param name="routerConfigFileName"></param>
        /// <param name="messageFileName"></param>
        /// <param name="description"></param>
        public static void MeasureRouting(
            string routerConfigFileName,
            string messageFileName,
            string description)
        {
            Console.WriteLine("===== Benchmark of using a validator within a content-based router =====");
            Console.WriteLine();

            Console.WriteLine("Description: {0}", description);
            Console.WriteLine();
            Console.WriteLine("CBR configuration: {0}", routerConfigFileName);
            Console.WriteLine("Routed message: {0}", messageFileName);
            Console.WriteLine();

            XDocument xMessage = XDocument.Load(messageFileName, LoadOptions.SetLineInfo);

            Console.WriteLine("==== XSLT ISO Schematron validator ====");
            Console.WriteLine("Note: full validation is performed.");
            Console.WriteLine();

            MeasureRouter(xMessage, routerConfigFileName, ValidatorImplementation.XSLT);

            Console.WriteLine();

            Console.WriteLine("==== SchemaTron - native C# ISO Schematron validator ====");
            Console.WriteLine("Note: partial validation is performed.");
            Console.WriteLine();

            MeasureRouter(xMessage, routerConfigFileName, ValidatorImplementation.Native);
        }

        private static void MeasureRouter(XDocument xMessage, String routerConfigFileName, ValidatorImplementation implementation)
        {
            CBR router = CBR.Deserialize(routerConfigFileName);
            router.Compile(implementation);

            Meter.MeasureActionByTime(() =>
            {
                String selectedPredicateId = router.Route(xMessage);
            }, "Route");
        }
    }
}
