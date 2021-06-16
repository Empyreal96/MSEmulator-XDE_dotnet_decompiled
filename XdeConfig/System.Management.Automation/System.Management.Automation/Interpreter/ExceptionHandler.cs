using System;
using System.Globalization;

namespace System.Management.Automation.Interpreter
{
	// Token: 0x020006E5 RID: 1765
	internal sealed class ExceptionHandler
	{
		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x060048CA RID: 18634 RVA: 0x0017F712 File Offset: 0x0017D912
		public bool IsFault
		{
			get
			{
				return this.ExceptionType == null;
			}
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x0017F720 File Offset: 0x0017D920
		internal ExceptionHandler(int start, int end, int labelIndex, int handlerStartIndex, int handlerEndIndex, Type exceptionType)
		{
			this.StartIndex = start;
			this.EndIndex = end;
			this.LabelIndex = labelIndex;
			this.ExceptionType = exceptionType;
			this.HandlerStartIndex = handlerStartIndex;
			this.HandlerEndIndex = handlerEndIndex;
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x0017F755 File Offset: 0x0017D955
		internal void SetParent(TryCatchFinallyHandler tryHandler)
		{
			this.Parent = tryHandler;
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x0017F75E File Offset: 0x0017D95E
		public bool Matches(Type exceptionType)
		{
			return this.ExceptionType == null || this.ExceptionType.IsAssignableFrom(exceptionType);
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x0017F77F File Offset: 0x0017D97F
		public bool IsBetterThan(ExceptionHandler other)
		{
			return other == null || this.HandlerStartIndex < other.HandlerStartIndex;
		}

		// Token: 0x060048CF RID: 18639 RVA: 0x0017F794 File Offset: 0x0017D994
		internal bool IsInsideTryBlock(int index)
		{
			return index >= this.StartIndex && index < this.EndIndex;
		}

		// Token: 0x060048D0 RID: 18640 RVA: 0x0017F7AA File Offset: 0x0017D9AA
		internal bool IsInsideCatchBlock(int index)
		{
			return index >= this.HandlerStartIndex && index < this.HandlerEndIndex;
		}

		// Token: 0x060048D1 RID: 18641 RVA: 0x0017F7C0 File Offset: 0x0017D9C0
		internal bool IsInsideFinallyBlock(int index)
		{
			return this.Parent.IsFinallyBlockExist && index >= this.Parent.FinallyStartIndex && index < this.Parent.FinallyEndIndex;
		}

		// Token: 0x060048D2 RID: 18642 RVA: 0x0017F7F0 File Offset: 0x0017D9F0
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} [{1}-{2}] [{3}->{4}]", new object[]
			{
				this.IsFault ? "fault" : ("catch(" + this.ExceptionType.Name + ")"),
				this.StartIndex,
				this.EndIndex,
				this.HandlerStartIndex,
				this.HandlerEndIndex
			});
		}

		// Token: 0x04002390 RID: 9104
		public readonly Type ExceptionType;

		// Token: 0x04002391 RID: 9105
		public readonly int StartIndex;

		// Token: 0x04002392 RID: 9106
		public readonly int EndIndex;

		// Token: 0x04002393 RID: 9107
		public readonly int LabelIndex;

		// Token: 0x04002394 RID: 9108
		public readonly int HandlerStartIndex;

		// Token: 0x04002395 RID: 9109
		public readonly int HandlerEndIndex;

		// Token: 0x04002396 RID: 9110
		internal TryCatchFinallyHandler Parent;
	}
}
