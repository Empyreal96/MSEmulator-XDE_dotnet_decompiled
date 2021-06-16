using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004B2 RID: 1202
	internal sealed class FormatErrorManager
	{
		// Token: 0x0600357C RID: 13692 RVA: 0x00123A30 File Offset: 0x00121C30
		internal FormatErrorManager(FormatErrorPolicy formatErrorPolicy)
		{
			this.formatErrorPolicy = formatErrorPolicy;
		}

		// Token: 0x0600357D RID: 13693 RVA: 0x00123A4C File Offset: 0x00121C4C
		internal void LogMshExpressionFailedResult(MshExpressionResult result, object sourceObject)
		{
			if (!this.formatErrorPolicy.ShowErrorsAsMessages)
			{
				return;
			}
			MshExpressionError mshExpressionError = new MshExpressionError();
			mshExpressionError.result = result;
			mshExpressionError.sourceObject = sourceObject;
			this.formattingErrorList.Add(mshExpressionError);
		}

		// Token: 0x0600357E RID: 13694 RVA: 0x00123A87 File Offset: 0x00121C87
		internal void LogStringFormatError(StringFormatError error)
		{
			if (!this.formatErrorPolicy.ShowErrorsAsMessages)
			{
				return;
			}
			this.formattingErrorList.Add(error);
		}

		// Token: 0x17000C03 RID: 3075
		// (get) Token: 0x0600357F RID: 13695 RVA: 0x00123AA3 File Offset: 0x00121CA3
		internal bool DisplayErrorStrings
		{
			get
			{
				return this.formatErrorPolicy.ShowErrorsInFormattedOutput;
			}
		}

		// Token: 0x17000C04 RID: 3076
		// (get) Token: 0x06003580 RID: 13696 RVA: 0x00123AB0 File Offset: 0x00121CB0
		internal bool DisplayFormatErrorString
		{
			get
			{
				return this.DisplayErrorStrings;
			}
		}

		// Token: 0x17000C05 RID: 3077
		// (get) Token: 0x06003581 RID: 13697 RVA: 0x00123AB8 File Offset: 0x00121CB8
		internal string ErrorString
		{
			get
			{
				return this.formatErrorPolicy.errorStringInFormattedOutput;
			}
		}

		// Token: 0x17000C06 RID: 3078
		// (get) Token: 0x06003582 RID: 13698 RVA: 0x00123AC5 File Offset: 0x00121CC5
		internal string FormatErrorString
		{
			get
			{
				return this.formatErrorPolicy.formatErrorStringInFormattedOutput;
			}
		}

		// Token: 0x06003583 RID: 13699 RVA: 0x00123AD4 File Offset: 0x00121CD4
		internal List<ErrorRecord> DrainFailedResultList()
		{
			if (!this.formatErrorPolicy.ShowErrorsAsMessages)
			{
				return null;
			}
			List<ErrorRecord> list = new List<ErrorRecord>();
			foreach (FormattingError error in this.formattingErrorList)
			{
				ErrorRecord errorRecord = FormatErrorManager.GenerateErrorRecord(error);
				if (errorRecord != null)
				{
					list.Add(errorRecord);
				}
			}
			this.formattingErrorList.Clear();
			return list;
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x00123B54 File Offset: 0x00121D54
		private static ErrorRecord GenerateErrorRecord(FormattingError error)
		{
			ErrorRecord errorRecord = null;
			MshExpressionError mshExpressionError = error as MshExpressionError;
			if (mshExpressionError != null)
			{
				errorRecord = new ErrorRecord(mshExpressionError.result.Exception, "mshExpressionError", ErrorCategory.InvalidArgument, mshExpressionError.sourceObject);
				string message = StringUtil.Format(FormatAndOut_format_xxx.MshExpressionError, mshExpressionError.result.ResolvedExpression.ToString());
				errorRecord.ErrorDetails = new ErrorDetails(message);
			}
			StringFormatError stringFormatError = error as StringFormatError;
			if (stringFormatError != null)
			{
				errorRecord = new ErrorRecord(stringFormatError.exception, "formattingError", ErrorCategory.InvalidArgument, stringFormatError.sourceObject);
				string message = StringUtil.Format(FormatAndOut_format_xxx.FormattingError, stringFormatError.formatString);
				errorRecord.ErrorDetails = new ErrorDetails(message);
			}
			return errorRecord;
		}

		// Token: 0x04001B46 RID: 6982
		private FormatErrorPolicy formatErrorPolicy;

		// Token: 0x04001B47 RID: 6983
		private List<FormattingError> formattingErrorList = new List<FormattingError>();
	}
}
