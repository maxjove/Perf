

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开始运行");
            try
            {
                Mateinfo mi = new Mateinfo();
                mi.funA("");
                mi.funB();
                Mateinfo mi2 = new Mateinfo();
                mi2.funA("");
                mi2.funB();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message+ex.StackTrace);
            }
           
            Console.ReadLine();
        }
    }
    class Mateinfo
    {
        string Name ="住院记账";
        public void funA(string hd)
        {
            //MethodExecLogger.MethodStarted("ConsoleApp3.Mateinfo", "funA", "System.String hd2");

            //MethodExecutionLogger.MethodStarted("ConsoleApp3.Mateinfo", "funA", "System.String hd");
            //LogManager.WriteErrorLog(@"C:\Kingstar Winning\Log\" + this.Name, string.Format("{2}方法 {0}开始执行时间{1}", MethodInfo.GetCurrentMethod().Name, DateTime.Now.ToString(),this.Name));
            Console.WriteLine("这是方法A");
           // MethodExecutionLogger.MethodCompleted("ConsoleApp3.Mateinfo", "funA", "System.String hd");
        }
        
        public void funB()
        {
            Console.WriteLine("这是方法B");
            //LogManager.WriteErrorLog(@"C:\Kingstar Winning\Log\" + this.Name, string.Format("{2}方法 {0}开始执行时间{1}", MethodInfo.GetCurrentMethod().Name, DateTime.Now.ToString(), this.Name));
        }

    }
    class LogManager
    {
        public static void WriteErrorLog(string filePath, string message)
        {
            try
            {
                Console.WriteLine(message);
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string path = filePath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
                string value = string.Concat(new object[]
                {
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:"),
            DateTime.Now.Millisecond,
            "  ",
            message
                });
                StreamWriter streamWriter = File.AppendText(path);
                streamWriter.WriteLine(value);
                streamWriter.Close();
            }
            catch
            {
            }
        }

    }
    //public class MethodExecLogger
    //{
    //    public static void MethodStarted(string typeName,
    //                       string methodName, string args)
    //    {
    //        Console.WriteLine("Start of " + typeName + "." +
    //                          methodName + "(" + args + ")");
    //    }

    //    public static void MethodCompleted(string typeName,
    //                       string methodName, string args)
    //    {
    //        Console.WriteLine("End of " + typeName + "." +
    //                          methodName + "(" + args + ")");
    //    }
    //}

}
