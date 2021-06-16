using System;
using System.Collections.Generic;
using System.Linq;

namespace System.Management.Automation.Language
{
	// Token: 0x02000992 RID: 2450
	internal sealed class AstArrayPair : AstParameterArgumentPair
	{
		// Token: 0x06005A78 RID: 23160 RVA: 0x001E6064 File Offset: 0x001E4264
		internal AstArrayPair(string parameterName, ICollection<ExpressionAst> arguments)
		{
			if (parameterName == null)
			{
				throw PSTraceSource.NewArgumentNullException("parameterName");
			}
			if (arguments == null || arguments.Count == 0)
			{
				throw PSTraceSource.NewArgumentNullException("arguments");
			}
			base.Parameter = null;
			base.ParameterArgumentType = AstParameterArgumentType.AstArray;
			base.ParameterSpecified = true;
			base.ArgumentSpecified = true;
			base.ParameterName = parameterName;
			base.ParameterText = parameterName;
			base.ArgumentType = typeof(Array);
			this._argument = arguments.ToArray<ExpressionAst>();
		}

		// Token: 0x17001211 RID: 4625
		// (get) Token: 0x06005A79 RID: 23161 RVA: 0x001E60E1 File Offset: 0x001E42E1
		public ExpressionAst[] Argument
		{
			get
			{
				return this._argument;
			}
		}

		// Token: 0x0400305E RID: 12382
		private ExpressionAst[] _argument;
	}
}
