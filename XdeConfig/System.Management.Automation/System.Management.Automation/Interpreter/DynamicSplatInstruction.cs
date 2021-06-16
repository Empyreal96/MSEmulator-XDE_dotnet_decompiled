using System;
using System.Runtime.CompilerServices;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006AC RID: 1708
	internal sealed class DynamicSplatInstruction : Instruction
	{
		// Token: 0x06004791 RID: 18321 RVA: 0x0017C7A8 File Offset: 0x0017A9A8
		internal DynamicSplatInstruction(int argumentCount, CallSite<Func<CallSite, ArgumentArray, object>> site)
		{
			this._site = site;
			this._argumentCount = argumentCount;
		}

		// Token: 0x17000F41 RID: 3905
		// (get) Token: 0x06004792 RID: 18322 RVA: 0x0017C7BE File Offset: 0x0017A9BE
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F42 RID: 3906
		// (get) Token: 0x06004793 RID: 18323 RVA: 0x0017C7C1 File Offset: 0x0017A9C1
		public override int ConsumedStack
		{
			get
			{
				return this._argumentCount;
			}
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x0017C7CC File Offset: 0x0017A9CC
		public override int Run(InterpretedFrame frame)
		{
			int num = frame.StackIndex - this._argumentCount;
			object obj = this._site.Target(this._site, new ArgumentArray(frame.Data, num, this._argumentCount));
			frame.Data[num] = obj;
			frame.StackIndex = num + 1;
			return 1;
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x0017C823 File Offset: 0x0017AA23
		public override string ToString()
		{
			return "DynamicSplatInstruction(" + this._site + ")";
		}

		// Token: 0x04002302 RID: 8962
		private readonly CallSite<Func<CallSite, ArgumentArray, object>> _site;

		// Token: 0x04002303 RID: 8963
		private readonly int _argumentCount;
	}
}
