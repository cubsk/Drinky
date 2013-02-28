using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Codegoggles.Epub
{
    public interface IManifestItem
    {
        string Id { get;}
        string MediaType { get; }
        string FileName { get; set; }
        void WriteTo(Stream output); 
    }
}
