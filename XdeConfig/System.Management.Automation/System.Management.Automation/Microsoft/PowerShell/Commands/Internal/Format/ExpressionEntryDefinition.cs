using System;
using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004BB RID: 1211
	internal class ExpressionEntryDefinition : HashtableEntryDefinition
	{
		// Token: 0x0600358D RID: 13709 RVA: 0x00123F00 File Offset: 0x00122100
		internal ExpressionEntryDefinition() : this(false)
		{
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x00123F0C File Offset: 0x0012210C
		internal ExpressionEntryDefinition(bool noGlobbing) : base("expression", new Type[]
		{
			typeof(string),
			typeof(ScriptBlock)
		}, true)
		{
			this._noGlobbing = noGlobbing;
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x00123F50 File Offset: 0x00122150
		internal override Hashtable CreateHashtableFromSingleType(object val)
		{
			return new Hashtable
			{
				{
					"expression",
					val
				}
			};
		}

		// Token: 0x06003590 RID: 13712 RVA: 0x00123F70 File Offset: 0x00122170
		internal override object Verify(object val, TerminatingErrorContext invocationContext, bool originalParameterWasHashTable)
		{
			if (val == null)
			{
				throw PSTraceSource.NewArgumentNullException("val");
			}
			ScriptBlock scriptBlock = val as ScriptBlock;
			if (scriptBlock != null)
			{
				return new MshExpression(scriptBlock);
			}
			string text = val as string;
			if (text != null)
			{
				if (string.IsNullOrEmpty(text))
				{
					this.ProcessEmptyStringError(originalParameterWasHashTable, invocationContext);
				}
				MshExpression mshExpression = new MshExpression(text);
				if (this._noGlobbing && mshExpression.HasWildCardCharacters)
				{
					this.ProcessGlobbingCharactersError(originalParameterWasHashTable, text, invocationContext);
				}
				return mshExpression;
			}
			PSTraceSource.NewArgumentException("val");
			return null;
		}

		// Token: 0x06003591 RID: 13713 RVA: 0x00123FE8 File Offset: 0x001221E8
		private void ProcessEmptyStringError(bool originalParameterWasHashTable, TerminatingErrorContext invocationContext)
		{
			string msg;
			string errorId;
			if (originalParameterWasHashTable)
			{
				msg = StringUtil.Format(FormatAndOut_MshParameter.MshExEmptyStringHashError, base.KeyName);
				errorId = "ExpressionEmptyString1";
			}
			else
			{
				msg = StringUtil.Format(FormatAndOut_MshParameter.MshExEmptyStringError, new object[0]);
				errorId = "ExpressionEmptyString2";
			}
			ParameterProcessor.ThrowParameterBindingException(invocationContext, errorId, msg);
		}

		// Token: 0x06003592 RID: 13714 RVA: 0x00124030 File Offset: 0x00122230
		private void ProcessGlobbingCharactersError(bool originalParameterWasHashTable, string expression, TerminatingErrorContext invocationContext)
		{
			string msg;
			string errorId;
			if (originalParameterWasHashTable)
			{
				msg = StringUtil.Format(FormatAndOut_MshParameter.MshExGlobbingHashError, base.KeyName, expression);
				errorId = "ExpressionGlobbing1";
			}
			else
			{
				msg = StringUtil.Format(FormatAndOut_MshParameter.MshExGlobbingStringError, expression);
				errorId = "ExpressionGlobbing2";
			}
			ParameterProcessor.ThrowParameterBindingException(invocationContext, errorId, msg);
		}

		// Token: 0x04001B61 RID: 7009
		private bool _noGlobbing;
	}
}
