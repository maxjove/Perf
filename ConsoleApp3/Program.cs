﻿

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
           
                Mateinfo mi = new Mateinfo();
                mi.funA("");
                mi.funB();
               
          
           
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
