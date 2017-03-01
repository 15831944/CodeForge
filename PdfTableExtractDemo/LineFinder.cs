using System;
using System.Collections.Generic;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Diagnostics;
using iTextSharp.text;


namespace PdfTableExtractDemo
{
    class LineFinder
    {
        public void ParsePDF(string pdffn)
        {
            PdfReader reader = new PdfReader(pdffn);

            for (int pageNumber = 1; pageNumber <= reader.NumberOfPages; pageNumber++)
            {
                PdfDictionary pg = reader.GetPageN(pageNumber);
                PdfDictionary res = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
                PdfDictionary xobj = (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
                PdfDictionary line = (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.LINE));
                PdfArray cont = (PdfArray)PdfReader.GetPdfObject(pg.Get(PdfName.CONTENTS));

                PdfObject contentObject = pg.Get(PdfName.CONTENTS);
                Listener listener = new Listener();
                PdfContentStreamProcessor processor = new PdfContentStreamProcessor(listener);
                CountOps counter = new CountOps();
                processor.RegisterContentOperator("q", counter);
                processor.RegisterContentOperator("cm", counter);
                processor.RegisterContentOperator("m", counter);
                processor.RegisterContentOperator("l", counter);
                processor.RegisterContentOperator("S", counter);
                processor.RegisterContentOperator("Q", counter);
                processor.RegisterContentOperator("gs", counter);
                processor.ProcessContent(ContentByteUtils.GetContentBytesForPage(reader, 1), res);
            }
        }
    }

    public class CountOps : IContentOperator
    {
        public int operationCount = 0;
        List<string> ops = new List<string>();
        Matrix m = new Matrix();
        float width = 0.0f, height = 0.0f;

