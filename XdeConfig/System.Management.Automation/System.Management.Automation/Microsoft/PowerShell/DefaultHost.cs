using System;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Host;

namespace Microsoft.PowerShell
{
	// Token: 0x020001FC RID: 508
	internal class DefaultHost : PSHost
	{
		// Token: 0x06001788 RID: 6024 RVA: 0x00092343 File Offset: 0x00090543
		internal DefaultHost(CultureInfo currentCulture, CultureInfo currentUICulture)
		{
			this.currentCulture = currentCulture;
			this.currentUICulture = currentUICulture;
		}

		// Token: 0x170005BC RID: 1468
		// (get) Token: 0x06001789 RID: 6025 RVA: 0x0009236F File Offset: 0x0009056F
		public override string Name
		{
			get
			{
				return "Default Host";
			}
		}

		// Token: 0x170005BD RID: 1469
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x00092376 File Offset: 0x00090576
		public override Version Version
		{
			get
			{
				return this.ver;
			}
		}

		// Token: 0x170005BE RID: 1470
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x0009237E File Offset: 0x0009057E
		public override Guid InstanceId
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x0600178C RID: 6028 RVA: 0x00092386 File Offset: 0x00090586
		public override PSHostUserInterface UI
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x0600178D RID: 6029 RVA: 0x00092389 File Offset: 0x00090589
		public override CultureInfo CurrentCulture
		{
			get
			{
				return this.currentCulture;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x0600178E RID: 6030 RVA: 0x00092391 File Offset: 0x00090591
		public override CultureInfo CurrentUICulture
		{
			get
			{
				return this.currentUICulture;
			}
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x00092399 File Offset: 0x00090599
		public override void SetShouldExit(int exitCode)
		{
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x0009239B File Offset: 0x0009059B
		public override void EnterNestedPrompt()
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x000923A2 File Offset: 0x000905A2
		public override void ExitNestedPrompt()
		{
			throw PSTraceSource.NewNotSupportedException();
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x000923A9 File Offset: 0x000905A9
		public override void NotifyBeginApplication()
		{
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x000923AB File Offset: 0x000905AB
		public override void NotifyEndApplication()
		{
		}

		// Token: 0x040009EF RID: 2543
		private CultureInfo currentCulture;

		// Token: 0x040009F0 RID: 2544
		private CultureInfo currentUICulture;

		// Token: 0x040009F1 RID: 2545
		private Guid id = Guid.NewGuid();

		// Token: 0x040009F2 RID: 2546
		private Version ver = PSVersionInfo.PSVersion;
	}
}
