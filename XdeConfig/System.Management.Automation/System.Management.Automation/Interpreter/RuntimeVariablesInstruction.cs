using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000704 RID: 1796
	internal sealed class RuntimeVariablesInstruction : Instruction
	{
		// Token: 0x060049DC RID: 18908 RVA: 0x001853CF File Offset: 0x001835CF
		public RuntimeVariablesInstruction(int count)
		{
			this._count = count;
		}

		// Token: 0x17000F8A RID: 3978
		// (get) Token: 0x060049DD RID: 18909 RVA: 0x001853DE File Offset: 0x001835DE
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F8B RID: 3979
		// (get) Token: 0x060049DE RID: 18910 RVA: 0x001853E1 File Offset: 0x001835E1
		public override int ConsumedStack
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x060049DF RID: 18911 RVA: 0x001853EC File Offset: 0x001835EC
		public override int Run(InterpretedFrame frame)
		{
			IStrongBox[] array = new IStrongBox[this._count];
			for (int i = array.Length - 1; i >= 0; i--)
			{
				array[i] = (IStrongBox)frame.Pop();
			}
			frame.Push(RuntimeVariables.Create(array));
			return 1;
		}

		// Token: 0x060049E0 RID: 18912 RVA: 0x00185430 File Offset: 0x00183630
		public override string ToString()
		{
			return "GetRuntimeVariables()";
		}

		// Token: 0x040023CE RID: 9166
		private readonly int _count;
	}
}
