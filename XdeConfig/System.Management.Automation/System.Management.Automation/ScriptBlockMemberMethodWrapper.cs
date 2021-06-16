using System;
using System.Management.Automation.Language;
using System.Management.Automation.Runspaces;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x0200062F RID: 1583
	public class ScriptBlockMemberMethodWrapper
	{
		// Token: 0x060044B3 RID: 17587 RVA: 0x0016FD4C File Offset: 0x0016DF4C
		internal ScriptBlockMemberMethodWrapper(IParameterMetadataProvider ast)
		{
			this._ast = ast;
			this._scriptBlock = new Lazy<ScriptBlock>(() => new ScriptBlock(this._ast, false));
			this._boundScriptBlock = new ThreadLocal<ScriptBlock>(() => this._scriptBlock.Value.Clone(false));
		}

		// Token: 0x060044B4 RID: 17588 RVA: 0x0016FDA4 File Offset: 0x0016DFA4
		internal void InitAtRuntime()
		{
			ScriptBlock value = this._boundScriptBlock.Value;
			ExecutionContext executionContext = Runspace.DefaultRunspace.ExecutionContext;
			value.SessionStateInternal = executionContext.EngineSessionState;
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x0016FDD4 File Offset: 0x0016DFD4
		public void InvokeHelper(object instance, object[] args)
		{
			this._boundScriptBlock.Value.InvokeAsMemberFunction(instance, args);
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x0016FDE8 File Offset: 0x0016DFE8
		public T InvokeHelperT<T>(object instance, object[] args)
		{
			return this._boundScriptBlock.Value.InvokeAsMemberFunctionT<T>(instance, args);
		}

		// Token: 0x0400221B RID: 8731
		public static readonly object[] _emptyArgumentArray = new object[0];

		// Token: 0x0400221C RID: 8732
		private Lazy<ScriptBlock> _scriptBlock;

		// Token: 0x0400221D RID: 8733
		private ThreadLocal<ScriptBlock> _boundScriptBlock;

		// Token: 0x0400221E RID: 8734
		private IParameterMetadataProvider _ast;
	}
}
