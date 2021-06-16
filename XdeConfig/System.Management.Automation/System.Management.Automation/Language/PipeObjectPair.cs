using System;

namespace System.Management.Automation.Language
{
	// Token: 0x02000991 RID: 2449
	internal sealed class PipeObjectPair : AstParameterArgumentPair
	{
		// Token: 0x06005A77 RID: 23159 RVA: 0x001E6010 File Offset: 0x001E4210
		internal PipeObjectPair(string parameterName, Type pipeObjType)
		{
			if (parameterName == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameterName");
			}
			base.Parameter = null;
			base.ParameterArgumentType = AstParameterArgumentType.PipeObject;
			base.ParameterSpecified = true;
			base.ArgumentSpecified = true;
			base.ParameterName = parameterName;
			base.ParameterText = parameterName;
			base.ArgumentType = pipeObjType;
		}
	}
}
