using System;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting.Client;
using System.Management.Automation.Runspaces;
using System.Security;
using Microsoft.Win32;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000338 RID: 824
	internal static class RemotingCommandUtil
	{
		// Token: 0x060027ED RID: 10221 RVA: 0x000DF3A4 File Offset: 0x000DD5A4
		internal static bool HasRepeatingRunspaces(PSSession[] runspaceInfos)
		{
			if (runspaceInfos == null)
			{
				throw PSTraceSource.NewArgumentNullException("runspaceInfos");
			}
			if (runspaceInfos.GetLength(0) == 0)
			{
				throw PSTraceSource.NewArgumentException("runspaceInfos");
			}
			for (int i = 0; i < runspaceInfos.GetLength(0); i++)
			{
				for (int j = 0; j < runspaceInfos.GetLength(0); j++)
				{
					if (i != j && runspaceInfos[i].Runspace.InstanceId == runspaceInfos[j].Runspace.InstanceId)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x000DF41F File Offset: 0x000DD61F
		internal static bool ExceedMaximumAllowableRunspaces(PSSession[] runspaceInfos)
		{
			if (runspaceInfos == null)
			{
				throw PSTraceSource.NewArgumentNullException("runspaceInfos");
			}
			if (runspaceInfos.GetLength(0) == 0)
			{
				throw PSTraceSource.NewArgumentException("runspaceInfos");
			}
			return false;
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x000DF444 File Offset: 0x000DD644
		internal static void CheckRemotingCmdletPrerequisites()
		{
			bool flag = true;
			string name = "Software\\Microsoft\\Windows\\CurrentVersion\\WSMAN\\";
			RemotingCommandUtil.CheckHostRemotingPrerequisites();
			try
			{
				string text = null;
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
				if (registryKey != null)
				{
					text = (string)registryKey.GetValue("ServiceStackVersion");
				}
				Version v = (!string.IsNullOrEmpty(text)) ? new Version(text.Trim()) : WSManNativeApi.WSMAN_STACK_VERSION;
				if (v >= new Version(2, 0))
				{
					flag = false;
				}
			}
			catch (FormatException)
			{
				flag = true;
			}
			catch (OverflowException)
			{
				flag = true;
			}
			catch (ArgumentException)
			{
				flag = true;
			}
			catch (SecurityException)
			{
				flag = true;
			}
			catch (ObjectDisposedException)
			{
				flag = true;
			}
			if (flag)
			{
				throw new InvalidOperationException("Windows PowerShell remoting features are not enabled or not supported on this machine.\nThis may be because you do not have the correct version of WS-Management installed or this version of Windows does not support remoting currently.\n For more information, type 'get-help about_remote_requirements'.");
			}
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x000DF518 File Offset: 0x000DD718
		internal static bool IsWinPEHost()
		{
			RegistryKey registryKey = null;
			if (RemotingCommandUtil.isWinPEHost == null)
			{
				try
				{
					registryKey = Registry.LocalMachine.OpenSubKey(RemotingCommandUtil.WinPEIdentificationRegKey);
					if (registryKey != null)
					{
						RemotingCommandUtil.isWinPEHost = new bool?(true);
					}
					else
					{
						RemotingCommandUtil.isWinPEHost = new bool?(false);
					}
				}
				catch (ArgumentException)
				{
				}
				catch (SecurityException)
				{
				}
				catch (ObjectDisposedException)
				{
				}
				finally
				{
					if (registryKey != null)
					{
						registryKey.Dispose();
					}
				}
			}
			return RemotingCommandUtil.isWinPEHost == true;
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x000DF5C8 File Offset: 0x000DD7C8
		internal static void CheckHostRemotingPrerequisites()
		{
			bool flag = RemotingCommandUtil.IsWinPEHost();
			if (flag)
			{
				ErrorRecord errorRecord = new ErrorRecord(new InvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.WinPERemotingNotSupported, new object[0])), null, ErrorCategory.InvalidOperation, null);
				throw new InvalidOperationException(errorRecord.ToString());
			}
		}

		// Token: 0x040013C6 RID: 5062
		internal static string WinPEIdentificationRegKey = "System\\CurrentControlSet\\Control\\MiniNT";

		// Token: 0x040013C7 RID: 5063
		internal static bool? isWinPEHost = null;
	}
}
