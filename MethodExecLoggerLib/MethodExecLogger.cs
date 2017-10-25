using System;
using System.Collections.Generic;
using System.Text;

namespace MethodExecLoggerLib
{
   public class MethodExecLogger
    {
        public static void MethodStarted(string typeName,
                   string methodName, string args)
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString()+"Start of " + typeName + "." +
                              methodName + "(" + args + ")");
        }

        public static void MethodCompleted(string typeName,
                           string methodName, string args)
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString() + "End of " + typeName + "." +
                              methodName + "(" + args + ")");
        }
    }
}
