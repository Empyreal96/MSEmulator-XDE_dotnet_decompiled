using System;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000320 RID: 800
	[Cmdlet("Disable", "PSRemoting", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=144298")]
	public sealed class DisablePSRemotingCommand : PSCmdlet
	{
		// Token: 0x06002639 RID: 9785 RVA: 0x000D65BC File Offset: 0x000D47BC
		static DisablePSRemotingCommand()
		{
			string localSddl = PSSessionConfigurationCommandBase.GetLocalSddl();
			string script = string.Format(CultureInfo.InvariantCulture, "\r\nfunction Disable-PSRemoting\r\n{{\r\n[CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Medium\")]\r\nparam(\r\n    [Parameter()]\r\n    [switch]\r\n    $force,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $queryForSet,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $captionForSet,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $restartWinRMMessage\r\n)\r\n\r\n    begin\r\n    {{\r\n        if ($pscmdlet.ShouldProcess($restartWinRMMessage))\r\n        {{\r\n            $svc = get-service winrm\r\n            if ($svc.Status -match \"Stopped\")\r\n            {{\r\n                Restart-Service winrm -force -confirm:$false\r\n            }}\r\n        }}\r\n    }} # end of begin block\r\n\r\n    end\r\n    {{\r\n        # Disable the network for all Session Configurations\r\n        Get-PSSessionConfiguration -Force:$force | % {{\r\n        \r\n            if ($_.Enabled)\r\n            {{\r\n                $sddl = $null\r\n                if ($_.psobject.members[\"SecurityDescriptorSddl\"])\r\n                {{\r\n                    $sddl = $_.psobject.members[\"SecurityDescriptorSddl\"].Value\r\n                }}\r\n\r\n                if (!$sddl)\r\n                {{\r\n                    # Disable network users from accessing this configuration\r\n                    $sddl = \"{0}\"\r\n                }}\r\n                else\r\n                {{\r\n                    # Construct SID for network users\r\n                    [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n                    $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n                \r\n                    # Add disable network to the existing sddl\r\n                    $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$sddl\r\n                    $disableNetworkExists = $false\r\n                    $sd.DiscretionaryAcl | % {{\r\n                        if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                        {{\r\n                            $disableNetworkExists = $true              \r\n                        }}\r\n                    }}\r\n\r\n                    if (!$disableNetworkExists)\r\n                    {{\r\n                        $sd.DiscretionaryAcl.AddAccess(\"deny\", $networkSID, 268435456, \"None\", \"None\")\r\n                        $sddl = $sd.GetSddlForm(\"all\")\r\n                    }}\r\n                    else\r\n                    {{\r\n                        # since disable network GA already exists, we dont need to change anything.\r\n                        $sddl = $null\r\n                    }}\r\n                }} ## end of if(!$sddl)\r\n\r\n                $qMessage = $queryForSet -f $_.name,$sddl\r\n                if (($sddl) -and ($force -or $pscmdlet.ShouldProcess($qMessage, $captionForSet)))\r\n                {{\r\n                    $null = Set-PSSessionConfiguration -Name $_.Name -SecurityDescriptorSddl $sddl -NoServiceRestart -force -WarningAction 0\r\n                }}\r\n            }} ## end of if($_.Enabled)\r\n        }} ## end of %\r\n    }} ## end of Process block\r\n}}\r\n\r\nDisable-PSRemoting -force:$args[0] -queryForSet $args[1] -captionForSet $args[2] -restartWinRMMessage $args[3] -whatif:$args[4] -confirm:$args[5]\r\n", new object[]
			{
				localSddl
			});
			DisablePSRemotingCommand.disableRemotingSb = ScriptBlock.Create(script);
			DisablePSRemotingCommand.disableRemotingSb.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x0600263A RID: 9786 RVA: 0x000D6606 File Offset: 0x000D4806
		// (set) Token: 0x0600263B RID: 9787 RVA: 0x000D6613 File Offset: 0x000D4813
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

		// Token: 0x0600263C RID: 9788 RVA: 0x000D6621 File Offset: 0x000D4821
		protected override void BeginProcessing()
		{
			RemotingCommandUtil.CheckRemotingCmdletPrerequisites();
			PSSessionConfigurationCommandUtilities.ThrowIfNotAdministrator();
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000D6630 File Offset: 0x000D4830
		protected override void EndProcessing()
		{
			base.WriteWarning(StringUtil.Format(RemotingErrorIdStrings.DcsWarningMessage, new object[0]));
			base.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.EcsScriptMessageV, "\r\nfunction Disable-PSRemoting\r\n{{\r\n[CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Medium\")]\r\nparam(\r\n    [Parameter()]\r\n    [switch]\r\n    $force,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $queryForSet,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $captionForSet,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $restartWinRMMessage\r\n)\r\n\r\n    begin\r\n    {{\r\n        if ($pscmdlet.ShouldProcess($restartWinRMMessage))\r\n        {{\r\n            $svc = get-service winrm\r\n            if ($svc.Status -match \"Stopped\")\r\n            {{\r\n                Restart-Service winrm -force -confirm:$false\r\n            }}\r\n        }}\r\n    }} # end of begin block\r\n\r\n    end\r\n    {{\r\n        # Disable the network for all Session Configurations\r\n        Get-PSSessionConfiguration -Force:$force | % {{\r\n        \r\n            if ($_.Enabled)\r\n            {{\r\n                $sddl = $null\r\n                if ($_.psobject.members[\"SecurityDescriptorSddl\"])\r\n                {{\r\n                    $sddl = $_.psobject.members[\"SecurityDescriptorSddl\"].Value\r\n                }}\r\n\r\n                if (!$sddl)\r\n                {{\r\n                    # Disable network users from accessing this configuration\r\n                    $sddl = \"{0}\"\r\n                }}\r\n                else\r\n                {{\r\n                    # Construct SID for network users\r\n                    [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n                    $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n                \r\n                    # Add disable network to the existing sddl\r\n                    $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$sddl\r\n                    $disableNetworkExists = $false\r\n                    $sd.DiscretionaryAcl | % {{\r\n                        if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                        {{\r\n                            $disableNetworkExists = $true              \r\n                        }}\r\n                    }}\r\n\r\n                    if (!$disableNetworkExists)\r\n                    {{\r\n                        $sd.DiscretionaryAcl.AddAccess(\"deny\", $networkSID, 268435456, \"None\", \"None\")\r\n                        $sddl = $sd.GetSddlForm(\"all\")\r\n                    }}\r\n                    else\r\n                    {{\r\n                        # since disable network GA already exists, we dont need to change anything.\r\n                        $sddl = $null\r\n                    }}\r\n                }} ## end of if(!$sddl)\r\n\r\n                $qMessage = $queryForSet -f $_.name,$sddl\r\n                if (($sddl) -and ($force -or $pscmdlet.ShouldProcess($qMessage, $captionForSet)))\r\n                {{\r\n                    $null = Set-PSSessionConfiguration -Name $_.Name -SecurityDescriptorSddl $sddl -NoServiceRestart -force -WarningAction 0\r\n                }}\r\n            }} ## end of if($_.Enabled)\r\n        }} ## end of %\r\n    }} ## end of Process block\r\n}}\r\n\r\nDisable-PSRemoting -force:$args[0] -queryForSet $args[1] -captionForSet $args[2] -restartWinRMMessage $args[3] -whatif:$args[4] -confirm:$args[5]\r\n"));
			bool flag = false;
			bool flag2 = true;
			PSSessionConfigurationCommandUtilities.CollectShouldProcessParameters(this, out flag, out flag2);
			string text = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessAction, "Set-PSSessionConfiguration");
			string disableRemotingShouldProcessTarget = RemotingErrorIdStrings.DisableRemotingShouldProcessTarget;
			string restartWinRMMessage = RemotingErrorIdStrings.RestartWinRMMessage;
			DisablePSRemotingCommand.disableRemotingSb.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[]
			{
				this.force,
				disableRemotingShouldProcessTarget,
				text,
				restartWinRMMessage,
				flag,
				flag2
			});
		}

		// Token: 0x040012E4 RID: 4836
		private const string disablePSRemotingFormat = "\r\nfunction Disable-PSRemoting\r\n{{\r\n[CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Medium\")]\r\nparam(\r\n    [Parameter()]\r\n    [switch]\r\n    $force,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $queryForSet,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $captionForSet,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $restartWinRMMessage\r\n)\r\n\r\n    begin\r\n    {{\r\n        if ($pscmdlet.ShouldProcess($restartWinRMMessage))\r\n        {{\r\n            $svc = get-service winrm\r\n            if ($svc.Status -match \"Stopped\")\r\n            {{\r\n                Restart-Service winrm -force -confirm:$false\r\n            }}\r\n        }}\r\n    }} # end of begin block\r\n\r\n    end\r\n    {{\r\n        # Disable the network for all Session Configurations\r\n        Get-PSSessionConfiguration -Force:$force | % {{\r\n        \r\n            if ($_.Enabled)\r\n            {{\r\n                $sddl = $null\r\n                if ($_.psobject.members[\"SecurityDescriptorSddl\"])\r\n                {{\r\n                    $sddl = $_.psobject.members[\"SecurityDescriptorSddl\"].Value\r\n                }}\r\n\r\n                if (!$sddl)\r\n                {{\r\n                    # Disable network users from accessing this configuration\r\n                    $sddl = \"{0}\"\r\n                }}\r\n                else\r\n                {{\r\n                    # Construct SID for network users\r\n                    [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n                    $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n                \r\n                    # Add disable network to the existing sddl\r\n                    $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$sddl\r\n                    $disableNetworkExists = $false\r\n                    $sd.DiscretionaryAcl | % {{\r\n                        if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                        {{\r\n                            $disableNetworkExists = $true              \r\n                        }}\r\n                    }}\r\n\r\n                    if (!$disableNetworkExists)\r\n                    {{\r\n                        $sd.DiscretionaryAcl.AddAccess(\"deny\", $networkSID, 268435456, \"None\", \"None\")\r\n                        $sddl = $sd.GetSddlForm(\"all\")\r\n                    }}\r\n                    else\r\n                    {{\r\n                        # since disable network GA already exists, we dont need to change anything.\r\n                        $sddl = $null\r\n                    }}\r\n                }} ## end of if(!$sddl)\r\n\r\n                $qMessage = $queryForSet -f $_.name,$sddl\r\n                if (($sddl) -and ($force -or $pscmdlet.ShouldProcess($qMessage, $captionForSet)))\r\n                {{\r\n                    $null = Set-PSSessionConfiguration -Name $_.Name -SecurityDescriptorSddl $sddl -NoServiceRestart -force -WarningAction 0\r\n                }}\r\n            }} ## end of if($_.Enabled)\r\n        }} ## end of %\r\n    }} ## end of Process block\r\n}}\r\n\r\nDisable-PSRemoting -force:$args[0] -queryForSet $args[1] -captionForSet $args[2] -restartWinRMMessage $args[3] -whatif:$args[4] -confirm:$args[5]\r\n";

		// Token: 0x040012E5 RID: 4837
		private static ScriptBlock disableRemotingSb;

		// Token: 0x040012E6 RID: 4838
		private bool force;
	}
}
