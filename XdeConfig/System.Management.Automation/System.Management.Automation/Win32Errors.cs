using System;

namespace System.Management.Automation
{
	// Token: 0x02000801 RID: 2049
	internal static class Win32Errors
	{
		// Token: 0x0400285B RID: 10331
		internal const uint NO_ERROR = 0U;

		// Token: 0x0400285C RID: 10332
		internal const uint E_FAIL = 2147500037U;

		// Token: 0x0400285D RID: 10333
		internal const uint TRUST_E_NOSIGNATURE = 2148204800U;

		// Token: 0x0400285E RID: 10334
		internal const uint TRUST_E_BAD_DIGEST = 2148098064U;

		// Token: 0x0400285F RID: 10335
		internal const uint TRUST_E_PROVIDER_UNKNOWN = 2148204545U;

		// Token: 0x04002860 RID: 10336
		internal const uint TRUST_E_SUBJECT_FORM_UNKNOWN = 2148204547U;

		// Token: 0x04002861 RID: 10337
		internal const uint CERT_E_UNTRUSTEDROOT = 2148204809U;

		// Token: 0x04002862 RID: 10338
		internal const uint TRUST_E_EXPLICIT_DISTRUST = 2148204817U;

		// Token: 0x04002863 RID: 10339
		internal const uint CRYPT_E_BAD_MSG = 2148081677U;

		// Token: 0x04002864 RID: 10340
		internal const uint NTE_BAD_ALGID = 2148073480U;
	}
}
