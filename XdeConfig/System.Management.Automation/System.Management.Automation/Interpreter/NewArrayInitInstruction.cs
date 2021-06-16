using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000664 RID: 1636
	internal sealed class NewArrayInitInstruction<TElement> : Instruction
	{
		// Token: 0x060045E2 RID: 17890 RVA: 0x001770B1 File Offset: 0x001752B1
		internal NewArrayInitInstruction(int elementCount)
		{
			this._elementCount = elementCount;
		}

		// Token: 0x17000EBE RID: 3774
		// (get) Token: 0x060045E3 RID: 17891 RVA: 0x001770C0 File Offset: 0x001752C0
		public override int ConsumedStack
		{
			get
			{
				return this._elementCount;
			}
		}

		// Token: 0x17000EBF RID: 3775
		// (get) Token: 0x060045E4 RID: 17892 RVA: 0x001770C8 File Offset: 0x001752C8
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x001770CC File Offset: 0x001752CC
		public override int Run(InterpretedFrame frame)
		{
			TElement[] array = new TElement[this._elementCount];
			for (int i = this._elementCount - 1; i >= 0; i--)
			{
				array[i] = (TElement)((object)frame.Pop());
			}
			frame.Push(array);
			return 1;
		}

		// Token: 0x04002296 RID: 8854
		private readonly int _elementCount;
	}
}
