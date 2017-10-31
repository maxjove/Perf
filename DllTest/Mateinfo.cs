using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DllTest
{
    class Mateinfo
    {
        public static void GH()
        {
            Console.WriteLine("这是方法GH()");
            string strnul = null;
            strnul.ToLower();
            Console.WriteLine(strnul);
        }
        string Name = "住院记账";
        public void funA(string hd)
        {
            //MethodExecLogger.MethodStarted("ConsoleApp3.Mateinfo", "funA", "System.String hd2");

            //MethodExecutionLogger.MethodStarted("ConsoleApp3.Mateinfo", "funA", "System.String hd");
            //LogManager.WriteErrorLog(@"C:\Kingstar Winning\Log\" + this.Name, string.Format("{2}方法 {0}开始执行时间{1}", MethodInfo.GetCurrentMethod().Name, DateTime.Now.ToString(),this.Name));
            Console.WriteLine("这是方法A");
            string strnul = null;
            strnul.ToLower();
            Console.WriteLine(strnul);


            // MethodExecutionLogger.MethodCompleted("ConsoleApp3.Mateinfo", "funA", "System.String hd");
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public MEMORYSTATUSEX()
            {
                this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        public void funB()
        {
            Console.WriteLine("这是方法B");
            //LogManager.WriteErrorLog(@"C:\Kingstar Winning\Log\" + this.Name, string.Format("{2}方法 {0}开始执行时间{1}", MethodInfo.GetCurrentMethod().Name, DateTime.Now.ToString(), this.Name));
        }
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

    }
}
