using System;
using System.Collections.Generic;
using System.Text;
using QUT;
using QUT.PERWAPI;
using System.Diagnostics;

namespace MethodLogger
{
    static class MethodLoggerUtil
    {
        static readonly string startLogMethodName = "MethodStarted";
        static readonly string endLogMethodName = "MethodCompleted";

        public static string GetQualifiedClassName(ClassDef classDef)
        {
            return classDef.NameSpace().Length == 0 ? classDef.Name() : classDef.NameSpace() + "." + classDef.Name();
        }

        private static bool CheckLogMethodParameters(QUT.PERWAPI.Type[] types)
        {
            if (types.Length != 4)
                return false;

            return System.Array.TrueForAll(types, delegate(QUT.PERWAPI.Type type)
            {
                return type.TypeName() == "System.String";
            });
        }

        internal static string GetParamsAsString(Param[] parms)
        {
            string strParams = "";
            System.Array.ForEach(parms, delegate(Param param)
            {
                strParams += GetParameterTypeName(param.GetParType()) + " " + param.GetName() + ",";
            });
            return strParams.Length > 0 ? strParams.Substring(0, strParams.Length - 1) : strParams;
        }

        // Returns string representation of the type. The TypeName() method doesn't work for arrays, this method takes care of that.
        static string GetParameterTypeName(QUT.PERWAPI.Type type)
        {
            if (type is QUT.PERWAPI.Array)
            {
                return GetParameterTypeName(((QUT.PERWAPI.Array)type).ElemType()) + "[]";
            }
            else
            {
                return type.TypeName();
            }
        }

        /// <summary>
        /// get method from class MethodExecLogger
        /// </summary>
        /// <param name="strMethodName"></param>
        /// <param name="TagetMethod"></param>
        /// <returns></returns>
        internal static bool GetMethodsFromClass(string strMethodName, out Method TagetMethod)
        {
            return GetMethodsFromClass("", strMethodName, out TagetMethod); 
        }
        internal static bool GetMethodsFromClass(string strClassName, string strMethodName, out Method TagetMethod)
        {
            if (string.IsNullOrEmpty(strClassName))
                strClassName = "MethodExecLogger";
            string str1 = Process.GetCurrentProcess().MainModule.FileName;
            Console.WriteLine(str1);
           
            PEFile file = PEFile.ReadPEFile(str1);
            string Assemblyname = System.IO.Path.GetFileNameWithoutExtension(str1); ;
            AssemblyRef newAssemblyRef = file.MakeExternAssembly(Assemblyname);
            ClassRef newMethodLoggerRef = TryGetMethodLoggerFromAssembly(newAssemblyRef, "MethodExecLogger");
            
            return GEtMethodsFromClass(newMethodLoggerRef, strMethodName, out TagetMethod);
         
        }
        internal static ClassRef GetClass(string strClassName)
        {
            if (string.IsNullOrEmpty(strClassName))
                strClassName = "Myexce";
            string str1 = Process.GetCurrentProcess().MainModule.FileName;
            Console.WriteLine(str1);
            PEFile file = PEFile.ReadPEFile(str1);
            string Assemblyname = System.IO.Path.GetFileNameWithoutExtension(str1); ;
            AssemblyRef newAssemblyRef = file.MakeExternAssembly(Assemblyname);
            ClassRef newMethodLoggerRef = TryGetMethodLoggerFromAssembly(newAssemblyRef, "MethodExecLogger");
            return newMethodLoggerRef;
        }

        internal static bool GEtMethodsFromClass(ClassRef methodClass,string strMethodName, out Method TagetMethod)
        {

           
            TagetMethod = null;
            MethodRef tmpTagetMethod= methodClass.GetMethod(strMethodName);

            if (tmpTagetMethod != null)
            {
                TagetMethod = tmpTagetMethod;
                return true;
            }


            return false;
        }
        private static bool GEtMethodsFromClass(ClassDef methodClass, string strMethodName, out Method TagetMethod)
        {
            TagetMethod = null;
            Method tmpTagetMethod = methodClass.GetMethodDesc(strMethodName);

            if (tmpTagetMethod != null)
            {
                TagetMethod = tmpTagetMethod;
                return true;
            }


            return false;
        }
        private static bool GetLoggerMethodsFromClass(ClassRef methodLogger, out Method startLogMethod, out Method endLogMethod)
        {
            startLogMethod = endLogMethod = null;

            MethodRef tempStartLogMethod = methodLogger.GetMethod(startLogMethodName);
            MethodRef tempEndLogMethod = methodLogger.GetMethod(endLogMethodName);
            
            if (tempStartLogMethod != null && tempEndLogMethod != null)
            {
                if (CheckLogMethodParameters(tempStartLogMethod.GetParTypes()) && CheckLogMethodParameters(tempEndLogMethod.GetParTypes()))
                {
                    startLogMethod = tempStartLogMethod;
                    endLogMethod = tempEndLogMethod;
                    return true;
                }
            }

            return false;
        }
       
