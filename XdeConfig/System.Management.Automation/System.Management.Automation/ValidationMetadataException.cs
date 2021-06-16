using System;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x0200087F RID: 2175
	[Serializable]
	public class ValidationMetadataException : MetadataException
	{
		// Token: 0x06005338 RID: 21304 RVA: 0x001BA193 File Offset: 0x001B8393
		protected ValidationMetadataException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x001BA19D File Offset: 0x001B839D
		public ValidationMetadataException() : base(typeof(ValidationMetadataException).FullName)
		{
		}

		// Token: 0x0600533A RID: 21306 RVA: 0x001BA1B4 File Offset: 0x001B83B4
		public ValidationMetadataException(string message) : this(message, false)
		{
		}

		// Token: 0x0600533B RID: 21307 RVA: 0x001BA1BE File Offset: 0x001B83BE
		public ValidationMetadataException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// Token: 0x0600533C RID: 21308 RVA: 0x001BA1C8 File Offset: 0x001B83C8
		internal ValidationMetadataException(string errorId, Exception innerException, string resourceStr, params object[] arguments) : base(errorId, innerException, resourceStr, arguments)
		{
		}

		// Token: 0x0600533D RID: 21309 RVA: 0x001BA1D5 File Offset: 0x001B83D5
		internal ValidationMetadataException(string message, bool swallowException) : base(message)
		{
			this._swallowException = swallowException;
		}

		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x0600533E RID: 21310 RVA: 0x001BA1E5 File Offset: 0x001B83E5
		internal bool SwallowException
		{
			get
			{
				return this._swallowException;
			}
		}

		// Token: 0x04002ABB RID: 10939
		internal const string ValidateRangeElementType = "ValidateRangeElementType";

		// Token: 0x04002ABC RID: 10940
		internal const string ValidateRangeMinRangeMaxRangeType = "ValidateRangeMinRangeMaxRangeType";

		// Token: 0x04002ABD RID: 10941
		internal const string ValidateRangeNotIComparable = "ValidateRangeNotIComparable";

		// Token: 0x04002ABE RID: 10942
		internal const string ValidateRangeMaxRangeSmallerThanMinRange = "ValidateRangeMaxRangeSmallerThanMinRange";

		// Token: 0x04002ABF RID: 10943
		internal const string ValidateRangeGreaterThanMaxRangeFailure = "ValidateRangeGreaterThanMaxRangeFailure";

		// Token: 0x04002AC0 RID: 10944
		internal const string ValidateRangeSmallerThanMinRangeFailure = "ValidateRangeSmallerThanMinRangeFailure";

		// Token: 0x04002AC1 RID: 10945
		internal const string ValidateFailureResult = "ValidateFailureResult";

		// Token: 0x04002AC2 RID: 10946
		internal const string ValidatePatternFailure = "ValidatePatternFailure";

		// Token: 0x04002AC3 RID: 10947
		internal const string ValidateScriptFailure = "ValidateScriptFailure";

		// Token: 0x04002AC4 RID: 10948
		internal const string ValidateCountNotInArray = "ValidateCountNotInArray";

		// Token: 0x04002AC5 RID: 10949
		internal const string ValidateCountMaxLengthSmallerThanMinLength = "ValidateCountMaxLengthSmallerThanMinLength";

		// Token: 0x04002AC6 RID: 10950
		internal const string ValidateCountMinLengthFailure = "ValidateCountMinLengthFailure";

		// Token: 0x04002AC7 RID: 10951
		internal const string ValidateCountMaxLengthFailure = "ValidateCountMaxLengthFailure";

		// Token: 0x04002AC8 RID: 10952
		internal const string ValidateLengthMaxLengthSmallerThanMinLength = "ValidateLengthMaxLengthSmallerThanMinLength";

		// Token: 0x04002AC9 RID: 10953
		internal const string ValidateLengthNotString = "ValidateLengthNotString";

		// Token: 0x04002ACA RID: 10954
		internal const string ValidateLengthMinLengthFailure = "ValidateLengthMinLengthFailure";

		// Token: 0x04002ACB RID: 10955
		internal const string ValidateLengthMaxLengthFailure = "ValidateLengthMaxLengthFailure";

		// Token: 0x04002ACC RID: 10956
		internal const string ValidateSetFailure = "ValidateSetFailure";

		// Token: 0x04002ACD RID: 10957
		internal const string ValidateVersionFailure = "ValidateVersionFailure";

		// Token: 0x04002ACE RID: 10958
		internal const string InvalidValueFailure = "InvalidValueFailure";

		// Token: 0x04002ACF RID: 10959
		private bool _swallowException;
	}
}
