using System;
using System.Management.Automation.Host;

namespace System.Management.Automation
{
	// Token: 0x020007FF RID: 2047
	public class AuthorizationManager
	{
		// Token: 0x06004F3B RID: 20283 RVA: 0x001A4466 File Offset: 0x001A2666
		public AuthorizationManager(string shellId)
		{
			this.shellId = shellId;
		}

		// Token: 0x06004F3C RID: 20284 RVA: 0x001A4480 File Offset: 0x001A2680
		internal void ShouldRunInternal(CommandInfo commandInfo, CommandOrigin origin, PSHost host)
		{
			bool flag = false;
			bool flag2 = false;
			Exception ex = null;
			try
			{
				lock (this.policyCheckLock)
				{
					flag = this.ShouldRun(commandInfo, origin, host, out ex);
				}
			}
			catch (Exception ex2)
			{
				CommandProcessorBase.CheckForSevereException(ex2);
				ex = ex2;
				flag2 = true;
				flag = false;
			}
			if (flag)
			{
				return;
			}
			if (ex == null)
			{
				throw new PSSecurityException(AuthorizationManagerBase.AuthorizationManagerDefaultFailureReason);
			}
			if (ex is PSSecurityException)
			{
				throw ex;
			}
			string message = ex.Message;
			if (flag2)
			{
				message = AuthorizationManagerBase.AuthorizationManagerDefaultFailureReason;
			}
			PSSecurityException ex3 = new PSSecurityException(message, ex);
			throw ex3;
		}

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x06004F3D RID: 20285 RVA: 0x001A4528 File Offset: 0x001A2728
		internal string ShellId
		{
			get
			{
				return this.shellId;
			}
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x001A4530 File Offset: 0x001A2730
		protected internal virtual bool ShouldRun(CommandInfo commandInfo, CommandOrigin origin, PSHost host, out Exception reason)
		{
			reason = null;
			return true;
		}

		// Token: 0x04002859 RID: 10329
		private string shellId;

		// Token: 0x0400285A RID: 10330
		private object policyCheckLock = new object();
	}
}
