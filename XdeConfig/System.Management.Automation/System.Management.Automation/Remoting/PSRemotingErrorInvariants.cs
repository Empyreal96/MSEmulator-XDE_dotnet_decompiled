using System;
using System.Management.Automation.Internal;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002C0 RID: 704
	internal static class PSRemotingErrorInvariants
	{
		// Token: 0x0600219D RID: 8605 RVA: 0x000C0E9C File Offset: 0x000BF09C
		internal static string FormatResourceString(string resourceString, params object[] args)
		{
			return StringUtil.Format(resourceString, args);
		}
	}
}
