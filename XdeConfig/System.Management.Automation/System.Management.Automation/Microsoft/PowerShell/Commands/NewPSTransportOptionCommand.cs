using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200034F RID: 847
	[OutputType(new Type[]
	{
		typeof(WSManConfigurationOption)
	})]
	[Cmdlet("New", "PSTransportOption", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=210608", RemotingCapability = RemotingCapability.None)]
	public sealed class NewPSTransportOptionCommand : PSCmdlet
	{
		// Token: 0x17000A0B RID: 2571
		// (get) Token: 0x060029DB RID: 10715 RVA: 0x000E84F1 File Offset: 0x000E66F1
		// (set) Token: 0x060029DC RID: 10716 RVA: 0x000E84FE File Offset: 0x000E66FE
		[ValidateRange(60, 2147483)]
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public int? MaxIdleTimeoutSec
		{
			get
			{
				return this.option.MaxIdleTimeoutSec;
			}
			set
			{
				this.option.MaxIdleTimeoutSec = value;
			}
		}

		// Token: 0x17000A0C RID: 2572
		// (get) Token: 0x060029DD RID: 10717 RVA: 0x000E850C File Offset: 0x000E670C
		// (set) Token: 0x060029DE RID: 10718 RVA: 0x000E8519 File Offset: 0x000E6719
		[ValidateRange(0, 1209600)]
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public int? ProcessIdleTimeoutSec
		{
			get
			{
				return this.option.ProcessIdleTimeoutSec;
			}
			set
			{
				this.option.ProcessIdleTimeoutSec = value;
			}
		}

		// Token: 0x17000A0D RID: 2573
		// (get) Token: 0x060029DF RID: 10719 RVA: 0x000E8527 File Offset: 0x000E6727
		// (set) Token: 0x060029E0 RID: 10720 RVA: 0x000E8534 File Offset: 0x000E6734
		[ValidateRange(1, 2147483647)]
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public int? MaxSessions
		{
			get
			{
				return this.option.MaxSessions;
			}
			set
			{
				this.option.MaxSessions = value;
			}
		}

		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060029E1 RID: 10721 RVA: 0x000E8542 File Offset: 0x000E6742
		// (set) Token: 0x060029E2 RID: 10722 RVA: 0x000E854F File Offset: 0x000E674F
		[Parameter(ValueFromPipelineByPropertyName = true)]
		[ValidateRange(1, 2147483647)]
		public int? MaxConcurrentCommandsPerSession
		{
			get
			{
				return this.option.MaxConcurrentCommandsPerSession;
			}
			set
			{
				this.option.MaxConcurrentCommandsPerSession = value;
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x060029E3 RID: 10723 RVA: 0x000E855D File Offset: 0x000E675D
		// (set) Token: 0x060029E4 RID: 10724 RVA: 0x000E856A File Offset: 0x000E676A
		[ValidateRange(1, 2147483647)]
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public int? MaxSessionsPerUser
		{
			get
			{
				return this.option.MaxSessionsPerUser;
			}
			set
			{
				this.option.MaxSessionsPerUser = value;
			}
		}

		// Token: 0x17000A10 RID: 2576
		// (get) Token: 0x060029E5 RID: 10725 RVA: 0x000E8578 File Offset: 0x000E6778
		// (set) Token: 0x060029E6 RID: 10726 RVA: 0x000E8585 File Offset: 0x000E6785
		[Parameter(ValueFromPipelineByPropertyName = true)]
		[ValidateRange(5, 2147483647)]
		public int? MaxMemoryPerSessionMB
		{
			get
			{
				return this.option.MaxMemoryPerSessionMB;
			}
			set
			{
				this.option.MaxMemoryPerSessionMB = value;
			}
		}

		// Token: 0x17000A11 RID: 2577
		// (get) Token: 0x060029E7 RID: 10727 RVA: 0x000E8593 File Offset: 0x000E6793
		// (set) Token: 0x060029E8 RID: 10728 RVA: 0x000E85A0 File Offset: 0x000E67A0
		[ValidateRange(1, 2147483647)]
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public int? MaxProcessesPerSession
		{
			get
			{
				return this.option.MaxProcessesPerSession;
			}
			set
			{
				this.option.MaxProcessesPerSession = value;
			}
		}

		// Token: 0x17000A12 RID: 2578
		// (get) Token: 0x060029E9 RID: 10729 RVA: 0x000E85AE File Offset: 0x000E67AE
		// (set) Token: 0x060029EA RID: 10730 RVA: 0x000E85BB File Offset: 0x000E67BB
		[ValidateRange(1, 100)]
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public int? MaxConcurrentUsers
		{
			get
			{
				return this.option.MaxConcurrentUsers;
			}
			set
			{
				this.option.MaxConcurrentUsers = value;
			}
		}

		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x060029EB RID: 10731 RVA: 0x000E85C9 File Offset: 0x000E67C9
		// (set) Token: 0x060029EC RID: 10732 RVA: 0x000E85D6 File Offset: 0x000E67D6
		[ValidateRange(60, 2147483)]
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public int? IdleTimeoutSec
		{
			get
			{
				return this.option.IdleTimeoutSec;
			}
			set
			{
				this.option.IdleTimeoutSec = value;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x060029ED RID: 10733 RVA: 0x000E85E4 File Offset: 0x000E67E4
		// (set) Token: 0x060029EE RID: 10734 RVA: 0x000E85F1 File Offset: 0x000E67F1
		[Parameter(ValueFromPipelineByPropertyName = true)]
		public OutputBufferingMode? OutputBufferingMode
		{
			get
			{
				return this.option.OutputBufferingMode;
			}
			set
			{
				this.option.OutputBufferingMode = value;
			}
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x000E85FF File Offset: 0x000E67FF
		protected override void ProcessRecord()
		{
			base.WriteObject(this.option);
		}

		// Token: 0x040014BE RID: 5310
		private WSManConfigurationOption option = new WSManConfigurationOption();
	}
}
