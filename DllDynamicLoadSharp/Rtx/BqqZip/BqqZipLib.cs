using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DllDynamicLoadSharp.Rtx.BqqZip
{
    public class BqqZipLib
    {
        [DllImport("BqqZip.dll", EntryPoint = "Zip")]
        public static extern int Zip(
            [MarshalAs(UnmanagedType.LPWStr)]
            string srcFile,
            [MarshalAs(UnmanagedType.LPWStr)]
            string dstFile);

        [DllImport("BqqZip.dll", EntryPoint = "UnZip")]
        public static extern int UnZip(
            [MarshalAs(UnmanagedType.LPWStr)]
            string srcFile,
            [MarshalAs(UnmanagedType.LPWStr)]
            string dstFile);
    }
}
