using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

using SchemaTron;

namespace SchemaTron.Console
{
    /// <summary>
    /// Console front-end of the SchemaTron validator.
    /// </summary>
    /// <remarks>
    /// It can validate one or more documents at once using a single
    /// Schematron schema. Validation results including details of violated
    /// assertions can be displayed. A particular schema phase can be
    /// selected.
    /// </remarks>
    public class Program
    {
        string schemaFileName;
        IList<string> documentFileNames;
        string phase;

        bool fullValidationEnabled;
        bool areViolationsVerbose;
        bool isProcessingVerbose;

        static void Main(string[] args)
        {
            Program program = new Program();
            try
            {
                program.Run(args);
            }
            catch (Exception ex)
            {
                System.Console.Error.WriteLine("Error: " + ex.Message);
            }
        }

        private void Run(string[] args)
        {
            ParseArguments(args);

            if (isProcessingVerbose)
            {
                System.Console.WriteLine("Loading schema: " + schemaFileName);
            }
            XDocument schema = LoadXDocument(schemaFileName);

            Validator validator = CreateValidator(phase, schema);

            foreach (var documentFileName in documentFileNames)
            {
                if (areViolationsVerbose)
                {
                    System.Console.WriteLine();
                }
                if (isProcessingVerbose)
                {
                    System.Console.WriteLine("Loading document: " + documentFileName);
                }
                XDocument document = LoadXDocument(documentFileName);
                System.Console.WriteLine(string.Format(
                    "Performing {0} validation of document '{1}'.",
                    (fullValidationEnabled ? "full" : "partial"),
                    documentFileName));
                ValidatorResults results = validator.Validate(document, fullValidationEnabled);

                PrintResults(results);
            }
        }

        private XDocument LoadXDocument(string fileName)
        {
            var settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse };
            XmlReader reader = XmlReader.Create(fileName, settings);
            XDocument data = XDocument.Load(reader, LoadOptions.SetLineInfo);
            return data;
        }

        private Validator CreateValidator(string phase, XDocument schema)
        {
            ValidatorSettings validatorSettings = new ValidatorSettings();
            if (!string.IsNullOrWhiteSpace(phase))
            {
                validatorSettings.Phase = phase;
            }
            if (isProcessingVerbose)
            {
                System.Console.WriteLine(string.Format(
                    "Creating the validator with phase '{0}'.",
                    validatorSettings.Phase));
            }
            Validator validator = Validator.Create(schema, validatorSettings);
            return validator;
        }

        private void PrintResults(ValidatorResults results)
        {
            System.Console.WriteLine("Validation result: the document " + (results.IsValid ? "IS" : "is NOT") + " valid.");
            if (!results.IsValid)
            {
                System.Console.WriteLine(string.Format("Violated assertions ({0}):",
                    results.ViolatedAssertions.Count()));
                int index = 0;
                foreach (var assertion in results.ViolatedAssertions
                    .OrderBy((info) => info.LinePosition)
                    .OrderBy((info) => info.LineNumber))
                {
                    index++;
                    if (areViolationsVerbose)
                    {
                        System.Console.WriteLine(string.Format(
@"
User message: {0}
Line number: {2}, Line position: {3}
Assertion type: {1}
XPath location: {4}
Pattern id: '{5}'
Rule id: '{7}'
Rule context: {6}",
                        assertion.UserMessage,
                        assertion.IsReport ? "report" : "assert",
                        assertion.LineNumber,
                        assertion.LinePosition,
                        assertion.Location,
                        assertion.PatternId,
                        assertion.RuleContext,
                        assertion.RuleId));
                    }
                    else
                    {
                        System.Console.WriteLine(string.Format("[{0}]: {1}", index, assertion.UserMessage));
                    }
                }
            }
        }

        private void ParseArguments(string[] args)
        {
            List<string> arguments = args.ToList();
            fullValidationEnabled = true;
            areViolationsVerbose = false;
            isProcessingVerbose = false;
            phase = null;

            string partialFlag = "-p";
            if (arguments.Contains(partialFlag))
            {
                fullValidationEnabled = false;
                arguments.Remove(partialFlag);
            }
            string verboseViolationsFlag = "-v";
            if (arguments.Contains(verboseViolationsFlag))
            {
                areViolationsVerbose = true;
                arguments.Remove(verboseViolationsFlag);
            }
            string verboseFlag = "-vv";
            if (arguments.Contains(verboseFlag))
            {
                areViolationsVerbose = true;
                isProcessingVerbose = true;
                arguments.Remove(verboseFlag);
            }
            phase = arguments.FirstOrDefault((arg) => arg.StartsWith("--phase="));
            if (phase != null)
            {
                arguments.Remove(phase);
                phase = phase.Replace("--phase=", "");
            }

            if (arguments.Count < 2)
            {
                PrintUsage();
                Environment.Exit(-1);
            }
            schemaFileName = arguments[0];
            documentFileNames = new List<string>(arguments.Count - 1);
            for (int i = 1; i < arguments.Count; i++)
            {
                documentFileNames.Add(arguments[i]);
            }
        }

        private static void PrintUsage()
        {
            System.Console.WriteLine(
@"Usage: SchemaTron.Console.exe [options] SCHEMA DOCUMENTS
SCHEMA - file name or URI of the Schematron schema to be used for validation
DOCUMENTS - file names or URIs of a documents to be validated
            (separated by whitespace)
Options:
  --phase=PHASE - select schema phase  
  -p - disables full validation (stops on the first violated assertion)
  -v - print more information on assertion violations
  -vv - be more vebose overall");
        }
    }
}
