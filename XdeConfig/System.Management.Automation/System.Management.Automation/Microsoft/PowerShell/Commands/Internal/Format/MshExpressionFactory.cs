using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x02000913 RID: 2323
	internal sealed class MshExpressionFactory
	{
		// Token: 0x0600573A RID: 22330 RVA: 0x001C8035 File Offset: 0x001C6235
		internal void VerifyScriptBlockText(string scriptText)
		{
			ScriptBlock.Create(scriptText);
		}

		// Token: 0x0600573B RID: 22331 RVA: 0x001C803E File Offset: 0x001C623E
		internal MshExpression CreateFromExpressionToken(ExpressionToken et)
		{
			return this.CreateFromExpressionToken(et, null);
		}

		// Token: 0x0600573C RID: 22332 RVA: 0x001C8048 File Offset: 0x001C6248
		internal MshExpression CreateFromExpressionToken(ExpressionToken et, DatabaseLoadingInfo loadingInfo)
		{
			if (et.isScriptBlock)
			{
				if (this._expressionCache != null)
				{
					MshExpression result;
					if (this._expressionCache.TryGetValue(et, out result))
					{
						return result;
					}
				}
				else
				{
					this._expressionCache = new Dictionary<ExpressionToken, MshExpression>();
				}
				ScriptBlock scriptBlock = ScriptBlock.Create(et.expressionValue);
				scriptBlock.DebuggerStepThrough = true;
				if (loadingInfo != null && loadingInfo.isFullyTrusted)
				{
					scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
				}
				MshExpression mshExpression = new MshExpression(scriptBlock);
				this._expressionCache.Add(et, mshExpression);
				return mshExpression;
			}
			return new MshExpression(et.expressionValue);
		}

		// Token: 0x04002E74 RID: 11892
		private Dictionary<ExpressionToken, MshExpression> _expressionCache;
	}
}
