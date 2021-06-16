using System;

namespace Microsoft.Xde.Common
{
	// Token: 0x02000025 RID: 37
	public interface IWindowsCompatCheck
	{
		// Token: 0x060000D4 RID: 212
		bool IsWindows10OrBetter();

		// Token: 0x060000D5 RID: 213
		bool IsBuildEqualOrGreater(int buildNumber);

		// Token: 0x060000D6 RID: 214
		bool IsBuildEqualOrGreater(MinBuildVersion minBuildVersion);

		// Token: 0x060000D7 RID: 215
		bool Is64BitWindows();

		// Token: 0x060000D8 RID: 216
		bool IsSlatPresent();

		// Token: 0x060000D9 RID: 217
		bool IsHyperVFeatureInstalled();

		// Token: 0x060000DA RID: 218
		bool IsHyperVFeatureOptionAvailable();

		// Token: 0x060000DB RID: 219
		bool IsHypervisorPresent();

		// Token: 0x060000DC RID: 220
		bool IsHyperVManagementServiceRunning();
	}
}
