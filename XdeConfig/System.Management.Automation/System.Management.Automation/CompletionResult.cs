using System;

namespace System.Management.Automation
{
	// Token: 0x0200098C RID: 2444
	public class CompletionResult
	{
		// Token: 0x170011FF RID: 4607
		// (get) Token: 0x06005A4E RID: 23118 RVA: 0x001E5C7B File Offset: 0x001E3E7B
		public string CompletionText
		{
			get
			{
				if (this == CompletionResult.NullInstance)
				{
					throw PSTraceSource.NewInvalidOperationException(TabCompletionStrings.NoAccessToProperties, new object[0]);
				}
				return this.completionText;
			}
		}

		// Token: 0x17001200 RID: 4608
		// (get) Token: 0x06005A4F RID: 23119 RVA: 0x001E5C9C File Offset: 0x001E3E9C
		public string ListItemText
		{
			get
			{
				if (this == CompletionResult.NullInstance)
				{
					throw PSTraceSource.NewInvalidOperationException(TabCompletionStrings.NoAccessToProperties, new object[0]);
				}
				return this.listItemText;
			}
		}

		// Token: 0x17001201 RID: 4609
		// (get) Token: 0x06005A50 RID: 23120 RVA: 0x001E5CBD File Offset: 0x001E3EBD
		public CompletionResultType ResultType
		{
			get
			{
				if (this == CompletionResult.NullInstance)
				{
					throw PSTraceSource.NewInvalidOperationException(TabCompletionStrings.NoAccessToProperties, new object[0]);
				}
				return this.resultType;
			}
		}

		// Token: 0x17001202 RID: 4610
		// (get) Token: 0x06005A51 RID: 23121 RVA: 0x001E5CDE File Offset: 0x001E3EDE
		public string ToolTip
		{
			get
			{
				if (this == CompletionResult.NullInstance)
				{
					throw PSTraceSource.NewInvalidOperationException(TabCompletionStrings.NoAccessToProperties, new object[0]);
				}
				return this.toolTip;
			}
		}

		// Token: 0x17001203 RID: 4611
		// (get) Token: 0x06005A52 RID: 23122 RVA: 0x001E5CFF File Offset: 0x001E3EFF
		internal static CompletionResult Null
		{
			get
			{
				return CompletionResult.NullInstance;
			}
		}

		// Token: 0x06005A53 RID: 23123 RVA: 0x001E5D08 File Offset: 0x001E3F08
		public CompletionResult(string completionText, string listItemText, CompletionResultType resultType, string toolTip)
		{
			if (string.IsNullOrEmpty(completionText))
			{
				throw PSTraceSource.NewArgumentNullException("completionText");
			}
			if (string.IsNullOrEmpty(listItemText))
			{
				throw PSTraceSource.NewArgumentNullException("listItemText");
			}
			if (resultType < CompletionResultType.Text || resultType > CompletionResultType.DynamicKeyword)
			{
				throw PSTraceSource.NewArgumentOutOfRangeException("resultType", resultType);
			}
			if (string.IsNullOrEmpty(toolTip))
			{
				throw PSTraceSource.NewArgumentNullException("toolTip");
			}
			this.completionText = completionText;
			this.listItemText = listItemText;
			this.toolTip = toolTip;
			this.resultType = resultType;
		}

		// Token: 0x06005A54 RID: 23124 RVA: 0x001E5D8C File Offset: 0x001E3F8C
		public CompletionResult(string completionText) : this(completionText, completionText, CompletionResultType.Text, completionText)
		{
		}

		// Token: 0x06005A55 RID: 23125 RVA: 0x001E5D98 File Offset: 0x001E3F98
		private CompletionResult()
		{
		}

		// Token: 0x04003045 RID: 12357
		private string completionText;

		// Token: 0x04003046 RID: 12358
		private string listItemText;

		// Token: 0x04003047 RID: 12359
		private string toolTip;

		// Token: 0x04003048 RID: 12360
		private CompletionResultType resultType;

		// Token: 0x04003049 RID: 12361
		private static readonly CompletionResult NullInstance = new CompletionResult();
	}
}
