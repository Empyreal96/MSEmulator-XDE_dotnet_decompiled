using System;
using System.Linq;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006E6 RID: 1766
	internal sealed class TryCatchFinallyHandler
	{
		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x060048D3 RID: 18643 RVA: 0x0017F878 File Offset: 0x0017DA78
		internal bool IsFinallyBlockExist
		{
			get
			{
				return this.FinallyStartIndex != int.MaxValue && this.FinallyEndIndex != int.MaxValue;
			}
		}

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x060048D4 RID: 18644 RVA: 0x0017F899 File Offset: 0x0017DA99
		internal bool IsCatchBlockExist
		{
			get
			{
				return this._handlers != null;
			}
		}

		// Token: 0x060048D5 RID: 18645 RVA: 0x0017F8A7 File Offset: 0x0017DAA7
		internal TryCatchFinallyHandler(int tryStart, int tryEnd, int gotoEndTargetIndex, ExceptionHandler[] handlers) : this(tryStart, tryEnd, gotoEndTargetIndex, int.MaxValue, int.MaxValue, handlers)
		{
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x0017F8BE File Offset: 0x0017DABE
		internal TryCatchFinallyHandler(int tryStart, int tryEnd, int gotoEndTargetIndex, int finallyStart, int finallyEnd) : this(tryStart, tryEnd, gotoEndTargetIndex, finallyStart, finallyEnd, null)
		{
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x0017F8D0 File Offset: 0x0017DAD0
		internal TryCatchFinallyHandler(int tryStart, int tryEnd, int gotoEndLabelIndex, int finallyStart, int finallyEnd, ExceptionHandler[] handlers)
		{
			this.TryStartIndex = tryStart;
			this.TryEndIndex = tryEnd;
			this.FinallyStartIndex = finallyStart;
			this.FinallyEndIndex = finallyEnd;
			this.GotoEndTargetIndex = gotoEndLabelIndex;
			this._handlers = handlers;
			if (this._handlers != null)
			{
				for (int i = 0; i < this._handlers.Length; i++)
				{
					ExceptionHandler exceptionHandler = this._handlers[i];
					exceptionHandler.SetParent(this);
				}
			}
		}

		// Token: 0x060048D8 RID: 18648 RVA: 0x0017F990 File Offset: 0x0017DB90
		internal int GotoHandler(InterpretedFrame frame, object exception, out ExceptionHandler handler)
		{
			handler = this._handlers.FirstOrDefault((ExceptionHandler t) => t.Matches(exception.GetType()));
			if (handler == null)
			{
				return 0;
			}
			return frame.Goto(handler.LabelIndex, exception, true);
		}

		// Token: 0x04002397 RID: 9111
		internal readonly int TryStartIndex = int.MaxValue;

		// Token: 0x04002398 RID: 9112
		internal readonly int TryEndIndex = int.MaxValue;

		// Token: 0x04002399 RID: 9113
		internal readonly int FinallyStartIndex = int.MaxValue;

		// Token: 0x0400239A RID: 9114
		internal readonly int FinallyEndIndex = int.MaxValue;

		// Token: 0x0400239B RID: 9115
		internal readonly int GotoEndTargetIndex = int.MaxValue;

		// Token: 0x0400239C RID: 9116
		private readonly ExceptionHandler[] _handlers;
	}
}
