using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000749 RID: 1865
	internal sealed class TypeIsInstruction<T> : Instruction
	{
		// Token: 0x06004ABD RID: 19133 RVA: 0x001880A4 File Offset: 0x001862A4
		internal TypeIsInstruction()
		{
		}

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x06004ABE RID: 19134 RVA: 0x001880AC File Offset: 0x001862AC
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x06004ABF RID: 19135 RVA: 0x001880AF File Offset: 0x001862AF
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004AC0 RID: 19136 RVA: 0x001880B2 File Offset: 0x001862B2
		public override int Run(InterpretedFrame frame)
		{
			frame.Push(ScriptingRuntimeHelpers.BooleanToObject(frame.Pop() is T));
			return 1;
		}

		// Token: 0x06004AC1 RID: 19137 RVA: 0x001880CE File Offset: 0x001862CE
		public override string ToString()
		{
			return "TypeIs " + typeof(T).Name;
		}
	}
}
