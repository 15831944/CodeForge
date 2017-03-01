using iTextSharp.text.pdf.parser;
using System.Collections.Generic;
using System.util;

namespace PdfTableExtractDemo
{
    /**
 * Allows you to find the rectangle that contains all the content in a page.
 * <p>
 * This class is derived from the iText class {@link TextMarginFinder}. While that original class only implements
 * {@link RenderListener} and even there ignores {@link RenderListener#renderImage(ImageRenderInfo)}, this class
 * implements {@link ExtRenderListener} and also respects {@link RenderListener#renderImage(ImageRenderInfo)} calls.
 * 
 * @see TextMarginFinder
 */
    public class MarginFinder : IExtRenderListener
    {
    private RectangleJ _textRectangle;
    private RectangleJ _currentPathRectangle;

    /**
     * Method invokes by the PdfContentStreamProcessor.
     * Passes a TextRenderInfo for every text chunk that is encountered.
     * We'll use this object to obtain coordinates.
     * @see RenderListener#renderText(TextRenderInfo)
     */
    public void RenderText(TextRenderInfo renderInfo)
    {
        if (_textRectangle == null)
            _textRectangle = renderInfo.GetDescentLine().GetBoundingRectange();
        else
            _textRectangle.Add(renderInfo.GetDescentLine().GetBoundingRectange());

        _textRectangle.Add(renderInfo.GetAscentLine().GetBoundingRectange());
    }

    /**
     * Getter for the left margin.
     * @return the X position of the left margin
     */
    public float GetLlx()
    {
        return _textRectangle.X;
    }

    /**
     * Getter for the bottom margin.
     * @return the Y position of the bottom margin
     */
    public float GetLly()
    {
        return _textRectangle.Y;
    }

    /**
     * Getter for the right margin.
     * @return the X position of the right margin
     */
    public float GetUrx()
    {
        return _textRectangle.X + _textRectangle.Width;
    }

    /**
     * Getter for the top margin.
     * @return the Y position of the top margin
     */
    public float GetUry()
    {
        return _textRectangle.Y + _textRectangle.Height;
    }

    /**
     * Gets the width of the text block.
     * @return a width
     */
    public float GetWidth()
    {
        return _textRectangle.Width;
    }

    /**
     * Gets the height of the text block.
     * @return a height
     */
    public float GetHeight()
    {
        return _textRectangle.Height;
    }

    /**
     * @see RenderListener#beginTextBlock()
     */
    public void BeginTextBlock()
    {
        // do nothing
    }

    /**
     * @see RenderListener#endTextBlock()
     */
    public void EndTextBlock()
    {
        // do nothing
    }

    /**
     * @see RenderListener#renderImage(ImageRenderInfo)
     */
    public void RenderImage(ImageRenderInfo renderInfo)
    {
        Matrix imageCtm = renderInfo.GetImageCTM();
        Vector a = new Vector(0, 0, 1).Cross(imageCtm);
        Vector b = new Vector(1, 0, 1).Cross(imageCtm);
        Vector c = new Vector(0, 1, 1).Cross(imageCtm);
        Vector d = new Vector(1, 1, 1).Cross(imageCtm);
        LineSegment bottom = new LineSegment(a, b);
        LineSegment top = new LineSegment(c, d);
        if (_textRectangle == null)
            _textRectangle = bottom.GetBoundingRectange();
        else
            _textRectangle.Add(bottom.GetBoundingRectange());

        _textRectangle.Add(top.GetBoundingRectange());
    }

    //@Override
    public void ModifyPath(PathConstructionRenderInfo renderInfo)
    {
        List<Vector> points = new List<Vector>();
        if (renderInfo.Operation == PathConstructionRenderInfo.RECT)
        {
            float x = renderInfo.SegmentData[0];
            float y = renderInfo.SegmentData[1];
                float w = renderInfo.SegmentData[2];
                float h = renderInfo.SegmentData[3];
                points.Add(new Vector(x, y, 1));
            points.Add(new Vector(x + w, y, 1));
            points.Add(new Vector(x, y + h, 1));
            points.Add(new Vector(x + w, y + h, 1));
        }
        else if (renderInfo.SegmentData != null)
        {
            for (int i = 0; i < renderInfo.SegmentData.Count - 1; i += 2)
            {
                points.Add(new Vector(renderInfo.SegmentData[i], renderInfo.SegmentData[i + 1], 1));
            }
        }

        foreach (Vector pt in points)
        {
            var point = pt.Cross(renderInfo.Ctm);
            RectangleJ pointRectangle = new RectangleJ(point[Vector.I1], point[Vector.I2], 0, 0);
            if (_currentPathRectangle == null)
                _currentPathRectangle = pointRectangle;
            else
                _currentPathRectangle.Add(pointRectangle);
        }
    }

    //@Override
    public Path RenderPath(PathPaintingRenderInfo renderInfo)
    {
        if (renderInfo.Operation != PathPaintingRenderInfo.NO_OP)
        {
            if (_textRectangle == null)
                _textRectangle = _currentPathRectangle;
            else
                _textRectangle.Add(_currentPathRectangle);
        }
        _currentPathRectangle = null;

        return null;
    }

    //@Override
    public void ClipPath(int rule)
    {
        // TODO Auto-generated method stub

    }
}
}
