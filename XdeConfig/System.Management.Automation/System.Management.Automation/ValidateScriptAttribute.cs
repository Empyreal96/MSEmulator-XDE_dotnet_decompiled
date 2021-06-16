using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation
{
	// Token: 0x02000422 RID: 1058
	public sealed class ValidateScriptAttribute : ValidateEnumeratedArgumentsAttribute
	{
		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06002EEA RID: 12010 RVA: 0x00100AC5 File Offset: 0x000FECC5
		public ScriptBlock ScriptBlock
		{
			get
			{
				return this._scriptBlock;
			}
		}

		// Token: 0x06002EEB RID: 12011 RVA: 0x00100AD0 File Offset: 0x000FECD0
		protected override void ValidateElement(object element)
		{
			if (element == null)
			{
				throw new ValidationMetadataException("ArgumentIsEmpty", null, Metadata.ValidateNotNullFailure, new object[0]);
			}
			object obj = this._scriptBlock.DoInvokeReturnAsIs(true, ScriptBlock.ErrorHandlingBehavior.WriteToExternalErrorPipe, LanguagePrimitives.AsPSObjectOrNull(element), AutomationNull.Value, AutomationNull.Value, new object[0]);
			if (!LanguagePrimitives.IsTrue(obj))
			{
				throw new ValidationMetadataException("ValidateScriptFailure", null, Metadata.ValidateScriptFailure, new object[]
				{
					element,
					this._scriptBlock
				});
			}
		}

		// Token: 0x06002EEC RID: 12012 RVA: 0x00100B49 File Offset: 0x000FED49
		public ValidateScriptAttribute(ScriptBlock scriptBlock)
		{
			if (scriptBlock == null)
			{
				throw PSTraceSource.NewArgumentException("scriptBlock");
			}
			this._scriptBlock = scriptBlock;
		}

		// Token: 0x040018AC RID: 6316
		private ScriptBlock _scriptBlock;
	}
}
