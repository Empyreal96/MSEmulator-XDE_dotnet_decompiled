using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Remoting;
using System.Management.Automation.Runspaces;
using System.Management.Automation.Sqm;
using System.Management.Automation.Tracing;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000318 RID: 792
	[Cmdlet("Register", "PSSessionConfiguration", DefaultParameterSetName = "NameParameterSet", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=144306")]
	public sealed class RegisterPSSessionConfigurationCommand : PSSessionConfigurationCommandBase
	{
		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x060025E3 RID: 9699 RVA: 0x000D2A34 File Offset: 0x000D0C34
		// (set) Token: 0x060025E4 RID: 9700 RVA: 0x000D2A3C File Offset: 0x000D0C3C
		[Alias(new string[]
		{
			"PA"
		})]
		[ValidateSet(new string[]
		{
			"x86",
			"amd64"
		})]
		[ValidateNotNullOrEmpty]
		[Parameter]
		public string ProcessorArchitecture
		{
			get
			{
				return this.architecture;
			}
			set
			{
				this.architecture = value;
			}
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x060025E5 RID: 9701 RVA: 0x000D2A45 File Offset: 0x000D0C45
		// (set) Token: 0x060025E6 RID: 9702 RVA: 0x000D2A4D File Offset: 0x000D0C4D
		[Parameter(ParameterSetName = "NameParameterSet")]
		public PSSessionType SessionType
		{
			get
			{
				return this.sessionType;
			}
			set
			{
				this.sessionType = value;
			}
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x000D2A58 File Offset: 0x000D0C58
		static RegisterPSSessionConfigurationCommand()
		{
			string localSddl = PSSessionConfigurationCommandBase.GetLocalSddl();
			string script = string.Format(CultureInfo.InvariantCulture, "\r\nfunction Register-PSSessionConfiguration\r\n{{\r\n    [CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Medium\")]\r\n    param(  \r\n      [string] $filepath,\r\n      [string] $pluginName,\r\n      [bool] $shouldShowUI,\r\n      [bool] $force,\r\n      [string] $restartWSManTarget,\r\n      [string] $restartWSManAction,\r\n      [string] $restartWSManRequired,\r\n\t  [string] $runAsUserName,\r\n\t  [system.security.securestring] $runAsPassword,\r\n      [System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode] $accessMode,\r\n      [bool] $isSddlSpecified,\r\n      [string] $configPath\r\n    )\r\n\r\n    begin\r\n    {{\r\n        ## Construct SID for network users\r\n        [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n        $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n\r\n        ## If all session configurations have Network Access disabled,\r\n        ## then we create this endpoint as Local as well.\r\n        $newSDDL = $null\r\n        $foundRemoteEndpoint = $false;\r\n        Get-PSSessionConfiguration -Force:$force | Foreach-Object {{\r\n            if ($_.Enabled)\r\n            {{\r\n                $sddl = $null\r\n                if ($_.psobject.members[\"SecurityDescriptorSddl\"])\r\n                {{\r\n                    $sddl = $_.psobject.members[\"SecurityDescriptorSddl\"].Value\r\n                }}\r\n        \r\n                if($sddl)\r\n                {{\r\n                    # See if it has 'Disable Network Access'\r\n                    $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$sddl\r\n                    $disableNetworkExists = $false\r\n                    $sd.DiscretionaryAcl | % {{\r\n                        if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                        {{\r\n                            $disableNetworkExists = $true              \r\n                        }}\r\n                    }}\r\n\r\n                    if(-not $disableNetworkExists) {{ $foundRemoteEndpoint = $true }}\r\n                }}\r\n            }}\r\n        }}\r\n\r\n        if(-not $foundRemoteEndpoint)\r\n        {{\r\n            $newSDDL = \"{1}\"\r\n        }}\r\n    }}\r\n\r\n    process\r\n    {{\r\n        if ($force)\r\n        {{\r\n            if (Test-Path WSMan:\\localhost\\Plugin\\\"$pluginName\")\r\n            {{\r\n                Unregister-PSSessionConfiguration -name \"$pluginName\" -force\r\n            }}\r\n        }}\r\n\r\n        new-item -path WSMan:\\localhost\\Plugin -file \"$filepath\" -name \"$pluginName\"\r\n        # $? is to make sure the last operation is succeeded\r\n\r\n\t\tif ($? -and $runAsUserName) \r\n\t\t{{\r\n\t\t\ttry {{\r\n\t\t\t\t$runAsCredential = new-object system.management.automation.PSCredential($runAsUserName, $runAsPassword)\r\n\t\t\t\tset-item -WarningAction SilentlyContinue WSMan:\\localhost\\Plugin\\\"$pluginName\"\\RunAsUser $runAsCredential -confirm:$false\r\n\t\t\t}} catch {{\r\n\t\t\t\tremove-item WSMan:\\localhost\\Plugin\\\"$pluginName\" -recurse -force\r\n\t\t\t\twrite-error $_\r\n                # Do not add anymore clean up code after Write-Error, because if EA=Stop is set by user\r\n                # any code at this point will not execute.\r\n\r\n\t\t\t\treturn\r\n\t\t\t}}\r\n\t\t}}\r\n\r\n        ## Replace the SDDL with any groups defined in the PSSessionConfigurationFile, if any\r\n        if($? -and $configPath -and (-not $isSddlSpecified))\r\n        {{\r\n            $config = Import-PowerShellDataFile $configPath\r\n            if($config.RoleDefinitions.Keys)\r\n            {{\r\n                ## Create a CommonSecurityDescriptor object with a known good security descriptor\r\n                $curPlugin = Get-PSSessionConfiguration -Name $pluginName -Force:$force\r\n                $existingSddl = $curPlugin.SecurityDescriptorSddl\r\n                $arguments = $false,$false,$existingSddl\r\n                $mapper = New-Object Security.AccessControl.CommonSecurityDescriptor $arguments\r\n\r\n                ## Purge all existing access rules so that only role definition principals are \r\n                ## granted access.\r\n                $sidsToRemove = @()\r\n                $mapper.DiscretionaryAcl | % {{\r\n                    $sidsToRemove += $_.SecurityIdentifier\r\n                }}\r\n                foreach ($sidToRemove in $sidsToRemove)\r\n                {{\r\n                    $mapper.PurgeAccessControl($sidToRemove)\r\n                }} \r\n\r\n                foreach ($principal in @($config.RoleDefinitions.Keys))\r\n                {{\r\n                    try\r\n                    {{\r\n                        ## Get the SID for the principal\r\n                        $account = New-Object Security.Principal.NTAccount $principal\r\n                        $sid = $account.Translate([Security.Principal.SecurityIdentifier]).Value\r\n\r\n                        ## Create a new access rule that adds the principal to the endpoint\r\n                        ## 268435456 - GenericAll\r\n                        $mapper.DiscretionaryAcl.AddAccess('Allow', $sid, 268435456, 'None', 'None')\r\n                    }}\r\n                    catch\r\n                    {{\r\n                        $translationError = $_.Exception.InnerException.Message\r\n                        $errorMessage = [Microsoft.PowerShell.Commands.Internal.RemotingErrorResources]::CouldNotResolveRoleDefinitionPrincipal -f $principal, $translationError\r\n                        Write-Error $errorMessage\r\n                    }}\r\n                }}\r\n\r\n                ## Get the new SDDL for that configuration\r\n                if ($mapper.DiscretionaryAcl.Count -gt 0)\r\n                {{\r\n                    $configSDDL = $mapper.GetSddlForm('All')\r\n                    $null = Set-PSSessionConfiguration -Name $pluginName -SecurityDescriptorSddl $configSDDL -Force:$force\r\n                }}\r\n            }}\r\n        }}\r\n\r\n        if ($? -and $shouldShowUI)\r\n        {{\r\n           $null = winrm configsddl \"{0}$pluginName\"\r\n\r\n           # if AccessMode is Disabled OR the winrm configsddl failed, we just return\r\n           if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Disabled.Equals($accessMode) -or !$?)\r\n           {{\r\n               return\r\n           }}\r\n        }} # end of if ($shouldShowUI)\r\n\r\n        if ($?)\r\n        {{\r\n           # if AccessMode is Local or Remote, we need to check the SDDL the user set in the UI or passed in to the cmdlet.\r\n           $newSDDL = $null\r\n           $curPlugin = Get-PSSessionConfiguration -Name $pluginName -Force:$force\r\n           $curSDDL = $curPlugin.SecurityDescriptorSddl\r\n           if (!$curSDDL)\r\n           {{\r\n               if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Local.Equals($accessMode))\r\n               {{\r\n                    $newSDDL = \"{1}\"\r\n               }}\r\n           }}\r\n           else\r\n           {{\r\n               # Construct SID for network users\r\n               [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n               $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n                \r\n               $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$curSDDL\r\n               $haveDisableACE = $false\r\n               $securityIdentifierToPurge = $null\r\n               $sd.DiscretionaryAcl | % {{\r\n                    if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                    {{\r\n                        $haveDisableACE = $true\r\n                        $securityIdentifierToPurge = $_.securityidentifier\r\n                    }}\r\n               }}\r\n               if (([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Local.Equals($accessMode) -or\r\n                    ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Remote.Equals($accessMode)-and $disableNetworkExists)) -and\r\n                   !$haveDisableACE)\r\n               {{\r\n                    # Add network deny ACE for local access or remote access with PSRemoting disabled ($disableNetworkExists)\r\n                    $sd.DiscretionaryAcl.AddAccess(\"deny\", $networkSID, 268435456, \"None\", \"None\")\r\n                    $newSDDL = $sd.GetSddlForm(\"all\")\r\n               }}\r\n               if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Remote.Equals($accessMode) -and -not $disableNetworkExists -and $haveDisableACE)\r\n               {{\r\n                    # Remove the specific ACE\r\n                    $sd.discretionaryacl.RemoveAccessSpecific('Deny', $securityIdentifierToPurge, 268435456, 'none', 'none')\r\n\r\n                    # if there is no discretionaryacl..add Builtin Administrators and Remote Management Users\r\n                    # to the DACL group as this is the default WSMan behavior\r\n                    if ($sd.discretionaryacl.count -eq 0)\r\n                    {{\r\n                        [system.security.principal.wellknownsidtype]$bast = \"BuiltinAdministratorsSid\"\r\n                        $basid = new-object system.security.principal.securityidentifier $bast,$null\r\n                        $sd.DiscretionaryAcl.AddAccess('Allow',$basid, 268435456, 'none', 'none')\r\n\r\n                        # Remote Management Users, Win8+ only\r\n                        if ([System.Environment]::OSVersion.Version -ge \"6.2.0.0\")\r\n                        {{\r\n                            $rmSidId = new-object system.security.principal.securityidentifier \"{2}\"\r\n                            $sd.DiscretionaryAcl.AddAccess('Allow', $rmSidId, 268435456, 'none', 'none')\r\n                        }}\r\n\r\n                        # Interactive Users\r\n                        $iaSidId = new-object system.security.principal.securityidentifier \"{3}\"\r\n                        $sd.DiscretionaryAcl.AddAccess('Allow', $iaSidId, 268435456, 'none', 'none')\r\n                    }}\r\n\r\n                    $newSDDL = $sd.GetSddlForm(\"all\")\r\n               }}\r\n           }} # end of if(!$curSDDL)\r\n        }} # end of if ($?)\r\n\r\n        if ($? -and $newSDDL)\r\n        {{\r\n            try {{\r\n                if ($runAsUserName)\r\n                {{\r\n                    $runAsCredential = new-object system.management.automation.PSCredential($runAsUserName, $runAsPassword)\r\n                    $null = Set-PSSessionConfiguration -Name $pluginName -SecurityDescriptorSddl $newSDDL -NoServiceRestart -force -WarningAction 0 -RunAsCredential $runAsCredential\r\n                }}\r\n                else\r\n                {{\r\n                    $null = Set-PSSessionConfiguration -Name $pluginName -SecurityDescriptorSddl $newSDDL -NoServiceRestart -force -WarningAction 0\r\n                }}\r\n\r\n            }} catch {{\r\n\t\t\t\tremove-item WSMan:\\localhost\\Plugin\\\"$pluginName\" -recurse -force\r\n\t\t\t\twrite-error $_\r\n                # Do not add anymore clean up code after Write-Error, because if EA=Stop is set by user\r\n                # any code at this point will not execute.\r\n\r\n\t\t\t\treturn\r\n\t\t\t}}\r\n        }}\r\n\r\n        if ($?){{\r\n            try{{\r\n                $s = New-PSSession -ComputerName localhost -ConfigurationName $pluginName -ErrorAction Stop\r\n                # session is ok, no need to restart WinRM service                \r\n                Remove-PSSession $s -Confirm:$false\r\n            }}catch{{\r\n                # session is NOT ok, we need to restart winrm if -Force was specified, otherwise show a warning\r\n                if ($force){{\r\n                    Restart-Service -Name WinRM -Force -Confirm:$false\r\n                }}else{{\r\n                    $warningWSManRestart = [Microsoft.PowerShell.Commands.Internal.RemotingErrorResources]::WinRMRestartWarning -f $PSCmdlet.MyInvocation.MyCommand.Name\r\n                    Write-Warning $warningWSManRestart\r\n                }}\r\n            }}\r\n        }}\r\n    }}\r\n}}\r\n\r\nif ($args[14] -eq $null)\r\n{{\r\n    Register-PSSessionConfiguration -filepath $args[0] -pluginName $args[1] -shouldShowUI $args[2] -force $args[3] -whatif:$args[4] -confirm:$args[5] -restartWSManTarget $args[6] -restartWSManAction $args[7] -restartWSManRequired $args[8] -runAsUserName $args[9] -runAsPassword $args[10] -accessMode $args[11] -isSddlSpecified $args[12] -configPath $args[13]\r\n}}\r\nelse\r\n{{\r\n    Register-PSSessionConfiguration -filepath $args[0] -pluginName $args[1] -shouldShowUI $args[2] -force $args[3] -whatif:$args[4] -confirm:$args[5] -restartWSManTarget $args[6] -restartWSManAction $args[7] -restartWSManRequired $args[8] -runAsUserName $args[9] -runAsPassword $args[10] -accessMode $args[11] -isSddlSpecified $args[12] -configPath $args[13] -erroraction $args[14]\r\n}}\r\n", new object[]
			{
				"http://schemas.microsoft.com/powershell/",
				localSddl,
				"S-1-5-32-580",
				"S-1-5-4"
			});
			RegisterPSSessionConfigurationCommand.newPluginSb = ScriptBlock.Create(script);
			RegisterPSSessionConfigurationCommand.newPluginSb.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x000D2ABC File Offset: 0x000D0CBC
		protected override void BeginProcessing()
		{
			if (this.isSddlSpecified && this.showUISpecified)
			{
				string message = StringUtil.Format(RemotingErrorIdStrings.ShowUIAndSDDLCannotExist, "SecurityDescriptorSddl", "ShowSecurityDescriptorUI");
				throw new PSInvalidOperationException(message);
			}
			if (this.isRunAsCredentialSpecified)
			{
				base.WriteWarning(RemotingErrorIdStrings.RunAsSessionConfigurationSecurityWarning);
			}
			if (this.isSddlSpecified)
			{
				CommonSecurityDescriptor commonSecurityDescriptor = new CommonSecurityDescriptor(false, false, this.sddl);
				SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.NetworkSid, null);
				bool flag = false;
				foreach (GenericAce genericAce in commonSecurityDescriptor.DiscretionaryAcl)
				{
					CommonAce commonAce = (CommonAce)genericAce;
					if (commonAce.AceQualifier.Equals(AceQualifier.AccessDenied) && commonAce.SecurityIdentifier.Equals(sid) && commonAce.AccessMask == 268435456)
					{
						flag = true;
						break;
					}
				}
				switch (base.AccessMode)
				{
				case PSSessionConfigurationAccessMode.Local:
					if (!flag)
					{
						commonSecurityDescriptor.DiscretionaryAcl.AddAccess(AccessControlType.Deny, sid, 268435456, InheritanceFlags.None, PropagationFlags.None);
						this.sddl = commonSecurityDescriptor.GetSddlForm(AccessControlSections.All);
					}
					break;
				case PSSessionConfigurationAccessMode.Remote:
					if (flag)
					{
						commonSecurityDescriptor.DiscretionaryAcl.RemoveAccessSpecific(AccessControlType.Deny, sid, 268435456, InheritanceFlags.None, PropagationFlags.None);
						if (commonSecurityDescriptor.DiscretionaryAcl.Count == 0)
						{
							SecurityIdentifier sid2 = new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null);
							commonSecurityDescriptor.DiscretionaryAcl.AddAccess(AccessControlType.Allow, sid2, 268435456, InheritanceFlags.None, PropagationFlags.None);
							if (Environment.OSVersion.Version >= new Version(6, 2))
							{
								SecurityIdentifier sid3 = new SecurityIdentifier("S-1-5-32-580");
								commonSecurityDescriptor.DiscretionaryAcl.AddAccess(AccessControlType.Allow, sid3, 268435456, InheritanceFlags.None, PropagationFlags.None);
							}
							SecurityIdentifier sid4 = new SecurityIdentifier("S-1-5-4");
							commonSecurityDescriptor.DiscretionaryAcl.AddAccess(AccessControlType.Allow, sid4, 268435456, InheritanceFlags.None, PropagationFlags.None);
						}
						this.sddl = commonSecurityDescriptor.GetSddlForm(AccessControlSections.All);
					}
					break;
				}
			}
			if (!this.isSddlSpecified && !this.showUISpecified)
			{
				if (base.AccessMode.Equals(PSSessionConfigurationAccessMode.Local))
				{
					this.sddl = PSSessionConfigurationCommandBase.GetLocalSddl();
				}
				else if (base.AccessMode.Equals(PSSessionConfigurationAccessMode.Remote))
				{
					this.sddl = PSSessionConfigurationCommandBase.GetRemoteSddl();
				}
			}
			RemotingCommandUtil.CheckRemotingCmdletPrerequisites();
			PSSessionConfigurationCommandUtilities.ThrowIfNotAdministrator();
			WSManConfigurationOption wsmanConfigurationOption = this.transportOption as WSManConfigurationOption;
			if (wsmanConfigurationOption != null && wsmanConfigurationOption.ProcessIdleTimeoutSec != null && !this.isUseSharedProcessSpecified)
			{
				PSInvalidOperationException ex = new PSInvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.InvalidConfigurationXMLAttribute, "ProcessIdleTimeoutSec", "UseSharedProcess"));
				base.ThrowTerminatingError(ex.ErrorRecord);
			}
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x000D2D3C File Offset: 0x000D0F3C
		protected override void ProcessRecord()
		{
			base.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.NcsScriptMessageV, "\r\nfunction Register-PSSessionConfiguration\r\n{{\r\n    [CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Medium\")]\r\n    param(  \r\n      [string] $filepath,\r\n      [string] $pluginName,\r\n      [bool] $shouldShowUI,\r\n      [bool] $force,\r\n      [string] $restartWSManTarget,\r\n      [string] $restartWSManAction,\r\n      [string] $restartWSManRequired,\r\n\t  [string] $runAsUserName,\r\n\t  [system.security.securestring] $runAsPassword,\r\n      [System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode] $accessMode,\r\n      [bool] $isSddlSpecified,\r\n      [string] $configPath\r\n    )\r\n\r\n    begin\r\n    {{\r\n        ## Construct SID for network users\r\n        [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n        $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n\r\n        ## If all session configurations have Network Access disabled,\r\n        ## then we create this endpoint as Local as well.\r\n        $newSDDL = $null\r\n        $foundRemoteEndpoint = $false;\r\n        Get-PSSessionConfiguration -Force:$force | Foreach-Object {{\r\n            if ($_.Enabled)\r\n            {{\r\n                $sddl = $null\r\n                if ($_.psobject.members[\"SecurityDescriptorSddl\"])\r\n                {{\r\n                    $sddl = $_.psobject.members[\"SecurityDescriptorSddl\"].Value\r\n                }}\r\n        \r\n                if($sddl)\r\n                {{\r\n                    # See if it has 'Disable Network Access'\r\n                    $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$sddl\r\n                    $disableNetworkExists = $false\r\n                    $sd.DiscretionaryAcl | % {{\r\n                        if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                        {{\r\n                            $disableNetworkExists = $true              \r\n                        }}\r\n                    }}\r\n\r\n                    if(-not $disableNetworkExists) {{ $foundRemoteEndpoint = $true }}\r\n                }}\r\n            }}\r\n        }}\r\n\r\n        if(-not $foundRemoteEndpoint)\r\n        {{\r\n            $newSDDL = \"{1}\"\r\n        }}\r\n    }}\r\n\r\n    process\r\n    {{\r\n        if ($force)\r\n        {{\r\n            if (Test-Path WSMan:\\localhost\\Plugin\\\"$pluginName\")\r\n            {{\r\n                Unregister-PSSessionConfiguration -name \"$pluginName\" -force\r\n            }}\r\n        }}\r\n\r\n        new-item -path WSMan:\\localhost\\Plugin -file \"$filepath\" -name \"$pluginName\"\r\n        # $? is to make sure the last operation is succeeded\r\n\r\n\t\tif ($? -and $runAsUserName) \r\n\t\t{{\r\n\t\t\ttry {{\r\n\t\t\t\t$runAsCredential = new-object system.management.automation.PSCredential($runAsUserName, $runAsPassword)\r\n\t\t\t\tset-item -WarningAction SilentlyContinue WSMan:\\localhost\\Plugin\\\"$pluginName\"\\RunAsUser $runAsCredential -confirm:$false\r\n\t\t\t}} catch {{\r\n\t\t\t\tremove-item WSMan:\\localhost\\Plugin\\\"$pluginName\" -recurse -force\r\n\t\t\t\twrite-error $_\r\n                # Do not add anymore clean up code after Write-Error, because if EA=Stop is set by user\r\n                # any code at this point will not execute.\r\n\r\n\t\t\t\treturn\r\n\t\t\t}}\r\n\t\t}}\r\n\r\n        ## Replace the SDDL with any groups defined in the PSSessionConfigurationFile, if any\r\n        if($? -and $configPath -and (-not $isSddlSpecified))\r\n        {{\r\n            $config = Import-PowerShellDataFile $configPath\r\n            if($config.RoleDefinitions.Keys)\r\n            {{\r\n                ## Create a CommonSecurityDescriptor object with a known good security descriptor\r\n                $curPlugin = Get-PSSessionConfiguration -Name $pluginName -Force:$force\r\n                $existingSddl = $curPlugin.SecurityDescriptorSddl\r\n                $arguments = $false,$false,$existingSddl\r\n                $mapper = New-Object Security.AccessControl.CommonSecurityDescriptor $arguments\r\n\r\n                ## Purge all existing access rules so that only role definition principals are \r\n                ## granted access.\r\n                $sidsToRemove = @()\r\n                $mapper.DiscretionaryAcl | % {{\r\n                    $sidsToRemove += $_.SecurityIdentifier\r\n                }}\r\n                foreach ($sidToRemove in $sidsToRemove)\r\n                {{\r\n                    $mapper.PurgeAccessControl($sidToRemove)\r\n                }} \r\n\r\n                foreach ($principal in @($config.RoleDefinitions.Keys))\r\n                {{\r\n                    try\r\n                    {{\r\n                        ## Get the SID for the principal\r\n                        $account = New-Object Security.Principal.NTAccount $principal\r\n                        $sid = $account.Translate([Security.Principal.SecurityIdentifier]).Value\r\n\r\n                        ## Create a new access rule that adds the principal to the endpoint\r\n                        ## 268435456 - GenericAll\r\n                        $mapper.DiscretionaryAcl.AddAccess('Allow', $sid, 268435456, 'None', 'None')\r\n                    }}\r\n                    catch\r\n                    {{\r\n                        $translationError = $_.Exception.InnerException.Message\r\n                        $errorMessage = [Microsoft.PowerShell.Commands.Internal.RemotingErrorResources]::CouldNotResolveRoleDefinitionPrincipal -f $principal, $translationError\r\n                        Write-Error $errorMessage\r\n                    }}\r\n                }}\r\n\r\n                ## Get the new SDDL for that configuration\r\n                if ($mapper.DiscretionaryAcl.Count -gt 0)\r\n                {{\r\n                    $configSDDL = $mapper.GetSddlForm('All')\r\n                    $null = Set-PSSessionConfiguration -Name $pluginName -SecurityDescriptorSddl $configSDDL -Force:$force\r\n                }}\r\n            }}\r\n        }}\r\n\r\n        if ($? -and $shouldShowUI)\r\n        {{\r\n           $null = winrm configsddl \"{0}$pluginName\"\r\n\r\n           # if AccessMode is Disabled OR the winrm configsddl failed, we just return\r\n           if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Disabled.Equals($accessMode) -or !$?)\r\n           {{\r\n               return\r\n           }}\r\n        }} # end of if ($shouldShowUI)\r\n\r\n        if ($?)\r\n        {{\r\n           # if AccessMode is Local or Remote, we need to check the SDDL the user set in the UI or passed in to the cmdlet.\r\n           $newSDDL = $null\r\n           $curPlugin = Get-PSSessionConfiguration -Name $pluginName -Force:$force\r\n           $curSDDL = $curPlugin.SecurityDescriptorSddl\r\n           if (!$curSDDL)\r\n           {{\r\n               if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Local.Equals($accessMode))\r\n               {{\r\n                    $newSDDL = \"{1}\"\r\n               }}\r\n           }}\r\n           else\r\n           {{\r\n               # Construct SID for network users\r\n               [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n               $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n                \r\n               $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$curSDDL\r\n               $haveDisableACE = $false\r\n               $securityIdentifierToPurge = $null\r\n               $sd.DiscretionaryAcl | % {{\r\n                    if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                    {{\r\n                        $haveDisableACE = $true\r\n                        $securityIdentifierToPurge = $_.securityidentifier\r\n                    }}\r\n               }}\r\n               if (([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Local.Equals($accessMode) -or\r\n                    ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Remote.Equals($accessMode)-and $disableNetworkExists)) -and\r\n                   !$haveDisableACE)\r\n               {{\r\n                    # Add network deny ACE for local access or remote access with PSRemoting disabled ($disableNetworkExists)\r\n                    $sd.DiscretionaryAcl.AddAccess(\"deny\", $networkSID, 268435456, \"None\", \"None\")\r\n                    $newSDDL = $sd.GetSddlForm(\"all\")\r\n               }}\r\n               if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Remote.Equals($accessMode) -and -not $disableNetworkExists -and $haveDisableACE)\r\n               {{\r\n                    # Remove the specific ACE\r\n                    $sd.discretionaryacl.RemoveAccessSpecific('Deny', $securityIdentifierToPurge, 268435456, 'none', 'none')\r\n\r\n                    # if there is no discretionaryacl..add Builtin Administrators and Remote Management Users\r\n                    # to the DACL group as this is the default WSMan behavior\r\n                    if ($sd.discretionaryacl.count -eq 0)\r\n                    {{\r\n                        [system.security.principal.wellknownsidtype]$bast = \"BuiltinAdministratorsSid\"\r\n                        $basid = new-object system.security.principal.securityidentifier $bast,$null\r\n                        $sd.DiscretionaryAcl.AddAccess('Allow',$basid, 268435456, 'none', 'none')\r\n\r\n                        # Remote Management Users, Win8+ only\r\n                        if ([System.Environment]::OSVersion.Version -ge \"6.2.0.0\")\r\n                        {{\r\n                            $rmSidId = new-object system.security.principal.securityidentifier \"{2}\"\r\n                            $sd.DiscretionaryAcl.AddAccess('Allow', $rmSidId, 268435456, 'none', 'none')\r\n                        }}\r\n\r\n                        # Interactive Users\r\n                        $iaSidId = new-object system.security.principal.securityidentifier \"{3}\"\r\n                        $sd.DiscretionaryAcl.AddAccess('Allow', $iaSidId, 268435456, 'none', 'none')\r\n                    }}\r\n\r\n                    $newSDDL = $sd.GetSddlForm(\"all\")\r\n               }}\r\n           }} # end of if(!$curSDDL)\r\n        }} # end of if ($?)\r\n\r\n        if ($? -and $newSDDL)\r\n        {{\r\n            try {{\r\n                if ($runAsUserName)\r\n                {{\r\n                    $runAsCredential = new-object system.management.automation.PSCredential($runAsUserName, $runAsPassword)\r\n                    $null = Set-PSSessionConfiguration -Name $pluginName -SecurityDescriptorSddl $newSDDL -NoServiceRestart -force -WarningAction 0 -RunAsCredential $runAsCredential\r\n                }}\r\n                else\r\n                {{\r\n                    $null = Set-PSSessionConfiguration -Name $pluginName -SecurityDescriptorSddl $newSDDL -NoServiceRestart -force -WarningAction 0\r\n                }}\r\n\r\n            }} catch {{\r\n\t\t\t\tremove-item WSMan:\\localhost\\Plugin\\\"$pluginName\" -recurse -force\r\n\t\t\t\twrite-error $_\r\n                # Do not add anymore clean up code after Write-Error, because if EA=Stop is set by user\r\n                # any code at this point will not execute.\r\n\r\n\t\t\t\treturn\r\n\t\t\t}}\r\n        }}\r\n\r\n        if ($?){{\r\n            try{{\r\n                $s = New-PSSession -ComputerName localhost -ConfigurationName $pluginName -ErrorAction Stop\r\n                # session is ok, no need to restart WinRM service                \r\n                Remove-PSSession $s -Confirm:$false\r\n            }}catch{{\r\n                # session is NOT ok, we need to restart winrm if -Force was specified, otherwise show a warning\r\n                if ($force){{\r\n                    Restart-Service -Name WinRM -Force -Confirm:$false\r\n                }}else{{\r\n                    $warningWSManRestart = [Microsoft.PowerShell.Commands.Internal.RemotingErrorResources]::WinRMRestartWarning -f $PSCmdlet.MyInvocation.MyCommand.Name\r\n                    Write-Warning $warningWSManRestart\r\n                }}\r\n            }}\r\n        }}\r\n    }}\r\n}}\r\n\r\nif ($args[14] -eq $null)\r\n{{\r\n    Register-PSSessionConfiguration -filepath $args[0] -pluginName $args[1] -shouldShowUI $args[2] -force $args[3] -whatif:$args[4] -confirm:$args[5] -restartWSManTarget $args[6] -restartWSManAction $args[7] -restartWSManRequired $args[8] -runAsUserName $args[9] -runAsPassword $args[10] -accessMode $args[11] -isSddlSpecified $args[12] -configPath $args[13]\r\n}}\r\nelse\r\n{{\r\n    Register-PSSessionConfiguration -filepath $args[0] -pluginName $args[1] -shouldShowUI $args[2] -force $args[3] -whatif:$args[4] -confirm:$args[5] -restartWSManTarget $args[6] -restartWSManAction $args[7] -restartWSManRequired $args[8] -runAsUserName $args[9] -runAsPassword $args[10] -accessMode $args[11] -isSddlSpecified $args[12] -configPath $args[13] -erroraction $args[14]\r\n}}\r\n"));
			if (!this.force)
			{
				string action = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessAction, base.CommandInfo.Name);
				string target;
				if (this.isSddlSpecified)
				{
					target = StringUtil.Format(RemotingErrorIdStrings.NcsShouldProcessTargetSDDL, base.Name, this.sddl);
				}
				else
				{
					target = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessTargetAdminEnable, base.Name);
				}
				string o = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessAction, base.CommandInfo.Name);
				base.WriteWarning(StringUtil.Format(RemotingErrorIdStrings.WinRMRestartWarning, o));
				if (!base.ShouldProcess(target, action))
				{
					return;
				}
			}
			string text;
			string text2;
			string pluginContent = this.ConstructPluginContent(out text, out text2);
			string text3 = this.ConstructTemporaryFile(pluginContent);
			if (this.isRunAsCredentialSpecified || base.RunAsVirtualAccountSpecified)
			{
				PSSessionConfigurationCommandUtilities.MoveWinRmToIsolatedServiceHost(base.RunAsVirtualAccountSpecified);
			}
			try
			{
				string restartWSManServiceAction = RemotingErrorIdStrings.RestartWSManServiceAction;
				string text4 = StringUtil.Format(RemotingErrorIdStrings.RestartWSManServiceTarget, "WinRM");
				string text5 = StringUtil.Format(RemotingErrorIdStrings.RestartWSManRequiredShowUI, string.Format(CultureInfo.InvariantCulture, "Set-PSSessionConfiguration {0} -ShowSecurityDescriptorUI", new object[]
				{
					this.shellName
				}));
				bool flag = false;
				bool flag2 = true;
				PSSessionConfigurationCommandUtilities.CollectShouldProcessParameters(this, out flag, out flag2);
				object obj = null;
				if (base.Context.CurrentCommandProcessor.CommandRuntime.IsErrorActionSet)
				{
					obj = base.Context.CurrentCommandProcessor.CommandRuntime.ErrorAction;
				}
				ArrayList arrayList = (ArrayList)base.Context.DollarErrorVariable;
				int count = arrayList.Count;
				RegisterPSSessionConfigurationCommand.newPluginSb.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[]
				{
					text3,
					this.shellName,
					base.ShowSecurityDescriptorUI.ToBool(),
					this.force,
					flag,
					flag2,
					text4,
					restartWSManServiceAction,
					text5,
					(this.runAsCredential != null) ? this.runAsCredential.UserName : null,
					(this.runAsCredential != null) ? this.runAsCredential.Password : null,
					base.AccessMode,
					this.isSddlSpecified,
					base.Path,
					obj
				});
				arrayList = (ArrayList)base.Context.DollarErrorVariable;
				this.isErrorReported = (arrayList.Count > count);
			}
			finally
			{
				this.DeleteFile(text3);
			}
			if (text != null && text2 != null && !File.Exists(text2))
			{
				try
				{
					File.Copy(text, text2, true);
				}
				catch (IOException)
				{
				}
				catch (ArgumentException)
				{
				}
				catch (NotSupportedException)
				{
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x000D307C File Offset: 0x000D127C
		protected override void EndProcessing()
		{
			Tracer tracer = new Tracer();
			tracer.EndpointRegistered(base.Name, this.sessionType.ToString(), WindowsIdentity.GetCurrent().Name);
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x000D30B8 File Offset: 0x000D12B8
		private void DeleteFile(string tmpFileName)
		{
			Exception ex = null;
			try
			{
				File.Delete(tmpFileName);
			}
			catch (UnauthorizedAccessException ex2)
			{
				ex = ex2;
			}
			catch (ArgumentException ex3)
			{
				ex = ex3;
			}
			catch (PathTooLongException ex4)
			{
				ex = ex4;
			}
			catch (DirectoryNotFoundException ex5)
			{
				ex = ex5;
			}
			catch (IOException ex6)
			{
				ex = ex6;
			}
			catch (NotSupportedException ex7)
			{
				ex = ex7;
			}
			if (ex != null)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.NcsCannotDeleteFileAfterInstall, new object[]
				{
					tmpFileName,
					ex.Message
				});
			}
		}

		// Token: 0x060025EC RID: 9708 RVA: 0x000D3168 File Offset: 0x000D1368
		private string ConstructTemporaryFile(string pluginContent)
		{
			string text = System.IO.Path.Combine(System.IO.Path.GetTempPath(), System.IO.Path.GetRandomFileName()) + "psshell.xml";
			Exception ex = null;
			if (File.Exists(text))
			{
				FileInfo fileInfo = new FileInfo(text);
				if (fileInfo != null)
				{
					try
					{
						fileInfo.Attributes &= ~(FileAttributes.ReadOnly | FileAttributes.Hidden);
						fileInfo.Delete();
					}
					catch (FileNotFoundException ex2)
					{
						ex = ex2;
					}
					catch (DirectoryNotFoundException ex3)
					{
						ex = ex3;
					}
					catch (UnauthorizedAccessException ex4)
					{
						ex = ex4;
					}
					catch (SecurityException ex5)
					{
						ex = ex5;
					}
					catch (ArgumentNullException ex6)
					{
						ex = ex6;
					}
					catch (ArgumentException ex7)
					{
						ex = ex7;
					}
					catch (PathTooLongException ex8)
					{
						ex = ex8;
					}
					catch (NotSupportedException ex9)
					{
						ex = ex9;
					}
					catch (IOException ex10)
					{
						ex = ex10;
					}
					if (ex != null)
					{
						throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.NcsCannotDeleteFile, new object[]
						{
							text,
							ex.Message
						});
					}
				}
			}
			try
			{
				StreamWriter streamWriter = File.CreateText(text);
				streamWriter.Write(pluginContent);
				streamWriter.Flush();
				streamWriter.Dispose();
			}
			catch (UnauthorizedAccessException ex11)
			{
				ex = ex11;
			}
			catch (ArgumentException ex12)
			{
				ex = ex12;
			}
			catch (PathTooLongException ex13)
			{
				ex = ex13;
			}
			catch (DirectoryNotFoundException ex14)
			{
				ex = ex14;
			}
			if (ex != null)
			{
				throw PSTraceSource.NewInvalidOperationException(RemotingErrorIdStrings.NcsCannotWritePluginContent, new object[]
				{
					text,
					ex.Message
				});
			}
			return text;
		}

		// Token: 0x060025ED RID: 9709 RVA: 0x000D3328 File Offset: 0x000D1528
		private string ConstructPluginContent(out string srcConfigFilePath, out string destConfigFilePath)
		{
			srcConfigFilePath = null;
			destConfigFilePath = null;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			if (this.sessionType == PSSessionType.Workflow)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"sessiontype",
					this.sessionType,
					Environment.NewLine
				}));
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"assemblyname",
					"Microsoft.PowerShell.Workflow.ServiceCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL",
					Environment.NewLine
				}));
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"pssessionconfigurationtypename",
					"Microsoft.PowerShell.Workflow.PSWorkflowSessionConfiguration",
					Environment.NewLine
				}));
				flag = true;
			}
			if (base.Path != null)
			{
				ProviderInfo providerInfo = null;
				PSDriveInfo psdriveInfo;
				string unresolvedProviderPathFromPSPath = base.SessionState.Path.GetUnresolvedProviderPathFromPSPath(base.Path, out providerInfo, out psdriveInfo);
				if (!providerInfo.NameEquals(base.Context.ProviderNames.FileSystem) || !unresolvedProviderPathFromPSPath.EndsWith(".pssc", StringComparison.OrdinalIgnoreCase))
				{
					string message = StringUtil.Format(RemotingErrorIdStrings.InvalidPSSessionConfigurationFilePath, unresolvedProviderPathFromPSPath);
					InvalidOperationException exception = new InvalidOperationException(message);
					ErrorRecord errorRecord = new ErrorRecord(exception, "InvalidPSSessionConfigurationFilePath", ErrorCategory.InvalidArgument, base.Path);
					base.ThrowTerminatingError(errorRecord);
				}
				Guid guid = Guid.Empty;
				Hashtable hashtable = null;
				try
				{
					string text;
					ExternalScriptInfo scriptInfoForFile = DISCUtils.GetScriptInfoForFile(base.Context, unresolvedProviderPathFromPSPath, out text);
					hashtable = DISCUtils.LoadConfigFile(base.Context, scriptInfoForFile);
				}
				catch (RuntimeException ex)
				{
					string message2 = StringUtil.Format(RemotingErrorIdStrings.InvalidPSSessionConfigurationFileErrorProcessing, unresolvedProviderPathFromPSPath, ex.Message);
					InvalidOperationException exception2 = new InvalidOperationException(message2, ex);
					ErrorRecord errorRecord2 = new ErrorRecord(exception2, "InvalidPSSessionConfigurationFilePath", ErrorCategory.InvalidArgument, base.Path);
					base.ThrowTerminatingError(errorRecord2);
				}
				if (hashtable == null)
				{
					string message3 = StringUtil.Format(RemotingErrorIdStrings.InvalidPSSessionConfigurationFile, unresolvedProviderPathFromPSPath);
					InvalidOperationException exception3 = new InvalidOperationException(message3);
					ErrorRecord errorRecord3 = new ErrorRecord(exception3, "InvalidPSSessionConfigurationFile", ErrorCategory.InvalidArgument, base.Path);
					base.ThrowTerminatingError(errorRecord3);
				}
				else
				{
					if (hashtable.ContainsKey(ConfigFileConstants.Guid))
					{
						try
						{
							if (hashtable[ConfigFileConstants.Guid] != null)
							{
								guid = Guid.Parse(hashtable[ConfigFileConstants.Guid].ToString());
							}
							else
							{
								InvalidOperationException exception4 = new InvalidOperationException(StringUtil.Format(RemotingErrorIdStrings.ErrorParsingTheKeyInPSSessionConfigurationFile, ConfigFileConstants.Guid, unresolvedProviderPathFromPSPath));
								base.ThrowTerminatingError(new ErrorRecord(exception4, "InvalidGuidInPSSessionConfigurationFile", ErrorCategory.InvalidOperation, null));
							}
						}
						catch (FormatException exception5)
						{
							base.ThrowTerminatingError(new ErrorRecord(exception5, "InvalidGuidInPSSessionConfigurationFile", ErrorCategory.InvalidOperation, null));
						}
					}
					if (hashtable.ContainsKey(ConfigFileConstants.PowerShellVersion) && !this.isPSVersionSpecified)
					{
						try
						{
							base.PSVersion = new Version(hashtable[ConfigFileConstants.PowerShellVersion].ToString());
						}
						catch (ArgumentException exception6)
						{
							base.ThrowTerminatingError(new ErrorRecord(exception6, "InvalidPowerShellVersion", ErrorCategory.InvalidOperation, null));
						}
						catch (FormatException exception7)
						{
							base.ThrowTerminatingError(new ErrorRecord(exception7, "InvalidPowerShellVersion", ErrorCategory.InvalidOperation, null));
						}
						catch (OverflowException exception8)
						{
							base.ThrowTerminatingError(new ErrorRecord(exception8, "InvalidPowerShellVersion", ErrorCategory.InvalidOperation, null));
						}
					}
					if (hashtable.ContainsKey(ConfigFileConstants.RunAsVirtualAccount))
					{
						base.RunAsVirtualAccount = LanguagePrimitives.ConvertTo<bool>(hashtable[ConfigFileConstants.RunAsVirtualAccount]);
						base.RunAsVirtualAccountSpecified = true;
					}
					if (hashtable.ContainsKey(ConfigFileConstants.RunAsVirtualAccountGroups))
					{
						base.RunAsVirtualAccountGroups = PSSessionConfigurationCommandUtilities.GetRunAsVirtualAccountGroupsString(DISCPowerShellConfiguration.TryGetStringArray(hashtable[ConfigFileConstants.RunAsVirtualAccountGroups]));
					}
					try
					{
						DISCUtils.ValidateAbsolutePaths(base.SessionState, hashtable, base.Path);
					}
					catch (InvalidOperationException exception9)
					{
						base.ThrowTerminatingError(new ErrorRecord(exception9, "RelativePathsNotSupported", ErrorCategory.InvalidOperation, null));
					}
					try
					{
						DISCUtils.ValidateExtensions(hashtable, base.Path);
					}
					catch (InvalidOperationException exception10)
					{
						base.ThrowTerminatingError(new ErrorRecord(exception10, "FileExtensionNotSupported", ErrorCategory.InvalidOperation, null));
					}
				}
				string text2 = System.IO.Path.Combine(Utils.GetApplicationBase(Utils.DefaultPowerShellShellID), "SessionConfig", this.shellName + "_" + guid.ToString() + ".pssc");
				if (string.Equals(this.ProcessorArchitecture, "x86", StringComparison.OrdinalIgnoreCase))
				{
					string environmentVariable = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
					if (string.Equals(environmentVariable, "amd64", StringComparison.OrdinalIgnoreCase) || string.Equals(environmentVariable, "ia64", StringComparison.OrdinalIgnoreCase))
					{
						text2 = text2.ToLowerInvariant().Replace("\\system32\\", "\\syswow64\\");
					}
				}
				srcConfigFilePath = unresolvedProviderPathFromPSPath;
				destConfigFilePath = text2;
				File.Copy(srcConfigFilePath, destConfigFilePath, true);
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"ConfigFilePath",
					text2,
					Environment.NewLine
				}));
			}
			if (!flag)
			{
				if (!string.IsNullOrEmpty(this.configurationTypeName))
				{
					stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
					{
						"pssessionconfigurationtypename",
						this.configurationTypeName,
						Environment.NewLine
					}));
				}
				if (!string.IsNullOrEmpty(this.assemblyName))
				{
					stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
					{
						"assemblyname",
						this.assemblyName,
						Environment.NewLine
					}));
				}
			}
			if (!string.IsNullOrEmpty(this.applicationBase))
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"applicationbase",
					this.applicationBase,
					Environment.NewLine
				}));
			}
			if (!string.IsNullOrEmpty(this.configurationScript))
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"startupscript",
					this.configurationScript,
					Environment.NewLine
				}));
			}
			if (this.maxCommandSizeMB != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"psmaximumreceiveddatasizepercommandmb",
					this.maxCommandSizeMB.Value,
					Environment.NewLine
				}));
			}
			if (this.maxObjectSizeMB != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"psmaximumreceivedobjectsizemb",
					this.maxObjectSizeMB.Value,
					Environment.NewLine
				}));
			}
			if (this.threadAptState != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"pssessionthreadapartmentstate",
					this.threadAptState.Value,
					Environment.NewLine
				}));
			}
			if (this.threadOptions != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"pssessionthreadoptions",
					this.threadOptions.Value,
					Environment.NewLine
				}));
			}
			if (!this.isPSVersionSpecified)
			{
				this.psVersion = PSVersionInfo.PSVersion;
			}
			if (this.psVersion != null)
			{
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"PSVersion",
					PSSessionConfigurationCommandUtilities.ConstructVersionFormatForConfigXml(this.psVersion),
					Environment.NewLine
				}));
				this.MaxPSVersion = PSSessionConfigurationCommandUtilities.CalculateMaxPSVersion(this.psVersion);
				if (this.MaxPSVersion != null)
				{
					stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
					{
						"MaxPSVersion",
						PSSessionConfigurationCommandUtilities.ConstructVersionFormatForConfigXml(this.MaxPSVersion),
						Environment.NewLine
					}));
				}
			}
			string text3 = "";
			if (!string.IsNullOrEmpty(this.sddl))
			{
				text3 = string.Format(CultureInfo.InvariantCulture, "<Security Uri='{0}' ExactMatch='true' Sddl='{1}' />", new object[]
				{
					"http://schemas.microsoft.com/powershell/" + this.shellName,
					this.sddl
				});
			}
			string text4 = string.Empty;
			if (!string.IsNullOrEmpty(this.architecture))
			{
				string text5 = "32";
				string a;
				if ((a = this.architecture.ToLowerInvariant()) != null)
				{
					if (!(a == "x86"))
					{
						if (a == "amd64")
						{
							text5 = "64";
						}
					}
					else
					{
						text5 = "32";
					}
				}
				text4 = string.Format(CultureInfo.InvariantCulture, "\r\n\tArchitecture='{0}'", new object[]
				{
					text5
				});
			}
			if (this.sessionType == PSSessionType.Workflow && !this.isUseSharedProcessSpecified)
			{
				base.UseSharedProcess = true;
			}
			string text6 = string.Empty;
			if (this.isUseSharedProcessSpecified)
			{
				text6 = string.Format(CultureInfo.InvariantCulture, "\r\n\tUseSharedProcess='{0}'", new object[]
				{
					base.UseSharedProcess.ToString()
				});
			}
			string text7 = string.Empty;
			string text8 = string.Empty;
			if (base.RunAsVirtualAccount)
			{
				text7 = string.Format(CultureInfo.InvariantCulture, "\r\n    RunAsVirtualAccount='{0}'", new object[]
				{
					base.RunAsVirtualAccount.ToString()
				});
				if (!string.IsNullOrEmpty(base.RunAsVirtualAccountGroups))
				{
					text8 = string.Format(CultureInfo.InvariantCulture, "\r\n    RunAsVirtualAccountGroups='{0}'", new object[]
					{
						base.RunAsVirtualAccountGroups
					});
				}
			}
			string text9 = string.Empty;
			switch (base.AccessMode)
			{
			case PSSessionConfigurationAccessMode.Disabled:
				text9 = string.Format(CultureInfo.InvariantCulture, "\r\n\tEnabled='{0}'", new object[]
				{
					false.ToString()
				});
				break;
			case PSSessionConfigurationAccessMode.Local:
			case PSSessionConfigurationAccessMode.Remote:
				text9 = string.Format(CultureInfo.InvariantCulture, "\r\n\tEnabled='{0}'", new object[]
				{
					true.ToString()
				});
				break;
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			if (this.sessionType == PSSessionType.Workflow)
			{
				List<object> list = new List<object>(this.modulesToImport ?? ((object[])new string[0]));
				list.Insert(0, "%windir%\\system32\\windowspowershell\\v1.0\\Modules\\PSWorkflow");
				this.modulesToImport = list.ToArray();
			}
			if (this.modulesToImport != null && this.modulesToImport.Length > 0)
			{
				stringBuilder2.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"modulestoimport",
					PSSessionConfigurationCommandUtilities.GetModulePathAsString(this.modulesToImport),
					string.Empty
				}));
			}
			if (this.sessionTypeOption != null)
			{
				string text10 = this.sessionTypeOption.ConstructPrivateData();
				if (!string.IsNullOrEmpty(text10))
				{
					stringBuilder2.Append(string.Format(CultureInfo.InvariantCulture, "<Param Name='PrivateData'>{0}</Param>", new object[]
					{
						text10
					}));
				}
			}
			if (stringBuilder2.Length > 0)
			{
				string str = string.Format(CultureInfo.InvariantCulture, "<SessionConfigurationData>{0}</SessionConfigurationData>", new object[]
				{
					stringBuilder2
				});
				string text11 = SecurityElement.Escape(str);
				stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "\r\n<Param Name='{0}' Value='{1}' />{2}", new object[]
				{
					"sessionconfigurationdata",
					text11,
					string.Empty
				}));
			}
			if (this.transportOption == null)
			{
				this.transportOption = new WSManConfigurationOption();
			}
			else
			{
				Hashtable hashtable2 = this.transportOption.ConstructQuotasAsHashtable();
				int idleTimeout;
				if (hashtable2.ContainsKey("IdleTimeoutms") && LanguagePrimitives.TryConvertTo<int>(hashtable2["IdleTimeoutms"], out idleTimeout))
				{
					PSSQMAPI.NoteSessionConfigurationIdleTimeout(idleTimeout);
				}
				Hashtable hashtable3 = this.transportOption.ConstructOptionsAsHashtable();
				string optBufferingMode;
				if (hashtable3.ContainsKey("OutputBufferingMode") && LanguagePrimitives.TryConvertTo<string>(hashtable3["OutputBufferingMode"], out optBufferingMode))
				{
					PSSQMAPI.NoteSessionConfigurationOutputBufferingMode(optBufferingMode);
				}
			}
			this.transportOption = (this.transportOption.Clone() as PSTransportOption);
			this.transportOption.LoadFromDefaults(this.sessionType, true);
			if (this.isUseSharedProcessSpecified && !base.UseSharedProcess)
			{
				(this.transportOption as WSManConfigurationOption).ProcessIdleTimeoutSec = new int?(0);
			}
			return string.Format(CultureInfo.InvariantCulture, "\r\n<PlugInConfiguration xmlns='http://schemas.microsoft.com/wbem/wsman/1/config/PluginConfiguration'\r\n    Name='{0}'\r\n    Filename='%windir%\\system32\\{1}'\r\n    SDKVersion='{12}'\r\n    XmlRenderingType='text' {2} {6} {7} {8} {9} {10}>\r\n  <InitializationParameters>    \r\n{3}\r\n  </InitializationParameters> \r\n  <Resources>\r\n    <Resource ResourceUri='{4}' SupportsOptions='true' ExactMatch='true'>\r\n{5}\r\n      <Capability Type='Shell' />\r\n    </Resource>\r\n  </Resources>\r\n  {11}\r\n</PlugInConfiguration>\r\n", new object[]
			{
				this.shellName,
				"pwrshplugin.dll",
				text4,
				stringBuilder.ToString(),
				"http://schemas.microsoft.com/powershell/" + this.shellName,
				text3,
				text6,
				text7,
				text8,
				text9,
				this.transportOption.ConstructOptionsAsXmlAttributes(),
				this.transportOption.ConstructQuotas(),
				(this.psVersion.Major < 3) ? 1 : 2
			});
		}

		// Token: 0x0400129F RID: 4767
		private const string newPluginSbFormat = "\r\nfunction Register-PSSessionConfiguration\r\n{{\r\n    [CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Medium\")]\r\n    param(  \r\n      [string] $filepath,\r\n      [string] $pluginName,\r\n      [bool] $shouldShowUI,\r\n      [bool] $force,\r\n      [string] $restartWSManTarget,\r\n      [string] $restartWSManAction,\r\n      [string] $restartWSManRequired,\r\n\t  [string] $runAsUserName,\r\n\t  [system.security.securestring] $runAsPassword,\r\n      [System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode] $accessMode,\r\n      [bool] $isSddlSpecified,\r\n      [string] $configPath\r\n    )\r\n\r\n    begin\r\n    {{\r\n        ## Construct SID for network users\r\n        [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n        $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n\r\n        ## If all session configurations have Network Access disabled,\r\n        ## then we create this endpoint as Local as well.\r\n        $newSDDL = $null\r\n        $foundRemoteEndpoint = $false;\r\n        Get-PSSessionConfiguration -Force:$force | Foreach-Object {{\r\n            if ($_.Enabled)\r\n            {{\r\n                $sddl = $null\r\n                if ($_.psobject.members[\"SecurityDescriptorSddl\"])\r\n                {{\r\n                    $sddl = $_.psobject.members[\"SecurityDescriptorSddl\"].Value\r\n                }}\r\n        \r\n                if($sddl)\r\n                {{\r\n                    # See if it has 'Disable Network Access'\r\n                    $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$sddl\r\n                    $disableNetworkExists = $false\r\n                    $sd.DiscretionaryAcl | % {{\r\n                        if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                        {{\r\n                            $disableNetworkExists = $true              \r\n                        }}\r\n                    }}\r\n\r\n                    if(-not $disableNetworkExists) {{ $foundRemoteEndpoint = $true }}\r\n                }}\r\n            }}\r\n        }}\r\n\r\n        if(-not $foundRemoteEndpoint)\r\n        {{\r\n            $newSDDL = \"{1}\"\r\n        }}\r\n    }}\r\n\r\n    process\r\n    {{\r\n        if ($force)\r\n        {{\r\n            if (Test-Path WSMan:\\localhost\\Plugin\\\"$pluginName\")\r\n            {{\r\n                Unregister-PSSessionConfiguration -name \"$pluginName\" -force\r\n            }}\r\n        }}\r\n\r\n        new-item -path WSMan:\\localhost\\Plugin -file \"$filepath\" -name \"$pluginName\"\r\n        # $? is to make sure the last operation is succeeded\r\n\r\n\t\tif ($? -and $runAsUserName) \r\n\t\t{{\r\n\t\t\ttry {{\r\n\t\t\t\t$runAsCredential = new-object system.management.automation.PSCredential($runAsUserName, $runAsPassword)\r\n\t\t\t\tset-item -WarningAction SilentlyContinue WSMan:\\localhost\\Plugin\\\"$pluginName\"\\RunAsUser $runAsCredential -confirm:$false\r\n\t\t\t}} catch {{\r\n\t\t\t\tremove-item WSMan:\\localhost\\Plugin\\\"$pluginName\" -recurse -force\r\n\t\t\t\twrite-error $_\r\n                # Do not add anymore clean up code after Write-Error, because if EA=Stop is set by user\r\n                # any code at this point will not execute.\r\n\r\n\t\t\t\treturn\r\n\t\t\t}}\r\n\t\t}}\r\n\r\n        ## Replace the SDDL with any groups defined in the PSSessionConfigurationFile, if any\r\n        if($? -and $configPath -and (-not $isSddlSpecified))\r\n        {{\r\n            $config = Import-PowerShellDataFile $configPath\r\n            if($config.RoleDefinitions.Keys)\r\n            {{\r\n                ## Create a CommonSecurityDescriptor object with a known good security descriptor\r\n                $curPlugin = Get-PSSessionConfiguration -Name $pluginName -Force:$force\r\n                $existingSddl = $curPlugin.SecurityDescriptorSddl\r\n                $arguments = $false,$false,$existingSddl\r\n                $mapper = New-Object Security.AccessControl.CommonSecurityDescriptor $arguments\r\n\r\n                ## Purge all existing access rules so that only role definition principals are \r\n                ## granted access.\r\n                $sidsToRemove = @()\r\n                $mapper.DiscretionaryAcl | % {{\r\n                    $sidsToRemove += $_.SecurityIdentifier\r\n                }}\r\n                foreach ($sidToRemove in $sidsToRemove)\r\n                {{\r\n                    $mapper.PurgeAccessControl($sidToRemove)\r\n                }} \r\n\r\n                foreach ($principal in @($config.RoleDefinitions.Keys))\r\n                {{\r\n                    try\r\n                    {{\r\n                        ## Get the SID for the principal\r\n                        $account = New-Object Security.Principal.NTAccount $principal\r\n                        $sid = $account.Translate([Security.Principal.SecurityIdentifier]).Value\r\n\r\n                        ## Create a new access rule that adds the principal to the endpoint\r\n                        ## 268435456 - GenericAll\r\n                        $mapper.DiscretionaryAcl.AddAccess('Allow', $sid, 268435456, 'None', 'None')\r\n                    }}\r\n                    catch\r\n                    {{\r\n                        $translationError = $_.Exception.InnerException.Message\r\n                        $errorMessage = [Microsoft.PowerShell.Commands.Internal.RemotingErrorResources]::CouldNotResolveRoleDefinitionPrincipal -f $principal, $translationError\r\n                        Write-Error $errorMessage\r\n                    }}\r\n                }}\r\n\r\n                ## Get the new SDDL for that configuration\r\n                if ($mapper.DiscretionaryAcl.Count -gt 0)\r\n                {{\r\n                    $configSDDL = $mapper.GetSddlForm('All')\r\n                    $null = Set-PSSessionConfiguration -Name $pluginName -SecurityDescriptorSddl $configSDDL -Force:$force\r\n                }}\r\n            }}\r\n        }}\r\n\r\n        if ($? -and $shouldShowUI)\r\n        {{\r\n           $null = winrm configsddl \"{0}$pluginName\"\r\n\r\n           # if AccessMode is Disabled OR the winrm configsddl failed, we just return\r\n           if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Disabled.Equals($accessMode) -or !$?)\r\n           {{\r\n               return\r\n           }}\r\n        }} # end of if ($shouldShowUI)\r\n\r\n        if ($?)\r\n        {{\r\n           # if AccessMode is Local or Remote, we need to check the SDDL the user set in the UI or passed in to the cmdlet.\r\n           $newSDDL = $null\r\n           $curPlugin = Get-PSSessionConfiguration -Name $pluginName -Force:$force\r\n           $curSDDL = $curPlugin.SecurityDescriptorSddl\r\n           if (!$curSDDL)\r\n           {{\r\n               if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Local.Equals($accessMode))\r\n               {{\r\n                    $newSDDL = \"{1}\"\r\n               }}\r\n           }}\r\n           else\r\n           {{\r\n               # Construct SID for network users\r\n               [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n               $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n                \r\n               $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$curSDDL\r\n               $haveDisableACE = $false\r\n               $securityIdentifierToPurge = $null\r\n               $sd.DiscretionaryAcl | % {{\r\n                    if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                    {{\r\n                        $haveDisableACE = $true\r\n                        $securityIdentifierToPurge = $_.securityidentifier\r\n                    }}\r\n               }}\r\n               if (([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Local.Equals($accessMode) -or\r\n                    ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Remote.Equals($accessMode)-and $disableNetworkExists)) -and\r\n                   !$haveDisableACE)\r\n               {{\r\n                    # Add network deny ACE for local access or remote access with PSRemoting disabled ($disableNetworkExists)\r\n                    $sd.DiscretionaryAcl.AddAccess(\"deny\", $networkSID, 268435456, \"None\", \"None\")\r\n                    $newSDDL = $sd.GetSddlForm(\"all\")\r\n               }}\r\n               if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Remote.Equals($accessMode) -and -not $disableNetworkExists -and $haveDisableACE)\r\n               {{\r\n                    # Remove the specific ACE\r\n                    $sd.discretionaryacl.RemoveAccessSpecific('Deny', $securityIdentifierToPurge, 268435456, 'none', 'none')\r\n\r\n                    # if there is no discretionaryacl..add Builtin Administrators and Remote Management Users\r\n                    # to the DACL group as this is the default WSMan behavior\r\n                    if ($sd.discretionaryacl.count -eq 0)\r\n                    {{\r\n                        [system.security.principal.wellknownsidtype]$bast = \"BuiltinAdministratorsSid\"\r\n                        $basid = new-object system.security.principal.securityidentifier $bast,$null\r\n                        $sd.DiscretionaryAcl.AddAccess('Allow',$basid, 268435456, 'none', 'none')\r\n\r\n                        # Remote Management Users, Win8+ only\r\n                        if ([System.Environment]::OSVersion.Version -ge \"6.2.0.0\")\r\n                        {{\r\n                            $rmSidId = new-object system.security.principal.securityidentifier \"{2}\"\r\n                            $sd.DiscretionaryAcl.AddAccess('Allow', $rmSidId, 268435456, 'none', 'none')\r\n                        }}\r\n\r\n                        # Interactive Users\r\n                        $iaSidId = new-object system.security.principal.securityidentifier \"{3}\"\r\n                        $sd.DiscretionaryAcl.AddAccess('Allow', $iaSidId, 268435456, 'none', 'none')\r\n                    }}\r\n\r\n                    $newSDDL = $sd.GetSddlForm(\"all\")\r\n               }}\r\n           }} # end of if(!$curSDDL)\r\n        }} # end of if ($?)\r\n\r\n        if ($? -and $newSDDL)\r\n        {{\r\n            try {{\r\n                if ($runAsUserName)\r\n                {{\r\n                    $runAsCredential = new-object system.management.automation.PSCredential($runAsUserName, $runAsPassword)\r\n                    $null = Set-PSSessionConfiguration -Name $pluginName -SecurityDescriptorSddl $newSDDL -NoServiceRestart -force -WarningAction 0 -RunAsCredential $runAsCredential\r\n                }}\r\n                else\r\n                {{\r\n                    $null = Set-PSSessionConfiguration -Name $pluginName -SecurityDescriptorSddl $newSDDL -NoServiceRestart -force -WarningAction 0\r\n                }}\r\n\r\n            }} catch {{\r\n\t\t\t\tremove-item WSMan:\\localhost\\Plugin\\\"$pluginName\" -recurse -force\r\n\t\t\t\twrite-error $_\r\n                # Do not add anymore clean up code after Write-Error, because if EA=Stop is set by user\r\n                # any code at this point will not execute.\r\n\r\n\t\t\t\treturn\r\n\t\t\t}}\r\n        }}\r\n\r\n        if ($?){{\r\n            try{{\r\n                $s = New-PSSession -ComputerName localhost -ConfigurationName $pluginName -ErrorAction Stop\r\n                # session is ok, no need to restart WinRM service                \r\n                Remove-PSSession $s -Confirm:$false\r\n            }}catch{{\r\n                # session is NOT ok, we need to restart winrm if -Force was specified, otherwise show a warning\r\n                if ($force){{\r\n                    Restart-Service -Name WinRM -Force -Confirm:$false\r\n                }}else{{\r\n                    $warningWSManRestart = [Microsoft.PowerShell.Commands.Internal.RemotingErrorResources]::WinRMRestartWarning -f $PSCmdlet.MyInvocation.MyCommand.Name\r\n                    Write-Warning $warningWSManRestart\r\n                }}\r\n            }}\r\n        }}\r\n    }}\r\n}}\r\n\r\nif ($args[14] -eq $null)\r\n{{\r\n    Register-PSSessionConfiguration -filepath $args[0] -pluginName $args[1] -shouldShowUI $args[2] -force $args[3] -whatif:$args[4] -confirm:$args[5] -restartWSManTarget $args[6] -restartWSManAction $args[7] -restartWSManRequired $args[8] -runAsUserName $args[9] -runAsPassword $args[10] -accessMode $args[11] -isSddlSpecified $args[12] -configPath $args[13]\r\n}}\r\nelse\r\n{{\r\n    Register-PSSessionConfiguration -filepath $args[0] -pluginName $args[1] -shouldShowUI $args[2] -force $args[3] -whatif:$args[4] -confirm:$args[5] -restartWSManTarget $args[6] -restartWSManAction $args[7] -restartWSManRequired $args[8] -runAsUserName $args[9] -runAsPassword $args[10] -accessMode $args[11] -isSddlSpecified $args[12] -configPath $args[13] -erroraction $args[14]\r\n}}\r\n";

		// Token: 0x040012A0 RID: 4768
		private const string pluginXmlFormat = "\r\n<PlugInConfiguration xmlns='http://schemas.microsoft.com/wbem/wsman/1/config/PluginConfiguration'\r\n    Name='{0}'\r\n    Filename='%windir%\\system32\\{1}'\r\n    SDKVersion='{12}'\r\n    XmlRenderingType='text' {2} {6} {7} {8} {9} {10}>\r\n  <InitializationParameters>    \r\n{3}\r\n  </InitializationParameters> \r\n  <Resources>\r\n    <Resource ResourceUri='{4}' SupportsOptions='true' ExactMatch='true'>\r\n{5}\r\n      <Capability Type='Shell' />\r\n    </Resource>\r\n  </Resources>\r\n  {11}\r\n</PlugInConfiguration>\r\n";

		// Token: 0x040012A1 RID: 4769
		private const string architectureAttribFormat = "\r\n\tArchitecture='{0}'";

		// Token: 0x040012A2 RID: 4770
		private const string sharedHostAttribFormat = "\r\n\tUseSharedProcess='{0}'";

		// Token: 0x040012A3 RID: 4771
		private const string runasVirtualAccountAttribFormat = "\r\n    RunAsVirtualAccount='{0}'";

		// Token: 0x040012A4 RID: 4772
		private const string runAsVirtualAccountGroupsAttribFormat = "\r\n    RunAsVirtualAccountGroups='{0}'";

		// Token: 0x040012A5 RID: 4773
		private const string allowRemoteShellAccessFormat = "\r\n\tEnabled='{0}'";

		// Token: 0x040012A6 RID: 4774
		private const string initParamFormat = "\r\n<Param Name='{0}' Value='{1}' />{2}";

		// Token: 0x040012A7 RID: 4775
		private const string privateDataFormat = "<Param Name='PrivateData'>{0}</Param>";

		// Token: 0x040012A8 RID: 4776
		private const string securityElementFormat = "<Security Uri='{0}' ExactMatch='true' Sddl='{1}' />";

		// Token: 0x040012A9 RID: 4777
		private const string SessionConfigDataFormat = "<SessionConfigurationData>{0}</SessionConfigurationData>";

		// Token: 0x040012AA RID: 4778
		private static readonly ScriptBlock newPluginSb;

		// Token: 0x040012AB RID: 4779
		private bool isErrorReported;

		// Token: 0x040012AC RID: 4780
		private string architecture;

		// Token: 0x040012AD RID: 4781
		internal PSSessionType sessionType;
	}
}
