using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace MethodLogger
{
    public enum NotificationType
    {
        Silent,
        Inform,
        Ask
    }


    public abstract class LoggerImplementation
    {
        public abstract void LogError(string error);
    }


    public class ExceptionLogger
    {


        private static string GetExceptionTypeStack(Exception e)
        {
            if (e.InnerException != null)
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(GetExceptionTypeStack(e.InnerException));
                message.AppendLine("   " + e.GetType().ToString());
                return (message.ToString());
            }
            else
            {
                return "   " + e.GetType().ToString();
            }
        }

        private static string GetExceptionMessageStack(Exception e)
        {
            if (e.InnerException != null)
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(GetExceptionMessageStack(e.InnerException));
                message.AppendLine("   " + e.Message);
                return (message.ToString());
            }
            else
            {
                return "   " + e.Message;
            }
        }

        private static string GetExceptionCallStack(Exception e)
        {
            if (e.InnerException != null)
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(GetExceptionCallStack(e.InnerException));
                message.AppendLine("--- 下一个调用栈:");
                message.AppendLine(e.StackTrace);
                return (message.ToString());
            }
            else
            {
                return e.StackTrace;
            }
        }

        private static TimeSpan GetSystemUpTime()
        {
            PerformanceCounter upTime = new PerformanceCounter("System", "System Up Time");
            upTime.NextValue();
            return TimeSpan.FromSeconds(upTime.NextValue());
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

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);



        public static string  LogException(Exception exception)
        {
            try
            {

                if (exception == null)
                    return null;
               

                StringBuilder error = new StringBuilder();



                error.AppendLine("Application:       " + Application.ProductName);
                error.AppendLine("Version:           " + Application.ProductVersion);
                error.AppendLine("Date:              " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                error.AppendLine("Computer name:     " + SystemInformation.ComputerName);
                error.AppendLine("User name:         " + SystemInformation.UserName);
                error.AppendLine("OS:                " + Environment.OSVersion.ToString());
                error.AppendLine("Culture:           " + CultureInfo.CurrentCulture.Name);
                error.AppendLine("Resolution:        " + SystemInformation.PrimaryMonitorSize.ToString());
                error.AppendLine("System up time:    " + GetSystemUpTime());
                error.AppendLine("App up time:       " +
                  (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString());
             
               
                MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
                if (GlobalMemoryStatusEx(memStatus))
                {
                    error.AppendLine("Total memory:      " + memStatus.ullTotalPhys / (1024 * 1024) + "Mb");
                    error.AppendLine("Available memory:  " + memStatus.ullAvailPhys / (1024 * 1024) + "Mb");
                }

                error.AppendLine("");

                error.AppendLine("Exception classes:   ");
                error.Append(GetExceptionTypeStack(exception));
                error.AppendLine("");
                error.AppendLine("Exception messages: ");
                error.Append(GetExceptionMessageStack(exception));

                error.AppendLine("");
                error.AppendLine("Stack Traces:");
                error.Append(GetExceptionCallStack(exception));
                error.AppendLine("");
                //error.AppendLine("Loaded Modules:");
                //Process thisProcess = Process.GetCurrentProcess();
                //foreach (ProcessModule module in thisProcess.Modules)
                //{
                //    try
                //    {
                //        error.AppendLine(module.FileName + " " + module.FileVersionInfo.FileVersion);
                //    }
                //    catch 
                //    {
                //        error.AppendLine(module.FileName);
                //    }
                //}

               return error.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("LOG发生异常 - " + ex.Message);
                return null;
            }
        }
    }
}


