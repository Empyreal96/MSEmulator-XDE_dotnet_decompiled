using System;
using System.Collections;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200034E RID: 846
	public class WSManConfigurationOption : PSTransportOption
	{
		// Token: 0x060029C0 RID: 10688 RVA: 0x000E7C90 File Offset: 0x000E5E90
		internal WSManConfigurationOption()
		{
		}

		// Token: 0x060029C1 RID: 10689 RVA: 0x000E7D1C File Offset: 0x000E5F1C
		protected internal override void LoadFromDefaults(PSSessionType sessionType, bool keepAssigned)
		{
			if (!keepAssigned || this.outputBufferingMode == null)
			{
				this.outputBufferingMode = WSManConfigurationOption.DefaultOutputBufferingMode;
			}
			if (!keepAssigned || this._processIdleTimeoutSec == null)
			{
				this._processIdleTimeoutSec = ((sessionType == PSSessionType.Workflow) ? WSManConfigurationOption.DefaultProcessIdleTimeout_ForWorkflow : WSManConfigurationOption.DefaultProcessIdleTimeout_ForPSRemoting);
			}
			if (!keepAssigned || this._maxIdleTimeoutSec == null)
			{
				this._maxIdleTimeoutSec = WSManConfigurationOption.DefaultMaxIdleTimeout;
			}
			if (!keepAssigned || this._idleTimeoutSec == null)
			{
				this._idleTimeoutSec = WSManConfigurationOption.DefaultIdleTimeout;
			}
			if (!keepAssigned || this.maxConcurrentUsers == null)
			{
				this.maxConcurrentUsers = WSManConfigurationOption.DefaultMaxConcurrentUsers;
			}
			if (!keepAssigned || this.maxProcessesPerSession == null)
			{
				this.maxProcessesPerSession = WSManConfigurationOption.DefaultMaxProcessesPerSession;
			}
			if (!keepAssigned || this.maxMemoryPerSessionMB == null)
			{
				this.maxMemoryPerSessionMB = WSManConfigurationOption.DefaultMaxMemoryPerSessionMB;
			}
			if (!keepAssigned || this.maxSessions == null)
			{
				this.maxSessions = WSManConfigurationOption.DefaultMaxSessions;
			}
			if (!keepAssigned || this.maxSessionsPerUser == null)
			{
				this.maxSessionsPerUser = WSManConfigurationOption.DefaultMaxSessionsPerUser;
			}
			if (!keepAssigned || this.maxConcurrentCommandsPerSession == null)
			{
				this.maxConcurrentCommandsPerSession = WSManConfigurationOption.DefaultMaxConcurrentCommandsPerSession;
			}
		}

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x060029C2 RID: 10690 RVA: 0x000E7E42 File Offset: 0x000E6042
		// (set) Token: 0x060029C3 RID: 10691 RVA: 0x000E7E4A File Offset: 0x000E604A
		public int? ProcessIdleTimeoutSec
		{
			get
			{
				return this._processIdleTimeoutSec;
			}
			internal set
			{
				this._processIdleTimeoutSec = value;
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x060029C4 RID: 10692 RVA: 0x000E7E53 File Offset: 0x000E6053
		// (set) Token: 0x060029C5 RID: 10693 RVA: 0x000E7E5B File Offset: 0x000E605B
		public int? MaxIdleTimeoutSec
		{
			get
			{
				return this._maxIdleTimeoutSec;
			}
			internal set
			{
				this._maxIdleTimeoutSec = value;
			}
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x060029C6 RID: 10694 RVA: 0x000E7E64 File Offset: 0x000E6064
		// (set) Token: 0x060029C7 RID: 10695 RVA: 0x000E7E6C File Offset: 0x000E606C
		public int? MaxSessions
		{
			get
			{
				return this.maxSessions;
			}
			internal set
			{
				this.maxSessions = value;
			}
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x060029C8 RID: 10696 RVA: 0x000E7E75 File Offset: 0x000E6075
		// (set) Token: 0x060029C9 RID: 10697 RVA: 0x000E7E7D File Offset: 0x000E607D
		public int? MaxConcurrentCommandsPerSession
		{
			get
			{
				return this.maxConcurrentCommandsPerSession;
			}
			internal set
			{
				this.maxConcurrentCommandsPerSession = value;
			}
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060029CA RID: 10698 RVA: 0x000E7E86 File Offset: 0x000E6086
		// (set) Token: 0x060029CB RID: 10699 RVA: 0x000E7E8E File Offset: 0x000E608E
		public int? MaxSessionsPerUser
		{
			get
			{
				return this.maxSessionsPerUser;
			}
			internal set
			{
				this.maxSessionsPerUser = value;
			}
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060029CC RID: 10700 RVA: 0x000E7E97 File Offset: 0x000E6097
		// (set) Token: 0x060029CD RID: 10701 RVA: 0x000E7E9F File Offset: 0x000E609F
		public int? MaxMemoryPerSessionMB
		{
			get
			{
				return this.maxMemoryPerSessionMB;
			}
			internal set
			{
				this.maxMemoryPerSessionMB = value;
			}
		}

		// Token: 0x17000A07 RID: 2567
		// (get) Token: 0x060029CE RID: 10702 RVA: 0x000E7EA8 File Offset: 0x000E60A8
		// (set) Token: 0x060029CF RID: 10703 RVA: 0x000E7EB0 File Offset: 0x000E60B0
		public int? MaxProcessesPerSession
		{
			get
			{
				return this.maxProcessesPerSession;
			}
			internal set
			{
				this.maxProcessesPerSession = value;
			}
		}

		// Token: 0x17000A08 RID: 2568
		// (get) Token: 0x060029D0 RID: 10704 RVA: 0x000E7EB9 File Offset: 0x000E60B9
		// (set) Token: 0x060029D1 RID: 10705 RVA: 0x000E7EC1 File Offset: 0x000E60C1
		public int? MaxConcurrentUsers
		{
			get
			{
				return this.maxConcurrentUsers;
			}
			internal set
			{
				this.maxConcurrentUsers = value;
			}
		}

		// Token: 0x17000A09 RID: 2569
		// (get) Token: 0x060029D2 RID: 10706 RVA: 0x000E7ECA File Offset: 0x000E60CA
		// (set) Token: 0x060029D3 RID: 10707 RVA: 0x000E7ED2 File Offset: 0x000E60D2
		public int? IdleTimeoutSec
		{
			get
			{
				return this._idleTimeoutSec;
			}
			internal set
			{
				this._idleTimeoutSec = value;
			}
		}

		// Token: 0x17000A0A RID: 2570
		// (get) Token: 0x060029D4 RID: 10708 RVA: 0x000E7EDB File Offset: 0x000E60DB
		// (set) Token: 0x060029D5 RID: 10709 RVA: 0x000E7EE3 File Offset: 0x000E60E3
		public OutputBufferingMode? OutputBufferingMode
		{
			get
			{
				return this.outputBufferingMode;
			}
			internal set
			{
				this.outputBufferingMode = value;
			}
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x000E7EEC File Offset: 0x000E60EC
		internal override Hashtable ConstructQuotasAsHashtable()
		{
			Hashtable hashtable = new Hashtable();
			if (this._idleTimeoutSec != null)
			{
				hashtable["IdleTimeoutms"] = (1000 * this._idleTimeoutSec.Value).ToString(CultureInfo.InvariantCulture);
			}
			if (this.maxConcurrentUsers != null)
			{
				hashtable["MaxConcurrentUsers"] = this.maxConcurrentUsers.Value.ToString(CultureInfo.InvariantCulture);
			}
			if (this.maxProcessesPerSession != null)
			{
				hashtable["MaxProcessesPerShell"] = this.maxProcessesPerSession.Value.ToString(CultureInfo.InvariantCulture);
			}
			if (this.maxMemoryPerSessionMB != null)
			{
				hashtable["MaxMemoryPerShellMB"] = this.maxMemoryPerSessionMB.Value.ToString(CultureInfo.InvariantCulture);
			}
			if (this.maxSessionsPerUser != null)
			{
				hashtable["MaxShellsPerUser"] = this.maxSessionsPerUser.Value.ToString(CultureInfo.InvariantCulture);
			}
			if (this.maxConcurrentCommandsPerSession != null)
			{
				hashtable["MaxConcurrentCommandsPerShell"] = this.maxConcurrentCommandsPerSession.Value.ToString(CultureInfo.InvariantCulture);
			}
			if (this.maxSessions != null)
			{
				hashtable["MaxShells"] = this.maxSessions.Value.ToString(CultureInfo.InvariantCulture);
			}
			if (this._maxIdleTimeoutSec != null)
			{
				hashtable["MaxIdleTimeoutms"] = (1000 * this._maxIdleTimeoutSec.Value).ToString(CultureInfo.InvariantCulture);
			}
			return hashtable;
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x000E8094 File Offset: 0x000E6294
		internal override string ConstructQuotas()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this._idleTimeoutSec != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, " {0}='{1}'", new object[]
				{
					"IdleTimeoutms",
					1000 * this._idleTimeoutSec
				}));
			}
			if (this.maxConcurrentUsers != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, " {0}='{1}'", new object[]
				{
					"MaxConcurrentUsers",
					this.maxConcurrentUsers
				}));
			}
			if (this.maxProcessesPerSession != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, " {0}='{1}'", new object[]
				{
					"MaxProcessesPerShell",
					this.maxProcessesPerSession
				}));
			}
			if (this.maxMemoryPerSessionMB != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, " {0}='{1}'", new object[]
				{
					"MaxMemoryPerShellMB",
					this.maxMemoryPerSessionMB
				}));
			}
			if (this.maxSessionsPerUser != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, " {0}='{1}'", new object[]
				{
					"MaxShellsPerUser",
					this.maxSessionsPerUser
				}));
			}
			if (this.maxConcurrentCommandsPerSession != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, " {0}='{1}'", new object[]
				{
					"MaxConcurrentCommandsPerShell",
					this.maxConcurrentCommandsPerSession
				}));
			}
			if (this.maxSessions != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, " {0}='{1}'", new object[]
				{
					"MaxShells",
					this.maxSessions
				}));
			}
			if (this._maxIdleTimeoutSec != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, " {0}='{1}'", new object[]
				{
					"MaxIdleTimeoutms",
					1000 * this._maxIdleTimeoutSec
				}));
			}
			if (stringBuilder.Length <= 0)
			{
				return string.Empty;
			}
			return string.Format(CultureInfo.InvariantCulture, "<Quotas {0} />", new object[]
			{
				stringBuilder.ToString()
			});
		}

		// Token: 0x060029D8 RID: 10712 RVA: 0x000E8350 File Offset: 0x000E6550
		internal override string ConstructOptionsAsXmlAttributes()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.outputBufferingMode != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, " {0}='{1}'", new object[]
				{
					"OutputBufferingMode",
					this.outputBufferingMode.ToString()
				}));
			}
			if (this._processIdleTimeoutSec != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, " {0}='{1}'", new object[]
				{
					"ProcessIdleTimeoutSec",
					this._processIdleTimeoutSec
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060029D9 RID: 10713 RVA: 0x000E83F4 File Offset: 0x000E65F4
		internal override Hashtable ConstructOptionsAsHashtable()
		{
			Hashtable hashtable = new Hashtable();
			if (this.outputBufferingMode != null)
			{
				hashtable["OutputBufferingMode"] = this.outputBufferingMode.ToString();
			}
			if (this._processIdleTimeoutSec != null)
			{
				hashtable["ProcessIdleTimeoutSec"] = this._processIdleTimeoutSec;
			}
			return hashtable;
		}

		// Token: 0x0400149D RID: 5277
		private const string Token = " {0}='{1}'";

		// Token: 0x0400149E RID: 5278
		private const string QuotasToken = "<Quotas {0} />";

		// Token: 0x0400149F RID: 5279
		internal const string AttribOutputBufferingMode = "OutputBufferingMode";

		// Token: 0x040014A0 RID: 5280
		private const string AttribProcessIdleTimeout = "ProcessIdleTimeoutSec";

		// Token: 0x040014A1 RID: 5281
		internal const string AttribMaxIdleTimeout = "MaxIdleTimeoutms";

		// Token: 0x040014A2 RID: 5282
		internal const string AttribIdleTimeout = "IdleTimeoutms";

		// Token: 0x040014A3 RID: 5283
		private const string AttribMaxConcurrentUsers = "MaxConcurrentUsers";

		// Token: 0x040014A4 RID: 5284
		private const string AttribMaxProcessesPerSession = "MaxProcessesPerShell";

		// Token: 0x040014A5 RID: 5285
		private const string AttribMaxMemoryPerSessionMB = "MaxMemoryPerShellMB";

		// Token: 0x040014A6 RID: 5286
		private const string AttribMaxSessions = "MaxShells";

		// Token: 0x040014A7 RID: 5287
		private const string AttribMaxSessionsPerUser = "MaxShellsPerUser";

		// Token: 0x040014A8 RID: 5288
		private const string AttribMaxConcurrentCommandsPerSession = "MaxConcurrentCommandsPerShell";

		// Token: 0x040014A9 RID: 5289
		internal static OutputBufferingMode? DefaultOutputBufferingMode = new OutputBufferingMode?(System.Management.Automation.Runspaces.OutputBufferingMode.Block);

		// Token: 0x040014AA RID: 5290
		private OutputBufferingMode? outputBufferingMode = null;

		// Token: 0x040014AB RID: 5291
		internal static readonly int? DefaultProcessIdleTimeout_ForPSRemoting = new int?(0);

		// Token: 0x040014AC RID: 5292
		internal static readonly int? DefaultProcessIdleTimeout_ForWorkflow = new int?(1209600);

		// Token: 0x040014AD RID: 5293
		private int? _processIdleTimeoutSec = null;

		// Token: 0x040014AE RID: 5294
		internal static readonly int? DefaultMaxIdleTimeout = new int?(43200);

		// Token: 0x040014AF RID: 5295
		private int? _maxIdleTimeoutSec = null;

		// Token: 0x040014B0 RID: 5296
		internal static readonly int? DefaultIdleTimeout = new int?(7200);

		// Token: 0x040014B1 RID: 5297
		private int? _idleTimeoutSec = null;

		// Token: 0x040014B2 RID: 5298
		internal static readonly int? DefaultMaxConcurrentUsers = new int?(5);

		// Token: 0x040014B3 RID: 5299
		private int? maxConcurrentUsers = null;

		// Token: 0x040014B4 RID: 5300
		internal static readonly int? DefaultMaxProcessesPerSession = new int?(15);

		// Token: 0x040014B5 RID: 5301
		private int? maxProcessesPerSession = null;

		// Token: 0x040014B6 RID: 5302
		internal static readonly int? DefaultMaxMemoryPerSessionMB = new int?(1024);

		// Token: 0x040014B7 RID: 5303
		private int? maxMemoryPerSessionMB = null;

		// Token: 0x040014B8 RID: 5304
		internal static readonly int? DefaultMaxSessions = new int?(25);

		// Token: 0x040014B9 RID: 5305
		private int? maxSessions = null;

		// Token: 0x040014BA RID: 5306
		internal static readonly int? DefaultMaxSessionsPerUser = new int?(25);

		// Token: 0x040014BB RID: 5307
		private int? maxSessionsPerUser = null;

		// Token: 0x040014BC RID: 5308
		internal static readonly int? DefaultMaxConcurrentCommandsPerSession = new int?(1000);

		// Token: 0x040014BD RID: 5309
		private int? maxConcurrentCommandsPerSession = null;
	}
}
