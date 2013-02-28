using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drinky.DAC;
using Codegoggles.Epub;
using NHibernate;
using NHibernate.Linq;

namespace Drinky.TestHarness
{
    public class Program
    {
        static void Main(string[] args)
        {
            var session = SessionManager.GetSession();
            var results = session.Query<UserInfo>().ToList();

            /*
            System.IO.File.Delete(@"C:\temp\EpubTest\test.epub");

            EpubBuilder epubBuilder = new EpubBuilder();
            epubBuilder.BookUniqueIdentifier = "Drinky - Export";

            var firstSection = epubBuilder.CreateTextChunk("first_section");
            firstSection.Content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\"><html><head><title>test</title></head><body>aaaa</body></html>";

            var secondSection = epubBuilder.CreateTextChunk("second_section");
            secondSection.Content = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><html xmlns=\"http://www.w3.org/1999/xhtml\" xml:lang=\"en\"><html><head><title>test 2</title></head><body>bbbbbbbbbbbbbbbbbbb</body></html>";



            epubBuilder.Build();
             * */




        }
    }
}
