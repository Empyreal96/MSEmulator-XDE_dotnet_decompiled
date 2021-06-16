using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200074B RID: 1867
	internal sealed class TypeEqualsInstruction : Instruction
	{
		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x06004AC7 RID: 19143 RVA: 0x00188140 File Offset: 0x00186340
		public override int ConsumedStack
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x06004AC8 RID: 19144 RVA: 0x00188143 File Offset: 0x00186343
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004AC9 RID: 19145 RVA: 0x00188146 File Offset: 0x00186346
		private TypeEqualsInstruction()
		{
		}

		// Token: 0x06004ACA RID: 19146 RVA: 0x00188150 File Offset: 0x00186350
		public override int Run(InterpretedFrame frame)
		{
			object obj = frame.Pop();
			object obj2 = frame.Pop();
			frame.Push(ScriptingRuntimeHelpers.BooleanToObject(obj2 != null && obj2.GetType() == obj));
			return 1;
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x06004ACB RID: 19147 RVA: 0x00188186 File Offset: 0x00186386
		public override string InstructionName
		{
			get
			{
				return "TypeEquals()";
			}
		}

		// Token: 0x04002426 RID: 9254
		public static readonly TypeEqualsInstruction Instance = new TypeEqualsInstruction();
	}
}
