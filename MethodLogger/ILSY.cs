using QUT.PERWAPI;
using System;
using System.Collections.Generic;
using System.IO;

namespace MethodLogger
{
   internal class ILSY
    {
        internal  static void ILSpyMtNoRt(ClassDef classDef, MethodDef methodDef, Method startLogMethod, Method endLogMethod)
        {
            string classNameString = MethodLoggerUtil.GetQualifiedClassName(classDef);
            string methodNameString = methodDef.Name();
            string paramsString = MethodLoggerUtil.GetParamsAsString(methodDef.GetParams());

            List<Local> CLRLocals = new List<Local>();
            CLRLocals.Clear();

            Param[] parms = methodDef.GetParams();


            if (methodDef.GetMaxStack() < 3)
            {
                methodDef.SetMaxStack(3);
            }
            string strGuid = Guid.NewGuid().ToString();
            CILInstructions instructions = methodDef.GetCodeBuffer();
            if (instructions == null)
                return;


            instructions.StartInsert();


            instructions.Inst(Op.nop);
            instructions.StartBlock(); // Try #1
            instructions.StartBlock(); // Try #2
            instructions.Inst(Op.nop);



            instructions.ldstr(strGuid);
            instructions.ldstr(classNameString);
            instructions.ldstr(methodNameString);
            instructions.ldstr(paramsString);
            instructions.MethInst(MethodOp.call, startLogMethod);
            instructions.EndInsert();


            while (instructions.GetNextInstruction().GetPos() < instructions.NumInstructions() - 2) ;




            instructions.StartInsert();
            instructions.Inst(Op.nop);
            CILLabel cel0 = instructions.NewLabel();
            CILLabel cel9 = instructions.NewLabel();
            instructions.Branch(BranchOp.leave_s, cel9);



            TryBlock tBlk2 = instructions.EndTryBlock(); // #2
            instructions.StartBlock();
            int istloc = 0;
            if (methodDef.GetLocals() != null)
            {
                istloc = methodDef.GetLocals().Length;
            }
            instructions.IntInst(IntOp.stloc_s, istloc);
            instructions.Inst(Op.nop);
            //instructions.Inst(Op.rethrow);

            CILLabel cel = instructions.NewLabel();



            instructions.CodeLabel(cel0);



            instructions.OpenScope();
            //start---add exceptiong to stocks variables
            Local loc = new Local("SpyMtNoRt", Runtime.SystemExceptionRef);
            if (methodDef.GetLocals() != null)
            {
                foreach (Local lab in methodDef.GetLocals())
                {
                    CLRLocals.Add(lab);
                }
            }
            CLRLocals.Add(loc);

            methodDef.AddLocals(CLRLocals.ToArray(), false);
            foreach (Local la in methodDef.GetLocals())
            {
                instructions.BindLocal(la);
            }
            //start---add exceptiong to stocks variables


            instructions.CloseScope();

            instructions.IntInst(IntOp.ldloc_s, istloc);
            Method LogException = null;
            MethodLoggerUtil.GetMethodsFromClass("LogException", out LogException);
            instructions.MethInst(MethodOp.call, LogException);
            instructions.Inst(Op.nop);
          
            instructions.Inst(Op.nop);
            instructions.Branch(BranchOp.leave_s, cel9);
            
            instructions.EndCatchBlock(Runtime.SystemExceptionRef, tBlk2);
            instructions.CodeLabel(cel9);
            


            instructions.Branch(BranchOp.leave_s, cel);

            



            TryBlock tBlk1 = instructions.EndTryBlock(); // #1
            instructions.StartBlock(); // Finally
            instructions.Inst(Op.nop);





            instructions.ldstr(strGuid);
            instructions.ldstr(classNameString);
            instructions.ldstr(methodNameString);
            instructions.ldstr(paramsString);
            instructions.MethInst(MethodOp.call, endLogMethod);
            instructions.Inst(Op.nop);
            instructions.Inst(Op.nop);
            instructions.Inst(Op.endfinally);

            instructions.EndFinallyBlock(tBlk1);
            
            instructions.CodeLabel(cel);
            


            instructions.EndInsert();
        }

