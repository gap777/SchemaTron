using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SchemaTron")]
[assembly: AssemblyDescription("The native ISO Schematron validator over XPath 1.0 query language binding.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("The XRouter Team")]
[assembly: AssemblyProduct("SchemaTron")]
[assembly: AssemblyCopyright("Copyright © The XRouter Team 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("79216316-bd85-426b-bd93-269be2a4348e")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]


// Explicitly allow unit test assembly to access internal classes and members.
[assembly: InternalsVisibleToAttribute("SchemaTron.Test")]
