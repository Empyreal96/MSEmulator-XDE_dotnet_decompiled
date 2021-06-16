using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200074A RID: 1866
	internal sealed class TypeAsInstruction<T> : Instruction
	{
		// Token: 0x06004AC2 RID: 19138 RVA: 0x001880E9 File Offset: 0x001862E9
		internal TypeAsInstruction()
		{
		}

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x06004AC3 RID: 19139 RVA: 0x001880F1 File Offset: 0x001862F1
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x06004AC4 RID: 19140 RVA: 0x001880F4 File Offset: 0x001862F4
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06004AC5 RID: 19141 RVA: 0x001880F8 File Offset: 0x001862F8
		public override int Run(InterpretedFrame frame)
		{
			object obj = frame.Pop();
			if (obj is T)
			{
				frame.Push(obj);
			}
			else
			{
				frame.Push(null);
			}
			return 1;
		}

		// Token: 0x06004AC6 RID: 19142 RVA: 0x00188125 File Offset: 0x00186325
		public override string ToString()
		{
			return "TypeAs " + typeof(T).Name;
		}
	}
}
