namespace SchemaTron.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SchemaTron;
    using Xunit;
    using System.IO;
    using System.IO.Compression;
    using System.Xml.Linq;

    public class FileInclusionResolverTest
    {
        // Note: FileInclusionResolver is internal and access to it
        // must have been allowed explicitly.

        [Fact]
        public void ResolveNullHref()
        {
            FileInclusionResolver resolver = new FileInclusionResolver();
            Assert.Throws<ArgumentNullException>(() => resolver.Resolve(null));
        }

        [Fact]
        public void ResolveExistingFile()
        {
            FileInclusionResolver resolver = new FileInclusionResolver();
            Assert.DoesNotThrow(() => resolver.Resolve(@"..\..\Resources\basics_sch.xml"));
        }

        [Fact]
        public void ResolveUri()
        {
            FileInclusionResolver resolver = new FileInclusionResolver();
            Assert.DoesNotThrow(() => resolver.Resolve("http://www.w3schools.com/xml/note.xml"));
        }
        
        [Fact]
        public void ResolveGzippedFileWithCustomResolver()
        {
            string href = @"..\..\Resources\basics_sch.xml.gz";
            var resolver = new GzipInclusionResolver();
            XDocument xdoc = resolver.Resolve(href);
            Console.WriteLine(xdoc);
        }

        private class GzipInclusionResolver : IInclusionResolver
        {
            #region IInclusionResolver Members

            public XDocument Resolve(string href)
            {
                using (FileStream fs = new FileStream(href, FileMode.Open))
                {
                    Stream stream = fs;
                    if (href.EndsWith(".gz"))
                    {
                        stream = new GZipStream(stream, CompressionMode.Decompress);
                    }
                    return XDocument.Load(stream, LoadOptions.SetLineInfo);
                }
            }

            #endregion
        }
    }
}
