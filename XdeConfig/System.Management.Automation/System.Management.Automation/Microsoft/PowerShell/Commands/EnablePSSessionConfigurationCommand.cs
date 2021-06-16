using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Tracing;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200031D RID: 797
	[Cmdlet("Enable", "PSSessionConfiguration", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=144301")]
	public sealed class EnablePSSessionConfigurationCommand : PSCmdlet
	{
		// Token: 0x06002617 RID: 9751 RVA: 0x000D5E84 File Offset: 0x000D4084
		static EnablePSSessionConfigurationCommand()
		{
			string script = string.Format(CultureInfo.InvariantCulture, "\r\n\r\nfunction Test-WinRMQuickConfigNeeded\r\n{{\r\n# Checking the following items\r\n#1. Starting or restarting (if already started) the WinRM service\r\n#2. Setting the WinRM service startup type to Automatic\r\n#3. Creating a listener to accept requests on any IP address\r\n#4. Enabling Windows Firewall inbound rule exceptions for WS-Management traffic (for http only).\r\n\r\n    $winrmQuickConfigNeeded = $false\r\n\r\n    # check if WinRM service is running\r\n    if ((Get-Service winrm).Status -ne 'Running'){{\r\n        $winrmQuickConfigNeeded = $true\r\n    }}\r\n\r\n    # check if WinRM service startup is Auto\r\n    elseif ((Get-WmiObject -Query \"Select StartMode From Win32_Service Where Name='winmgmt'\").StartMode -ne 'Auto'){{\r\n        $winrmQuickConfigNeeded = $true\r\n    }}\r\n\r\n    # check if a winrm listener is present\r\n    elseif (!(Test-Path WSMan:\\localhost\\Listener) -or ((Get-ChildItem WSMan:\\localhost\\Listener) -eq $null)){{\r\n        $winrmQuickConfigNeeded = $true\r\n    }}\r\n\r\n    # check if WinRM firewall is enabled for HTTP\r\n    else{{\r\n        if (Get-Command Get-NetFirewallRule -ErrorAction SilentlyContinue){{\r\n            $winrmFirewall = Get-NetFirewallRule -Name 'WINRM-HTTP-In-TCP' -ErrorAction SilentlyContinue\r\n            if (!$winrmFirewall -or $winrmFirewall.Enabled -ne $true){{            \r\n                $winrmQuickConfigNeeded = $true\r\n            }}\r\n        }}\r\n        else{{\r\n            $winrmQuickConfigNeeded = $true\r\n        }}\r\n    }}\r\n\r\n    $winrmQuickConfigNeeded\r\n}}\r\n\r\nfunction Enable-PSSessionConfiguration\r\n{{\r\n[CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Medium\")]\r\nparam(\r\n    [Parameter(Position=0, ValueFromPipeline=$true)]\r\n    [System.String]\r\n    $Name,\r\n\r\n    [Parameter()]\r\n    [bool]\r\n    $Force,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $sddl,\r\n    \r\n    [Parameter()]\r\n    [bool]\r\n    $isSDDLSpecified,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $queryForSet,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $captionForSet,\r\n        \r\n    [Parameter()]\r\n    [string]\r\n    $queryForQC,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $captionForQC,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $shouldProcessDescForQC,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $setEnabledTarget,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $setEnabledAction,\r\n\r\n    [Parameter()]\r\n    [bool]\r\n    $skipNetworkProfileCheck,\r\n\t\r\n    [Parameter()]\r\n    [bool]\r\n    $noServiceRestart\r\n    )\r\n     \r\n    begin\r\n    {{\r\n        $winrmQuickConfigNeeded = Test-WinRMQuickConfigNeeded\r\n\r\n        if ($winrmQuickConfigNeeded -and ($force -or $pscmdlet.ShouldProcess($shouldProcessDescForQC, $queryForQC, $captionForQC)))\r\n        {{\r\n            # get the status of winrm before Quick Config. if it is already\r\n            # running..restart the service after Quick Config.\r\n            $svc = get-service winrm\r\n            if ($skipNetworkProfileCheck)\r\n            {{\r\n                {0} -force -SkipNetworkProfileCheck\r\n            }}\r\n            else\r\n            {{\r\n                {0} -force\r\n            }}\r\n            if ($svc.Status -match \"Running\")\r\n            {{\r\n               Restart-Service winrm -force -confirm:$false\r\n            }}\r\n        }}\r\n    }} #end of Begin block   \r\n        \r\n    process\r\n    {{\r\n       Get-PSSessionConfiguration $name -Force:$Force | % {{\r\n\r\n          if ($_.Enabled -eq $false -and ($force -or $pscmdlet.ShouldProcess($setEnabledTarget, $setEnabledAction)))\r\n          {{\r\n             Set-Item -WarningAction SilentlyContinue -Path \"WSMan:\\localhost\\Plugin\\$name\\Enabled\" -Value $true -confirm:$false\r\n          }}\r\n\r\n          if (!$isSDDLSpecified)\r\n          {{\r\n             $sddlTemp = $null\r\n             if ($_.psobject.members[\"SecurityDescriptorSddl\"])\r\n             {{\r\n                 $sddlTemp = $_.psobject.members[\"SecurityDescriptorSddl\"].Value\r\n             }}\r\n\r\n             $securityIdentifierToPurge = $null\r\n             # strip out Disable-Everyone DACL from the SDDL\r\n             if ($sddlTemp)\r\n             {{\r\n                # construct SID for \"EveryOne\"\r\n                [system.security.principal.wellknownsidtype]$evst = \"worldsid\"\r\n                $everyOneSID = new-object system.security.principal.securityidentifier $evst,$null\r\n                                \r\n                $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$sddlTemp                \r\n                $sd.DiscretionaryAcl | % {{\r\n                    if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $everyOneSID))\r\n                    {{\r\n                       $securityIdentifierToPurge = $_.securityidentifier\r\n                    }}\r\n                }}\r\n             \r\n                if ($securityIdentifierToPurge)\r\n                {{\r\n                   $sd.discretionaryacl.purge($securityIdentifierToPurge)\r\n\r\n                   # if there is no discretionaryacl..add Builtin Administrators and Remote Management Users\r\n                   # to the DACL group as this is the default WSMan behavior\r\n                   if ($sd.discretionaryacl.count -eq 0)\r\n                   {{\r\n                      # Built-in administrators\r\n                      [system.security.principal.wellknownsidtype]$bast = \"BuiltinAdministratorsSid\"\r\n                      $basid = new-object system.security.principal.securityidentifier $bast,$null\r\n                      $sd.DiscretionaryAcl.AddAccess('Allow',$basid, 268435456, 'none', 'none')\r\n\r\n                      # Remote Management Users, Win8+ only\r\n                      if ([System.Environment]::OSVersion.Version -ge \"6.2.0.0\")\r\n                      {{\r\n                          $rmSidId = new-object system.security.principal.securityidentifier \"{1}\"\r\n                          $sd.DiscretionaryAcl.AddAccess('Allow', $rmSidId, 268435456, 'none', 'none')\r\n                      }}\r\n\r\n                      # Interactive Users\r\n                      $iuSidId = new-object system.security.principal.securityidentifier \"{2}\"\r\n                      $sd.DiscretionaryAcl.AddAccess('Allow', $iuSidId, 268435456, 'none', 'none')\r\n                   }}\r\n\r\n                   $sddl = $sd.GetSddlForm(\"all\")\r\n                }}\r\n             }} # if ($sddlTemp)\r\n          }} # if (!$isSDDLSpecified) \r\n          \r\n          $qMessage = $queryForSet -f $_.name,$sddl\r\n          if (($sddl -or $isSDDLSpecified) -and ($force -or $pscmdlet.ShouldProcess($qMessage, $captionForSet)))\r\n          {{\r\n              $null = Set-PSSessionConfiguration -Name $_.Name -SecurityDescriptorSddl $sddl -NoServiceRestart -force -WarningAction 0\r\n          }}\r\n       }} #end of Get-PSSessionConfiguration | foreach\r\n    }} # end of Process block\r\n\r\n    End\r\n    {{\r\n    }}\r\n}}\r\n\r\n$_ | Enable-PSSessionConfiguration -force $args[0] -sddl $args[1] -isSDDLSpecified $args[2] -queryForSet $args[3] -captionForSet $args[4] -queryForQC $args[5] -captionForQC $args[6] -whatif:$args[7] -confirm:$args[8] -shouldProcessDescForQC $args[9] -setEnabledTarget $args[10] -setEnabledAction $args[11] -skipNetworkProfileCheck $args[12] -noServiceRestart $args[13] \r\n", new object[]
			{
				"Set-WSManQuickConfig",
				"S-1-5-32-580",
				"S-1-5-4"
			});
			EnablePSSessionConfigurationCommand.enablePluginSb = ScriptBlock.Create(script);
			EnablePSSessionConfigurationCommand.enablePluginSb.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
		}

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06002618 RID: 9752 RVA: 0x000D5EDC File Offset: 0x000D40DC
		// (set) Token: 0x06002619 RID: 9753 RVA: 0x000D5EE4 File Offset: 0x000D40E4
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

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x0600261A RID: 9754 RVA: 0x000D5EED File Offset: 0x000D40ED
		// (set) Token: 0x0600261B RID: 9755 RVA: 0x000D5EFA File Offset: 0x000D40FA
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

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x0600261C RID: 9756 RVA: 0x000D5F08 File Offset: 0x000D4108
		// (set) Token: 0x0600261D RID: 9757 RVA: 0x000D5F10 File Offset: 0x000D4110
		[Parameter]
		public string SecurityDescriptorSddl
		{
			get
			{
				return this.sddl;
			}
			set
			{
				if (!string.IsNullOrEmpty(value) && new CommonSecurityDescriptor(false, false, value) == null)
				{
					throw new NotSupportedException();
				}
				this.sddl = value;
				this.isSddlSpecified = true;
			}
		}

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x0600261E RID: 9758 RVA: 0x000D5F45 File Offset: 0x000D4145
		// (set) Token: 0x0600261F RID: 9759 RVA: 0x000D5F52 File Offset: 0x000D4152
		[Parameter]
		public SwitchParameter SkipNetworkProfileCheck
		{
			get
			{
				return this.skipNetworkProfileCheck;
			}
			set
			{
				this.skipNetworkProfileCheck = value;
			}
		}

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06002620 RID: 9760 RVA: 0x000D5F60 File Offset: 0x000D4160
		// (set) Token: 0x06002621 RID: 9761 RVA: 0x000D5F6D File Offset: 0x000D416D
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

		// Token: 0x06002622 RID: 9762 RVA: 0x000D5F7B File Offset: 0x000D417B
		protected override void BeginProcessing()
		{
			RemotingCommandUtil.CheckRemotingCmdletPrerequisites();
			PSSessionConfigurationCommandUtilities.ThrowIfNotAdministrator();
		}

		// Token: 0x06002623 RID: 9763 RVA: 0x000D5F88 File Offset: 0x000D4188
		protected override void ProcessRecord()
		{
			if (this.shellName != null)
			{
				foreach (string item in this.shellName)
				{
					this.shellsToEnable.Add(item);
				}
			}
		}

		// Token: 0x06002624 RID: 9764 RVA: 0x000D5FC4 File Offset: 0x000D41C4
		protected override void EndProcessing()
		{
			if (this.shellsToEnable.Count == 0)
			{
				this.shellsToEnable.Add("Microsoft.PowerShell");
			}
			base.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.EcsScriptMessageV, "\r\n\r\nfunction Test-WinRMQuickConfigNeeded\r\n{{\r\n# Checking the following items\r\n#1. Starting or restarting (if already started) the WinRM service\r\n#2. Setting the WinRM service startup type to Automatic\r\n#3. Creating a listener to accept requests on any IP address\r\n#4. Enabling Windows Firewall inbound rule exceptions for WS-Management traffic (for http only).\r\n\r\n    $winrmQuickConfigNeeded = $false\r\n\r\n    # check if WinRM service is running\r\n    if ((Get-Service winrm).Status -ne 'Running'){{\r\n        $winrmQuickConfigNeeded = $true\r\n    }}\r\n\r\n    # check if WinRM service startup is Auto\r\n    elseif ((Get-WmiObject -Query \"Select StartMode From Win32_Service Where Name='winmgmt'\").StartMode -ne 'Auto'){{\r\n        $winrmQuickConfigNeeded = $true\r\n    }}\r\n\r\n    # check if a winrm listener is present\r\n    elseif (!(Test-Path WSMan:\\localhost\\Listener) -or ((Get-ChildItem WSMan:\\localhost\\Listener) -eq $null)){{\r\n        $winrmQuickConfigNeeded = $true\r\n    }}\r\n\r\n    # check if WinRM firewall is enabled for HTTP\r\n    else{{\r\n        if (Get-Command Get-NetFirewallRule -ErrorAction SilentlyContinue){{\r\n            $winrmFirewall = Get-NetFirewallRule -Name 'WINRM-HTTP-In-TCP' -ErrorAction SilentlyContinue\r\n            if (!$winrmFirewall -or $winrmFirewall.Enabled -ne $true){{            \r\n                $winrmQuickConfigNeeded = $true\r\n            }}\r\n        }}\r\n        else{{\r\n            $winrmQuickConfigNeeded = $true\r\n        }}\r\n    }}\r\n\r\n    $winrmQuickConfigNeeded\r\n}}\r\n\r\nfunction Enable-PSSessionConfiguration\r\n{{\r\n[CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Medium\")]\r\nparam(\r\n    [Parameter(Position=0, ValueFromPipeline=$true)]\r\n    [System.String]\r\n    $Name,\r\n\r\n    [Parameter()]\r\n    [bool]\r\n    $Force,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $sddl,\r\n    \r\n    [Parameter()]\r\n    [bool]\r\n    $isSDDLSpecified,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $queryForSet,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $captionForSet,\r\n        \r\n    [Parameter()]\r\n    [string]\r\n    $queryForQC,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $captionForQC,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $shouldProcessDescForQC,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $setEnabledTarget,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $setEnabledAction,\r\n\r\n    [Parameter()]\r\n    [bool]\r\n    $skipNetworkProfileCheck,\r\n\t\r\n    [Parameter()]\r\n    [bool]\r\n    $noServiceRestart\r\n    )\r\n     \r\n    begin\r\n    {{\r\n        $winrmQuickConfigNeeded = Test-WinRMQuickConfigNeeded\r\n\r\n        if ($winrmQuickConfigNeeded -and ($force -or $pscmdlet.ShouldProcess($shouldProcessDescForQC, $queryForQC, $captionForQC)))\r\n        {{\r\n            # get the status of winrm before Quick Config. if it is already\r\n            # running..restart the service after Quick Config.\r\n            $svc = get-service winrm\r\n            if ($skipNetworkProfileCheck)\r\n            {{\r\n                {0} -force -SkipNetworkProfileCheck\r\n            }}\r\n            else\r\n            {{\r\n                {0} -force\r\n            }}\r\n            if ($svc.Status -match \"Running\")\r\n            {{\r\n               Restart-Service winrm -force -confirm:$false\r\n            }}\r\n        }}\r\n    }} #end of Begin block   \r\n        \r\n    process\r\n    {{\r\n       Get-PSSessionConfiguration $name -Force:$Force | % {{\r\n\r\n          if ($_.Enabled -eq $false -and ($force -or $pscmdlet.ShouldProcess($setEnabledTarget, $setEnabledAction)))\r\n          {{\r\n             Set-Item -WarningAction SilentlyContinue -Path \"WSMan:\\localhost\\Plugin\\$name\\Enabled\" -Value $true -confirm:$false\r\n          }}\r\n\r\n          if (!$isSDDLSpecified)\r\n          {{\r\n             $sddlTemp = $null\r\n             if ($_.psobject.members[\"SecurityDescriptorSddl\"])\r\n             {{\r\n                 $sddlTemp = $_.psobject.members[\"SecurityDescriptorSddl\"].Value\r\n             }}\r\n\r\n             $securityIdentifierToPurge = $null\r\n             # strip out Disable-Everyone DACL from the SDDL\r\n             if ($sddlTemp)\r\n             {{\r\n                # construct SID for \"EveryOne\"\r\n                [system.security.principal.wellknownsidtype]$evst = \"worldsid\"\r\n                $everyOneSID = new-object system.security.principal.securityidentifier $evst,$null\r\n                                \r\n                $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$sddlTemp                \r\n                $sd.DiscretionaryAcl | % {{\r\n                    if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $everyOneSID))\r\n                    {{\r\n                       $securityIdentifierToPurge = $_.securityidentifier\r\n                    }}\r\n                }}\r\n             \r\n                if ($securityIdentifierToPurge)\r\n                {{\r\n                   $sd.discretionaryacl.purge($securityIdentifierToPurge)\r\n\r\n                   # if there is no discretionaryacl..add Builtin Administrators and Remote Management Users\r\n                   # to the DACL group as this is the default WSMan behavior\r\n                   if ($sd.discretionaryacl.count -eq 0)\r\n                   {{\r\n                      # Built-in administrators\r\n                      [system.security.principal.wellknownsidtype]$bast = \"BuiltinAdministratorsSid\"\r\n                      $basid = new-object system.security.principal.securityidentifier $bast,$null\r\n                      $sd.DiscretionaryAcl.AddAccess('Allow',$basid, 268435456, 'none', 'none')\r\n\r\n                      # Remote Management Users, Win8+ only\r\n                      if ([System.Environment]::OSVersion.Version -ge \"6.2.0.0\")\r\n                      {{\r\n                          $rmSidId = new-object system.security.principal.securityidentifier \"{1}\"\r\n                          $sd.DiscretionaryAcl.AddAccess('Allow', $rmSidId, 268435456, 'none', 'none')\r\n                      }}\r\n\r\n                      # Interactive Users\r\n                      $iuSidId = new-object system.security.principal.securityidentifier \"{2}\"\r\n                      $sd.DiscretionaryAcl.AddAccess('Allow', $iuSidId, 268435456, 'none', 'none')\r\n                   }}\r\n\r\n                   $sddl = $sd.GetSddlForm(\"all\")\r\n                }}\r\n             }} # if ($sddlTemp)\r\n          }} # if (!$isSDDLSpecified) \r\n          \r\n          $qMessage = $queryForSet -f $_.name,$sddl\r\n          if (($sddl -or $isSDDLSpecified) -and ($force -or $pscmdlet.ShouldProcess($qMessage, $captionForSet)))\r\n          {{\r\n              $null = Set-PSSessionConfiguration -Name $_.Name -SecurityDescriptorSddl $sddl -NoServiceRestart -force -WarningAction 0\r\n          }}\r\n       }} #end of Get-PSSessionConfiguration | foreach\r\n    }} # end of Process block\r\n\r\n    End\r\n    {{\r\n    }}\r\n}}\r\n\r\n$_ | Enable-PSSessionConfiguration -force $args[0] -sddl $args[1] -isSDDLSpecified $args[2] -queryForSet $args[3] -captionForSet $args[4] -queryForQC $args[5] -captionForQC $args[6] -whatif:$args[7] -confirm:$args[8] -shouldProcessDescForQC $args[9] -setEnabledTarget $args[10] -setEnabledAction $args[11] -skipNetworkProfileCheck $args[12] -noServiceRestart $args[13] \r\n"));
			bool flag = false;
			bool flag2 = true;
			PSSessionConfigurationCommandUtilities.CollectShouldProcessParameters(this, out flag, out flag2);
			string text = StringUtil.Format(RemotingErrorIdStrings.EcsWSManQCCaption, new object[0]);
			string text2 = StringUtil.Format(RemotingErrorIdStrings.EcsWSManQCQuery, "Set-WSManQuickConfig");
			string text3 = StringUtil.Format(RemotingErrorIdStrings.EcsWSManShouldProcessDesc, "Set-WSManQuickConfig");
			string text4 = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessAction, "Set-PSSessionConfiguration");
			string ecsShouldProcessTarget = RemotingErrorIdStrings.EcsShouldProcessTarget;
			string text5 = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessAction, "Set-Item");
			string setEnabledTrueTarget = RemotingErrorIdStrings.SetEnabledTrueTarget;
			EnablePSSessionConfigurationCommand.enablePluginSb.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, this.shellsToEnable, new object[0], AutomationNull.Value, new object[]
			{
				this.force,
				this.sddl,
				this.isSddlSpecified,
				ecsShouldProcessTarget,
				text4,
				text2,
				text,
				flag,
				flag2,
				text3,
				setEnabledTrueTarget,
				text5,
				this.skipNetworkProfileCheck,
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
			tracer.EndpointEnabled(stringBuilder.ToString(), WindowsIdentity.GetCurrent().Name);
		}

		// Token: 0x040012CF RID: 4815
		private const string setWSManConfigCommand = "Set-WSManQuickConfig";

		// Token: 0x040012D0 RID: 4816
		private const string enablePluginSbFormat = "\r\n\r\nfunction Test-WinRMQuickConfigNeeded\r\n{{\r\n# Checking the following items\r\n#1. Starting or restarting (if already started) the WinRM service\r\n#2. Setting the WinRM service startup type to Automatic\r\n#3. Creating a listener to accept requests on any IP address\r\n#4. Enabling Windows Firewall inbound rule exceptions for WS-Management traffic (for http only).\r\n\r\n    $winrmQuickConfigNeeded = $false\r\n\r\n    # check if WinRM service is running\r\n    if ((Get-Service winrm).Status -ne 'Running'){{\r\n        $winrmQuickConfigNeeded = $true\r\n    }}\r\n\r\n    # check if WinRM service startup is Auto\r\n    elseif ((Get-WmiObject -Query \"Select StartMode From Win32_Service Where Name='winmgmt'\").StartMode -ne 'Auto'){{\r\n        $winrmQuickConfigNeeded = $true\r\n    }}\r\n\r\n    # check if a winrm listener is present\r\n    elseif (!(Test-Path WSMan:\\localhost\\Listener) -or ((Get-ChildItem WSMan:\\localhost\\Listener) -eq $null)){{\r\n        $winrmQuickConfigNeeded = $true\r\n    }}\r\n\r\n    # check if WinRM firewall is enabled for HTTP\r\n    else{{\r\n        if (Get-Command Get-NetFirewallRule -ErrorAction SilentlyContinue){{\r\n            $winrmFirewall = Get-NetFirewallRule -Name 'WINRM-HTTP-In-TCP' -ErrorAction SilentlyContinue\r\n            if (!$winrmFirewall -or $winrmFirewall.Enabled -ne $true){{            \r\n                $winrmQuickConfigNeeded = $true\r\n            }}\r\n        }}\r\n        else{{\r\n            $winrmQuickConfigNeeded = $true\r\n        }}\r\n    }}\r\n\r\n    $winrmQuickConfigNeeded\r\n}}\r\n\r\nfunction Enable-PSSessionConfiguration\r\n{{\r\n[CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Medium\")]\r\nparam(\r\n    [Parameter(Position=0, ValueFromPipeline=$true)]\r\n    [System.String]\r\n    $Name,\r\n\r\n    [Parameter()]\r\n    [bool]\r\n    $Force,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $sddl,\r\n    \r\n    [Parameter()]\r\n    [bool]\r\n    $isSDDLSpecified,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $queryForSet,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $captionForSet,\r\n        \r\n    [Parameter()]\r\n    [string]\r\n    $queryForQC,\r\n    \r\n    [Parameter()]\r\n    [string]\r\n    $captionForQC,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $shouldProcessDescForQC,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $setEnabledTarget,\r\n\r\n    [Parameter()]\r\n    [string]\r\n    $setEnabledAction,\r\n\r\n    [Parameter()]\r\n    [bool]\r\n    $skipNetworkProfileCheck,\r\n\t\r\n    [Parameter()]\r\n    [bool]\r\n    $noServiceRestart\r\n    )\r\n     \r\n    begin\r\n    {{\r\n        $winrmQuickConfigNeeded = Test-WinRMQuickConfigNeeded\r\n\r\n        if ($winrmQuickConfigNeeded -and ($force -or $pscmdlet.ShouldProcess($shouldProcessDescForQC, $queryForQC, $captionForQC)))\r\n        {{\r\n            # get the status of winrm before Quick Config. if it is already\r\n            # running..restart the service after Quick Config.\r\n            $svc = get-service winrm\r\n            if ($skipNetworkProfileCheck)\r\n            {{\r\n                {0} -force -SkipNetworkProfileCheck\r\n            }}\r\n            else\r\n            {{\r\n                {0} -force\r\n            }}\r\n            if ($svc.Status -match \"Running\")\r\n            {{\r\n               Restart-Service winrm -force -confirm:$false\r\n            }}\r\n        }}\r\n    }} #end of Begin block   \r\n        \r\n    process\r\n    {{\r\n       Get-PSSessionConfiguration $name -Force:$Force | % {{\r\n\r\n          if ($_.Enabled -eq $false -and ($force -or $pscmdlet.ShouldProcess($setEnabledTarget, $setEnabledAction)))\r\n          {{\r\n             Set-Item -WarningAction SilentlyContinue -Path \"WSMan:\\localhost\\Plugin\\$name\\Enabled\" -Value $true -confirm:$false\r\n          }}\r\n\r\n          if (!$isSDDLSpecified)\r\n          {{\r\n             $sddlTemp = $null\r\n             if ($_.psobject.members[\"SecurityDescriptorSddl\"])\r\n             {{\r\n                 $sddlTemp = $_.psobject.members[\"SecurityDescriptorSddl\"].Value\r\n             }}\r\n\r\n             $securityIdentifierToPurge = $null\r\n             # strip out Disable-Everyone DACL from the SDDL\r\n             if ($sddlTemp)\r\n             {{\r\n                # construct SID for \"EveryOne\"\r\n                [system.security.principal.wellknownsidtype]$evst = \"worldsid\"\r\n                $everyOneSID = new-object system.security.principal.securityidentifier $evst,$null\r\n                                \r\n                $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$sddlTemp                \r\n                $sd.DiscretionaryAcl | % {{\r\n                    if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $everyOneSID))\r\n                    {{\r\n                       $securityIdentifierToPurge = $_.securityidentifier\r\n                    }}\r\n                }}\r\n             \r\n                if ($securityIdentifierToPurge)\r\n                {{\r\n                   $sd.discretionaryacl.purge($securityIdentifierToPurge)\r\n\r\n                   # if there is no discretionaryacl..add Builtin Administrators and Remote Management Users\r\n                   # to the DACL group as this is the default WSMan behavior\r\n                   if ($sd.discretionaryacl.count -eq 0)\r\n                   {{\r\n                      # Built-in administrators\r\n                      [system.security.principal.wellknownsidtype]$bast = \"BuiltinAdministratorsSid\"\r\n                      $basid = new-object system.security.principal.securityidentifier $bast,$null\r\n                      $sd.DiscretionaryAcl.AddAccess('Allow',$basid, 268435456, 'none', 'none')\r\n\r\n                      # Remote Management Users, Win8+ only\r\n                      if ([System.Environment]::OSVersion.Version -ge \"6.2.0.0\")\r\n                      {{\r\n                          $rmSidId = new-object system.security.principal.securityidentifier \"{1}\"\r\n                          $sd.DiscretionaryAcl.AddAccess('Allow', $rmSidId, 268435456, 'none', 'none')\r\n                      }}\r\n\r\n                      # Interactive Users\r\n                      $iuSidId = new-object system.security.principal.securityidentifier \"{2}\"\r\n                      $sd.DiscretionaryAcl.AddAccess('Allow', $iuSidId, 268435456, 'none', 'none')\r\n                   }}\r\n\r\n                   $sddl = $sd.GetSddlForm(\"all\")\r\n                }}\r\n             }} # if ($sddlTemp)\r\n          }} # if (!$isSDDLSpecified) \r\n          \r\n          $qMessage = $queryForSet -f $_.name,$sddl\r\n          if (($sddl -or $isSDDLSpecified) -and ($force -or $pscmdlet.ShouldProcess($qMessage, $captionForSet)))\r\n          {{\r\n              $null = Set-PSSessionConfiguration -Name $_.Name -SecurityDescriptorSddl $sddl -NoServiceRestart -force -WarningAction 0\r\n          }}\r\n       }} #end of Get-PSSessionConfiguration | foreach\r\n    }} # end of Process block\r\n\r\n    End\r\n    {{\r\n    }}\r\n}}\r\n\r\n$_ | Enable-PSSessionConfiguration -force $args[0] -sddl $args[1] -isSDDLSpecified $args[2] -queryForSet $args[3] -captionForSet $args[4] -queryForQC $args[5] -captionForQC $args[6] -whatif:$args[7] -confirm:$args[8] -shouldProcessDescForQC $args[9] -setEnabledTarget $args[10] -setEnabledAction $args[11] -skipNetworkProfileCheck $args[12] -noServiceRestart $args[13] \r\n";

		// Token: 0x040012D1 RID: 4817
		private static ScriptBlock enablePluginSb;

		// Token: 0x040012D2 RID: 4818
		private string[] shellName;

		// Token: 0x040012D3 RID: 4819
		private Collection<string> shellsToEnable = new Collection<string>();

		// Token: 0x040012D4 RID: 4820
		private bool force;

		// Token: 0x040012D5 RID: 4821
		internal string sddl;

		// Token: 0x040012D6 RID: 4822
		internal bool isSddlSpecified;

		// Token: 0x040012D7 RID: 4823
		private bool skipNetworkProfileCheck;

		// Token: 0x040012D8 RID: 4824
		private bool noRestart;
	}
}
