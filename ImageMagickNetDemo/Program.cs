using System;
using System.Drawing;
using System.IO;
using GhostscriptSharp;
using iTextSharp.text.pdf;

namespace ImageMagickNetDemo
{
    class Program
    {
        static void AddMarginRectangle(String src, String dest, Rectangle rect, int imgHeight, int dpi)
        {
            PdfReader reader = new PdfReader(src);
            //PdfReaderContentParser parser = new PdfReaderContentParser(reader);
            PdfStamper stamper = new PdfStamper(reader, new FileStream(dest, FileMode.OpenOrCreate));
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                //var finder = parser.ProcessContent(i, new TextMarginFinder());
                PdfContentByte cb = stamper.GetOverContent(i);
                //cb.Rectangle(finder.GetLlx(), finder.GetLly(),
                //    finder.GetWidth(), finder.GetHeight());
                cb.Rectangle(PixelsToPoints(rect.X, dpi), PixelsToPoints(imgHeight, dpi)
                    - PixelsToPoints(rect.Y, dpi) - PixelsToPoints(rect.Height, dpi),
                    PixelsToPoints(rect.Width, dpi), PixelsToPoints(rect.Height, dpi));
                cb.Stroke();
            }
            stamper.Close();
            reader.Close();
        }

        static void CropPdfWithRectangleAndHeight(string src, string dest, Rectangle rect, 
            int imgHeight, int dpi)
        {
            var reader = new PdfReader(src);
            var stamper = new PdfStamper(reader, new FileStream(dest, FileMode.OpenOrCreate));
            float llx = PixelsToPoints(rect.X, dpi);
            float lly = PixelsToPoints(imgHeight, dpi)
                        - PixelsToPoints(rect.Y, dpi) - PixelsToPoints(rect.Height, dpi);
            float urx = llx + PixelsToPoints(rect.Width, dpi);
            float ury = lly + PixelsToPoints(rect.Height, dpi);
            PdfRectangle pdfRectangle = new PdfRectangle(llx, lly, urx, ury);
            for (var i = 1; i <= reader.NumberOfPages; ++i)
            {
                var pageDict = reader.GetPageN(i);
                pageDict.Put(PdfName.CROPBOX, pdfRectangle);
            }
            stamper.Close();
            reader.Close();
        }

        public static float PixelsToPoints(float value, int dpi)
        {
            return value / dpi * 72;
        }

        static void Main(string[] args)
        {
            var pdfFilePath = Environment.CurrentDirectory + @"\test.pdf";
            var imgFilePath = Environment.CurrentDirectory + @"\test.jpg";

            GhostscriptWrapper.GeneratePageThumb(pdfFilePath, imgFilePath, 1, 100, 100);

            Bitmap bitmap = new Bitmap(imgFilePath);
            Rgb[,] rgbs = bitmap.ToRgbArray();
            //for (var i = 0; i < rgbs.GetLength(0); ++i)
            //{
            //    for (var j = 0; j < rgbs.GetLength(1); ++j)
            //    {
            //        Console.WriteLine(rgbs[i, j]);
            //    }
            //}
            Console.WriteLine($@"Height: {bitmap.Height}, Width: {bitmap.Width}");
            Console.WriteLine(bitmap.FindBoundRectangle());
            //var blank = new Rgb {Red = 255, Green = 255, Blue = 255};
            //for (var i = 0; i < rgbs.GetLength(1); ++i)
            //{
            //    if (rgbs[500, i] != blank)
            //    {
            //        Console.WriteLine(i);
            //        Console.WriteLine(rgbs.GetLength(1));
            //        break;
            //    }
            //    else
            //    {
            //        Console.Write(rgbs[500, i]);
            //    }
            //} 
            AddMarginRectangle(Environment.CurrentDirectory + @"\test.pdf",
                Environment.CurrentDirectory + @"\test mark.pdf",
                bitmap.FindBoundRectangle(), bitmap.Height, 100);
            CropPdfWithRectangleAndHeight(Environment.CurrentDirectory + @"\test.pdf",
                Environment.CurrentDirectory + @"\test crop.pdf",
                bitmap.FindBoundRectangle(), bitmap.Height, 100);
        }
    }
}
