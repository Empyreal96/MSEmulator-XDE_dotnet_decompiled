using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000995 RID: 2453
	internal sealed class AstPair : AstParameterArgumentPair
	{
		// Token: 0x06005A7D RID: 23165 RVA: 0x001E61BC File Offset: 0x001E43BC
		internal AstPair(CommandParameterAst parameterAst)
		{
			if (parameterAst == null || parameterAst.Argument == null)
			{
				throw PSTraceSource.NewArgumentException("parameterAst");
			}
			base.Parameter = parameterAst;
			base.ParameterArgumentType = AstParameterArgumentType.AstPair;
			base.ParameterSpecified = true;
			base.ArgumentSpecified = true;
			base.ParameterName = parameterAst.ParameterName;
			base.ParameterText = "-" + base.ParameterName + ":";
			base.ArgumentType = parameterAst.Argument.StaticType;
			this._parameterContainsArgument = true;
			this._argument = parameterAst.Argument;
		}

		// Token: 0x06005A7E RID: 23166 RVA: 0x001E624C File Offset: 0x001E444C
		internal AstPair(CommandParameterAst parameterAst, ExpressionAst argumentAst)
		{
			if (parameterAst != null && parameterAst.Argument != null)
			{
				throw PSTraceSource.NewArgumentException("parameterAst");
			}
			if (parameterAst == null && argumentAst == null)
			{
				throw PSTraceSource.NewArgumentNullException("argumentAst");
			}
			base.Parameter = parameterAst;
			base.ParameterArgumentType = AstParameterArgumentType.AstPair;
			base.ParameterSpecified = (parameterAst != null);
			base.ArgumentSpecified = (argumentAst != null);
			base.ParameterName = ((parameterAst != null) ? parameterAst.ParameterName : null);
			base.ParameterText = ((parameterAst != null) ? parameterAst.ParameterName : null);
			base.ArgumentType = ((argumentAst != null) ? argumentAst.StaticType : null);
			this._parameterContainsArgument = false;
			this._argument = argumentAst;
		}

		// Token: 0x06005A7F RID: 23167 RVA: 0x001E62F4 File Offset: 0x001E44F4
		internal AstPair(CommandParameterAst parameterAst, CommandElementAst argumentAst)
		{
			if (parameterAst != null && parameterAst.Argument != null)
			{
				throw PSTraceSource.NewArgumentException("parameterAst");
			}
			if (parameterAst == null || argumentAst == null)
			{
				throw PSTraceSource.NewArgumentNullException("argumentAst");
			}
			base.Parameter = parameterAst;
			base.ParameterArgumentType = AstParameterArgumentType.AstPair;
			base.ParameterSpecified = true;
			base.ArgumentSpecified = true;
			base.ParameterName = parameterAst.ParameterName;
			base.ParameterText = parameterAst.ParameterName;
			base.ArgumentType = typeof(string);
			this._parameterContainsArgument = false;
			this._argument = argumentAst;
			this._argumentIsCommandParameterAst = true;
		}

		// Token: 0x17001213 RID: 4627
		// (get) Token: 0x06005A80 RID: 23168 RVA: 0x001E6387 File Offset: 0x001E4587
		public bool ParameterContainsArgument
		{
			get
			{
				return this._parameterContainsArgument;
			}
		}

		// Token: 0x17001214 RID: 4628
		// (get) Token: 0x06005A81 RID: 23169 RVA: 0x001E638F File Offset: 0x001E458F
		public bool ArgumentIsCommandParameterAst
		{
			get
			{
				return this._argumentIsCommandParameterAst;
			}
		}

		// Token: 0x17001215 RID: 4629
		// (get) Token: 0x06005A82 RID: 23170 RVA: 0x001E6397 File Offset: 0x001E4597
		public CommandElementAst Argument
		{
			get
			{
				return this._argument;
			}
		}

		// Token: 0x0400305F RID: 12383
		private CommandElementAst _argument;

		// Token: 0x04003060 RID: 12384
		private bool _parameterContainsArgument;

		// Token: 0x04003061 RID: 12385
		private bool _argumentIsCommandParameterAst;
	}
}
