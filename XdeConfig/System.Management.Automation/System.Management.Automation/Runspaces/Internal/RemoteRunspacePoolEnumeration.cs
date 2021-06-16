using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation.Remoting;

namespace System.Management.Automation.Runspaces.Internal
{
	// Token: 0x020002A1 RID: 673
	internal static class RemoteRunspacePoolEnumeration
	{
		// Token: 0x0600205C RID: 8284 RVA: 0x000BB840 File Offset: 0x000B9A40
		internal static Collection<PSObject> GetRemotePools(WSManConnectionInfo wsmanConnectionInfo)
		{
			Collection<PSObject> result;
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.AddCommand("Get-WSManInstance");
				powerShell.AddParameter("ResourceURI", "Shell");
				powerShell.AddParameter("Enumerate", true);
				powerShell.AddParameter("ComputerName", wsmanConnectionInfo.ComputerName);
				powerShell.AddParameter("Authentication", RemoteRunspacePoolEnumeration.ConvertPSAuthToWSManAuth(wsmanConnectionInfo.AuthenticationMechanism));
				if (wsmanConnectionInfo.Credential != null)
				{
					powerShell.AddParameter("Credential", wsmanConnectionInfo.Credential);
				}
				if (wsmanConnectionInfo.CertificateThumbprint != null)
				{
					powerShell.AddParameter("CertificateThumbprint", wsmanConnectionInfo.CertificateThumbprint);
				}
				if (wsmanConnectionInfo.PortSetting != -1)
				{
					powerShell.AddParameter("Port", wsmanConnectionInfo.Port);
				}
				if (RemoteRunspacePoolEnumeration.CheckForSSL(wsmanConnectionInfo))
				{
					powerShell.AddParameter("UseSSL", true);
				}
				if (!string.IsNullOrEmpty(wsmanConnectionInfo.AppName))
				{
					string value = wsmanConnectionInfo.AppName.TrimStart(new char[]
					{
						'/'
					});
					powerShell.AddParameter("ApplicationName", value);
				}
				powerShell.AddParameter("SessionOption", RemoteRunspacePoolEnumeration.GetSessionOptions(wsmanConnectionInfo));
				result = powerShell.Invoke();
			}
			return result;
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000BB994 File Offset: 0x000B9B94
		internal static Collection<PSObject> GetRemoteCommands(Guid shellId, WSManConnectionInfo wsmanConnectionInfo)
		{
			Collection<PSObject> result;
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.AddCommand("Get-WSManInstance");
				string value = string.Format(CultureInfo.InvariantCulture, "ShellId='{0}'", new object[]
				{
					shellId.ToString().ToUpperInvariant()
				});
				powerShell.AddParameter("ResourceURI", "Shell/Command");
				powerShell.AddParameter("Enumerate", true);
				powerShell.AddParameter("Dialect", "Selector");
				powerShell.AddParameter("Filter", value);
				powerShell.AddParameter("ComputerName", wsmanConnectionInfo.ComputerName);
				powerShell.AddParameter("Authentication", RemoteRunspacePoolEnumeration.ConvertPSAuthToWSManAuth(wsmanConnectionInfo.AuthenticationMechanism));
				if (wsmanConnectionInfo.Credential != null)
				{
					powerShell.AddParameter("Credential", wsmanConnectionInfo.Credential);
				}
				if (wsmanConnectionInfo.CertificateThumbprint != null)
				{
					powerShell.AddParameter("CertificateThumbprint", wsmanConnectionInfo.CertificateThumbprint);
				}
				if (wsmanConnectionInfo.PortSetting != -1)
				{
					powerShell.AddParameter("Port", wsmanConnectionInfo.Port);
				}
				if (RemoteRunspacePoolEnumeration.CheckForSSL(wsmanConnectionInfo))
				{
					powerShell.AddParameter("UseSSL", true);
				}
				if (!string.IsNullOrEmpty(wsmanConnectionInfo.AppName))
				{
					string value2 = wsmanConnectionInfo.AppName.TrimStart(new char[]
					{
						'/'
					});
					powerShell.AddParameter("ApplicationName", value2);
				}
				powerShell.AddParameter("SessionOption", RemoteRunspacePoolEnumeration.GetSessionOptions(wsmanConnectionInfo));
				result = powerShell.Invoke();
			}
			return result;
		}

		// Token: 0x0600205E RID: 8286 RVA: 0x000BBB38 File Offset: 0x000B9D38
		private static object GetSessionOptions(WSManConnectionInfo wsmanConnectionInfo)
		{
			Collection<PSObject> collection;
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.AddCommand("New-WSManSessionOption");
				if (wsmanConnectionInfo.ProxyAccessType != ProxyAccessType.None)
				{
					powerShell.AddParameter("ProxyAccessType", "Proxy" + wsmanConnectionInfo.ProxyAccessType.ToString());
					powerShell.AddParameter("ProxyAuthentication", wsmanConnectionInfo.ProxyAuthentication.ToString());
					if (wsmanConnectionInfo.ProxyCredential != null)
					{
						powerShell.AddParameter("ProxyCredential", wsmanConnectionInfo.ProxyCredential);
					}
				}
				if (wsmanConnectionInfo.IncludePortInSPN)
				{
					powerShell.AddParameter("SPNPort", wsmanConnectionInfo.Port);
				}
				powerShell.AddParameter("SkipCACheck", wsmanConnectionInfo.SkipCACheck);
				powerShell.AddParameter("SkipCNCheck", wsmanConnectionInfo.SkipCNCheck);
				powerShell.AddParameter("SkipRevocationCheck", wsmanConnectionInfo.SkipRevocationCheck);
				powerShell.AddParameter("OperationTimeout", wsmanConnectionInfo.OperationTimeout);
				powerShell.AddParameter("NoEncryption", wsmanConnectionInfo.NoEncryption);
				powerShell.AddParameter("UseUTF16", wsmanConnectionInfo.UseUTF16);
				collection = powerShell.Invoke();
			}
			return collection[0].BaseObject;
		}

		// Token: 0x0600205F RID: 8287 RVA: 0x000BBCA0 File Offset: 0x000B9EA0
		private static bool CheckForSSL(WSManConnectionInfo wsmanConnectionInfo)
		{
			return !string.IsNullOrEmpty(wsmanConnectionInfo.Scheme) && wsmanConnectionInfo.Scheme.IndexOf("https", StringComparison.OrdinalIgnoreCase) != -1;
		}

		// Token: 0x06002060 RID: 8288 RVA: 0x000BBCC8 File Offset: 0x000B9EC8
		private static int ConvertPSAuthToWSManAuth(AuthenticationMechanism psAuth)
		{
			switch (psAuth)
			{
			case AuthenticationMechanism.Default:
				return 1;
			case AuthenticationMechanism.Basic:
				return 8;
			case AuthenticationMechanism.Negotiate:
				return 4;
			case AuthenticationMechanism.Credssp:
				return 128;
			case AuthenticationMechanism.Digest:
				return 2;
			case AuthenticationMechanism.Kerberos:
				return 16;
			}
			return 1;
		}
	}
}
