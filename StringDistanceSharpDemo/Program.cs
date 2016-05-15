using System;
using System.Linq;

namespace StringDistanceSharpDemo
{
    class Program
    {
        static int EditDistance(string src, string dest)
        {
            int[,] temp = new int[src.Length + 1, dest.Length + 1];
            int i, j;

            temp[0, 0] = 0;
            for (i = 1; i <= src.Length; ++i)
            {
                temp[i, 0] = i;
            }
            for (j = 1; j <= dest.Length; ++j)
            {
                temp[0, j] = j;
            }

            for (i = 1; i <= src.Length; ++i)
            {
                for (j = 1; j <= dest.Length; ++j)
                {
                    if (src[i - 1] == dest[j - 1])
                    {
                        temp[i, j] = temp[i - 1, j - 1];
                    }
                    else
                    {
                        temp[i, j] = new[]
                        {
                            temp[i - 1, j],
                            temp[i, j - 1],
                            temp[i - 1, j - 1]
                        }.Min() + 1;
                    }
                }
            }
            return temp[src.Length, dest.Length];
        }
        static void Main(/*string[] args*/)
        {
            string strA = "efsdfdabcdefgaabcdefgaabcdefgaabcdefgasfabcdefgefsdfdabcdefgaabcdefgaabcdefgaabcdefgasfabcdefg";
            string strB = "efsdfdabcdefgaabcdefgaaefsdfdabcdefgaabcdefgaabcdefgaabcdefgasfabcdabcdefggaabcdefgasfabcdefg";

            Console.WriteLine("The distance is: {0}.", EditDistance(strA, strB));
        }
    }
}
