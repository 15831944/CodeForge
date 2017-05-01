using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageMagickNetDemo
{
    public static class BitmapExtentions
    {
        public static Rgb[,] ToRgbArray(this Bitmap bitmap)
        {
            try
            {
                int height = bitmap.Height, width = bitmap.Width;
                var rgbMatrix = new Rgb[height, width];
                var rect = new Rectangle(0, 0, width, height);
                var bmpData = bitmap.LockBits(rect, ImageLockMode.ReadWrite, bitmap.PixelFormat);
                IntPtr iPtr = bmpData.Scan0;
                //var bytesCnt = height*Math.Abs(bmpData.Stride);
                //byte[] pixelValues = new byte[bytesCnt];
                //System.Runtime.InteropServices.Marshal.Copy(iPtr, pixelValues, 0, bytesCnt);
                //bitmap.UnlockBits(bmpData);
                //var pointIdx = 0;
                for (var i = 0; i < height; ++i)
                {
                    var pixelValues = new byte[bmpData.Stride];
                    System.Runtime.InteropServices.Marshal.Copy(iPtr + i*bmpData.Stride, 
                        pixelValues, 0, bmpData.Stride);
                    for (var j = 0; j < width; ++j)
                    {
                        //rgbMatrix[i, j].Blue = pixelValues[pointIdx++];
                        //rgbMatrix[i, j].Green = pixelValues[pointIdx++];
                        //rgbMatrix[i, j].Red = pixelValues[pointIdx++];
                        rgbMatrix[i, j].Blue = pixelValues[j * 3];
                        rgbMatrix[i, j].Green = pixelValues[j * 3 + 1];
                        rgbMatrix[i, j].Red = pixelValues[j * 3 + 2];
                    }
                }
                bitmap.UnlockBits(bmpData);
                return rgbMatrix;
            }
            catch
            {
                return null;
            }
        }

        public static Rectangle FindBoundRectangle(this Bitmap bitmap)
        {
            var rgbs = bitmap.ToRgbArray();
            int height = rgbs.GetLength(0), width = rgbs.GetLength(1);
            int xCenter = width / 2, yCenter = height / 2;
            int left = 0, right = width - 1, bottom = height - 1, top = 0;
            for (var i = 0; i < xCenter; ++i)
            {
                if (rgbs[yCenter, i] == rgbs[yCenter, i + 1]) continue;
                left = i;
                break;
            }
            for (var j = width - 1; j > xCenter; --j)
            {
                if (rgbs[yCenter, j] == rgbs[yCenter, j - 1]) continue;
                right = j;
                break;
            }
            for (var k = 0; k < yCenter; ++k)
            {
                if (rgbs[k, xCenter] == rgbs[k + 1, xCenter]) continue;
                top = k;
                break;
            }
            for (var l = height - 1; l > yCenter; --l)
            {
                if (rgbs[l, xCenter] == rgbs[l - 1, xCenter]) continue;
                bottom = l;
                break;
            }
            var rect = new Rectangle(left, top, right - left + 1, bottom - top + 1);
            return rect;
        }
    }

    public struct Rgb
    {
        public bool Equals(Rgb other)
        {
            return Red == other.Red && Green == other.Green && Blue == other.Blue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Rgb && Equals((Rgb) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Red;
                hashCode = (hashCode*397) ^ Green;
                hashCode = (hashCode*397) ^ Blue;
                return hashCode;
            }
        }

        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public static bool operator ==(Rgb left, Rgb right)
        {
            return left.Red == right.Red && left.Green == right.Green && left.Blue == right.Blue;
        }

        public static bool operator !=(Rgb left, Rgb right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return $@"({Red}, {Green}, {Blue})";
        }
    }
}
