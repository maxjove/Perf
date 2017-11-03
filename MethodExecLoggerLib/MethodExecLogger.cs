using MethodLogger;
using System;
using System.Collections.Generic;
using System.Text;

namespace MethodExecLoggerLib
{
    public class MethodExecLogger
    {
        public static void MethodStarted(string strGuid, string typeName,
                   string methodName, string args)
        {
            /// Console.WriteLine("Invok Pk:" + strGuid);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " Start of " + typeName + "." +
                              methodName + "(" + args + ")");
        }

        public static void MethodCompleted(string strGuid, string typeName,
                           string methodName, string args)
        {
            // Console.WriteLine("Invok Pk:" + strGuid);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + " End of " + typeName + "." +
                              methodName + "(" + args + ")");
        }

        public static void LogException(Exception ex)
        {
            Console.WriteLine(ExceptionLogger.LogException(ex));
        }
    }
}
