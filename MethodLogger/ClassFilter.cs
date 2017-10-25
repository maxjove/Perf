using System;
using System.Collections.Generic;
using System.Text;
using QUT.PERWAPI;

namespace MethodLogger
{
    class ClassFilter
    {
        List<string> includeList;
        List<string> excludeList;

        public ClassFilter(List<string> includeList, List<string> excludeList)
        {
            this.includeList = includeList;
            this.excludeList = excludeList;
        }

        public bool PassesFilter(ClassDef classDef)
        {
            if (IsIncluded(classDef))
                return true;

            return (IsExcluded(classDef) == false);
        }

        public bool IsIncluded(ClassDef classDef)
        {
            return IsMatch(classDef, includeList);
        }

        public bool IsExcluded(ClassDef classDef)
        {
            return IsMatch(classDef, excludeList);
        }

        bool IsMatch(ClassDef classDef, List<string> list)
        {
            string classNameSpace = classDef.NameSpace();
            string className = MethodLoggerUtil.GetQualifiedClassName(classDef);
            foreach (string s in list)
            {
                if (s.EndsWith(".*"))
                {
                    string namespaceName = s.Substring(0, s.Length - 2);
                    if (classNameSpace == namespaceName)
                        return true;
                }
                else if (s == className)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
