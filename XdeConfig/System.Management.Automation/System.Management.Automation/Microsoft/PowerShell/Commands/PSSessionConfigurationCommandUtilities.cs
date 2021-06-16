using System;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Security;
using System.Security.Principal;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000319 RID: 793
	internal static class PSSessionConfigurationCommandUtilities
	{
		// Token: 0x060025EF RID: 9711 RVA: 0x000D4034 File Offset: 0x000D2234
		internal static void RestartWinRMService(PSCmdlet cmdlet, bool isErrorReported, bool force, bool noServiceRestart)
		{
			if (!isErrorReported && !noServiceRestart)
			{
				string restartWSManServiceAction = RemotingErrorIdStrings.RestartWSManServiceAction;
				string target = StringUtil.Format(RemotingErrorIdStrings.RestartWSManServiceTarget, "WinRM");
				if (force || cmdlet.ShouldProcess(target, restartWSManServiceAction))
				{
					cmdlet.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.RestartWSManServiceMessageV, new object[0]));
					ScriptBlock scriptBlock = cmdlet.InvokeCommand.NewScriptBlock("restart-service winrm -force -confirm:$false");
					scriptBlock.InvokeUsingCmdlet(cmdlet, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[0]);
				}
			}
		}

		// Token: 0x060025F0 RID: 9712 RVA: 0x000D40B0 File Offset: 0x000D22B0
		internal static void MoveWinRmToIsolatedServiceHost(bool forVirtualAccount)
		{
			string text = "sc.exe config winrm type= own";
			if (forVirtualAccount)
			{
				text += "\r\n                    $requiredPrivileges = Get-ItemPropertyValue -Path HKLM:\\SYSTEM\\CurrentControlSet\\Services\\winrm -Name RequiredPrivileges\r\n                    if($requiredPrivileges -notcontains 'SeTcbPrivilege')\r\n                    {\r\n                        $requiredPrivileges += @('SeTcbPrivilege')\r\n                    }\r\n                    Set-ItemProperty -Path HKLM:\\SYSTEM\\CurrentControlSet\\Services\\winrm -Name RequiredPrivileges -Value $requiredPrivileges\r\n                    Set-ItemProperty -Path HKLM:\\SYSTEM\\CurrentControlSet\\Services\\winrm -Name ObjectName -Value 'LocalSystem'";
			}
			using (PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace))
			{
				powerShell.AddScript(text).Invoke();
			}
		}

		// Token: 0x060025F1 RID: 9713 RVA: 0x000D4104 File Offset: 0x000D2304
		internal static void CollectShouldProcessParameters(PSCmdlet cmdlet, out bool whatIf, out bool confirm)
		{
			whatIf = false;
			confirm = false;
			MshCommandRuntime mshCommandRuntime = cmdlet.CommandRuntime as MshCommandRuntime;
			if (mshCommandRuntime != null)
			{
				whatIf = mshCommandRuntime.WhatIf;
				if (mshCommandRuntime.IsConfirmFlagSet)
				{
					confirm = mshCommandRuntime.Confirm;
				}
			}
		}

		// Token: 0x060025F2 RID: 9714 RVA: 0x000D4148 File Offset: 0x000D2348
		internal static void ThrowIfNotAdministrator()
		{
			WindowsIdentity current = WindowsIdentity.GetCurrent();
			WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
			if (!windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator))
			{
				string message = StringUtil.Format(RemotingErrorIdStrings.EDcsRequiresElevation, new object[0]);
				throw new InvalidOperationException(message);
			}
		}

		// Token: 0x060025F3 RID: 9715 RVA: 0x000D4188 File Offset: 0x000D2388
		internal static Version CalculateMaxPSVersion(Version psVersion)
		{
			Version result = null;
			if (psVersion != null && psVersion.Major == 2)
			{
				result = new Version(2, 0);
			}
			return result;
		}

		// Token: 0x060025F4 RID: 9716 RVA: 0x000D41B4 File Offset: 0x000D23B4
		internal static string GetModulePathAsString(object[] modulePath)
		{
			if (modulePath != null && modulePath.Length > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (object obj in modulePath)
				{
					string text = obj as string;
					if (text != null)
					{
						stringBuilder.Append(obj);
						stringBuilder.Append(',');
					}
					else
					{
						ModuleSpecification moduleSpecification = obj as ModuleSpecification;
						if (moduleSpecification != null)
						{
							stringBuilder.Append(SecurityElement.Escape(SecurityElement.Escape(moduleSpecification.ToString())));
							stringBuilder.Append(',');
						}
					}
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				return stringBuilder.ToString();
			}
			return string.Empty;
		}

		// Token: 0x060025F5 RID: 9717 RVA: 0x000D4258 File Offset: 0x000D2458
		internal static Version ConstructVersionFormatForConfigXml(Version psVersion)
		{
			Version result = null;
			if (psVersion != null)
			{
				result = new Version(psVersion.Major, psVersion.Minor);
			}
			return result;
		}

		// Token: 0x060025F6 RID: 9718 RVA: 0x000D4284 File Offset: 0x000D2484
		internal static void CheckIfPowerShellVersionIsInstalled(Version version)
		{
			if (version != null && version.Major == 2)
			{
				try
				{
					PSSnapInReader.GetPSEngineKey(PSVersionInfo.RegistryVersion1Key);
					if (!PsUtils.FrameworkRegistryInstallation.IsFrameworkInstalled(2, 0, 0))
					{
						throw new ArgumentException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.NetFrameWorkV2NotInstalled, new object[0]));
					}
				}
				catch (PSArgumentException)
				{
					throw new ArgumentException(PSRemotingErrorInvariants.FormatResourceString(RemotingErrorIdStrings.PowerShellNotInstalled, new object[]
					{
						version,
						"PSVersion"
					}));
				}
			}
		}

		// Token: 0x060025F7 RID: 9719 RVA: 0x000D4308 File Offset: 0x000D2508
		internal static string GetRunAsVirtualAccountGroupsString(string[] groups)
		{
			if (groups == null)
			{
				return string.Empty;
			}
			return string.Join(";", groups);
		}

		// Token: 0x040012AE RID: 4782
		internal const string restartWSManFormat = "restart-service winrm -force -confirm:$false";

		// Token: 0x040012AF RID: 4783
		internal const string PSCustomShellTypeName = "Microsoft.PowerShell.Commands.PSSessionConfigurationCommands#PSSessionConfiguration";
	}
}
