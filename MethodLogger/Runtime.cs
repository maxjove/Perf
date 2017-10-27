using QUT.PERWAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MethodLogger
{
    class Runtime
    {
        internal static AssemblyRef System = AssemblyRef.MakeAssemblyRef("mscorlib");
        internal static ClassRef SystemExceptionRef = AddClass(System, "System", "Exception");
        internal static ClassRef AddClass(AssemblyRef assembly, string ns, string name)
        {
            ClassRef klass = assembly.GetClass(ns, name);

            if (klass != null)
                return klass;
            else
                return assembly.AddClass(ns, name);
        }
    }
}
