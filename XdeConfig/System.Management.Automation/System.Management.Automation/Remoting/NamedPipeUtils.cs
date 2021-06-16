using System;
using System.Diagnostics;
using System.Globalization;

namespace System.Management.Automation.Remoting
{
	// Token: 0x020002E9 RID: 745
	internal static class NamedPipeUtils
	{
		// Token: 0x06002384 RID: 9092 RVA: 0x000C787A File Offset: 0x000C5A7A
		internal static string CreateProcessPipeName(int procId)
		{
			return NamedPipeUtils.CreateProcessPipeName(Process.GetProcessById(procId));
		}

		// Token: 0x06002385 RID: 9093 RVA: 0x000C7887 File Offset: 0x000C5A87
		internal static string CreateProcessPipeName(Process proc)
		{
			return NamedPipeUtils.CreateProcessPipeName(proc, "DefaultAppDomain");
		}

		// Token: 0x06002386 RID: 9094 RVA: 0x000C7894 File Offset: 0x000C5A94
		internal static string CreateProcessPipeName(int procId, string appDomainName)
		{
			return NamedPipeUtils.CreateProcessPipeName(Process.GetProcessById(procId), appDomainName);
		}

		// Token: 0x06002387 RID: 9095 RVA: 0x000C78A4 File Offset: 0x000C5AA4
		internal static string CreateProcessPipeName(Process proc, string appDomainName)
		{
			if (proc == null)
			{
				throw new PSArgumentNullException("proc");
			}
			if (string.IsNullOrEmpty(appDomainName))
			{
				appDomainName = "DefaultAppDomain";
			}
			return string.Concat(new string[]
			{
				"PSHost.",
				proc.StartTime.ToFileTime().ToString(CultureInfo.InvariantCulture),
				".",
				proc.Id.ToString(CultureInfo.InvariantCulture),
				".",
				NamedPipeUtils.CleanAppDomainNameForPipeName(appDomainName),
				".",
				proc.ProcessName
			});
		}

		// Token: 0x06002388 RID: 9096 RVA: 0x000C7941 File Offset: 0x000C5B41
		private static string CleanAppDomainNameForPipeName(string appDomainName)
		{
			return appDomainName.Replace(":", "").Replace(" ", "");
		}

		// Token: 0x06002389 RID: 9097 RVA: 0x000C7962 File Offset: 0x000C5B62
		internal static string GetCurrentAppDomainName()
		{
			if (!AppDomain.CurrentDomain.IsDefaultAppDomain())
			{
				return AppDomain.CurrentDomain.FriendlyName;
			}
			return "DefaultAppDomain";
		}

		// Token: 0x04001149 RID: 4425
		internal const string DefaultAppDomainName = "DefaultAppDomain";

		// Token: 0x0400114A RID: 4426
		internal const string NamedPipeNamePrefix = "PSHost.";

		// Token: 0x0400114B RID: 4427
		internal const string NamedPipeNamePrefixSearch = "PSHost*";
	}
}
