using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200068B RID: 1675
	internal sealed class EnterExceptionHandlerInstruction : Instruction
	{
		// Token: 0x060046F4 RID: 18164 RVA: 0x0017AA8A File Offset: 0x00178C8A
		private EnterExceptionHandlerInstruction(bool hasValue)
		{
			this._hasValue = hasValue;
		}

		// Token: 0x17000F12 RID: 3858
		// (get) Token: 0x060046F5 RID: 18165 RVA: 0x0017AA99 File Offset: 0x00178C99
		public override int ConsumedStack
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

		// Token: 0x17000F13 RID: 3859
		// (get) Token: 0x060046F6 RID: 18166 RVA: 0x0017AAA6 File Offset: 0x00178CA6
		public override int ProducedStack
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x0017AAA9 File Offset: 0x00178CA9
		public override int Run(InterpretedFrame frame)
		{
			return 1;
		}

		// Token: 0x040022CF RID: 8911
		internal static readonly EnterExceptionHandlerInstruction Void = new EnterExceptionHandlerInstruction(false);

		// Token: 0x040022D0 RID: 8912
		internal static readonly EnterExceptionHandlerInstruction NonVoid = new EnterExceptionHandlerInstruction(true);

		// Token: 0x040022D1 RID: 8913
		private readonly bool _hasValue;
	}
}
