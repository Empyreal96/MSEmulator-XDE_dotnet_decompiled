using System;
using System.Threading;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x02000688 RID: 1672
	internal sealed class EnterTryCatchFinallyInstruction : IndexedBranchInstruction
	{
		// Token: 0x060046E2 RID: 18146 RVA: 0x0017A725 File Offset: 0x00178925
		internal void SetTryHandler(TryCatchFinallyHandler tryHandler)
		{
			this._tryHandler = tryHandler;
		}

		// Token: 0x17000F0D RID: 3853
		// (get) Token: 0x060046E3 RID: 18147 RVA: 0x0017A72E File Offset: 0x0017892E
		public override int ProducedContinuations
		{
			get
			{
				if (!this._hasFinally)
				{
					return 0;
				}
				return 1;
			}
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x0017A73B File Offset: 0x0017893B
		private EnterTryCatchFinallyInstruction(int targetIndex, bool hasFinally) : base(targetIndex)
		{
			this._hasFinally = hasFinally;
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x0017A74B File Offset: 0x0017894B
		internal static EnterTryCatchFinallyInstruction CreateTryFinally(int labelIndex)
		{
			return new EnterTryCatchFinallyInstruction(labelIndex, true);
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x0017A754 File Offset: 0x00178954
		internal static EnterTryCatchFinallyInstruction CreateTryCatch()
		{
			return new EnterTryCatchFinallyInstruction(int.MaxValue, false);
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x0017A764 File Offset: 0x00178964
		public override int Run(InterpretedFrame frame)
		{
			if (this._hasFinally)
			{
				frame.PushContinuation(this._labelIndex);
			}
			int instructionIndex = frame.InstructionIndex;
			frame.InstructionIndex++;
			Instruction[] instructions = frame.Interpreter.Instructions.Instructions;
			try
			{
				int num = frame.InstructionIndex;
				while (num >= this._tryHandler.TryStartIndex && num < this._tryHandler.TryEndIndex)
				{
					num += instructions[num].Run(frame);
					frame.InstructionIndex = num;
				}
				if (num == this._tryHandler.GotoEndTargetIndex)
				{
					frame.InstructionIndex += instructions[num].Run(frame);
				}
			}
			catch (RethrowException)
			{
				throw;
			}
			catch (Exception ex)
			{
				frame.SaveTraceToException(ex);
				if (!this._tryHandler.IsCatchBlockExist)
				{
					throw;
				}
				ExceptionHandler exceptionHandler;
				frame.InstructionIndex += this._tryHandler.GotoHandler(frame, ex, out exceptionHandler);
				if (exceptionHandler == null)
				{
					throw;
				}
				ThreadAbortException ex2 = ex as ThreadAbortException;
				if (ex2 != null)
				{
					Interpreter.AnyAbortException = ex2;
					frame.CurrentAbortHandler = exceptionHandler;
				}
				bool flag = false;
				try
				{
					int num2 = frame.InstructionIndex;
					while (num2 >= exceptionHandler.HandlerStartIndex && num2 < exceptionHandler.HandlerEndIndex)
					{
						num2 += instructions[num2].Run(frame);
						frame.InstructionIndex = num2;
					}
					if (num2 == this._tryHandler.GotoEndTargetIndex)
					{
						frame.InstructionIndex += instructions[num2].Run(frame);
					}
				}
				catch (RethrowException)
				{
					flag = true;
				}
				if (flag)
				{
					throw;
				}
			}
			finally
			{
				if (this._tryHandler.IsFinallyBlockExist)
				{
					int num3 = frame.InstructionIndex = this._tryHandler.FinallyStartIndex;
					while (num3 >= this._tryHandler.FinallyStartIndex && num3 < this._tryHandler.FinallyEndIndex)
					{
						num3 += instructions[num3].Run(frame);
						frame.InstructionIndex = num3;
					}
				}
			}
			return frame.InstructionIndex - instructionIndex;
		}

		// Token: 0x17000F0E RID: 3854
		// (get) Token: 0x060046E8 RID: 18152 RVA: 0x0017A9A0 File Offset: 0x00178BA0
		public override string InstructionName
		{
			get
			{
				if (!this._hasFinally)
				{
					return "EnterTryCatch";
				}
				return "EnterTryFinally";
			}
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x0017A9B5 File Offset: 0x00178BB5
		public override string ToString()
		{
			if (!this._hasFinally)
			{
				return "EnterTryCatch";
			}
			return "EnterTryFinally[" + this._labelIndex + "]";
		}

		// Token: 0x040022CB RID: 8907
		private readonly bool _hasFinally;

		// Token: 0x040022CC RID: 8908
		private TryCatchFinallyHandler _tryHandler;
	}
}
