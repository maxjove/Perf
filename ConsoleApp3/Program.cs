

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Data;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开始运行");
           
                Mateinfo mi = new Mateinfo();
              //Console.WriteLine("funA返回值:"+mi.funA(""));
            Console.WriteLine("funA返回值:" + mi.funA(""));
            mi.funB();
          
               
          
           
            Console.ReadLine();
        }
    }
    class Mateinfo
    {
        private DataTable dtCacheItem = new DataTable();
        private DataRow GetMedicine(string xmdm, string dxmdm)
        {
            DataRow drresault = null;
            try
            {
                if (xmdm != "" && dxmdm != "")
                {
                    string query = String.Format("medicineid='{0}' and MedicineCategoryID={1}", xmdm, dxmdm);
                    DataRow[] drs = this.dtCacheItem.Select(query);
                    if (drs != null && drs.Length > 0)
                    {
                        drresault = drs[0];
                    }
                }
            }
            catch
            {
                return null;
            }

            return drresault;
        }
        public string funA(string hd)
        {
            //MethodExecLogger.MethodStarted("ConsoleApp3.Mateinfo", "funA", "System.String hd2");

            //MethodExecutionLogger.MethodStarted("ConsoleApp3.Mateinfo", "funA", "System.String hd");

            //LogManager.WriteErrorLog(@"C:\Kingstar Winning\Log\" + this.Name, string.Format("{2}方法 {0}开始执行时间{1}", MethodInfo.GetCurrentMethod().Name, DateTime.Now.ToString(),this.Name));
            Console.WriteLine("这是方法A");
            //string strtmp = null;
            //strtmp.Trim();
            //string strtmp1 = null;
            //int itmp0 = 0;
            //itmp0.ToString();
            //bool bis = false;
            //bis.ToString();

            return "funA_return";



            // MethodExecutionLogger.MethodCompleted("ConsoleApp3.Mateinfo", "funA", "System.String hd");
        }

        private DataTable funC(string hd)
        {
            //MethodExecLogger.MethodStarted("ConsoleApp3.Mateinfo", "funA", "System.String hd2");

            //MethodExecutionLogger.MethodStarted("ConsoleApp3.Mateinfo", "funA", "System.String hd");

            //LogManager.WriteErrorLog(@"C:\Kingstar Winning\Log\" + this.Name, string.Format("{2}方法 {0}开始执行时间{1}", MethodInfo.GetCurrentMethod().Name, DateTime.Now.ToString(),this.Name));
            Console.WriteLine("这是方法C");
      
            string strtmp = null;
            strtmp.Trim();
            string strtmp1 = null;
            int itmp0 = 0;
            itmp0.ToString();
            bool bis = false;
            bis.ToString();
            DataSet ds = new DataSet();
            ds.Clear();
            //strtmp.Trim();
            return new DataTable();



            // MethodExecutionLogger.MethodCompleted("ConsoleApp3.Mateinfo", "funA", "System.String hd");
        }

        public void funB()
        {
            Console.WriteLine("这是方法B");
            //LogManager.WriteErrorLog(@"C:\Kingstar Winning\Log\" + this.Name, string.Format("{2}方法 {0}开始执行时间{1}", MethodInfo.GetCurrentMethod().Name, DateTime.Now.ToString(), this.Name));
        }

        private static DataTable GetFeeDataTable()
        {
            DataTable dt = new DataTable("FeeData");
            try
            {
                dt.Columns.Clear();
                dt.Columns.Add("No", Type.GetType("System.Int32"));
                dt.Columns.Add("Name", Type.GetType("System.String"));
                dt.Columns.Add("DetailCount", Type.GetType("System.Int32"));
                dt.Columns.Add("DetailNoes", Type.GetType("System.String"));
                dt.Columns.Add("UsedCount", Type.GetType("System.Int32"));
                dt.Columns.Add("NameIndex", Type.GetType("System.Int32"));
            }
            catch
            {
                dt = null;
            }

            return dt;
        }

    }
   
   

}
