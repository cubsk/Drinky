using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Codegoggles.Epub
{
    public class ManualTextChunk : IManifestItem
    {
        public string Id { get; protected set; }
        public string MediaType { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }

        public ManualTextChunk(string id)
        {
            this.Id = id;
        }

        public void WriteTo(Stream output)
        {
            using (StreamWriter writer = new StreamWriter(output))
            {
                writer.Write(Content);
            }
        }
    }
}
