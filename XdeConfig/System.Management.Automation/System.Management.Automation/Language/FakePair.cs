using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000993 RID: 2451
	internal sealed class FakePair : AstParameterArgumentPair
	{
		// Token: 0x06005A7A RID: 23162 RVA: 0x001E60EC File Offset: 0x001E42EC
		internal FakePair(CommandParameterAst parameterAst)
		{
			if (parameterAst == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameterAst");
			}
			base.Parameter = parameterAst;
			base.ParameterArgumentType = AstParameterArgumentType.Fake;
			base.ParameterSpecified = true;
			base.ArgumentSpecified = true;
			base.ParameterName = parameterAst.ParameterName;
			base.ParameterText = parameterAst.ParameterName;
			base.ArgumentType = typeof(object);
		}
	}
}
