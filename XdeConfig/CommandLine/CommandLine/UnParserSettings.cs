using System;
using CommandLine.Infrastructure;

namespace CommandLine
{
	// Token: 0x0200004E RID: 78
	public class UnParserSettings
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001BB RID: 443 RVA: 0x00007CA1 File Offset: 0x00005EA1
		// (set) Token: 0x060001BC RID: 444 RVA: 0x00007CA9 File Offset: 0x00005EA9
		public bool PreferShortName
		{
			get
			{
				return this.preferShortName;
			}
			set
			{
				PopsicleSetter.Set<bool>(this.Consumed, ref this.preferShortName, value);
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00007CBD File Offset: 0x00005EBD
		// (set) Token: 0x060001BE RID: 446 RVA: 0x00007CC5 File Offset: 0x00005EC5
		public bool GroupSwitches
		{
			get
			{
				return this.groupSwitches;
			}
			set
			{
				PopsicleSetter.Set<bool>(this.Consumed, ref this.groupSwitches, value);
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00007CD9 File Offset: 0x00005ED9
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x00007CE1 File Offset: 0x00005EE1
		public bool UseEqualToken
		{
			get
			{
				return this.useEqualToken;
			}
			set
			{
				PopsicleSetter.Set<bool>(this.Consumed, ref this.useEqualToken, value);
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00007CF5 File Offset: 0x00005EF5
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x00007CFD File Offset: 0x00005EFD
		public bool ShowHidden
		{
			get
			{
				return this.showHidden;
			}
			set
			{
				PopsicleSetter.Set<bool>(this.Consumed, ref this.showHidden, value);
			}
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00007D11 File Offset: 0x00005F11
		public static UnParserSettings WithGroupSwitchesOnly()
		{
			return new UnParserSettings
			{
				GroupSwitches = true
			};
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00007D1F File Offset: 0x00005F1F
		public static UnParserSettings WithUseEqualTokenOnly()
		{
			return new UnParserSettings
			{
				UseEqualToken = true
			};
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00007D2D File Offset: 0x00005F2D
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00007D35 File Offset: 0x00005F35
		internal bool Consumed { get; set; }

		// Token: 0x04000085 RID: 133
		private bool preferShortName;

		// Token: 0x04000086 RID: 134
		private bool groupSwitches;

		// Token: 0x04000087 RID: 135
		private bool useEqualToken;

		// Token: 0x04000088 RID: 136
		private bool showHidden;
	}
}
