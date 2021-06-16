using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Tracing;
using System.Security.Principal;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200031E RID: 798
	[Cmdlet("Disable", "PSSessionConfiguration", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Low, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=144299")]
	public sealed class DisablePSSessionConfigurationCommand : PSCmdlet
	{
		// Token: 0x06002626 RID: 9766 RVA: 0x000D61BC File Offset: 0x000D43BC
		static DisablePSSessionConfigurationCommand()
		{
			string script = string.Format(CultureInfo.InvariantCulture, "\r\nfunction Disable-PSSessionConfiguration\r\n{{\r\n[CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Low\")]\r\nparam(\r\n    [Parameter(Position=0, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true)]\r\n    [System.String]\r\n    $Name,\r\n\r\n    [Parameter()]\r\n    [bool]\r\n    $Force,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $restartWinRMMessage,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $setEnabledTarget,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $setEnabledAction,\r\n\r\n    [Parameter()]\r\n    [bool]\r\n    $noServiceRestart\r\n)\r\n    \r\n    begin\r\n    {{\r\n        if ($force -or $pscmdlet.ShouldProcess($restartWinRMMessage))\r\n        {{\r\n            $svc = get-service winrm\r\n            if ($svc.Status -match \"Stopped\")\r\n            {{\r\n               Restart-Service winrm -force -confirm:$false\r\n            }}\r\n        }}       \r\n    }} #end of Begin block   \r\n\r\n    process\r\n    {{\r\n       Get-PSSessionConfiguration $name -Force:$Force | % {{\r\n           \r\n           if ($_.Enabled -and ($force -or $pscmdlet.ShouldProcess($setEnabledTarget, $setEnabledAction)))\r\n           {{\r\n                Set-Item -WarningAction SilentlyContinue -Path \"WSMan:\\localhost\\Plugin\\$name\\Enabled\" -Value $false -Force -Confirm:$false\r\n           }}\r\n       }} # end of foreach block\r\n    }} #end of process block\r\n\r\n    # no longer necessary to restart the winrm to apply the config change\r\n    End\r\n    {{\r\n    }}\r\n}}\r\n\r\n$_ | Disable-PSSessionConfiguration -force $args[0] -whatif:$args[1] -confirm:$args[2] -restartWinRMMessage $args[3] -setEnabledTarget $args[4] -setEnabledAction $args[5] -noServiceRestart $args[6]\r\n", new object[0]);
			DisablePSSessionConfigurationCommand.disablePluginSb = ScriptBlock.Create(script);
			DisablePSSessionConfigurationCommand.disablePluginSb.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
		}

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06002627 RID: 9767 RVA: 0x000D61FA File Offset: 0x000D43FA
		// (set) Token: 0x06002628 RID: 9768 RVA: 0x000D6202 File Offset: 0x000D4402
		[ValidateNotNullOrEmpty]
		[Parameter(Position = 0, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
		public string[] Name
		{
			get
			{
				return this.shellName;
			}
			set
			{
				this.shellName = value;
			}
		}

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06002629 RID: 9769 RVA: 0x000D620B File Offset: 0x000D440B
		// (set) Token: 0x0600262A RID: 9770 RVA: 0x000D6218 File Offset: 0x000D4418
		[Parameter]
		public SwitchParameter Force
		{
			get
			{
				return this.force;
			}
			set
			{
				this.force = value;
			}
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x0600262B RID: 9771 RVA: 0x000D6226 File Offset: 0x000D4426
		// (set) Token: 0x0600262C RID: 9772 RVA: 0x000D6233 File Offset: 0x000D4433
		[Parameter]
		public SwitchParameter NoServiceRestart
		{
			get
			{
				return this.noRestart;
			}
			set
			{
				this.noRestart = value;
			}
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x000D6241 File Offset: 0x000D4441
		protected override void BeginProcessing()
		{
			RemotingCommandUtil.CheckRemotingCmdletPrerequisites();
			PSSessionConfigurationCommandUtilities.ThrowIfNotAdministrator();
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x000D6250 File Offset: 0x000D4450
		protected override void ProcessRecord()
		{
			if (this.shellName != null)
			{
				foreach (string item in this.shellName)
				{
					this.shellsToDisable.Add(item);
				}
			}
		}

		// Token: 0x0600262F RID: 9775 RVA: 0x000D628C File Offset: 0x000D448C
		protected override void EndProcessing()
		{
			if (this.shellsToDisable.Count == 0)
			{
				this.shellsToDisable.Add("Microsoft.PowerShell");
			}
			base.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.EcsScriptMessageV, "\r\nfunction Disable-PSSessionConfiguration\r\n{{\r\n[CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Low\")]\r\nparam(\r\n    [Parameter(Position=0, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true)]\r\n    [System.String]\r\n    $Name,\r\n\r\n    [Parameter()]\r\n    [bool]\r\n    $Force,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $restartWinRMMessage,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $setEnabledTarget,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $setEnabledAction,\r\n\r\n    [Parameter()]\r\n    [bool]\r\n    $noServiceRestart\r\n)\r\n    \r\n    begin\r\n    {{\r\n        if ($force -or $pscmdlet.ShouldProcess($restartWinRMMessage))\r\n        {{\r\n            $svc = get-service winrm\r\n            if ($svc.Status -match \"Stopped\")\r\n            {{\r\n               Restart-Service winrm -force -confirm:$false\r\n            }}\r\n        }}       \r\n    }} #end of Begin block   \r\n\r\n    process\r\n    {{\r\n       Get-PSSessionConfiguration $name -Force:$Force | % {{\r\n           \r\n           if ($_.Enabled -and ($force -or $pscmdlet.ShouldProcess($setEnabledTarget, $setEnabledAction)))\r\n           {{\r\n                Set-Item -WarningAction SilentlyContinue -Path \"WSMan:\\localhost\\Plugin\\$name\\Enabled\" -Value $false -Force -Confirm:$false\r\n           }}\r\n       }} # end of foreach block\r\n    }} #end of process block\r\n\r\n    # no longer necessary to restart the winrm to apply the config change\r\n    End\r\n    {{\r\n    }}\r\n}}\r\n\r\n$_ | Disable-PSSessionConfiguration -force $args[0] -whatif:$args[1] -confirm:$args[2] -restartWinRMMessage $args[3] -setEnabledTarget $args[4] -setEnabledAction $args[5] -noServiceRestart $args[6]\r\n"));
			bool flag = false;
			bool flag2 = true;
			PSSessionConfigurationCommandUtilities.CollectShouldProcessParameters(this, out flag, out flag2);
			string restartWinRMMessage = RemotingErrorIdStrings.RestartWinRMMessage;
			string setEnabledFalseTarget = RemotingErrorIdStrings.SetEnabledFalseTarget;
			string text = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessAction, "Set-Item");
			DisablePSSessionConfigurationCommand.disablePluginSb.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, this.shellsToDisable, new object[0], AutomationNull.Value, new object[]
			{
				this.force,
				flag,
				flag2,
				restartWinRMMessage,
				setEnabledFalseTarget,
				text,
				this.noRestart
			});
			Tracer tracer = new Tracer();
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in this.Name ?? new string[0])
			{
				stringBuilder.Append(value);
				stringBuilder.Append(", ");
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 2, 2);
			}
			tracer.EndpointDisabled(stringBuilder.ToString(), WindowsIdentity.GetCurrent().Name);
		}

		// Token: 0x040012D9 RID: 4825
		private const string disablePluginSbFormat = "\r\nfunction Disable-PSSessionConfiguration\r\n{{\r\n[CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Low\")]\r\nparam(\r\n    [Parameter(Position=0, ValueFromPipeline=$true, ValueFromPipelineByPropertyName=$true)]\r\n    [System.String]\r\n    $Name,\r\n\r\n    [Parameter()]\r\n    [bool]\r\n    $Force,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $restartWinRMMessage,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $setEnabledTarget,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $setEnabledAction,\r\n\r\n    [Parameter()]\r\n    [bool]\r\n    $noServiceRestart\r\n)\r\n    \r\n    begin\r\n    {{\r\n        if ($force -or $pscmdlet.ShouldProcess($restartWinRMMessage))\r\n        {{\r\n            $svc = get-service winrm\r\n            if ($svc.Status -match \"Stopped\")\r\n            {{\r\n               Restart-Service winrm -force -confirm:$false\r\n            }}\r\n        }}       \r\n    }} #end of Begin block   \r\n\r\n    process\r\n    {{\r\n       Get-PSSessionConfiguration $name -Force:$Force | % {{\r\n           \r\n           if ($_.Enabled -and ($force -or $pscmdlet.ShouldProcess($setEnabledTarget, $setEnabledAction)))\r\n           {{\r\n                Set-Item -WarningAction SilentlyContinue -Path \"WSMan:\\localhost\\Plugin\\$name\\Enabled\" -Value $false -Force -Confirm:$false\r\n           }}\r\n       }} # end of foreach block\r\n    }} #end of process block\r\n\r\n    # no longer necessary to restart the winrm to apply the config change\r\n    End\r\n    {{\r\n    }}\r\n}}\r\n\r\n$_ | Disable-PSSessionConfiguration -force $args[0] -whatif:$args[1] -confirm:$args[2] -restartWinRMMessage $args[3] -setEnabledTarget $args[4] -setEnabledAction $args[5] -noServiceRestart $args[6]\r\n";

		// Token: 0x040012DA RID: 4826
		private static ScriptBlock disablePluginSb;

		// Token: 0x040012DB RID: 4827
		private string[] shellName;

		// Token: 0x040012DC RID: 4828
		private Collection<string> shellsToDisable = new Collection<string>();

		// Token: 0x040012DD RID: 4829
		private bool force;

		// Token: 0x040012DE RID: 4830
		private bool noRestart;
	}
}
