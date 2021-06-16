using System;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x0200068C RID: 1676
	internal sealed class LeaveExceptionHandlerInstruction : IndexedBranchInstruction
	{
		// Token: 0x17000F14 RID: 3860
		// (get) Token: 0x060046F9 RID: 18169 RVA: 0x0017AAC4 File Offset: 0x00178CC4
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

		// Token: 0x17000F15 RID: 3861
		// (get) Token: 0x060046FA RID: 18170 RVA: 0x0017AAD1 File Offset: 0x00178CD1
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

		// Token: 0x060046FB RID: 18171 RVA: 0x0017AADE File Offset: 0x00178CDE
		private LeaveExceptionHandlerInstruction(int labelIndex, bool hasValue) : base(labelIndex)
		{
			this._hasValue = hasValue;
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x0017AAF0 File Offset: 0x00178CF0
		internal static LeaveExceptionHandlerInstruction Create(int labelIndex, bool hasValue)
		{
			if (labelIndex < 32)
			{
				int num = 2 * labelIndex | (hasValue ? 1 : 0);
				LeaveExceptionHandlerInstruction result;
				if ((result = LeaveExceptionHandlerInstruction.Cache[num]) == null)
				{
					result = (LeaveExceptionHandlerInstruction.Cache[num] = new LeaveExceptionHandlerInstruction(labelIndex, hasValue));
				}
				return result;
			}
			return new LeaveExceptionHandlerInstruction(labelIndex, hasValue);
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x0017AB32 File Offset: 0x00178D32
		public override int Run(InterpretedFrame frame)
		{
			Interpreter.AbortThreadIfRequested(frame, this._labelIndex);
			return base.GetLabel(frame).Index - frame.InstructionIndex;
		}

		// Token: 0x040022D2 RID: 8914
		private static LeaveExceptionHandlerInstruction[] Cache = new LeaveExceptionHandlerInstruction[64];

		// Token: 0x040022D3 RID: 8915
		private readonly bool _hasValue;
	}
}
