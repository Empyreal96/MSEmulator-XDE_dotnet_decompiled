using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000994 RID: 2452
	internal sealed class SwitchPair : AstParameterArgumentPair
	{
		// Token: 0x06005A7B RID: 23163 RVA: 0x001E6154 File Offset: 0x001E4354
		internal SwitchPair(CommandParameterAst parameterAst)
		{
			if (parameterAst == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameterAst");
			}
			base.Parameter = parameterAst;
			base.ParameterArgumentType = AstParameterArgumentType.Switch;
			base.ParameterSpecified = true;
			base.ArgumentSpecified = true;
			base.ParameterName = parameterAst.ParameterName;
			base.ParameterText = parameterAst.ParameterName;
			base.ArgumentType = typeof(bool);
		}

		// Token: 0x17001212 RID: 4626
		// (get) Token: 0x06005A7C RID: 23164 RVA: 0x001E61B9 File Offset: 0x001E43B9
		public bool Argument
		{
			get
			{
				return true;
			}
		}
	}
}
