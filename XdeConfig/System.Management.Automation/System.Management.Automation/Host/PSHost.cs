using System;
using System.Globalization;

namespace System.Management.Automation.Host
{
	// Token: 0x020001FB RID: 507
	public abstract class PSHost
	{
		// Token: 0x170005B3 RID: 1459
		// (get) Token: 0x06001778 RID: 6008
		public abstract string Name { get; }

		// Token: 0x170005B4 RID: 1460
		// (get) Token: 0x06001779 RID: 6009
		public abstract Version Version { get; }

		// Token: 0x170005B5 RID: 1461
		// (get) Token: 0x0600177A RID: 6010
		public abstract Guid InstanceId { get; }

		// Token: 0x170005B6 RID: 1462
		// (get) Token: 0x0600177B RID: 6011
		public abstract PSHostUserInterface UI { get; }

		// Token: 0x170005B7 RID: 1463
		// (get) Token: 0x0600177C RID: 6012
		public abstract CultureInfo CurrentCulture { get; }

		// Token: 0x170005B8 RID: 1464
		// (get) Token: 0x0600177D RID: 6013
		public abstract CultureInfo CurrentUICulture { get; }

		// Token: 0x0600177E RID: 6014
		public abstract void SetShouldExit(int exitCode);

		// Token: 0x0600177F RID: 6015
		public abstract void EnterNestedPrompt();

		// Token: 0x06001780 RID: 6016
		public abstract void ExitNestedPrompt();

		// Token: 0x170005B9 RID: 1465
		// (get) Token: 0x06001781 RID: 6017 RVA: 0x00092325 File Offset: 0x00090525
		public virtual PSObject PrivateData
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06001782 RID: 6018
		public abstract void NotifyBeginApplication();

		// Token: 0x06001783 RID: 6019
		public abstract void NotifyEndApplication();

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x06001784 RID: 6020 RVA: 0x00092328 File Offset: 0x00090528
		// (set) Token: 0x06001785 RID: 6021 RVA: 0x00092330 File Offset: 0x00090530
		internal bool ShouldSetThreadUILanguageToZero
		{
			get
			{
				return this.shouldSetThreadUILanguageToZero;
			}
			set
			{
				this.shouldSetThreadUILanguageToZero = value;
			}
		}

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x06001786 RID: 6022 RVA: 0x00092339 File Offset: 0x00090539
		// (set) Token: 0x06001787 RID: 6023 RVA: 0x0009233C File Offset: 0x0009053C
		public virtual bool DebuggerEnabled
		{
			get
			{
				return false;
			}
			set
			{
				throw new PSNotImplementedException();
			}
		}

		// Token: 0x040009EC RID: 2540
		internal const int MaximumNestedPromptLevel = 128;

		// Token: 0x040009ED RID: 2541
		internal static bool IsStdOutputRedirected;

		// Token: 0x040009EE RID: 2542
		private bool shouldSetThreadUILanguageToZero;
	}
}
