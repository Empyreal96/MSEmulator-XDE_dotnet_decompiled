using System;
using System.Globalization;
using System.Management.Automation.Internal;
using System.Threading;

namespace System.Management.Automation.Runspaces
{
	// Token: 0x02000260 RID: 608
	public sealed class PSSession
	{
		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x06001C61 RID: 7265 RVA: 0x000A50B2 File Offset: 0x000A32B2
		public string ComputerName
		{
			get
			{
				return this.remoteRunspace.ConnectionInfo.ComputerName;
			}
		}

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x06001C62 RID: 7266 RVA: 0x000A50C4 File Offset: 0x000A32C4
		public string ConfigurationName
		{
			get
			{
				return this.shell;
			}
		}

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x06001C63 RID: 7267 RVA: 0x000A50CC File Offset: 0x000A32CC
		public Guid InstanceId
		{
			get
			{
				return this.remoteRunspace.InstanceId;
			}
		}

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x06001C64 RID: 7268 RVA: 0x000A50D9 File Offset: 0x000A32D9
		public int Id
		{
			get
			{
				return this.sessionid;
			}
		}

		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06001C65 RID: 7269 RVA: 0x000A50E1 File Offset: 0x000A32E1
		// (set) Token: 0x06001C66 RID: 7270 RVA: 0x000A50E9 File Offset: 0x000A32E9
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06001C67 RID: 7271 RVA: 0x000A50F2 File Offset: 0x000A32F2
		public RunspaceAvailability Availability
		{
			get
			{
				return this.Runspace.RunspaceAvailability;
			}
		}

		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x06001C68 RID: 7272 RVA: 0x000A50FF File Offset: 0x000A32FF
		public PSPrimitiveDictionary ApplicationPrivateData
		{
			get
			{
				return this.Runspace.GetApplicationPrivateData();
			}
		}

		// Token: 0x170006F7 RID: 1783
		// (get) Token: 0x06001C69 RID: 7273 RVA: 0x000A510C File Offset: 0x000A330C
		public Runspace Runspace
		{
			get
			{
				return this.remoteRunspace;
			}
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000A5114 File Offset: 0x000A3314
		public override string ToString()
		{
			string formatSpec = "[PSSession]{0}";
			return StringUtil.Format(formatSpec, this.Name);
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000A5133 File Offset: 0x000A3333
		internal bool InsertRunspace(RemoteRunspace remoteRunspace)
		{
			if (remoteRunspace == null || remoteRunspace.InstanceId != this.remoteRunspace.InstanceId)
			{
				return false;
			}
			this.remoteRunspace = remoteRunspace;
			return true;
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000A515C File Offset: 0x000A335C
		internal PSSession(RemoteRunspace remoteRunspace)
		{
			this.remoteRunspace = remoteRunspace;
			if (remoteRunspace.PSSessionId != -1)
			{
				this.sessionid = remoteRunspace.PSSessionId;
			}
			else
			{
				this.sessionid = Interlocked.Increment(ref PSSession.seed);
				remoteRunspace.PSSessionId = this.sessionid;
			}
			if (!string.IsNullOrEmpty(remoteRunspace.PSSessionName))
			{
				this.name = remoteRunspace.PSSessionName;
			}
			else
			{
				this.name = this.AutoGenerateRunspaceName();
				remoteRunspace.PSSessionName = this.name;
			}
			string text = WSManConnectionInfo.ExtractPropertyAsWsManConnectionInfo<string>(remoteRunspace.ConnectionInfo, "ShellUri", string.Empty);
			this.shell = this.GetDisplayShellName(text);
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000A51FF File Offset: 0x000A33FF
		private string AutoGenerateRunspaceName()
		{
			return "Session" + this.sessionid.ToString(NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000A521C File Offset: 0x000A341C
		private string GetDisplayShellName(string shell)
		{
			string text = "http://schemas.microsoft.com/powershell/";
			int num = shell.IndexOf(text, StringComparison.OrdinalIgnoreCase);
			if (num != 0)
			{
				return shell;
			}
			return shell.Substring(text.Length);
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x000A524C File Offset: 0x000A344C
		internal static string GenerateRunspaceName(out int rtnId)
		{
			int num = Interlocked.Increment(ref PSSession.seed);
			rtnId = num;
			return PSSession.ComposeRunspaceName(num);
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x000A526D File Offset: 0x000A346D
		internal static int GenerateRunspaceId()
		{
			return Interlocked.Increment(ref PSSession.seed);
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x000A5279 File Offset: 0x000A3479
		internal static string ComposeRunspaceName(int id)
		{
			return "Session" + id.ToString(NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x04000C5F RID: 3167
		private RemoteRunspace remoteRunspace;

		// Token: 0x04000C60 RID: 3168
		private string shell;

		// Token: 0x04000C61 RID: 3169
		private static int seed;

		// Token: 0x04000C62 RID: 3170
		private int sessionid;

		// Token: 0x04000C63 RID: 3171
		private string name;
	}
}
