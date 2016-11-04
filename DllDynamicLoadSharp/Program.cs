using System;
using DllDynamicLoadSharp.Rtx.BqqZip;

namespace DllDynamicLoadSharp
{
    class Program
    {
        static void Main(/*string[] args*/)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            Console.WriteLine("Zip first: {0}", 
                BqqZipLib.Zip(Environment.CurrentDirectory + @"\1正面3.jpg",
                Environment.CurrentDirectory + @"\first.zip"));
            Console.WriteLine("Zip the second: {0}",
                BqqZipLib.Zip(Environment.CurrentDirectory + @"\2背面2.jpg",
                Environment.CurrentDirectory + @"\second.zip"));
            Console.WriteLine("UnZip the first: {0}",
                BqqZipLib.UnZip(Environment.CurrentDirectory + @"\first.zip",
                Environment.CurrentDirectory + @"\first.jpg"));
            Console.WriteLine("UnZip the second: {0}",
                BqqZipLib.UnZip(Environment.CurrentDirectory + @"\second.zip",
                Environment.CurrentDirectory + @"\second.jpg"));
        }
    }
}
