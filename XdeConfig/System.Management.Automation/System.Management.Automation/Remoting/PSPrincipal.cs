using System;
using System.Security.Principal;

namespace System.Management.Automation.Remoting
{
	// Token: 0x02000359 RID: 857
	public sealed class PSPrincipal : IPrincipal
	{
		// Token: 0x17000A57 RID: 2647
		// (get) Token: 0x06002A9A RID: 10906 RVA: 0x000EB35C File Offset: 0x000E955C
		public PSIdentity Identity
		{
			get
			{
				return this.psIdentity;
			}
		}

		// Token: 0x17000A58 RID: 2648
		// (get) Token: 0x06002A9B RID: 10907 RVA: 0x000EB364 File Offset: 0x000E9564
		public WindowsIdentity WindowsIdentity
		{
			get
			{
				return this.windowsIdentity;
			}
		}

		// Token: 0x17000A59 RID: 2649
		// (get) Token: 0x06002A9C RID: 10908 RVA: 0x000EB36C File Offset: 0x000E956C
		IIdentity IPrincipal.Identity
		{
			get
			{
				return this.Identity;
			}
		}

		// Token: 0x06002A9D RID: 10909 RVA: 0x000EB374 File Offset: 0x000E9574
		public bool IsInRole(string role)
		{
			if (this.windowsIdentity != null)
			{
				WindowsPrincipal windowsPrincipal = new WindowsPrincipal(this.windowsIdentity);
				return windowsPrincipal.IsInRole(role);
			}
			return false;
		}

		// Token: 0x06002A9E RID: 10910 RVA: 0x000EB3A0 File Offset: 0x000E95A0
		internal bool IsInRole(WindowsBuiltInRole role)
		{
			if (this.windowsIdentity != null)
			{
				WindowsPrincipal windowsPrincipal = new WindowsPrincipal(this.windowsIdentity);
				return windowsPrincipal.IsInRole(role);
			}
			return false;
		}

		// Token: 0x06002A9F RID: 10911 RVA: 0x000EB3CA File Offset: 0x000E95CA
		public PSPrincipal(PSIdentity identity, WindowsIdentity windowsIdentity)
		{
			this.psIdentity = identity;
			this.windowsIdentity = windowsIdentity;
		}

		// Token: 0x0400150E RID: 5390
		private PSIdentity psIdentity;

		// Token: 0x0400150F RID: 5391
		private WindowsIdentity windowsIdentity;
	}
}
