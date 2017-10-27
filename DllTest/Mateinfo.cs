using System;
using System.Collections.Generic;
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

        public void funB()
        {
            Console.WriteLine("这是方法B");
            //LogManager.WriteErrorLog(@"C:\Kingstar Winning\Log\" + this.Name, string.Format("{2}方法 {0}开始执行时间{1}", MethodInfo.GetCurrentMethod().Name, DateTime.Now.ToString(), this.Name));
        }

    }
}
