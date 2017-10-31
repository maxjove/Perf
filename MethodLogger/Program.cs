using QUT.PERWAPI;
using System;
using System.Collections.Generic;
using System.IO;

namespace MethodLogger
{
    class Program
    {
        static ClassFilter classFilter;
        static void Main(string[] args)
        {
            Environment.ExitCode = 1;

            if (args.Length < 1)
            {
                ShowHelp("No Arguments Provided. Please provide a directory or list of files");
                return;
            }

            string target;
            string assemblyName;
            string className;
            List<string> includeList;
            List<string> excludeList;

            if (!ParseArguments(args, out includeList, out excludeList, out target, out assemblyName, out className))
            {
                return;
            }

            classFilter = new ClassFilter(includeList, excludeList);

            if (Directory.Exists(target))
            {
                foreach (string file in Directory.GetFiles(target))
                {
                    ProcessFile(file);
                }
            }
            else if (File.Exists(target))
            {
                ProcessFile(target);
            }
            else
            {
                ShowHelp("Invalid Argument. Please provide a directory or list of files");
            }

            Environment.ExitCode = 1;
        }

        #region ParseArguments

        static bool ParseArguments(string[] args, out List<string> includeList, out List<string> excludeList, out string target, out string assemblyName, out string className)
        {
            bool parsingIncludes = false;
            bool parsingExcludes = false;
            bool parsingInput = false;
            target = assemblyName = className = null;
            includeList =  new List<string>();
            excludeList = new List<string>();

            for (int i = 0; i < args.Length; ++i)
            {
                if (args[i] == "-include")
                {
                    parsingIncludes = true;
                    parsingExcludes = parsingInput = false;
                    continue;
                }
                if (args[i] == "-exclude")
                {
                    parsingExcludes = true;
                    parsingIncludes = parsingInput = false;
                    continue;
                }
                if (args[i] == "-input")
                {
                    parsingInput = true;
                    parsingIncludes = parsingExcludes = false;
                    continue;
                }

                if (parsingIncludes)
                {
                    includeList.Add(args[i]);
                }
                else if (parsingExcludes)
                {
                    excludeList.Add(args[i]);
                }
                else if (parsingInput)
                {
                    if (args.Length - i < 1)
                    {
                        ShowHelp("-input requires three arguments");
                        return false;
                    }
                    else
                    {
                        if (args.Length >= 2)
                        {
                            target = args[i];
                        }
                        else
                        {
                            target = args[i];
                            assemblyName = args[i + 1];
                            className = args[i + 2];
                            i += 2;
                        }
                    }
                }
            }

            if (parsingInput == false)
            {
                ShowHelp("-input is required");
                return false;
            }

            return true;
        }
        #endregion
        private static void ShowHelp(string errorMessage)
        {
            //Console.WriteLine("MethodLogger - Modifies managed binaries to notify start and end of execution of methods");
            //Console.WriteLine("Syntax : MethodLogger -include <namespace/list of classes> -exclude <namespace/list of classes> -input <dir/list of files> <assemblyname> <classname>");
            //Console.WriteLine();
            Console.WriteLine(errorMessage);
        }

        private static void ProcessFile(string inputFile)
        {
            Console.WriteLine("Processing  File : " + inputFile);

            if (Directory.Exists(inputFile))
            {
                string directoryName = Path.GetDirectoryName(inputFile);
                if (directoryName != null)
                    Environment.CurrentDirectory = directoryName;
            }
            //QUT.PERWAPI.UIntConst unst = new UIntConst()
            //unst.GetULongAsLong();
            //QUT.PERWAPI.per
           
            PEFile file = PEFile.ReadPEFile(inputFile);

            Method startLogMethod, endLogMethod;
            if (!MethodLoggerUtil.LocateLoggerMethods(out startLogMethod, out endLogMethod))
            {
                ShowHelp("未成功获取注入内容!");
                return;
            }

            //Diag.DiagOn = true;
            ClassDef[] classes = file.GetClasses();
            
            System.Array.ForEach(classes, delegate(ClassDef classDef)
            {
                ProcessClass(classDef, startLogMethod, endLogMethod);
            });

            file.WritePEFile(false);
            Console.WriteLine("Processe end ,any key exit");
            Console.ReadKey();

        }

        static void ProcessClass(ClassDef classDef, Method startLogMethod, Method endLogMethod)
        {
            
            // Don't modify the class methods that we are going to emit calls to, otherwise we'll get unbounded recursion.
            //if (classDef.Name() == methodLoggerClassName)
            //    return;

            if (classFilter.PassesFilter(classDef) == false) 
                return;

            foreach (NestedClassDef c in classDef.GetNestedClasses())
            {
                ProcessClass(c, startLogMethod, endLogMethod);
            }
            Console.WriteLine("Processing  Class:" + classDef.Name());
            foreach (MethodDef methodDef in classDef.GetMethods())
            {
                if ( methodDef.Name().ToLower() == ".ctor" || methodDef.Name().ToLower() == "Dispose" || methodDef.Name().ToLower() == "InitializeComponent")
                    continue;
                ModifyCode(classDef, methodDef, startLogMethod, endLogMethod);
            }
        }

        static void ModifyCode(ClassDef classDef, MethodDef methodDef, Method startLogMethod, Method endLogMethod)
        {
            if (methodDef is null)
                return;
            bool bIsHavReturn = methodDef.GetRetType().TypeName().ToLower() != "void";

            Console.WriteLine("Processing  Method:" + classDef.Name() + "." + methodDef.Name() + "  Return Type: " + methodDef.GetRetType().TypeName());
            if (!bIsHavReturn)
            {
                ILSY.ILSpyMtNoRt(classDef, methodDef, startLogMethod, endLogMethod);
            }
            else
            {
                ILSY.ILSpyMtHvRt(classDef, methodDef, startLogMethod, endLogMethod);
            }


        }

      
    }
}
