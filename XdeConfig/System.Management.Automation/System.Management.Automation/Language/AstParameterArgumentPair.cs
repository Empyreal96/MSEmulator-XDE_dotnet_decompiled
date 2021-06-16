using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000990 RID: 2448
	internal abstract class AstParameterArgumentPair
	{
		// Token: 0x1700120A RID: 4618
		// (get) Token: 0x06005A68 RID: 23144 RVA: 0x001E5F91 File Offset: 0x001E4191
		// (set) Token: 0x06005A69 RID: 23145 RVA: 0x001E5F99 File Offset: 0x001E4199
		public CommandParameterAst Parameter
		{
			get
			{
				return this._parameter;
			}
			protected set
			{
				this._parameter = value;
			}
		}

		// Token: 0x1700120B RID: 4619
		// (get) Token: 0x06005A6A RID: 23146 RVA: 0x001E5FA2 File Offset: 0x001E41A2
		// (set) Token: 0x06005A6B RID: 23147 RVA: 0x001E5FAA File Offset: 0x001E41AA
		public AstParameterArgumentType ParameterArgumentType
		{
			get
			{
				return this._parameterArgumentType;
			}
			protected set
			{
				this._parameterArgumentType = value;
			}
		}

		// Token: 0x1700120C RID: 4620
		// (get) Token: 0x06005A6C RID: 23148 RVA: 0x001E5FB3 File Offset: 0x001E41B3
		// (set) Token: 0x06005A6D RID: 23149 RVA: 0x001E5FBB File Offset: 0x001E41BB
		public bool ParameterSpecified
		{
			get
			{
				return this._parameterSpecified;
			}
			protected set
			{
				this._parameterSpecified = value;
			}
		}

		// Token: 0x1700120D RID: 4621
		// (get) Token: 0x06005A6E RID: 23150 RVA: 0x001E5FC4 File Offset: 0x001E41C4
		// (set) Token: 0x06005A6F RID: 23151 RVA: 0x001E5FCC File Offset: 0x001E41CC
		public bool ArgumentSpecified
		{
			get
			{
				return this._argumentSpecified;
			}
			protected set
			{
				this._argumentSpecified = value;
			}
		}

		// Token: 0x1700120E RID: 4622
		// (get) Token: 0x06005A70 RID: 23152 RVA: 0x001E5FD5 File Offset: 0x001E41D5
		// (set) Token: 0x06005A71 RID: 23153 RVA: 0x001E5FDD File Offset: 0x001E41DD
		public string ParameterName
		{
			get
			{
				return this._parameterName;
			}
			protected set
			{
				this._parameterName = value;
			}
		}

		// Token: 0x1700120F RID: 4623
		// (get) Token: 0x06005A72 RID: 23154 RVA: 0x001E5FE6 File Offset: 0x001E41E6
		// (set) Token: 0x06005A73 RID: 23155 RVA: 0x001E5FEE File Offset: 0x001E41EE
		public string ParameterText
		{
			get
			{
				return this._parameterText;
			}
			protected set
			{
				this._parameterText = value;
			}
		}

		// Token: 0x17001210 RID: 4624
		// (get) Token: 0x06005A74 RID: 23156 RVA: 0x001E5FF7 File Offset: 0x001E41F7
		// (set) Token: 0x06005A75 RID: 23157 RVA: 0x001E5FFF File Offset: 0x001E41FF
		public Type ArgumentType
		{
			get
			{
				return this._argumentType;
			}
			protected set
			{
				this._argumentType = value;
			}
		}

		// Token: 0x04003057 RID: 12375
		private CommandParameterAst _parameter;

		// Token: 0x04003058 RID: 12376
		private AstParameterArgumentType _parameterArgumentType;

		// Token: 0x04003059 RID: 12377
		private bool _parameterSpecified;

		// Token: 0x0400305A RID: 12378
		private bool _argumentSpecified;

		// Token: 0x0400305B RID: 12379
		private string _parameterName;

		// Token: 0x0400305C RID: 12380
		private string _parameterText;

		// Token: 0x0400305D RID: 12381
		private Type _argumentType;
	}
}
