using System;

namespace System.Management.Automation
{
	// Token: 0x0200081F RID: 2079
	internal class SingleShellProviderNames : ProviderNames
	{
		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06004FD5 RID: 20437 RVA: 0x001A77DA File Offset: 0x001A59DA
		internal override string Environment
		{
			get
			{
				return "Microsoft.PowerShell.Core\\Environment";
			}
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06004FD6 RID: 20438 RVA: 0x001A77E1 File Offset: 0x001A59E1
		internal override string Certificate
		{
			get
			{
				return "Microsoft.PowerShell.Security\\Certificate";
			}
		}

		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x06004FD7 RID: 20439 RVA: 0x001A77E8 File Offset: 0x001A59E8
		internal override string Variable
		{
			get
			{
				return "Microsoft.PowerShell.Core\\Variable";
			}
		}

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x06004FD8 RID: 20440 RVA: 0x001A77EF File Offset: 0x001A59EF
		internal override string Alias
		{
			get
			{
				return "Microsoft.PowerShell.Core\\Alias";
			}
		}

		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x06004FD9 RID: 20441 RVA: 0x001A77F6 File Offset: 0x001A59F6
		internal override string Function
		{
			get
			{
				return "Microsoft.PowerShell.Core\\Function";
			}
		}

		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x06004FDA RID: 20442 RVA: 0x001A77FD File Offset: 0x001A59FD
		internal override string FileSystem
		{
			get
			{
				return "Microsoft.PowerShell.Core\\FileSystem";
			}
		}

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x06004FDB RID: 20443 RVA: 0x001A7804 File Offset: 0x001A5A04
		internal override string Registry
		{
			get
			{
				return "Microsoft.PowerShell.Core\\Registry";
			}
		}
	}
}
