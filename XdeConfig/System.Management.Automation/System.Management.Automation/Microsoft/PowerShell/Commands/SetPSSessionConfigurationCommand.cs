using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;
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
	// Token: 0x0200031C RID: 796
	[Cmdlet("Set", "PSSessionConfiguration", DefaultParameterSetName = "NameParameterSet", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=144307")]
	public sealed class SetPSSessionConfigurationCommand : PSSessionConfigurationCommandBase
	{
		// Token: 0x0600260B RID: 9739 RVA: 0x000D4690 File Offset: 0x000D2890
		static SetPSSessionConfigurationCommand()
		{
			string localSddl = PSSessionConfigurationCommandBase.GetLocalSddl();
			string script = string.Format(CultureInfo.InvariantCulture, "\r\nfunction Set-PSSessionConfiguration([PSObject]$customShellObject, \r\n     [Array]$initParametersMap,\r\n     [bool]$force,\r\n     [string]$sddl,\r\n     [bool]$isSddlSpecified,\r\n     [bool]$shouldShowUI,\r\n     [string]$resourceUri,\r\n     [string]$pluginNotFoundErrorMsg,\r\n     [string]$pluginNotPowerShellMsg,\r\n     [System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]$accessMode\r\n)\r\n{{\r\n   $wsmanPluginDir = 'WSMan:\\localhost\\Plugin'\r\n   $pluginName = $customShellObject.Name;\r\n   $pluginDir = Join-Path \"$wsmanPluginDir\" \"$pluginName\"\r\n   if ((!$pluginName) -or !(test-path \"$pluginDir\"))\r\n   {{\r\n      Write-Error $pluginNotFoundErrorMsg\r\n      return\r\n   }}\r\n\r\n   # check if the plugin is a PowerShell plugin   \r\n   $pluginFileNamePath = Join-Path \"$pluginDir\" 'FileName'\r\n   if (!(test-path \"$pluginFileNamePath\"))\r\n   {{\r\n      Write-Error $pluginNotPowerShellMsg\r\n      return\r\n   }}\r\n\r\n   $pluginFileName = get-item -literalpath \"$pluginFileNamePath\"\r\n   if ((!$pluginFileName) -or ($pluginFileName.Value -notmatch '{0}'))\r\n   {{\r\n      Write-Error $pluginNotPowerShellMsg\r\n      return\r\n   }}\r\n\r\n   # set Initialization Parameters\r\n   $initParametersPath = Join-Path \"$pluginDir\" 'InitializationParameters'  \r\n   foreach($initParameterName in $initParametersMap)\r\n   {{         \r\n        if ($customShellObject | get-member $initParameterName)\r\n        {{\r\n            $parampath = Join-Path \"$initParametersPath\" $initParameterName\r\n\r\n            if (test-path $parampath)\r\n            {{\r\n               remove-item -path \"$parampath\"\r\n            }}\r\n                \r\n            # 0 is an accepted value for MaximumReceivedDataSizePerCommandMB and MaximumReceivedObjectSizeMB\r\n            if (($customShellObject.$initParameterName) -or ($customShellObject.$initParameterName -eq 0))\r\n            {{\r\n               new-item -path \"$initParametersPath\" -paramname $initParameterName  -paramValue \"$($customShellObject.$initParameterName)\" -Force\r\n            }}\r\n        }}\r\n   }}\r\n\r\n   # sddl processing\r\n   if ($isSddlSpecified)\r\n   {{\r\n       $resourcesPath = Join-Path \"$pluginDir\" 'Resources'\r\n       dir -literalpath \"$resourcesPath\" | % {{\r\n            $securityPath = Join-Path \"$($_.pspath)\" 'Security'\r\n            if ((@(dir -literalpath \"$securityPath\")).count -gt 0)\r\n            {{\r\n                dir -literalpath \"$securityPath\" | % {{\r\n                    $securityIDPath = \"$($_.pspath)\"\r\n                    remove-item -path \"$securityIDPath\" -recurse -force\r\n                }} #end of securityPath\r\n\r\n                if ($sddl)\r\n                {{\r\n                    new-item -path \"$securityPath\" -Sddl $sddl -force\r\n                }}\r\n            }}\r\n            else\r\n            {{\r\n                if ($sddl)\r\n                {{\r\n                    new-item -path \"$securityPath\" -Sddl $sddl -force\r\n                }}\r\n            }}\r\n       }} # end of resources\r\n       return\r\n   }} #end of sddl processing\r\n   elseif ($shouldShowUI)\r\n   {{\r\n        $null = winrm configsddl $resourceUri\r\n   }}\r\n\r\n   # If accessmode is 'Disabled', we don't bother to check the sddl\r\n   if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Disabled.Equals($accessMode))\r\n   {{\r\n        return\r\n   }}\r\n\r\n   # Construct SID for network users\r\n   [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n   $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n\r\n   $resPath = Join-Path \"$pluginDir\" 'Resources'\r\n   dir -literalpath \"$resPath\" | % {{\r\n        $securityPath = Join-Path \"$($_.pspath)\" 'Security'\r\n        if ((@(dir -literalpath \"$securityPath\")).count -gt 0)\r\n        {{\r\n            dir -literalpath \"$securityPath\" | % {{\r\n                $sddlPath = Join-Path \"$($_.pspath)\" 'Sddl'\r\n                $curSDDL = (get-item -path $sddlPath).value\r\n                $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$curSDDL\r\n                $newSDDL = $null\r\n                \r\n                $disableNetworkExists = $false\r\n                $securityIdentifierToPurge = $null\r\n                $sd.DiscretionaryAcl | % {{\r\n                    if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                    {{\r\n                        $disableNetworkExists = $true\r\n                        $securityIdentifierToPurge = $_.securityidentifier\r\n                    }}\r\n                }}\r\n\r\n                if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Local.Equals($accessMode) -and !$disableNetworkExists)\r\n                {{\r\n                    $sd.DiscretionaryAcl.AddAccess(\"deny\", $networkSID, 268435456, \"None\", \"None\")\r\n                    $newSDDL = $sd.GetSddlForm(\"all\")\r\n                }}\r\n                if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Remote.Equals($accessMode) -and $disableNetworkExists)\r\n                {{\r\n                    # Remove the specific ACE\r\n                    $sd.discretionaryacl.RemoveAccessSpecific('Deny', $securityIdentifierToPurge, 268435456, 'none', 'none')\r\n\r\n                    # if there is no discretionaryacl..add Builtin Administrators and Remote Management Users\r\n                    # to the DACL group as this is the default WSMan behavior\r\n                    if ($sd.discretionaryacl.count -eq 0)\r\n                    {{\r\n                        # Built-in administrators\r\n                        [system.security.principal.wellknownsidtype]$bast = \"BuiltinAdministratorsSid\"\r\n                        $basid = new-object system.security.principal.securityidentifier $bast,$null\r\n                        $sd.DiscretionaryAcl.AddAccess('Allow',$basid, 268435456, 'none', 'none')\r\n\r\n                        # Remote Management Users, Win8+ only\r\n                        if ([System.Environment]::OSVersion.Version -ge \"6.2.0.0\")\r\n                        {{\r\n                            $rmSidId = new-object system.security.principal.securityidentifier \"{2}\"\r\n                            $sd.DiscretionaryAcl.AddAccess('Allow', $rmSidId, 268435456, 'none', 'none')\r\n                        }}\r\n\r\n                        # Interactive Users\r\n                        $iuSidId = new-object system.security.principal.securityidentifier \"{3}\"\r\n                        $sd.DiscretionaryAcl.AddAccess('Allow', $iuSidId, 268435456, 'none', 'none')\r\n                    }}\r\n\r\n                    $newSDDL = $sd.GetSddlForm(\"all\")\r\n                }}\r\n\r\n                if ($newSDDL)\r\n                {{\r\n                    set-item -WarningAction SilentlyContinue -path $sddlPath -value $newSDDL -force\r\n                }}\r\n            }}\r\n        }}\r\n        else\r\n        {{\r\n            if (([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Local.Equals($accessMode)))\r\n            {{\r\n                new-item -path \"$securityPath\" -Sddl \"{1}\" -force\r\n            }}\r\n        }}\r\n   }}\r\n}}\r\n\r\nSet-PSSessionConfiguration $args[0] $args[1] $args[2] $args[3] $args[4] $args[5] $args[6] $args[7] $args[8] $args[9]\r\n", new object[]
			{
				"pwrshplugin.dll",
				localSddl,
				"S-1-5-32-580",
				"S-1-5-4"
			});
			SetPSSessionConfigurationCommand.setPluginSb = ScriptBlock.Create(script);
			SetPSSessionConfigurationCommand.setPluginSb.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
		}

		// Token: 0x0600260C RID: 9740 RVA: 0x000D475C File Offset: 0x000D295C
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
			if (base.Path != null)
			{
				ProviderInfo providerInfo = null;
				PSDriveInfo psdriveInfo;
				this.configFilePath = base.SessionState.Path.GetUnresolvedProviderPathFromPSPath(base.Path, out providerInfo, out psdriveInfo);
				string text;
				ExternalScriptInfo scriptInfoForFile = DISCUtils.GetScriptInfoForFile(base.Context, this.configFilePath, out text);
				this.configTable = DISCUtils.LoadConfigFile(base.Context, scriptInfoForFile);
				if (!this.isSddlSpecified && this.configTable.ContainsKey("RoleDefinitions"))
				{
					if (base.AccessMode == PSSessionConfigurationAccessMode.Local)
					{
						this.sddl = PSSessionConfigurationCommandBase.GetLocalSddl();
					}
					else if (base.AccessMode == PSSessionConfigurationAccessMode.Remote)
					{
						this.sddl = PSSessionConfigurationCommandBase.GetRemoteSddl();
					}
					CommonSecurityDescriptor commonSecurityDescriptor = new CommonSecurityDescriptor(false, false, this.sddl);
					List<SecurityIdentifier> list = new List<SecurityIdentifier>();
					foreach (GenericAce genericAce in commonSecurityDescriptor.DiscretionaryAcl)
					{
						CommonAce commonAce = (CommonAce)genericAce;
						list.Add(commonAce.SecurityIdentifier);
					}
					foreach (SecurityIdentifier sid in list)
					{
						commonSecurityDescriptor.PurgeAccessControl(sid);
					}
					Hashtable hashtable = this.configTable["RoleDefinitions"] as Hashtable;
					foreach (object obj in hashtable.Keys)
					{
						string text2 = obj.ToString();
						try
						{
							NTAccount ntaccount = new NTAccount(text2);
							SecurityIdentifier sid2 = (SecurityIdentifier)ntaccount.Translate(typeof(SecurityIdentifier));
							commonSecurityDescriptor.DiscretionaryAcl.AddAccess(AccessControlType.Allow, sid2, 268435456, InheritanceFlags.None, PropagationFlags.None);
						}
						catch (IdentityNotMappedException innerException)
						{
							string message2 = StringUtil.Format(RemotingErrorIdStrings.CouldNotResolveRoleDefinitionPrincipal, text2);
							InvalidOperationException exception = new InvalidOperationException(message2, innerException);
							ErrorRecord errorRecord = new ErrorRecord(exception, "CouldNotResolveRoleDefinitionPrincipal", ErrorCategory.ObjectNotFound, text2);
							base.WriteError(errorRecord);
						}
					}
					if (commonSecurityDescriptor.DiscretionaryAcl.Count > 0)
					{
						this.isSddlSpecified = true;
						this.sddl = commonSecurityDescriptor.GetSddlForm(AccessControlSections.All);
						base.SecurityDescriptorSddl = this.sddl;
					}
				}
				if (this.configTable.ContainsKey(ConfigFileConstants.RunAsVirtualAccount))
				{
					base.RunAsVirtualAccount = LanguagePrimitives.ConvertTo<bool>(this.configTable[ConfigFileConstants.RunAsVirtualAccount]);
					base.RunAsVirtualAccountSpecified = true;
				}
				if (this.configTable.ContainsKey(ConfigFileConstants.RunAsVirtualAccountGroups))
				{
					base.RunAsVirtualAccountGroups = PSSessionConfigurationCommandUtilities.GetRunAsVirtualAccountGroupsString(DISCPowerShellConfiguration.TryGetStringArray(this.configTable[ConfigFileConstants.RunAsVirtualAccountGroups]));
				}
			}
			if (this.isSddlSpecified && this.accessModeSpecified)
			{
				CommonSecurityDescriptor commonSecurityDescriptor2 = new CommonSecurityDescriptor(false, false, this.sddl);
				SecurityIdentifier sid3 = new SecurityIdentifier(WellKnownSidType.NetworkSid, null);
				bool flag = false;
				foreach (GenericAce genericAce2 in commonSecurityDescriptor2.DiscretionaryAcl)
				{
					CommonAce commonAce2 = (CommonAce)genericAce2;
					if (commonAce2.AceQualifier.Equals(AceQualifier.AccessDenied) && commonAce2.SecurityIdentifier.Equals(sid3) && commonAce2.AccessMask == 268435456)
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
						commonSecurityDescriptor2.DiscretionaryAcl.AddAccess(AccessControlType.Deny, sid3, 268435456, InheritanceFlags.None, PropagationFlags.None);
						this.sddl = commonSecurityDescriptor2.GetSddlForm(AccessControlSections.All);
					}
					break;
				case PSSessionConfigurationAccessMode.Remote:
					if (flag)
					{
						commonSecurityDescriptor2.DiscretionaryAcl.RemoveAccessSpecific(AccessControlType.Deny, sid3, 268435456, InheritanceFlags.None, PropagationFlags.None);
						if (commonSecurityDescriptor2.DiscretionaryAcl.Count == 0)
						{
							this.sddl = PSSessionConfigurationCommandBase.GetRemoteSddl();
						}
						else
						{
							this.sddl = commonSecurityDescriptor2.GetSddlForm(AccessControlSections.All);
						}
					}
					break;
				}
			}
			RemotingCommandUtil.CheckRemotingCmdletPrerequisites();
			PSSessionConfigurationCommandUtilities.ThrowIfNotAdministrator();
		}

		// Token: 0x0600260D RID: 9741 RVA: 0x000D4B84 File Offset: 0x000D2D84
		protected override void ProcessRecord()
		{
			base.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.ScsScriptMessageV, "\r\nfunction Set-PSSessionConfiguration([PSObject]$customShellObject, \r\n     [Array]$initParametersMap,\r\n     [bool]$force,\r\n     [string]$sddl,\r\n     [bool]$isSddlSpecified,\r\n     [bool]$shouldShowUI,\r\n     [string]$resourceUri,\r\n     [string]$pluginNotFoundErrorMsg,\r\n     [string]$pluginNotPowerShellMsg,\r\n     [System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]$accessMode\r\n)\r\n{{\r\n   $wsmanPluginDir = 'WSMan:\\localhost\\Plugin'\r\n   $pluginName = $customShellObject.Name;\r\n   $pluginDir = Join-Path \"$wsmanPluginDir\" \"$pluginName\"\r\n   if ((!$pluginName) -or !(test-path \"$pluginDir\"))\r\n   {{\r\n      Write-Error $pluginNotFoundErrorMsg\r\n      return\r\n   }}\r\n\r\n   # check if the plugin is a PowerShell plugin   \r\n   $pluginFileNamePath = Join-Path \"$pluginDir\" 'FileName'\r\n   if (!(test-path \"$pluginFileNamePath\"))\r\n   {{\r\n      Write-Error $pluginNotPowerShellMsg\r\n      return\r\n   }}\r\n\r\n   $pluginFileName = get-item -literalpath \"$pluginFileNamePath\"\r\n   if ((!$pluginFileName) -or ($pluginFileName.Value -notmatch '{0}'))\r\n   {{\r\n      Write-Error $pluginNotPowerShellMsg\r\n      return\r\n   }}\r\n\r\n   # set Initialization Parameters\r\n   $initParametersPath = Join-Path \"$pluginDir\" 'InitializationParameters'  \r\n   foreach($initParameterName in $initParametersMap)\r\n   {{         \r\n        if ($customShellObject | get-member $initParameterName)\r\n        {{\r\n            $parampath = Join-Path \"$initParametersPath\" $initParameterName\r\n\r\n            if (test-path $parampath)\r\n            {{\r\n               remove-item -path \"$parampath\"\r\n            }}\r\n                \r\n            # 0 is an accepted value for MaximumReceivedDataSizePerCommandMB and MaximumReceivedObjectSizeMB\r\n            if (($customShellObject.$initParameterName) -or ($customShellObject.$initParameterName -eq 0))\r\n            {{\r\n               new-item -path \"$initParametersPath\" -paramname $initParameterName  -paramValue \"$($customShellObject.$initParameterName)\" -Force\r\n            }}\r\n        }}\r\n   }}\r\n\r\n   # sddl processing\r\n   if ($isSddlSpecified)\r\n   {{\r\n       $resourcesPath = Join-Path \"$pluginDir\" 'Resources'\r\n       dir -literalpath \"$resourcesPath\" | % {{\r\n            $securityPath = Join-Path \"$($_.pspath)\" 'Security'\r\n            if ((@(dir -literalpath \"$securityPath\")).count -gt 0)\r\n            {{\r\n                dir -literalpath \"$securityPath\" | % {{\r\n                    $securityIDPath = \"$($_.pspath)\"\r\n                    remove-item -path \"$securityIDPath\" -recurse -force\r\n                }} #end of securityPath\r\n\r\n                if ($sddl)\r\n                {{\r\n                    new-item -path \"$securityPath\" -Sddl $sddl -force\r\n                }}\r\n            }}\r\n            else\r\n            {{\r\n                if ($sddl)\r\n                {{\r\n                    new-item -path \"$securityPath\" -Sddl $sddl -force\r\n                }}\r\n            }}\r\n       }} # end of resources\r\n       return\r\n   }} #end of sddl processing\r\n   elseif ($shouldShowUI)\r\n   {{\r\n        $null = winrm configsddl $resourceUri\r\n   }}\r\n\r\n   # If accessmode is 'Disabled', we don't bother to check the sddl\r\n   if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Disabled.Equals($accessMode))\r\n   {{\r\n        return\r\n   }}\r\n\r\n   # Construct SID for network users\r\n   [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n   $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n\r\n   $resPath = Join-Path \"$pluginDir\" 'Resources'\r\n   dir -literalpath \"$resPath\" | % {{\r\n        $securityPath = Join-Path \"$($_.pspath)\" 'Security'\r\n        if ((@(dir -literalpath \"$securityPath\")).count -gt 0)\r\n        {{\r\n            dir -literalpath \"$securityPath\" | % {{\r\n                $sddlPath = Join-Path \"$($_.pspath)\" 'Sddl'\r\n                $curSDDL = (get-item -path $sddlPath).value\r\n                $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$curSDDL\r\n                $newSDDL = $null\r\n                \r\n                $disableNetworkExists = $false\r\n                $securityIdentifierToPurge = $null\r\n                $sd.DiscretionaryAcl | % {{\r\n                    if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                    {{\r\n                        $disableNetworkExists = $true\r\n                        $securityIdentifierToPurge = $_.securityidentifier\r\n                    }}\r\n                }}\r\n\r\n                if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Local.Equals($accessMode) -and !$disableNetworkExists)\r\n                {{\r\n                    $sd.DiscretionaryAcl.AddAccess(\"deny\", $networkSID, 268435456, \"None\", \"None\")\r\n                    $newSDDL = $sd.GetSddlForm(\"all\")\r\n                }}\r\n                if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Remote.Equals($accessMode) -and $disableNetworkExists)\r\n                {{\r\n                    # Remove the specific ACE\r\n                    $sd.discretionaryacl.RemoveAccessSpecific('Deny', $securityIdentifierToPurge, 268435456, 'none', 'none')\r\n\r\n                    # if there is no discretionaryacl..add Builtin Administrators and Remote Management Users\r\n                    # to the DACL group as this is the default WSMan behavior\r\n                    if ($sd.discretionaryacl.count -eq 0)\r\n                    {{\r\n                        # Built-in administrators\r\n                        [system.security.principal.wellknownsidtype]$bast = \"BuiltinAdministratorsSid\"\r\n                        $basid = new-object system.security.principal.securityidentifier $bast,$null\r\n                        $sd.DiscretionaryAcl.AddAccess('Allow',$basid, 268435456, 'none', 'none')\r\n\r\n                        # Remote Management Users, Win8+ only\r\n                        if ([System.Environment]::OSVersion.Version -ge \"6.2.0.0\")\r\n                        {{\r\n                            $rmSidId = new-object system.security.principal.securityidentifier \"{2}\"\r\n                            $sd.DiscretionaryAcl.AddAccess('Allow', $rmSidId, 268435456, 'none', 'none')\r\n                        }}\r\n\r\n                        # Interactive Users\r\n                        $iuSidId = new-object system.security.principal.securityidentifier \"{3}\"\r\n                        $sd.DiscretionaryAcl.AddAccess('Allow', $iuSidId, 268435456, 'none', 'none')\r\n                    }}\r\n\r\n                    $newSDDL = $sd.GetSddlForm(\"all\")\r\n                }}\r\n\r\n                if ($newSDDL)\r\n                {{\r\n                    set-item -WarningAction SilentlyContinue -path $sddlPath -value $newSDDL -force\r\n                }}\r\n            }}\r\n        }}\r\n        else\r\n        {{\r\n            if (([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Local.Equals($accessMode)))\r\n            {{\r\n                new-item -path \"$securityPath\" -Sddl \"{1}\" -force\r\n            }}\r\n        }}\r\n   }}\r\n}}\r\n\r\nSet-PSSessionConfiguration $args[0] $args[1] $args[2] $args[3] $args[4] $args[5] $args[6] $args[7] $args[8] $args[9]\r\n"));
			string action = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessAction, base.CommandInfo.Name);
			string target;
			if (!this.isSddlSpecified)
			{
				target = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessTarget, base.Name);
			}
			else
			{
				target = StringUtil.Format(RemotingErrorIdStrings.ScsShouldProcessTargetSDDL, base.Name, this.sddl);
			}
			if (!this.noRestart && !this.force)
			{
				string o = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessAction, base.CommandInfo.Name);
				base.WriteWarning(StringUtil.Format(RemotingErrorIdStrings.WinRMRestartWarning, o));
			}
			if (!this.force && !base.ShouldProcess(target, action))
			{
				return;
			}
			if (this.isUseSharedProcessSpecified)
			{
				using (PowerShell powerShell = PowerShell.Create())
				{
					powerShell.AddScript(string.Format(CultureInfo.InvariantCulture, "(get-item 'WSMan::localhost\\Plugin\\{0}\\InitializationParameters\\sessiontype' -ErrorAction SilentlyContinue).Value", new object[]
					{
						CodeGeneration.EscapeSingleQuotedStringContent(base.Name)
					}));
					Collection<PSObject> collection = powerShell.Invoke(new object[]
					{
						base.Name
					});
					if (collection != null)
					{
						int count = collection.Count;
					}
					if (base.UseSharedProcess == false && collection[0] != null && string.Compare(collection[0].ToString(), "Workflow", StringComparison.OrdinalIgnoreCase) == 0)
					{
						throw new PSInvalidOperationException(RemotingErrorIdStrings.UseSharedProcessCannotBeFalseForWorkflowSessionType);
					}
				}
			}
			if (this.configTable != null)
			{
				Guid guid = Guid.Empty;
				if (this.configTable.ContainsKey(ConfigFileConstants.Guid))
				{
					guid = Guid.Parse(this.configTable[ConfigFileConstants.Guid].ToString());
				}
				string destFileName = System.IO.Path.Combine(Utils.GetApplicationBase(Utils.DefaultPowerShellShellID), "SessionConfig", this.shellName + "_" + guid.ToString() + ".pssc");
				File.Copy(this.configFilePath, destFileName, true);
			}
			string text = StringUtil.Format(RemotingErrorIdStrings.CSCmdsShellNotFound, this.shellName);
			string text2 = StringUtil.Format(RemotingErrorIdStrings.CSCmdsShellNotPowerShellBased, this.shellName);
			PSObject psobject = this.ConstructPropertiesForUpdate();
			ArrayList arrayList = (ArrayList)base.Context.DollarErrorVariable;
			int count2 = arrayList.Count;
			SetPSSessionConfigurationCommand.setPluginSb.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[]
			{
				psobject,
				SetPSSessionConfigurationCommand.initParametersMap,
				this.force,
				this.sddl,
				this.isSddlSpecified,
				base.ShowSecurityDescriptorUI.ToBool(),
				"http://schemas.microsoft.com/powershell/" + this.shellName,
				text,
				text2,
				this.accessModeSpecified ? base.AccessMode : PSSessionConfigurationAccessMode.Disabled
			});
			arrayList = (ArrayList)base.Context.DollarErrorVariable;
			if (arrayList.Count > count2)
			{
				this.isErrorReported = true;
				return;
			}
			this.SetSessionConfigurationTypeOptions();
			if (arrayList.Count > count2)
			{
				this.isErrorReported = true;
				return;
			}
			this.SetQuotas();
			if (arrayList.Count > count2)
			{
				this.isErrorReported = true;
				return;
			}
			this.SetRunAs();
			if (arrayList.Count > count2)
			{
				this.isErrorReported = true;
				return;
			}
			this.SetVirtualAccount();
			if (arrayList.Count > count2)
			{
				this.isErrorReported = true;
				return;
			}
			if (this.isRunAsCredentialSpecified || base.RunAsVirtualAccountSpecified)
			{
				PSSessionConfigurationCommandUtilities.MoveWinRmToIsolatedServiceHost(base.RunAsVirtualAccountSpecified);
			}
			if (arrayList.Count > count2)
			{
				this.isErrorReported = true;
				return;
			}
			this.SetOptions();
			if (arrayList.Count > count2)
			{
				this.isErrorReported = true;
			}
		}

		// Token: 0x0600260E RID: 9742 RVA: 0x000D4F48 File Offset: 0x000D3148
		private void SetRunAs()
		{
			if (this.runAsCredential == null)
			{
				return;
			}
			ScriptBlock scriptBlock = ScriptBlock.Create(string.Format(CultureInfo.InvariantCulture, "\r\nfunction Set-RunAsCredential{{\r\n    param (\r\n        [string]$runAsUserName,\r\n\t    [system.security.securestring]$runAsPassword\r\n    )\r\n\r\n    $cred = new-object System.Management.Automation.PSCredential($runAsUserName, $runAsPassword)\r\n    set-item -WarningAction SilentlyContinue 'WSMan:\\localhost\\Plugin\\{0}\\RunAsUser' $cred -confirm:$false\r\n}}\r\nSet-RunAsCredential $args[0] $args[1]\r\n", new object[]
			{
				CodeGeneration.EscapeSingleQuotedStringContent(base.Name)
			}));
			scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
			scriptBlock.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[]
			{
				this.runAsCredential.UserName,
				this.runAsCredential.Password
			});
		}

		// Token: 0x0600260F RID: 9743 RVA: 0x000D4FD4 File Offset: 0x000D31D4
		private void SetVirtualAccount()
		{
			if (!base.RunAsVirtualAccountSpecified)
			{
				return;
			}
			using (PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace))
			{
				string value = StringUtil.Format("WSMAN:\\localhost\\plugin\\{0}\\RunAsVirtualAccount", base.Name);
				powerShell.AddCommand("Set-Item").AddParameter("Path", value).AddParameter("Value", base.RunAsVirtualAccount);
				powerShell.Invoke();
			}
		}

		// Token: 0x06002610 RID: 9744 RVA: 0x000D5054 File Offset: 0x000D3254
		private void SetQuotas()
		{
			if (this.transportOption != null)
			{
				ScriptBlock scriptBlock = ScriptBlock.Create(string.Format(CultureInfo.InvariantCulture, "\r\nfunction Set-SessionPluginQuota([hashtable] $quotas) {{\r\n    foreach($v in $quotas.GetEnumerator()) {{\r\n        $name = $v.Name; \r\n        $value = $v.Value;\r\n        if (!$value) {{\r\n            $value = [string]::empty;\r\n        }}\r\n        set-item -WarningAction SilentlyContinue ('WSMan:\\localhost\\Plugin\\{0}\\Quotas\\' + $name) -Value $value -confirm:$false\r\n    }}\r\n}}\r\nSet-SessionPluginQuota $args[0]\r\n", new object[]
				{
					CodeGeneration.EscapeSingleQuotedStringContent(base.Name)
				}));
				scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
				Hashtable hashtable = this.transportOption.ConstructQuotasAsHashtable();
				int num = 0;
				if (hashtable.ContainsKey("IdleTimeoutms") && LanguagePrimitives.TryConvertTo<int>(hashtable["IdleTimeoutms"], out num))
				{
					PSSQMAPI.NoteSessionConfigurationIdleTimeout(num);
				}
				if (num != 0 && hashtable.ContainsKey("MaxIdleTimeoutms"))
				{
					bool flag = true;
					int num2;
					if (LanguagePrimitives.TryConvertTo<int>(hashtable["MaxIdleTimeoutms"], out num2))
					{
						int? num3 = WSManConfigurationOption.DefaultIdleTimeout;
						using (PowerShell powerShell = PowerShell.Create())
						{
							powerShell.AddScript(string.Format(CultureInfo.InvariantCulture, "(Get-Item 'WSMan:\\localhost\\Plugin\\{0}\\Quotas\\IdleTimeoutms').Value", new object[]
							{
								CodeGeneration.EscapeSingleQuotedStringContent(base.Name)
							}));
							Collection<PSObject> collection = powerShell.Invoke(new object[]
							{
								base.Name
							});
							if (collection != null)
							{
								int count = collection.Count;
							}
							num3 = new int?(Convert.ToInt32(collection[0].ToString(), CultureInfo.InvariantCulture));
						}
						if (num3 >= num2 && num3 >= num)
						{
							flag = false;
						}
					}
					ScriptBlock scriptBlock2 = ScriptBlock.Create(string.Format(CultureInfo.InvariantCulture, "\r\nfunction Set-SessionPluginIdleTimeoutQuotas([int] $maxIdleTimeoutms, [int] $idleTimeoutms, [bool] $setMaxIdleTimoutFirst) {{\r\n    if ($setMaxIdleTimoutFirst) {{\r\n        set-item -WarningAction SilentlyContinue 'WSMan:\\localhost\\Plugin\\{0}\\Quotas\\MaxIdleTimeoutms' -Value $maxIdleTimeoutms -confirm:$false\r\n        set-item -WarningAction SilentlyContinue 'WSMan:\\localhost\\Plugin\\{0}\\Quotas\\IdleTimeoutms' -Value $idleTimeoutms -confirm:$false\r\n    }}\r\n    else {{\r\n        set-item -WarningAction SilentlyContinue 'WSMan:\\localhost\\Plugin\\{0}\\Quotas\\IdleTimeoutms' -Value $idleTimeoutms -confirm:$false\r\n        set-item -WarningAction SilentlyContinue 'WSMan:\\localhost\\Plugin\\{0}\\Quotas\\MaxIdleTimeoutms' -Value $maxIdleTimeoutms -confirm:$false\r\n    }}\r\n}}\r\nSet-SessionPluginIdleTimeoutQuotas $args[0] $args[1] $args[2]\r\n", new object[]
					{
						CodeGeneration.EscapeSingleQuotedStringContent(base.Name)
					}));
					scriptBlock2.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
					scriptBlock2.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[]
					{
						num2,
						num,
						flag
					});
					hashtable.Remove("MaxIdleTimeoutms");
					hashtable.Remove("IdleTimeoutms");
				}
				scriptBlock.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[]
				{
					hashtable
				});
			}
		}

		// Token: 0x06002611 RID: 9745 RVA: 0x000D52AC File Offset: 0x000D34AC
		private void SetOptions()
		{
			ScriptBlock scriptBlock = ScriptBlock.Create(string.Format(CultureInfo.InvariantCulture, "\r\nfunction Set-SessionPluginOptions([hashtable] $options) {{\r\n    if ($options[\"UsedSharedProcess\"]) {{\r\n        $value = $options[\"UseSharedProcess\"];\r\n        set-item -WarningAction SilentlyContinue 'WSMan:\\localhost\\Plugin\\{0}\\UseSharedProcess' -Value $value -confirm:$false\r\n        $options.Remove(\"UseSharedProcess\");\r\n    }}\r\n    foreach($v in $options.GetEnumerator()) {{\r\n        $name = $v.Name; \r\n        $value = $v.Value\r\n\r\n        if (!$value) {{\r\n            $value = 0;\r\n        }}\r\n\r\n        set-item -WarningAction SilentlyContinue ('WSMan:\\localhost\\Plugin\\{0}\\' + $name) -Value $value -confirm:$false\r\n    }}\r\n}}\r\nSet-SessionPluginOptions $args[0]\r\n", new object[]
			{
				CodeGeneration.EscapeSingleQuotedStringContent(base.Name)
			}));
			scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
			Hashtable hashtable = (this.transportOption != null) ? this.transportOption.ConstructOptionsAsHashtable() : new Hashtable();
			string optBufferingMode;
			if (hashtable.ContainsKey("OutputBufferingMode") && LanguagePrimitives.TryConvertTo<string>(hashtable["OutputBufferingMode"], out optBufferingMode))
			{
				PSSQMAPI.NoteSessionConfigurationOutputBufferingMode(optBufferingMode);
			}
			if (this.accessModeSpecified)
			{
				switch (base.AccessMode)
				{
				case PSSessionConfigurationAccessMode.Disabled:
					hashtable["Enabled"] = false.ToString();
					break;
				case PSSessionConfigurationAccessMode.Local:
				case PSSessionConfigurationAccessMode.Remote:
					hashtable["Enabled"] = true.ToString();
					break;
				}
			}
			if (this.isUseSharedProcessSpecified)
			{
				hashtable["UseSharedProcess"] = base.UseSharedProcess.ToBool().ToString();
			}
			scriptBlock.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[]
			{
				hashtable
			});
		}

		// Token: 0x06002612 RID: 9746 RVA: 0x000D53D8 File Offset: 0x000D35D8
		private void SetSessionConfigurationTypeOptions()
		{
			if (this.sessionTypeOption != null)
			{
				using (PowerShell powerShell = PowerShell.Create())
				{
					powerShell.AddScript(string.Format(CultureInfo.InvariantCulture, "(Get-Item 'WSMan:\\localhost\\Plugin\\{0}\\InitializationParameters\\SessionConfigurationData').Value", new object[]
					{
						CodeGeneration.EscapeSingleQuotedStringContent(base.Name)
					}));
					Collection<PSObject> collection = powerShell.Invoke(new object[]
					{
						base.Name
					});
					if (collection != null)
					{
						int count = collection.Count;
					}
					PSSessionConfigurationData pssessionConfigurationData = PSSessionConfigurationData.Create((collection[0] == null) ? string.Empty : PSSessionConfigurationData.Unescape(collection[0].BaseObject.ToString()));
					PSSessionTypeOption pssessionTypeOption = this.sessionTypeOption.ConstructObjectFromPrivateData(pssessionConfigurationData.PrivateData);
					pssessionTypeOption.CopyUpdatedValuesFrom(this.sessionTypeOption);
					StringBuilder stringBuilder = new StringBuilder();
					string text = null;
					string text2 = string.Empty;
					bool flag = false;
					if (this.modulePathSpecified)
					{
						bool flag2 = this.IsWorkflowConfigurationType(powerShell);
						if (this.modulesToImport == null || this.modulesToImport.Length == 0 || (this.modulesToImport.Length == 1 && this.modulesToImport[0] is string && ((string)this.modulesToImport[0]).Equals(string.Empty, StringComparison.OrdinalIgnoreCase)))
						{
							flag = true;
							text2 = (flag2 ? "%windir%\\system32\\windowspowershell\\v1.0\\Modules\\PSWorkflow" : string.Empty);
						}
						else
						{
							text = PSSessionConfigurationCommandUtilities.GetModulePathAsString(this.modulesToImport).Trim();
							if (!string.IsNullOrEmpty(text) && flag2)
							{
								List<object> list = new List<object>(this.modulesToImport);
								list.Insert(0, "%windir%\\system32\\windowspowershell\\v1.0\\Modules\\PSWorkflow");
								text = PSSessionConfigurationCommandUtilities.GetModulePathAsString(list.ToArray()).Trim();
							}
						}
					}
					if (!flag && string.IsNullOrEmpty(text))
					{
						text = ((pssessionConfigurationData.ModulesToImportInternal == null) ? null : PSSessionConfigurationCommandUtilities.GetModulePathAsString(pssessionConfigurationData.ModulesToImportInternal.ToArray()).Trim());
					}
					if (flag || string.IsNullOrEmpty(text))
					{
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Param Name='{0}' Value='{1}' />", new object[]
						{
							"modulestoimport",
							text2
						}));
					}
					else
					{
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Param Name='{0}' Value='{1}' />", new object[]
						{
							"modulestoimport",
							text
						}));
					}
					string text3 = pssessionTypeOption.ConstructPrivateData();
					if (!string.IsNullOrEmpty(text3))
					{
						stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Param Name='PrivateData'>{0}</Param>", new object[]
						{
							text3
						}));
					}
					if (stringBuilder.Length > 0)
					{
						string str = string.Format(CultureInfo.InvariantCulture, "<SessionConfigurationData>{0}</SessionConfigurationData>", new object[]
						{
							stringBuilder
						});
						string text4 = SecurityElement.Escape(str);
						ScriptBlock scriptBlock = ScriptBlock.Create(string.Format(CultureInfo.InvariantCulture, "\r\nfunction Set-SessionConfigurationData([string] $scd) {{\r\n    if (test-path 'WSMan:\\localhost\\Plugin\\{0}\\InitializationParameters\\sessionconfigurationdata')\r\n    {{\r\n        set-item -WarningAction SilentlyContinue -Force 'WSMan:\\localhost\\Plugin\\{0}\\InitializationParameters\\sessionconfigurationdata' -Value $scd\r\n    }}\r\n    else\r\n    {{\r\n        new-item -WarningAction SilentlyContinue -path 'WSMan:\\localhost\\Plugin\\{0}\\InitializationParameters' -paramname sessionconfigurationdata -paramValue $scd -Force\r\n    }}\r\n}}\r\nSet-SessionConfigurationData $args[0]\r\n", new object[]
						{
							CodeGeneration.EscapeSingleQuotedStringContent(base.Name)
						}));
						scriptBlock.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
						scriptBlock.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[]
						{
							text4
						});
					}
				}
			}
		}

		// Token: 0x06002613 RID: 9747 RVA: 0x000D5704 File Offset: 0x000D3904
		protected override void EndProcessing()
		{
			PSSessionConfigurationCommandUtilities.RestartWinRMService(this, this.isErrorReported, base.Force, this.noRestart);
			if (!this.isErrorReported && this.noRestart)
			{
				string o = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessAction, base.CommandInfo.Name);
				base.WriteWarning(StringUtil.Format(RemotingErrorIdStrings.WinRMRequiresRestart, o));
			}
			Tracer tracer = new Tracer();
			tracer.EndpointModified(base.Name, WindowsIdentity.GetCurrent().Name);
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x000D5784 File Offset: 0x000D3984
		private bool IsWorkflowConfigurationType(PowerShell ps)
		{
			ps.AddScript(string.Format(CultureInfo.InvariantCulture, "(Get-Item 'WSMan:\\localhost\\Plugin\\{0}\\InitializationParameters\\assemblyname').Value", new object[]
			{
				CodeGeneration.EscapeSingleQuotedStringContent(base.Name)
			}));
			Collection<PSObject> collection = ps.Invoke(new object[]
			{
				base.Name
			});
			if (collection != null)
			{
				int count = collection.Count;
			}
			if (collection[0] == null)
			{
				return false;
			}
			string text = collection[0].BaseObject.ToString();
			return text.Equals("Microsoft.PowerShell.Workflow.ServiceCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL", StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x000D580C File Offset: 0x000D3A0C
		private PSObject ConstructPropertiesForUpdate()
		{
			PSObject psobject = new PSObject();
			psobject.Properties.Add(new PSNoteProperty("Name", this.shellName));
			if (this.isAssemblyNameSpecified)
			{
				psobject.Properties.Add(new PSNoteProperty("assemblyname", this.assemblyName));
			}
			if (this.isApplicationBaseSpecified)
			{
				psobject.Properties.Add(new PSNoteProperty("applicationbase", this.applicationBase));
			}
			if (this.isConfigurationTypeNameSpecified)
			{
				psobject.Properties.Add(new PSNoteProperty("pssessionconfigurationtypename", this.configurationTypeName));
			}
			if (this.isConfigurationScriptSpecified)
			{
				psobject.Properties.Add(new PSNoteProperty("startupscript", this.configurationScript));
			}
			if (this.isMaxCommandSizeMBSpecified)
			{
				object value = (this.maxCommandSizeMB != null) ? this.maxCommandSizeMB.Value : null;
				psobject.Properties.Add(new PSNoteProperty("psmaximumreceiveddatasizepercommandmb", value));
			}
			if (this.isMaxObjectSizeMBSpecified)
			{
				object value2 = (this.maxObjectSizeMB != null) ? this.maxObjectSizeMB.Value : null;
				psobject.Properties.Add(new PSNoteProperty("psmaximumreceivedobjectsizemb", value2));
			}
			if (this.threadAptState != null)
			{
				psobject.Properties.Add(new PSNoteProperty("pssessionthreadapartmentstate", this.threadAptState.Value));
			}
			if (this.threadOptions != null)
			{
				psobject.Properties.Add(new PSNoteProperty("pssessionthreadoptions", this.threadOptions.Value));
			}
			if (this.isPSVersionSpecified)
			{
				psobject.Properties.Add(new PSNoteProperty("PSVersion", PSSessionConfigurationCommandUtilities.ConstructVersionFormatForConfigXml(this.psVersion)));
				this.MaxPSVersion = PSSessionConfigurationCommandUtilities.CalculateMaxPSVersion(this.psVersion);
				psobject.Properties.Add(new PSNoteProperty("MaxPSVersion", PSSessionConfigurationCommandUtilities.ConstructVersionFormatForConfigXml(this.MaxPSVersion)));
			}
			if (this.modulePathSpecified && this.sessionTypeOption == null)
			{
				bool flag = false;
				string text = null;
				if (this.modulesToImport == null || this.modulesToImport.Length == 0 || (this.modulesToImport.Length == 1 && this.modulesToImport[0] is string && ((string)this.modulesToImport[0]).Equals(string.Empty, StringComparison.OrdinalIgnoreCase)))
				{
					flag = true;
				}
				else
				{
					text = PSSessionConfigurationCommandUtilities.GetModulePathAsString(this.modulesToImport).Trim();
				}
				if (flag || !string.IsNullOrEmpty(text))
				{
					using (PowerShell powerShell = PowerShell.Create())
					{
						bool flag2 = this.IsWorkflowConfigurationType(powerShell);
						if (!string.IsNullOrEmpty(text) && flag2)
						{
							List<object> list = new List<object>(this.modulesToImport);
							list.Insert(0, "%windir%\\system32\\windowspowershell\\v1.0\\Modules\\PSWorkflow");
							text = PSSessionConfigurationCommandUtilities.GetModulePathAsString(list.ToArray()).Trim();
						}
						powerShell.AddScript(string.Format(CultureInfo.InvariantCulture, "(Get-Item 'WSMan:\\localhost\\Plugin\\{0}\\InitializationParameters\\SessionConfigurationData').Value", new object[]
						{
							CodeGeneration.EscapeSingleQuotedStringContent(base.Name)
						}));
						Collection<PSObject> collection = powerShell.Invoke(new object[]
						{
							base.Name
						});
						if (collection != null)
						{
							int count = collection.Count;
						}
						StringBuilder stringBuilder = new StringBuilder();
						if (collection[0] == null)
						{
							if (!flag)
							{
								stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Param Name='{0}' Value='{1}' />", new object[]
								{
									"modulestoimport",
									text
								}));
							}
						}
						else
						{
							PSSessionConfigurationData pssessionConfigurationData = PSSessionConfigurationData.Create(collection[0].BaseObject.ToString());
							string text2 = string.IsNullOrEmpty(pssessionConfigurationData.PrivateData) ? null : pssessionConfigurationData.PrivateData.Replace('"', '\'');
							if (flag)
							{
								if (pssessionConfigurationData.ModulesToImportInternal != null && pssessionConfigurationData.ModulesToImportInternal.Count != 0)
								{
									string text3 = flag2 ? "%windir%\\system32\\windowspowershell\\v1.0\\Modules\\PSWorkflow" : string.Empty;
									stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Param Name='{0}' Value='{1}' />", new object[]
									{
										"modulestoimport",
										text3
									}));
									if (!string.IsNullOrEmpty(text2))
									{
										stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Param Name='PrivateData'>{0}</Param>", new object[]
										{
											text2
										}));
									}
								}
							}
							else
							{
								stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Param Name='{0}' Value='{1}' />", new object[]
								{
									"modulestoimport",
									text
								}));
								if (!string.IsNullOrEmpty(text2))
								{
									stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "<Param Name='PrivateData'>{0}</Param>", new object[]
									{
										text2
									}));
								}
							}
						}
						if (stringBuilder.Length > 0)
						{
							string str = string.Format(CultureInfo.InvariantCulture, "<SessionConfigurationData>{0}</SessionConfigurationData>", new object[]
							{
								stringBuilder
							});
							string value3 = SecurityElement.Escape(str);
							psobject.Properties.Add(new PSNoteProperty("sessionconfigurationdata", value3));
						}
					}
				}
			}
			if (base.Path != null)
			{
				ProviderInfo providerInfo = null;
				PSDriveInfo psdriveInfo;
				string unresolvedProviderPathFromPSPath = base.SessionState.Path.GetUnresolvedProviderPathFromPSPath(base.Path, out providerInfo, out psdriveInfo);
				if (!providerInfo.NameEquals(base.Context.ProviderNames.FileSystem) || !unresolvedProviderPathFromPSPath.EndsWith(".pssc", StringComparison.OrdinalIgnoreCase))
				{
					string message = StringUtil.Format(RemotingErrorIdStrings.InvalidPSSessionConfigurationFilePath, base.Path);
					InvalidOperationException exception = new InvalidOperationException(message);
					ErrorRecord errorRecord = new ErrorRecord(exception, "InvalidPSSessionConfigurationFilePath", ErrorCategory.InvalidArgument, base.Path);
					base.ThrowTerminatingError(errorRecord);
				}
				Guid empty = Guid.Empty;
				string text4;
				ExternalScriptInfo scriptInfoForFile = DISCUtils.GetScriptInfoForFile(base.Context, unresolvedProviderPathFromPSPath, out text4);
				Hashtable hashtable = DISCUtils.LoadConfigFile(base.Context, scriptInfoForFile);
				foreach (object obj in hashtable.Keys)
				{
					if (psobject.Properties[obj.ToString()] == null)
					{
						psobject.Properties.Add(new PSNoteProperty(obj.ToString(), hashtable[obj]));
					}
					else
					{
						psobject.Properties[obj.ToString()].Value = hashtable[obj];
					}
				}
			}
			return psobject;
		}

		// Token: 0x040012BB RID: 4795
		private const string getSessionTypeFormat = "(get-item 'WSMan::localhost\\Plugin\\{0}\\InitializationParameters\\sessiontype' -ErrorAction SilentlyContinue).Value";

		// Token: 0x040012BC RID: 4796
		private const string getCurrentIdleTimeoutmsFormat = "(Get-Item 'WSMan:\\localhost\\Plugin\\{0}\\Quotas\\IdleTimeoutms').Value";

		// Token: 0x040012BD RID: 4797
		private const string getAssemblyNameDataFormat = "(Get-Item 'WSMan:\\localhost\\Plugin\\{0}\\InitializationParameters\\assemblyname').Value";

		// Token: 0x040012BE RID: 4798
		private const string getSessionConfigurationDataSbFormat = "(Get-Item 'WSMan:\\localhost\\Plugin\\{0}\\InitializationParameters\\SessionConfigurationData').Value";

		// Token: 0x040012BF RID: 4799
		private const string setSessionConfigurationDataSbFormat = "\r\nfunction Set-SessionConfigurationData([string] $scd) {{\r\n    if (test-path 'WSMan:\\localhost\\Plugin\\{0}\\InitializationParameters\\sessionconfigurationdata')\r\n    {{\r\n        set-item -WarningAction SilentlyContinue -Force 'WSMan:\\localhost\\Plugin\\{0}\\InitializationParameters\\sessionconfigurationdata' -Value $scd\r\n    }}\r\n    else\r\n    {{\r\n        new-item -WarningAction SilentlyContinue -path 'WSMan:\\localhost\\Plugin\\{0}\\InitializationParameters' -paramname sessionconfigurationdata -paramValue $scd -Force\r\n    }}\r\n}}\r\nSet-SessionConfigurationData $args[0]\r\n";

		// Token: 0x040012C0 RID: 4800
		private const string setSessionConfigurationQuotaSbFormat = "\r\nfunction Set-SessionPluginQuota([hashtable] $quotas) {{\r\n    foreach($v in $quotas.GetEnumerator()) {{\r\n        $name = $v.Name; \r\n        $value = $v.Value;\r\n        if (!$value) {{\r\n            $value = [string]::empty;\r\n        }}\r\n        set-item -WarningAction SilentlyContinue ('WSMan:\\localhost\\Plugin\\{0}\\Quotas\\' + $name) -Value $value -confirm:$false\r\n    }}\r\n}}\r\nSet-SessionPluginQuota $args[0]\r\n";

		// Token: 0x040012C1 RID: 4801
		private const string setSessionConfigurationTimeoutQuotasSbFormat = "\r\nfunction Set-SessionPluginIdleTimeoutQuotas([int] $maxIdleTimeoutms, [int] $idleTimeoutms, [bool] $setMaxIdleTimoutFirst) {{\r\n    if ($setMaxIdleTimoutFirst) {{\r\n        set-item -WarningAction SilentlyContinue 'WSMan:\\localhost\\Plugin\\{0}\\Quotas\\MaxIdleTimeoutms' -Value $maxIdleTimeoutms -confirm:$false\r\n        set-item -WarningAction SilentlyContinue 'WSMan:\\localhost\\Plugin\\{0}\\Quotas\\IdleTimeoutms' -Value $idleTimeoutms -confirm:$false\r\n    }}\r\n    else {{\r\n        set-item -WarningAction SilentlyContinue 'WSMan:\\localhost\\Plugin\\{0}\\Quotas\\IdleTimeoutms' -Value $idleTimeoutms -confirm:$false\r\n        set-item -WarningAction SilentlyContinue 'WSMan:\\localhost\\Plugin\\{0}\\Quotas\\MaxIdleTimeoutms' -Value $maxIdleTimeoutms -confirm:$false\r\n    }}\r\n}}\r\nSet-SessionPluginIdleTimeoutQuotas $args[0] $args[1] $args[2]\r\n";

		// Token: 0x040012C2 RID: 4802
		private const string setSessionConfigurationOptionsSbFormat = "\r\nfunction Set-SessionPluginOptions([hashtable] $options) {{\r\n    if ($options[\"UsedSharedProcess\"]) {{\r\n        $value = $options[\"UseSharedProcess\"];\r\n        set-item -WarningAction SilentlyContinue 'WSMan:\\localhost\\Plugin\\{0}\\UseSharedProcess' -Value $value -confirm:$false\r\n        $options.Remove(\"UseSharedProcess\");\r\n    }}\r\n    foreach($v in $options.GetEnumerator()) {{\r\n        $name = $v.Name; \r\n        $value = $v.Value\r\n\r\n        if (!$value) {{\r\n            $value = 0;\r\n        }}\r\n\r\n        set-item -WarningAction SilentlyContinue ('WSMan:\\localhost\\Plugin\\{0}\\' + $name) -Value $value -confirm:$false\r\n    }}\r\n}}\r\nSet-SessionPluginOptions $args[0]\r\n";

		// Token: 0x040012C3 RID: 4803
		private const string setRunAsSbFormat = "\r\nfunction Set-RunAsCredential{{\r\n    param (\r\n        [string]$runAsUserName,\r\n\t    [system.security.securestring]$runAsPassword\r\n    )\r\n\r\n    $cred = new-object System.Management.Automation.PSCredential($runAsUserName, $runAsPassword)\r\n    set-item -WarningAction SilentlyContinue 'WSMan:\\localhost\\Plugin\\{0}\\RunAsUser' $cred -confirm:$false\r\n}}\r\nSet-RunAsCredential $args[0] $args[1]\r\n";

		// Token: 0x040012C4 RID: 4804
		private const string setPluginSbFormat = "\r\nfunction Set-PSSessionConfiguration([PSObject]$customShellObject, \r\n     [Array]$initParametersMap,\r\n     [bool]$force,\r\n     [string]$sddl,\r\n     [bool]$isSddlSpecified,\r\n     [bool]$shouldShowUI,\r\n     [string]$resourceUri,\r\n     [string]$pluginNotFoundErrorMsg,\r\n     [string]$pluginNotPowerShellMsg,\r\n     [System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]$accessMode\r\n)\r\n{{\r\n   $wsmanPluginDir = 'WSMan:\\localhost\\Plugin'\r\n   $pluginName = $customShellObject.Name;\r\n   $pluginDir = Join-Path \"$wsmanPluginDir\" \"$pluginName\"\r\n   if ((!$pluginName) -or !(test-path \"$pluginDir\"))\r\n   {{\r\n      Write-Error $pluginNotFoundErrorMsg\r\n      return\r\n   }}\r\n\r\n   # check if the plugin is a PowerShell plugin   \r\n   $pluginFileNamePath = Join-Path \"$pluginDir\" 'FileName'\r\n   if (!(test-path \"$pluginFileNamePath\"))\r\n   {{\r\n      Write-Error $pluginNotPowerShellMsg\r\n      return\r\n   }}\r\n\r\n   $pluginFileName = get-item -literalpath \"$pluginFileNamePath\"\r\n   if ((!$pluginFileName) -or ($pluginFileName.Value -notmatch '{0}'))\r\n   {{\r\n      Write-Error $pluginNotPowerShellMsg\r\n      return\r\n   }}\r\n\r\n   # set Initialization Parameters\r\n   $initParametersPath = Join-Path \"$pluginDir\" 'InitializationParameters'  \r\n   foreach($initParameterName in $initParametersMap)\r\n   {{         \r\n        if ($customShellObject | get-member $initParameterName)\r\n        {{\r\n            $parampath = Join-Path \"$initParametersPath\" $initParameterName\r\n\r\n            if (test-path $parampath)\r\n            {{\r\n               remove-item -path \"$parampath\"\r\n            }}\r\n                \r\n            # 0 is an accepted value for MaximumReceivedDataSizePerCommandMB and MaximumReceivedObjectSizeMB\r\n            if (($customShellObject.$initParameterName) -or ($customShellObject.$initParameterName -eq 0))\r\n            {{\r\n               new-item -path \"$initParametersPath\" -paramname $initParameterName  -paramValue \"$($customShellObject.$initParameterName)\" -Force\r\n            }}\r\n        }}\r\n   }}\r\n\r\n   # sddl processing\r\n   if ($isSddlSpecified)\r\n   {{\r\n       $resourcesPath = Join-Path \"$pluginDir\" 'Resources'\r\n       dir -literalpath \"$resourcesPath\" | % {{\r\n            $securityPath = Join-Path \"$($_.pspath)\" 'Security'\r\n            if ((@(dir -literalpath \"$securityPath\")).count -gt 0)\r\n            {{\r\n                dir -literalpath \"$securityPath\" | % {{\r\n                    $securityIDPath = \"$($_.pspath)\"\r\n                    remove-item -path \"$securityIDPath\" -recurse -force\r\n                }} #end of securityPath\r\n\r\n                if ($sddl)\r\n                {{\r\n                    new-item -path \"$securityPath\" -Sddl $sddl -force\r\n                }}\r\n            }}\r\n            else\r\n            {{\r\n                if ($sddl)\r\n                {{\r\n                    new-item -path \"$securityPath\" -Sddl $sddl -force\r\n                }}\r\n            }}\r\n       }} # end of resources\r\n       return\r\n   }} #end of sddl processing\r\n   elseif ($shouldShowUI)\r\n   {{\r\n        $null = winrm configsddl $resourceUri\r\n   }}\r\n\r\n   # If accessmode is 'Disabled', we don't bother to check the sddl\r\n   if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Disabled.Equals($accessMode))\r\n   {{\r\n        return\r\n   }}\r\n\r\n   # Construct SID for network users\r\n   [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n   $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n\r\n   $resPath = Join-Path \"$pluginDir\" 'Resources'\r\n   dir -literalpath \"$resPath\" | % {{\r\n        $securityPath = Join-Path \"$($_.pspath)\" 'Security'\r\n        if ((@(dir -literalpath \"$securityPath\")).count -gt 0)\r\n        {{\r\n            dir -literalpath \"$securityPath\" | % {{\r\n                $sddlPath = Join-Path \"$($_.pspath)\" 'Sddl'\r\n                $curSDDL = (get-item -path $sddlPath).value\r\n                $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$curSDDL\r\n                $newSDDL = $null\r\n                \r\n                $disableNetworkExists = $false\r\n                $securityIdentifierToPurge = $null\r\n                $sd.DiscretionaryAcl | % {{\r\n                    if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                    {{\r\n                        $disableNetworkExists = $true\r\n                        $securityIdentifierToPurge = $_.securityidentifier\r\n                    }}\r\n                }}\r\n\r\n                if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Local.Equals($accessMode) -and !$disableNetworkExists)\r\n                {{\r\n                    $sd.DiscretionaryAcl.AddAccess(\"deny\", $networkSID, 268435456, \"None\", \"None\")\r\n                    $newSDDL = $sd.GetSddlForm(\"all\")\r\n                }}\r\n                if ([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Remote.Equals($accessMode) -and $disableNetworkExists)\r\n                {{\r\n                    # Remove the specific ACE\r\n                    $sd.discretionaryacl.RemoveAccessSpecific('Deny', $securityIdentifierToPurge, 268435456, 'none', 'none')\r\n\r\n                    # if there is no discretionaryacl..add Builtin Administrators and Remote Management Users\r\n                    # to the DACL group as this is the default WSMan behavior\r\n                    if ($sd.discretionaryacl.count -eq 0)\r\n                    {{\r\n                        # Built-in administrators\r\n                        [system.security.principal.wellknownsidtype]$bast = \"BuiltinAdministratorsSid\"\r\n                        $basid = new-object system.security.principal.securityidentifier $bast,$null\r\n                        $sd.DiscretionaryAcl.AddAccess('Allow',$basid, 268435456, 'none', 'none')\r\n\r\n                        # Remote Management Users, Win8+ only\r\n                        if ([System.Environment]::OSVersion.Version -ge \"6.2.0.0\")\r\n                        {{\r\n                            $rmSidId = new-object system.security.principal.securityidentifier \"{2}\"\r\n                            $sd.DiscretionaryAcl.AddAccess('Allow', $rmSidId, 268435456, 'none', 'none')\r\n                        }}\r\n\r\n                        # Interactive Users\r\n                        $iuSidId = new-object system.security.principal.securityidentifier \"{3}\"\r\n                        $sd.DiscretionaryAcl.AddAccess('Allow', $iuSidId, 268435456, 'none', 'none')\r\n                    }}\r\n\r\n                    $newSDDL = $sd.GetSddlForm(\"all\")\r\n                }}\r\n\r\n                if ($newSDDL)\r\n                {{\r\n                    set-item -WarningAction SilentlyContinue -path $sddlPath -value $newSDDL -force\r\n                }}\r\n            }}\r\n        }}\r\n        else\r\n        {{\r\n            if (([System.Management.Automation.Runspaces.PSSessionConfigurationAccessMode]::Local.Equals($accessMode)))\r\n            {{\r\n                new-item -path \"$securityPath\" -Sddl \"{1}\" -force\r\n            }}\r\n        }}\r\n   }}\r\n}}\r\n\r\nSet-PSSessionConfiguration $args[0] $args[1] $args[2] $args[3] $args[4] $args[5] $args[6] $args[7] $args[8] $args[9]\r\n";

		// Token: 0x040012C5 RID: 4805
		private const string initParamFormat = "<Param Name='{0}' Value='{1}' />";

		// Token: 0x040012C6 RID: 4806
		private const string privateDataFormat = "<Param Name='PrivateData'>{0}</Param>";

		// Token: 0x040012C7 RID: 4807
		private const string SessionConfigDataFormat = "<SessionConfigurationData>{0}</SessionConfigurationData>";

		// Token: 0x040012C8 RID: 4808
		private const string UseSharedProcessToken = "UseSharedProcess";

		// Token: 0x040012C9 RID: 4809
		private const string AllowRemoteAccessToken = "Enabled";

		// Token: 0x040012CA RID: 4810
		private static readonly ScriptBlock setPluginSb;

		// Token: 0x040012CB RID: 4811
		private static readonly string[] initParametersMap = new string[]
		{
			"applicationbase",
			"assemblyname",
			"pssessionconfigurationtypename",
			"startupscript",
			"psmaximumreceivedobjectsizemb",
			"psmaximumreceiveddatasizepercommandmb",
			"pssessionthreadoptions",
			"pssessionthreadapartmentstate",
			"PSVersion",
			"MaxPSVersion",
			"sessionconfigurationdata"
		};

		// Token: 0x040012CC RID: 4812
		private bool isErrorReported;

		// Token: 0x040012CD RID: 4813
		private Hashtable configTable;

		// Token: 0x040012CE RID: 4814
		private string configFilePath;
	}
}
