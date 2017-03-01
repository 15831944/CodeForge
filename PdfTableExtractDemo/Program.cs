using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PdfTableExtractDemo
{
    class Program
    {
        static readonly string _filePath = System.Environment.CurrentDirectory + @"\test.pdf";
        public static List<String> Read()
        {
            var pdfReader = new PdfReader(_filePath);
            var pages = new List<String>();

            for (int i = 0; i < pdfReader.NumberOfPages; i++)
            {
                string textFromPage = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, pdfReader.GetPageContent(i + 1)));
                
                pages.Add(GetDataConvertedData(textFromPage));
                //pages.AddRange(textFromPage.Split(new[] { "\n" }, StringSplitOptions.None)
                //                    .Where(text => text.Contains("Tj")).ToList());
                //pages.Add(textFromPage);
            }

            return pages;
        }

        static string GetDataConvertedData(string textFromPage)
        {
            var texts = textFromPage.Split(new[] { "\n" }, StringSplitOptions.None)
                                    .Where(text => text.Contains("TJ")).ToList();

            return texts.Aggregate(string.Empty, (current, t) => current +
                       t.TrimStart('(')
                        .TrimEnd('J')
                        .TrimEnd('T')
                        .TrimEnd(')'));
        }

        static void AddMarginRectangle(String src, String dest)
        {
            PdfReader reader = new PdfReader(src);
            PdfReaderContentParser parser = new PdfReaderContentParser(reader);
            PdfStamper stamper = new PdfStamper(reader, new FileStream(dest, FileMode.OpenOrCreate));
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                var finder = parser.ProcessContent(i, new TextMarginFinder());
                PdfContentByte cb = stamper.GetOverContent(i);
                cb.Rectangle(finder.GetLlx(), finder.GetLly(),
                    finder.GetWidth(), finder.GetHeight());
                cb.Stroke();
            }
            stamper.Close();
        }

        static void Main(string[] args)
        {
            List<string> tableStrs = Read();
            Console.WriteLine(tableStrs.Count);
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(System.Environment.CurrentDirectory + @"\tableContent.txt"))
            {
                foreach (string line in tableStrs)
                {
                    file.WriteLine(line);
                }
            }
            string tmp = "TJ[(2)-5(3)-5(,)-5(5)6(9)-5(2)-5(.)6(5)-5(5)]";
            var regex = new Regex(@"\(.*?\)");
            var matches = regex.Matches(tmp);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Match match in matches)
            {
                stringBuilder.Append(match.Value.Replace("(", "").Replace(")", ""));
            }
            Console.WriteLine(stringBuilder.ToString());
            AddMarginRectangle(Environment.CurrentDirectory + @"\Bluetooth-Quic-Ref-Guide.pdf", 
                Environment.CurrentDirectory + @"\Rect.pdf");
            new LineFinder().ParsePDF(Environment.CurrentDirectory + @"\081114_DFW_SG_PG3_V01.pdf");
        }
    }
}
