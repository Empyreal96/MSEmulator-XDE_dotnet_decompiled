using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000998 RID: 2456
	public class ParameterBindingResult
	{
		// Token: 0x06005A90 RID: 23184 RVA: 0x001E6CD8 File Offset: 0x001E4ED8
		internal ParameterBindingResult(CompiledCommandParameter parameter, CommandElementAst value, object constantValue)
		{
			this.Parameter = new ParameterMetadata(parameter);
			this.Value = value;
			this.ConstantValue = constantValue;
		}

		// Token: 0x06005A91 RID: 23185 RVA: 0x001E6CFA File Offset: 0x001E4EFA
		internal ParameterBindingResult()
		{
		}

		// Token: 0x17001218 RID: 4632
		// (get) Token: 0x06005A92 RID: 23186 RVA: 0x001E6D02 File Offset: 0x001E4F02
		// (set) Token: 0x06005A93 RID: 23187 RVA: 0x001E6D0A File Offset: 0x001E4F0A
		public ParameterMetadata Parameter { get; internal set; }

		// Token: 0x17001219 RID: 4633
		// (get) Token: 0x06005A94 RID: 23188 RVA: 0x001E6D13 File Offset: 0x001E4F13
		// (set) Token: 0x06005A95 RID: 23189 RVA: 0x001E6D1B File Offset: 0x001E4F1B
		public object ConstantValue
		{
			get
			{
				return this.constantValue;
			}
			internal set
			{
				if (value != null)
				{
					this.constantValue = value;
				}
			}
		}

		// Token: 0x1700121A RID: 4634
		// (get) Token: 0x06005A96 RID: 23190 RVA: 0x001E6D27 File Offset: 0x001E4F27
		// (set) Token: 0x06005A97 RID: 23191 RVA: 0x001E6D30 File Offset: 0x001E4F30
		public CommandElementAst Value
		{
			get
			{
				return this.value;
			}
			internal set
			{
				this.value = value;
				ConstantExpressionAst constantExpressionAst = value as ConstantExpressionAst;
				if (constantExpressionAst != null)
				{
					this.ConstantValue = constantExpressionAst.Value;
				}
			}
		}

		// Token: 0x04003067 RID: 12391
		private object constantValue;

		// Token: 0x04003068 RID: 12392
		private CommandElementAst value;
	}
}
