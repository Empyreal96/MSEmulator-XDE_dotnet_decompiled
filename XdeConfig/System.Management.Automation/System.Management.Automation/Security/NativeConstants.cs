using System;

namespace System.Management.Automation.Security
{
	// Token: 0x020007B7 RID: 1975
	internal class NativeConstants
	{
		// Token: 0x04002682 RID: 9858
		internal const int CRYPT_OID_INFO_OID_KEY = 1;

		// Token: 0x04002683 RID: 9859
		internal const int CRYPT_OID_INFO_NAME_KEY = 2;

		// Token: 0x04002684 RID: 9860
		internal const int CRYPT_OID_INFO_CNG_ALGID_KEY = 5;

		// Token: 0x04002685 RID: 9861
		public const int SAFER_TOKEN_NULL_IF_EQUAL = 1;

		// Token: 0x04002686 RID: 9862
		public const int SAFER_TOKEN_COMPARE_ONLY = 2;

		// Token: 0x04002687 RID: 9863
		public const int SAFER_TOKEN_MAKE_INERT = 4;

		// Token: 0x04002688 RID: 9864
		public const int SAFER_CRITERIA_IMAGEPATH = 1;

		// Token: 0x04002689 RID: 9865
		public const int SAFER_CRITERIA_NOSIGNEDHASH = 2;

		// Token: 0x0400268A RID: 9866
		public const int SAFER_CRITERIA_IMAGEHASH = 4;

		// Token: 0x0400268B RID: 9867
		public const int SAFER_CRITERIA_AUTHENTICODE = 8;

		// Token: 0x0400268C RID: 9868
		public const int SAFER_CRITERIA_URLZONE = 16;

		// Token: 0x0400268D RID: 9869
		public const int SAFER_CRITERIA_IMAGEPATH_NT = 4096;

		// Token: 0x0400268E RID: 9870
		public const int WTD_UI_NONE = 2;

		// Token: 0x0400268F RID: 9871
		public const int S_OK = 0;

		// Token: 0x04002690 RID: 9872
		public const int S_FALSE = 1;

		// Token: 0x04002691 RID: 9873
		public const int ERROR_MORE_DATA = 234;

		// Token: 0x04002692 RID: 9874
		public const int ERROR_ACCESS_DISABLED_BY_POLICY = 1260;

		// Token: 0x04002693 RID: 9875
		public const int ERROR_ACCESS_DISABLED_NO_SAFER_UI_BY_POLICY = 786;

		// Token: 0x04002694 RID: 9876
		public const int SAFER_MAX_HASH_SIZE = 64;

		// Token: 0x04002695 RID: 9877
		public const string SRP_POLICY_SCRIPT = "SCRIPT";

		// Token: 0x04002696 RID: 9878
		internal const int SIGNATURE_DISPLAYNAME_LENGTH = 260;

		// Token: 0x04002697 RID: 9879
		internal const int SIGNATURE_PUBLISHER_LENGTH = 128;

		// Token: 0x04002698 RID: 9880
		internal const int SIGNATURE_HASH_LENGTH = 64;

		// Token: 0x04002699 RID: 9881
		internal const int MAX_PATH = 260;
	}
}
