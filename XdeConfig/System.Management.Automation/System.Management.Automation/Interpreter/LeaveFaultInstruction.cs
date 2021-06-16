using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200068D RID: 1677
	internal sealed class LeaveFaultInstruction : Instruction
	{
		// Token: 0x17000F16 RID: 3862
		// (get) Token: 0x060046FF RID: 18175 RVA: 0x0017AB61 File Offset: 0x00178D61
		public override int ConsumedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000F17 RID: 3863
		// (get) Token: 0x06004700 RID: 18176 RVA: 0x0017AB64 File Offset: 0x00178D64
		public override int ProducedStack
		{
			get
			{
				if (!this._hasValue)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x0017AB71 File Offset: 0x00178D71
		private LeaveFaultInstruction(bool hasValue)
		{
			this._hasValue = hasValue;
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x0017AB80 File Offset: 0x00178D80
		public override int Run(InterpretedFrame frame)
		{
			frame.Pop();
			throw new RethrowException();
		}

		// Token: 0x040022D4 RID: 8916
		internal static readonly Instruction NonVoid = new LeaveFaultInstruction(true);

		// Token: 0x040022D5 RID: 8917
		internal static readonly Instruction Void = new LeaveFaultInstruction(false);

		// Token: 0x040022D6 RID: 8918
		private readonly bool _hasValue;
	}
}
