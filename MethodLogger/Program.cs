using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using QUT.PERWAPI;

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

            PEFile file = PEFile.ReadPEFile(inputFile);

            Method startLogMethod, endLogMethod;
            if (!MethodLoggerUtil.LocateLoggerMethods(out startLogMethod, out endLogMethod))
            {
                ShowHelp("未成功获取注入内容!");
                return;
            }
           

            ClassDef[] classes = file.GetClasses();
            
            System.Array.ForEach(classes, delegate(ClassDef classDef)
            {
                ProcessClass(classDef, startLogMethod, endLogMethod);
            });

            file.WritePEFile(true);
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

            foreach (MethodDef methodDef in classDef.GetMethods())
            {
                if (methodDef.Name().ToLower() == "main"|| methodDef.Name().ToLower() == ".ctor")
                    continue;
                ModifyCode(classDef, methodDef, startLogMethod, endLogMethod);
            }
        }

        static void ModifyCode(ClassDef classDef, MethodDef methodDef, Method startLogMethod, Method endLogMethod)
        {
            string classNameString = MethodLoggerUtil.GetQualifiedClassName(classDef);
            string methodNameString = methodDef.Name();
            string paramsString = MethodLoggerUtil.GetParamsAsString(methodDef.GetParams());

            Param[] parms = methodDef.GetParams();

            // We'll be pushing typeName, methodName and parameters as string parameters, so set max stack size to 3.
            if (methodDef.GetMaxStack() < 3)
            {
                methodDef.SetMaxStack(3);
            }
            string strGuid = Guid.NewGuid().ToString();
            CILInstructions instructions = methodDef.GetCodeBuffer();

           
            instructions.StartInsert();
            //instructions.Inst(Op.nop);
            //instructions.Inst(Op.nop);
            instructions.StartBlock(); // Try #1
            instructions.StartBlock(); // Try #2



            ////Method NewGuid = null;
            ////MethodLoggerUtil.GetMethodsFromClass("GetNewGuid", out NewGuid);
            ////instructions.MethInst(MethodOp.call, NewGuid);
            ////instructions.Inst(Op.stloc_1);

            instructions.ldstr(strGuid);
            instructions.ldstr(classNameString);
            instructions.ldstr(methodNameString);
            instructions.ldstr(paramsString);
            instructions.MethInst(MethodOp.call, startLogMethod);


      


            instructions.EndInsert();


            while (instructions.GetNextInstruction().GetPos() < instructions.NumInstructions() - 2) ;

            TryBlock tBlk2 = instructions.EndTryBlock(); // #2
            instructions.StartBlock(); // Catch
            //ClassRef clsrf = MethodLoggerUtil.GetClass("");
            //instructions.EndCatchBlock(clsrf, tBlk2);
            TryBlock tBlk1 = instructions.EndTryBlock(); // #1
            instructions.StartBlock(); // Finally
            instructions.StartInsert();

           
            instructions.StartBlock();

            instructions.ldstr(strGuid);
            instructions.ldstr(classNameString);
            instructions.ldstr(methodNameString);
            instructions.ldstr(paramsString);
            instructions.MethInst(MethodOp.call, endLogMethod);

      

            instructions.EndInsert();
            instructions.EndFinallyBlock(tBlk1);
        }
    }
}