        virtual public void Invoke(PdfContentStreamProcessor processor, PdfLiteral anoperator, List<PdfObject> operands)
        {
            ++operationCount;
            Debug.Write("Operation: " + operationCount.ToString() + " ");
            foreach (PdfObject o in operands)
            {
                Debug.Write(o.ToString() + " ");
            }
            Debug.WriteLine("");

            ops.Add(anoperator.ToString());

            //The premise of the switch case is that what we're looking for is always in a particular configuration
            //We know it's a line because it's opcode is in the correct order in the ops List.  So q comes first, then
            // cm, then m, then l, then S, then Q.  If that order matches it's a line that we want.

            //Our lines should look like this
            //q 1 0 0 1 19.96 538.9747 cm //q means save graphics state.  cm means concatenate matrix to the transformation matrix.
            //0 0 m //m means moveto, begins a new subpath
            //716.89 0 l //l means lineto, append straight line segment to path
            //S //S means stroke path
            //Q //Q means restore graphics state

            //The specification 32000_2008 in section 8.3.3 state the following:
            //• Translations shall be specified as [ 1 0 0 1 tx ty ], where tx and ty shall be the distances to 
            //  translate the origin of the coordinate system in the horizontal and vertical dimensions, 
            //  respectively.
            //• Scaling shall be obtained by [ sx  0 0 sy  0 0]. This scales the coordinates so that 1 unit in 
            //  the horizontal and vertical dimensions of the new coordinate system is the same size as sx and sy 
            //  units, respectively, in the previous coordinate system.
            //• Rotations shall be produced by [cos q sin q -sin q cos q 0 0 ], which has the effect of rotating 
            //  the coordinate system axes by an angle q counter clockwise.
            //• Skew shall be specified by [1 tan a tan b 1 0 0], which skews the x axis by an angle a and the 
            //  y axis by an angle b.

            //In the above example tx = 19.96 and ty = 538.9747.  Those numbers are in points.  72 points to an inch.
            //This is the x,y coordinate where the line starts assuming sx an sy are both 1

            //matrix             how     rotation                   translation scale
            //1     0        0   a b 0   cos(angle)  sin(angle) 0    1  0  0     sx 0  0
            //0     1        0   c d 0   -sin(angle) cos(angle) 0    0  1  0     0  sy 0
            //19.96 538.9747 1   e f 1   0           0          1    tx ty 1     0  0  1

            //x1 = a * x + c * y + e
            //y1 = b * x + d * y + f

            //x1 = 1 * x + 0 * y + 19.96
            //y1 = 0 * x + 1 * y + 538.9747

            switch (anoperator.ToString())
            {
                case "q": //save graphics state
                    {
                        //Since q is the beginning of what we're looking for reset every time we get here
                        ops.Clear();
                        ops.Add(anoperator.ToString());

                        //Taken from PdfContentStreamProcessor.cs PushGraphicsState()
                        GraphicsState gs = processor.Gs();
                        //GraphicsState gs = processor.gsStack.Peek();
                        GraphicsState copy = new GraphicsState(gs);
                        //processor.gsStack.Push(copy);
                    }
                    break;
                case "cm": //concat matrix
                    if (ops.Count == 2)
                    {
                        Matrix x = new Matrix(float.Parse(operands[0].ToString()), float.Parse(operands[1].ToString()),
                            float.Parse(operands[2].ToString()), float.Parse(operands[3].ToString()),
                            float.Parse(operands[4].ToString()), float.Parse(operands[5].ToString()));

                        //Taken from PdfContentStreamProcessor.cs ModifyCurrentTransformationMatrix()
                        float a = ((PdfNumber)operands[0]).FloatValue;
                        float b = ((PdfNumber)operands[1]).FloatValue;
                        float c = ((PdfNumber)operands[2]).FloatValue;
                        float d = ((PdfNumber)operands[3]).FloatValue;
                        float e = ((PdfNumber)operands[4]).FloatValue;
                        float f = ((PdfNumber)operands[5]).FloatValue;
                        Matrix matrix = new Matrix(a, b, c, d, e, f);
                        //modified iTextSharp PdfContentStreamProcessor.cs to make gsStack public
                        //GraphicsState gs = processor.gsStack.Peek();
                        //modified iTextSharp GraphicsState.cs to make ctm public
                        //gs.ctm = matrix.Multiply(gs.ctm);

                        //m = x.Multiply(gs.GetCtm());
                        //m = gs.ctm;
                    }
                    else
                        ops.Clear();
                    break;
                case "m": //moveto
                    if (ops.Count == 3)
                    {
                    }
                    else
                        ops.Clear();
                    break;
                case "l": //lineto
                    if (ops.Count == 4)
                    {
                        width = Math.Abs(float.Parse(operands[0].ToString()));
                        height = Math.Abs(float.Parse(operands[1].ToString()));
                    }
                    else
                        ops.Clear();
                    break;
                case "S": //stroke
                    if (ops.Count == 5)
                    {
                    }
                    else
                        ops.Clear();
                    break;
                case "Q": //restore graphics state
                    if (ops.Count == 6)
                    {
                        //This is the end of our line block.  We can do something with this data now, but only now.
                        string StartX = m[6].ToString();
                        string StartY = m[7].ToString();
                        string Length = width.ToString();
                        string Height = height.ToString();
                        Debug.WriteLine("Start X,Y= {0},{1} Length={2} Height={3}", StartX, StartY, Length, Height);
                        Debug.WriteLine(m.ToString());

                        //Reset everything since we're done with the line block.
                        m = new Matrix();
                        width = 0.0f;
                        height = 0.0f;
                        ops.Clear();

                        //Taken from PdfContentStreamProcessor.cs PopGraphicsState()
                        //processor.gsStack.Pop();
                    }
                    else
                        ops.Clear();
                    break;
                case "gs":
                    string o = anoperator.ToString();
                    break;
            }
        }
    }

    public class Listener : IRenderListener
    {
        public void BeginTextBlock()
        {
        }

        public void RenderText(TextRenderInfo info)
        {
            Console.WriteLine(info.GetText());
            List<TextRenderInfo> list = (List<TextRenderInfo>) info.GetCharacterRenderInfos();
            LineSegment ls = info.GetBaseline();
            Console.WriteLine(ls.GetStartPoint().ToString());
            //BaseColor bc = info.GetStrokeColor();
            //Console.WriteLine(bc.ToString());
            Console.WriteLine(info.ToString());
        }

        public void EndTextBlock()
        {
        }

        public void RenderImage(ImageRenderInfo info)
        {
            Matrix matrix = info.GetImageCTM();
            Console.WriteLine(matrix);
            Console.WriteLine(info.ToString());
        }
    }
}
