using System;
using System.Management.Automation.Language;

namespace System.Management.Automation
{
	// Token: 0x0200062E RID: 1582
	internal class ScriptBlockExpressionWrapper
	{
		// Token: 0x060044B1 RID: 17585 RVA: 0x0016FCCC File Offset: 0x0016DECC
		internal ScriptBlockExpressionWrapper(IParameterMetadataProvider ast)
		{
			this._ast = ast;
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x0016FCDC File Offset: 0x0016DEDC
		internal ScriptBlock GetScriptBlock(ExecutionContext context, bool isFilter)
		{
			ScriptBlock scriptBlock;
			if ((scriptBlock = this._scriptBlock) == null)
			{
				scriptBlock = (this._scriptBlock = new ScriptBlock(this._ast, isFilter));
			}
			ScriptBlock scriptBlock2 = scriptBlock.Clone(false);
			scriptBlock2.SessionStateInternal = context.EngineSessionState;
			return scriptBlock2;
		}

		// Token: 0x04002219 RID: 8729
		private ScriptBlock _scriptBlock;

		// Token: 0x0400221A RID: 8730
		private readonly IParameterMetadataProvider _ast;
	}
}