        internal static void ILSpyMtHvRt(ClassDef classDef, MethodDef methodDef, Method startLogMethod, Method endLogMethod)
        {
            
            string classNameString = MethodLoggerUtil.GetQualifiedClassName(classDef);
            string methodNameString = methodDef.Name();
            string paramsString = MethodLoggerUtil.GetParamsAsString(methodDef.GetParams());

            List<Local> CLRLocals = new List<Local>();
            CLRLocals.Clear();

            Param[] parms = methodDef.GetParams();


            if (methodDef.GetMaxStack() < 3)
            {
                methodDef.SetMaxStack(3);
            }
            string strGuid = Guid.NewGuid().ToString();
            CILInstructions instructions = methodDef.GetCodeBuffer();
            if (instructions == null)
                return;


            instructions.StartInsert();

           
            instructions.Inst(Op.nop);
            instructions.StartBlock(); // Try #1
            instructions.StartBlock(); // Try #2
            instructions.Inst(Op.nop);



            instructions.ldstr(strGuid);
            instructions.ldstr(classNameString);
            instructions.ldstr(methodNameString);
            instructions.ldstr(paramsString);
            instructions.MethInst(MethodOp.call, startLogMethod);
            instructions.EndInsert();


            while (instructions.GetNextInstruction().GetPos() < instructions.NumInstructions() - 2) ;




            instructions.StartInsert();
            instructions.Inst(Op.nop);

            CILLabel cel9 = instructions.NewLabel();
            instructions.Branch(BranchOp.leave_s, cel9);



            TryBlock tBlk2 = instructions.EndTryBlock(); // #2
            instructions.StartBlock();
            int istloc = 0;
            if (methodDef.GetLocals() != null)
            {
                istloc = methodDef.GetLocals().Length;
            }
            if (istloc > 4)
            {
                instructions.IntInst(IntOp.stloc_s, istloc);
            }
            else 
            {
                if (istloc == 0)
                {
                    instructions.Inst(Op.stloc_0);
                }
                else if (istloc == 1)
                {
                    instructions.Inst(Op.stloc_1);
                }
                else if (istloc == 2)
                {
                    instructions.Inst(Op.stloc_2);
                }
                else if (istloc == 3)
                {
                    instructions.Inst(Op.stloc_3);
                }
            }
            instructions.Inst(Op.nop);
            

            CILLabel cel = instructions.NewLabel();






            instructions.OpenScope();
            //start---add exceptiong to stocks variables
            Local loc = new Local("SpyMtHvR", Runtime.SystemExceptionRef);
            if (methodDef.GetLocals() != null)
            {
                foreach (Local lab in methodDef.GetLocals())
                {
                    CLRLocals.Add(lab);
                }
            }
            CLRLocals.Add(loc);

            methodDef.AddLocals(CLRLocals.ToArray(), false);
            foreach (Local la in methodDef.GetLocals())
            {
                instructions.BindLocal(la);
            }
            //start---add exceptiong to stocks variables


            instructions.CloseScope();

            

            if (istloc > 4)
            {
                instructions.IntInst(IntOp.ldloc_s, istloc);
            }
            else
            {
                if (istloc == 0)
                {
                    instructions.Inst(Op.ldloc_0);
                }
                else if (istloc == 1)
                {
                    instructions.Inst(Op.ldloc_1);
                }
                else if (istloc == 2)
                {
                    instructions.Inst(Op.ldloc_2);
                }
                else if (istloc == 3)
                {
                    instructions.Inst(Op.ldloc_3);
                }
            }

            Method LogException = null;
            MethodLoggerUtil.GetMethodsFromClass("LogException", out LogException);
            instructions.MethInst(MethodOp.call, LogException);
            instructions.Inst(Op.nop);
           
            instructions.Inst(Op.ldnull);
            instructions.Inst(Op.stloc_0);


           
            instructions.Branch(BranchOp.leave_s, cel);

           
            instructions.EndCatchBlock(Runtime.SystemExceptionRef, tBlk2);
            



            TryBlock tBlk1 = instructions.EndTryBlock(); // #1
            instructions.StartBlock(); // Finally
            instructions.Inst(Op.nop);





            instructions.ldstr(strGuid);
            instructions.ldstr(classNameString);
            instructions.ldstr(methodNameString);
            instructions.ldstr(paramsString);
            instructions.MethInst(MethodOp.call, endLogMethod);
            instructions.Inst(Op.nop);
            instructions.Inst(Op.nop);
            instructions.Inst(Op.endfinally);

            instructions.EndFinallyBlock(tBlk1);
        
            instructions.CodeLabel(cel);
            instructions.CodeLabel(cel9);
            //instructions.Inst(Op.ldloc_0);

            if (istloc > 4)
            {
                instructions.IntInst(IntOp.ldloc_s, istloc-1);
            }
            else
            {
                if (istloc == 0)
                {
                    instructions.Inst(Op.ldloc_0);
                }
                else if (istloc == 1)
                {
                    instructions.Inst(Op.ldloc_0);
                }
                else if (istloc == 2)
                {
                    instructions.Inst(Op.ldloc_1);
                }
                else if (istloc == 3)
                {
                    instructions.Inst(Op.ldloc_2);
                }
                else if (istloc == 4)
                {
                    instructions.Inst(Op.ldloc_3);
                }
            }



            instructions.EndInsert();
        }
    }
}
