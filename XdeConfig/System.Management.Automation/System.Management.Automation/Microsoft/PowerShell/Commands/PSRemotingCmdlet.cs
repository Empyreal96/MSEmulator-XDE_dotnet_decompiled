using System;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000323 RID: 803
	public abstract class PSRemotingCmdlet : PSCmdlet
	{
		// Token: 0x0600264A RID: 9802 RVA: 0x000D6A9E File Offset: 0x000D4C9E
		protected override void BeginProcessing()
		{
			if (!this._skipWinRMCheck)
			{
				RemotingCommandUtil.CheckRemotingCmdletPrerequisites();
			}
		}

		// Token: 0x0600264B RID: 9803 RVA: 0x000D6AAD File Offset: 0x000D4CAD
		internal void WriteStreamObject(Action<Cmdlet> action)
		{
			action(this);
		}

		// Token: 0x0600264C RID: 9804 RVA: 0x000D6AB8 File Offset: 0x000D4CB8
		protected void ResolveComputerNames(string[] computerNames, out string[] resolvedComputerNames)
		{
			if (computerNames == null)
			{
				resolvedComputerNames = new string[1];
				resolvedComputerNames[0] = this.ResolveComputerName(".");
				return;
			}
			if (computerNames.Length == 0)
			{
				resolvedComputerNames = new string[0];
				return;
			}
			resolvedComputerNames = new string[computerNames.Length];
			for (int i = 0; i < resolvedComputerNames.Length; i++)
			{
				resolvedComputerNames[i] = this.ResolveComputerName(computerNames[i]);
			}
		}

		// Token: 0x0600264D RID: 9805 RVA: 0x000D6B14 File Offset: 0x000D4D14
		protected string ResolveComputerName(string computerName)
		{
			if (string.Equals(computerName, ".", StringComparison.OrdinalIgnoreCase))
			{
				return PSRemotingCmdlet.LOCALHOST;
			}
			return computerName;
		}

		// Token: 0x0600264E RID: 9806 RVA: 0x000D6B2C File Offset: 0x000D4D2C
		internal string GetMessage(string resourceString)
		{
			return this.GetMessage(resourceString, null);
		}

		// Token: 0x0600264F RID: 9807 RVA: 0x000D6B44 File Offset: 0x000D4D44
		internal string GetMessage(string resourceString, params object[] args)
		{
			string result;
			if (args != null)
			{
				result = StringUtil.Format(resourceString, args);
			}
			else
			{
				result = resourceString;
			}
			return result;
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06002650 RID: 9808 RVA: 0x000D6B61 File Offset: 0x000D4D61
		// (set) Token: 0x06002651 RID: 9809 RVA: 0x000D6B69 File Offset: 0x000D4D69
		internal bool SkipWinRMCheck
		{
			get
			{
				return this._skipWinRMCheck;
			}
			set
			{
				this._skipWinRMCheck = value;
			}
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x000D6B74 File Offset: 0x000D4D74
		protected string ResolveShell(string shell)
		{
			string result;
			if (!string.IsNullOrEmpty(shell))
			{
				result = shell;
			}
			else
			{
				result = (string)base.SessionState.Internal.ExecutionContext.GetVariableValue(SpecialVariables.PSSessionConfigurationNameVarPath, "http://schemas.microsoft.com/powershell/Microsoft.PowerShell");
			}
			return result;
		}

		// Token: 0x06002653 RID: 9811 RVA: 0x000D6BB4 File Offset: 0x000D4DB4
		protected string ResolveAppName(string appName)
		{
			string result;
			if (!string.IsNullOrEmpty(appName))
			{
				result = appName;
			}
			else
			{
				result = (string)base.SessionState.Internal.ExecutionContext.GetVariableValue(SpecialVariables.PSSessionApplicationNameVarPath, "WSMan");
			}
			return result;
		}

		// Token: 0x040012EB RID: 4843
		protected const string ComputerNameParameterSet = "ComputerName";

		// Token: 0x040012EC RID: 4844
		protected const string SessionParameterSet = "Session";

		// Token: 0x040012ED RID: 4845
		protected const string DefaultPowerShellRemoteShellName = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell";

		// Token: 0x040012EE RID: 4846
		protected const string DefaultPowerShellRemoteShellAppName = "WSMan";

		// Token: 0x040012EF RID: 4847
		private static string LOCALHOST = "localhost";

		// Token: 0x040012F0 RID: 4848
		private bool _skipWinRMCheck;
	}
}
