using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace Codegoggles.Epub
{
    public class EpubBuilder
    {
        protected static string MetaFolder = "META-INF";
        protected static string ContentFolder = "OEBPS";
        protected static string MimeTypeName = "mimetype";

        protected Dictionary<string, IManifestItem> Content { get; set; }

        public string BookUniqueIdentifier { get; set; }

        public EpubBuilder()
        {
            this.Content = new Dictionary<string, IManifestItem>();
        }
        public void Build()
        {
            using (ZipFile zip = new ZipFile())
            {
                zip.EmitTimesInWindowsFormatWhenSaving = false;
                
                // add the mime type header file
                AddMimeType(zip);

                // create our directory structure
                var metaDirectory = zip.AddDirectoryByName(MetaFolder);
                var contentFolder = zip.AddDirectoryByName(ContentFolder);

                // create our container file
                CreateContainer(zip);
                // create content listing file
                CreateContent(zip);

                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
                zip.Save(@"C:\temp\EpubTest\test.epub");
            }

        }

        protected void AddMimeType(ZipFile zip)
        {
            using (MemoryStream s = new MemoryStream())
            {
                using (StreamWriter writer = new StreamWriter(s))
                {
                    writer.Write("application/epub+zip");
                }

                zip.AddEntry(MimeTypeName, s.ToArray());
                return ;
            }
        }

        protected void CreateContainer(ZipFile zip)
        {
            XNamespace nameSpace = XNamespace.Get("urn:oasis:names:tc:opendocument:xmlns:container");
            
            XDocument doc = new XDocument();

            XElement container = new XElement(nameSpace + "container");
            doc.Add(container);
            container.SetAttributeValue("xmlns", "urn:oasis:names:tc:opendocument:xmlns:container");
            container.SetAttributeValue("version", "1.0");

            XElement rootFiles = new XElement(nameSpace + "rootfiles");
            
            container.Add(rootFiles);
            XElement rootFile = new XElement(nameSpace + "rootfile");
            rootFiles.Add(rootFile);

            rootFile.SetAttributeValue("full-path", ContentFolder + @"/content.opf");
            rootFile.SetAttributeValue("media-type", @"application/oebps-package+xml");



            StringBuilder outputXml = new StringBuilder();
            outputXml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;

            using (XmlWriter writer = XmlWriter.Create(outputXml, settings))
            {
                doc.WriteTo(writer);
            }

            string xml = outputXml.ToString();

            zip.AddEntry(MetaFolder + @"\container.xml", xml);

            /*
            <?xml version="1.0" encoding="UTF-8"?>
            <container xmlns="urn:oasis:names:tc:opendocument:xmlns:container" version="1.0">
              <rootfiles>
                <rootfile full-path="Miev_9780345464521_epub_opf_r1.opf" media-type="application/oebps-package+xml"/>
              </rootfiles>
            </container>
            */

        }

        protected void CreateContent(ZipFile zip)
        {
            XNamespace opf = XNamespace.Get(@"http://www.idpf.org/2007/opf");
            XNamespace dc = XNamespace.Get(@"http://purl.org/dc/elements/1.1/");

            XDocument doc = new XDocument();

            XElement container = new XElement(opf + "package");
            doc.Add(container);
            container.SetAttributeValue("unique-identifier", "BookID");
            container.SetAttributeValue("version", "2.0");
            
            XElement metadata = new XElement( opf + "metadata",
                new XAttribute(XNamespace.Xmlns + "dc", dc.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "opf", opf.NamespaceName)
            );

            container.Add(metadata);

            metadata.Add(new XElement(dc + "title", "Sample Title"));
            metadata.Add(new XElement(dc + "creator", "Creator of book", new XAttribute(opf + "role", "aut") ));
            metadata.Add(new XElement(dc + "language", "en-US"));
            metadata.Add(new XElement(dc + "rights", "All rights reserved"));
            metadata.Add(new XElement(dc + "publisher", "Drinky"));
            metadata.Add(new XElement(
                dc + "identifier", 
                BookUniqueIdentifier, 
                new XAttribute("id", "BookID"),
                new XAttribute(opf + "scheme", "UUID")
                )
            );

            XElement manifest = new XElement(opf + "manifest");
            foreach (var manifestItem in Content)
            {
                using (MemoryStream contentStream = new MemoryStream())
                {
                    manifestItem.Value.WriteTo(contentStream);

                    zip.AddEntry(ContentFolder + @"\" + manifestItem.Value.FileName, contentStream.ToArray());
                }
                manifest.Add(new XElement(opf + "item",
                    new XAttribute("id", manifestItem.Value.Id),
                    new XAttribute("href", manifestItem.Value.FileName),
                    new XAttribute("media-type",  manifestItem.Value.MediaType)
                ));
            }
            container.Add(manifest);


            XElement spine = new XElement(opf + "spine");
            spine.Add(new XAttribute("toc", "ncx"));
            container.Add(spine);

            StringBuilder outputXml = new StringBuilder();
            outputXml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            // output xml to string
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.NamespaceHandling = NamespaceHandling.OmitDuplicates;
            settings.Encoding = Encoding.UTF8;

            using (XmlWriter writer = XmlWriter.Create(outputXml, settings))
            {
                doc.WriteTo(writer);
            }

            string xml = outputXml.ToString();

            zip.AddEntry(ContentFolder + @"/content.opf", xml);
        }

        public ManualTextChunk CreateTextChunk(string id)
        {
            ManualTextChunk chunk = new ManualTextChunk(id);
            chunk.MediaType = "application/xhtml+xml";
            chunk.FileName = id + ".xhtml";
            RegisterContent(chunk);
            return chunk;

        }

        protected void RegisterContent(IManifestItem contentItem)
        {
            Content[contentItem.Id] = contentItem;
        }
    }
}