        private static bool GetLoggerMethodsFromClass(ClassDef methodLogger, out Method startLogMethod, out Method endLogMethod)
        {
            startLogMethod = endLogMethod = null;

            Method tempStartLogMethod = methodLogger.GetMethodDesc(startLogMethodName);
            Method tempEndLogMethod = methodLogger.GetMethodDesc(endLogMethodName);

            if (tempStartLogMethod != null && tempEndLogMethod != null)
            {
                if (CheckLogMethodParameters(tempStartLogMethod.GetParTypes()) && CheckLogMethodParameters(tempEndLogMethod.GetParTypes()))
                {
                    startLogMethod = tempStartLogMethod;
                    endLogMethod = tempEndLogMethod;
                    return true;
                }
            }

            return false;
        }

        public static bool LocateLoggerMethods( out Method startLogMethod, out Method endLogMethod)
        {


            //PEFile file2=
            //PEFile file = PEFile.ReadPEFile("MethodLogger");
            string str1 = Process.GetCurrentProcess().MainModule.FileName;
            Console.WriteLine(str1);
            PEFile file = PEFile.ReadPEFile(str1);
            startLogMethod = endLogMethod = null;
            ClassDef methodLogger = file.GetClass("MethodExecLogger");

            //if (methodLogger != null)
            //{
            //    return GetLoggerMethodsFromClass(methodLogger, out startLogMethod, out endLogMethod);
            //}
            string Assemblyname = System.IO.Path.GetFileNameWithoutExtension(str1); ;
           
            AssemblyRef newAssemblyRef = file.MakeExternAssembly(Assemblyname);
            ClassRef newMethodLoggerRef = TryGetMethodLoggerFromAssembly(newAssemblyRef, "MethodExecLogger");
            if (newMethodLoggerRef != null)
            {
                if (GetLoggerMethodsFromClass(newMethodLoggerRef, out startLogMethod, out endLogMethod))
                    return true;
            }
            return false;
        }

        public static bool LocateLoggerMethods(PEFile file, string assemblyName, string className, out Method startLogMethod, out Method endLogMethod)
        {
           
            startLogMethod = endLogMethod = null;
            
            // Check if it is in this assembly itself
            if (file.GetThisAssembly().Name() == assemblyName)
            {
                ClassDef methodLogger = file.GetClass(className);

                if (methodLogger != null)
                {
                    return GetLoggerMethodsFromClass(methodLogger, out startLogMethod, out endLogMethod);
                }
            }

            // Check referenced assemblies
            foreach (AssemblyRef assemblyRef in file.GetImportedAssemblies())
            {
                if (assemblyRef.Name() == assemblyName)
                {
                    ClassRef methodLoggerRef = TryGetMethodLoggerFromAssembly(assemblyRef, className);
                    if (methodLoggerRef != null)
                    {
                        if (GetLoggerMethodsFromClass(methodLoggerRef, out startLogMethod, out endLogMethod))
                            return true;
                    }
                }
            }

            // Not found in this assembly or referenced assemblies. Try loading given assembly and adding it as reference
            AssemblyRef newAssemblyRef = file.MakeExternAssembly(assemblyName);
            ClassRef newMethodLoggerRef = TryGetMethodLoggerFromAssembly(newAssemblyRef, className);
            if (newMethodLoggerRef != null)
            {
                if (GetLoggerMethodsFromClass(newMethodLoggerRef, out startLogMethod, out endLogMethod))
                    return true;
            }
            return false;
        }

        private static ClassRef TryGetMethodLoggerFromAssembly(AssemblyRef assemblyRef, string className)
        {
            string fileName = "";
            if (assemblyRef.Name().ToLower().EndsWith(".dll")|| assemblyRef.Name().ToLower().EndsWith(".exe"))
                fileName = assemblyRef.Name();
            else 
             fileName = assemblyRef.Name() + ".exe";
            Console.WriteLine("TryGetMethodLoggerFromAssembly ->" + fileName);
            if (!System.IO.File.Exists(fileName))
            {
                Console.WriteLine(fileName + " not present in current directory. Skipping it in search");
                return null;
            }
           
               

            PEFile refFile = PEFile.ReadPEFile(fileName);
            ClassDef methodLogger = refFile.GetClass(className);

            if (methodLogger != null)
            {
                ClassRef methodLoggerRef = methodLogger.MakeRefOf();
                if (assemblyRef.GetClass(className) == null)
                {
                    assemblyRef.AddClass(methodLoggerRef);
                }

                System.Array.ForEach(methodLogger.GetMethods(), delegate(MethodDef methodDef)
                {
                    if (methodLoggerRef.GetMethod(methodDef.Name()) == null)
                    {
                        methodLoggerRef.AddMethod(methodDef.Name(), methodDef.GetRetType(), methodDef.GetParTypes());
                    }
                });
                return methodLoggerRef;
            }
            return null;
        }
    }
}
