using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000666 RID: 1638
	internal sealed class NewArrayBoundsInstruction : Instruction
	{
		// Token: 0x060045EA RID: 17898 RVA: 0x00177146 File Offset: 0x00175346
		internal NewArrayBoundsInstruction(Type elementType, int rank)
		{
			this._elementType = elementType;
			this._rank = rank;
		}

		// Token: 0x17000EC2 RID: 3778
		// (get) Token: 0x060045EB RID: 17899 RVA: 0x0017715C File Offset: 0x0017535C
		public override int ConsumedStack
		{
			get
			{
				return this._rank;
			}
		}

		// Token: 0x17000EC3 RID: 3779
		// (get) Token: 0x060045EC RID: 17900 RVA: 0x00177164 File Offset: 0x00175364
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060045ED RID: 17901 RVA: 0x00177168 File Offset: 0x00175368
		public override int Run(InterpretedFrame frame)
		{
			int[] array = new int[this._rank];
			for (int i = this._rank - 1; i >= 0; i--)
			{
				array[i] = (int)frame.Pop();
			}
			Array value = Array.CreateInstance(this._elementType, array);
			frame.Push(value);
			return 1;
		}

		// Token: 0x04002297 RID: 8855
		private readonly Type _elementType;

		// Token: 0x04002298 RID: 8856
		private readonly int _rank;
	}
}
