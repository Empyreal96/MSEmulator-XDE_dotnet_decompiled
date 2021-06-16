﻿using System;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200031F RID: 799
	[Cmdlet("Enable", "PSRemoting", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Medium, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=144300")]
	public sealed class EnablePSRemotingCommand : PSCmdlet
	{
		// Token: 0x06002631 RID: 9777 RVA: 0x000D63F8 File Offset: 0x000D45F8
		static EnablePSRemotingCommand()
		{
			string text = string.Format(CultureInfo.InvariantCulture, "\r\n<PlugInConfiguration xmlns='http://schemas.microsoft.com/wbem/wsman/1/config/PluginConfiguration'\r\n    Name='Microsoft.Powershell.Workflow'\r\n    Filename='%windir%\\system32\\pwrshplugin.dll'\r\n    SDKVersion='2'\r\n    XmlRenderingType='text'\r\n    UseSharedProcess='true'\r\n    ProcessIdleTimeoutSec='1209600'\r\n    OutputBufferingMode='Block'\r\n    Enabled='True'\r\n>\r\n<InitializationParameters>\r\n<Param Name='PSVersion'  Value='{0}' />\r\n<Param Name='AssemblyName' Value='Microsoft.PowerShell.Workflow.ServiceCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL'/>\r\n<Param Name='PSSessionConfigurationTypeName' Value='Microsoft.PowerShell.Workflow.PSWorkflowSessionConfiguration'/>\r\n<Param Name='SessionConfigurationData'\r\n    Value ='\r\n        &lt;SessionConfigurationData&gt;\r\n            &lt;Param Name=&quot;ModulesToImport&quot; Value=&quot;%windir%\\system32\\windowspowershell\\v1.0\\Modules\\PSWorkflow&quot; /&gt;\r\n            &lt;Param Name=&quot;PrivateData&quot;&gt;\r\n                &lt;PrivateData&gt;\r\n                    &lt;Param Name=&quot;enablevalidation&quot; Value=&quot;true&quot; /&gt;\r\n                &lt;/PrivateData&gt;\r\n            &lt;/Param&gt;\r\n       &lt;/SessionConfigurationData&gt;\r\n       '\r\n/>\r\n</InitializationParameters> \r\n<Resources>\r\n    <Resource ResourceUri='http://schemas.microsoft.com/powershell/Microsoft.Powershell.Workflow' SupportsOptions='true' ExactMatch='true'>\r\n        <Security Uri='http://schemas.microsoft.com/powershell/Microsoft.PowerShell.Workflow' Sddl='{1}' ExactMatch='False'/>\r\n        <Capability Type='Shell' />        \r\n    </Resource>\r\n</Resources>\r\n<Quotas MaxMemoryPerShellMB='1024' MaxIdleTimeoutms='2147483647' MaxConcurrentUsers='5' IdleTimeoutms='7200000' MaxProcessesPerShell='15' MaxConcurrentCommandsPerShell='1000' MaxShells='25' MaxShellsPerUser='25' />\r\n</PlugInConfiguration>\r\n", new object[]
			{
				string.Format(CultureInfo.InvariantCulture, "{0}.{1}", new object[]
				{
					PSVersionInfo.PSVersion.Major,
					PSVersionInfo.PSVersion.Minor
				}),
				PSSessionConfigurationCommandBase.GetLocalSddl()
			});
			string script = string.Format(CultureInfo.InvariantCulture, "\r\nfunction Enable-PSRemoting\r\n{{\r\n[CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Medium\")]\r\nparam(\r\n    [Parameter()] [bool] $Force,\r\n    [Parameter()] [string] $queryForRegisterDefault,    \r\n    [Parameter()] [string] $captionForRegisterDefault,\r\n    [Parameter()] [string] $queryForSet,    \r\n    [Parameter()] [string] $captionForSet,\r\n    [Parameter()] [bool] $skipNetworkProfileCheck\r\n)\r\n\r\n    end\r\n    {{\r\n        # Enable all Session Configurations\r\n        try {{\r\n            $null = $PSBoundParameters.Remove(\"queryForRegisterDefault\")  \r\n            $null = $PSBoundParameters.Remove(\"captionForRegisterDefault\") \r\n            $null = $PSBoundParameters.Remove(\"queryForSet\")  \r\n            $null = $PSBoundParameters.Remove(\"captionForSet\")  \r\n\r\n            $PSBoundParameters.Add(\"Name\",\"*\")\r\n\r\n            # first try to enable all the sessions\r\n            Enable-PSSessionConfiguration @PSBoundParameters\r\n\r\n            # make sure default powershell end points exist \r\n            #  ie., Microsoft.PowerShell\r\n            #       and Microsoft.PowerShell32 (wow64)\r\n            \r\n            $errorCount = $error.Count\r\n            $endPoint = Get-PSSessionConfiguration {0} -Force:$Force -ErrorAction silentlycontinue 2>&1\r\n            $newErrorCount = $error.Count\r\n\r\n            # remove the 'No Session Configuration matches criteria' errors\r\n            for ($index = 0; $index -lt ($newErrorCount - $errorCount); $index ++)\r\n            {{\r\n                $error.RemoveAt(0)\r\n            }}\r\n\r\n            $qMessage = $queryForRegisterDefault -f \"{0}\",\"Register-PSSessionConfiguration {0} -force\"\r\n            if ((!$endpoint) -and \r\n                ($force  -or $pscmdlet.ShouldProcess($qMessage, $captionForRegisterDefault)))\r\n            {{\r\n                $null = Register-PSSessionConfiguration {0} -force\r\n                set-item -WarningAction SilentlyContinue wsman:\\localhost\\plugin\\{0}\\Quotas\\MaxShellsPerUser -value \"25\" -confirm:$false\r\n                set-item -WarningAction SilentlyContinue wsman:\\localhost\\plugin\\{0}\\Quotas\\MaxIdleTimeoutms -value {4} -confirm:$false\r\n                restart-service winrm -confirm:$false\r\n            }}  \r\n\r\n            # Check Microsoft.PowerShell.Workflow endpoint\r\n            $errorCount = $error.Count\r\n            $endPoint = Get-PSSessionConfiguration {0}.workflow -Force:$Force -ErrorAction silentlycontinue 2>&1\r\n            $newErrorCount = $error.Count\r\n\r\n            # remove the 'No Session Configuration matches criteria' errors\r\n            for ($index = 0; $index -lt ($newErrorCount - $errorCount); $index ++)\r\n            {{\r\n                $error.RemoveAt(0)\r\n            }}\r\n\r\n            if (!$endpoint)\r\n            {{\r\n                $qMessage = $queryForRegisterDefault -f \"Microsoft.PowerShell.Workflow\",\"Register-PSSessionConfiguration Microsoft.PowerShell.Workflow -force\"\r\n                if ($force -or $pscmdlet.ShouldProcess($qMessage, $captionForRegisterDefault)) {{\r\n                    $tempxmlfile = [io.path]::Gettempfilename()\r\n                    \"{1}\" | out-file -force -filepath $tempxmlfile -confirm:$false\r\n                    $null = winrm create winrm/config/plugin?Name=Microsoft.PowerShell.Workflow -file:$tempxmlfile\r\n                    remove-item -path $tempxmlfile -force -confirm:$false\r\n                    restart-service winrm -confirm:$false\r\n                }}\r\n            }}\r\n\r\n            $pa = $env:PROCESSOR_ARCHITECTURE\r\n            if ($pa -eq \"x86\")\r\n            {{\r\n                # on 64-bit platforms, wow64 bit process has the correct architecture\r\n                # available in processor_architew6432 varialbe\r\n                $pa = $env:PROCESSOR_ARCHITEW6432\r\n            }}\r\n            if ((($pa -eq \"amd64\")) -and (test-path $env:windir\\syswow64\\pwrshplugin.dll))\r\n            {{\r\n                # Check availability of WOW64 endpoint. Register if not available.\r\n                $errorCount = $error.Count\r\n                $endPoint = Get-PSSessionConfiguration {0}32 -Force:$Force -ErrorAction silentlycontinue 2>&1\r\n                $newErrorCount = $error.Count\r\n\r\n                # remove the 'No Session Configuration matches criteria' errors\r\n                for ($index = 0; $index -lt ($newErrorCount - $errorCount); $index ++)\r\n                {{\r\n                    $error.RemoveAt(0)\r\n                }}\r\n\r\n                $qMessage = $queryForRegisterDefault -f \"{0}32\",\"Register-PSSessionConfiguration {0}32 -processorarchitecture x86 -force\"\r\n                if ((!$endpoint) -and \r\n                    ($force  -or $pscmdlet.ShouldProcess($qMessage, $captionForRegisterDefault)))\r\n                {{\r\n                    $null = Register-PSSessionConfiguration {0}32 -processorarchitecture x86 -force\r\n                    set-item -WarningAction SilentlyContinue wsman:\\localhost\\plugin\\{0}32\\Quotas\\MaxShellsPerUser -value \"25\" -confirm:$false\r\n                    set-item -WarningAction SilentlyContinue wsman:\\localhost\\plugin\\{0}32\\Quotas\\MaxIdleTimeoutms -value {4} -confirm:$false\r\n                    restart-service winrm -confirm:$false\r\n                }}\r\n            }}\r\n\r\n            # remove the 'network deny all' tag\r\n            Get-PSSessionConfiguration -Force:$Force | % {{\r\n                $sddl = $null\r\n                if ($_.psobject.members[\"SecurityDescriptorSddl\"])\r\n                {{\r\n                    $sddl = $_.psobject.members[\"SecurityDescriptorSddl\"].Value\r\n                }}\r\n\r\n                if ($sddl)\r\n                {{\r\n                    # Construct SID for network users\r\n                    [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n                    $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n                    \r\n                    $securityIdentifierToPurge = $null\r\n                    $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$sddl\r\n                    $sd.DiscretionaryAcl | % {{\r\n                        if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                        {{\r\n                            $securityIdentifierToPurge = $_.securityidentifier\r\n                        }}\r\n                    }}\r\n\r\n                    if ($securityIdentifierToPurge)\r\n                    {{\r\n                        # Remove the specific ACE\r\n                        $sd.discretionaryacl.RemoveAccessSpecific('Deny', $securityIdentifierToPurge, 268435456, 'none', 'none')\r\n\r\n                        # if there is no discretionaryacl..add Builtin Administrators and Remote Management Users\r\n                        # to the DACL group as this is the default WSMan behavior\r\n                        if ($sd.discretionaryacl.count -eq 0)\r\n                        {{\r\n                            # Built-in administrators.\r\n                            [system.security.principal.wellknownsidtype]$bast = \"BuiltinAdministratorsSid\"\r\n                            $basid = new-object system.security.principal.securityidentifier $bast,$null\r\n                            $sd.DiscretionaryAcl.AddAccess('Allow',$basid, 268435456, 'none', 'none')\r\n\r\n                            # Remote Management Users, Win8+ only\r\n                            if ([System.Environment]::OSVersion.Version -ge \"6.2.0.0\")\r\n                            {{\r\n                                $rmSidId = new-object system.security.principal.securityidentifier \"{2}\"\r\n                                $sd.DiscretionaryAcl.AddAccess('Allow', $rmSidId, 268435456, 'none', 'none')\r\n                            }}\r\n\r\n                            # Interactive Users\r\n                            $iaSidId = new-object system.security.principal.securityidentifier \"{3}\"\r\n                            $sd.DiscretionaryAcl.AddAccess('Allow', $iaSidId, 268435456, 'none', 'none')\r\n                        }}\r\n\r\n                        $sddl = $sd.GetSddlForm(\"all\")\r\n                    }}\r\n                }} ## end of if($sddl)\r\n\r\n                $qMessage = $queryForSet -f $_.name,$sddl\r\n                if (($sddl) -and ($force -or $pscmdlet.ShouldProcess($qMessage, $captionForSet)))\r\n                {{\r\n                    $null = Set-PSSessionConfiguration -Name $_.Name -SecurityDescriptorSddl $sddl -NoServiceRestart -force -WarningAction 0\r\n                }}\r\n            }} ## end of foreach-object\r\n        }} \r\n        catch {{\r\n            throw\r\n        }}  # end of catch   \r\n    }} # end of end block\r\n}} # end of Enable-PSRemoting\r\n\r\nEnable-PSRemoting -force $args[0] -queryForRegisterDefault $args[1] -captionForRegisterDefault $args[2] -queryForSet $args[3] -captionForSet $args[4] -whatif:$args[5] -confirm:$args[6] -skipNetworkProfileCheck $args[7]\r\n", new object[]
			{
				"Microsoft.PowerShell",
				text,
				"S-1-5-32-580",
				"S-1-5-4",
				"2147483647"
			});
			EnablePSRemotingCommand.enableRemotingSb = ScriptBlock.Create(script);
			EnablePSRemotingCommand.enableRemotingSb.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06002632 RID: 9778 RVA: 0x000D64C1 File Offset: 0x000D46C1
		// (set) Token: 0x06002633 RID: 9779 RVA: 0x000D64CE File Offset: 0x000D46CE
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

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06002634 RID: 9780 RVA: 0x000D64DC File Offset: 0x000D46DC
		// (set) Token: 0x06002635 RID: 9781 RVA: 0x000D64E9 File Offset: 0x000D46E9
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

		// Token: 0x06002636 RID: 9782 RVA: 0x000D64F7 File Offset: 0x000D46F7
		protected override void BeginProcessing()
		{
			RemotingCommandUtil.CheckRemotingCmdletPrerequisites();
			PSSessionConfigurationCommandUtilities.ThrowIfNotAdministrator();
		}

		// Token: 0x06002637 RID: 9783 RVA: 0x000D6504 File Offset: 0x000D4704
		protected override void EndProcessing()
		{
			bool flag = false;
			bool flag2 = true;
			PSSessionConfigurationCommandUtilities.CollectShouldProcessParameters(this, out flag, out flag2);
			string eremotingCaption = RemotingErrorIdStrings.ERemotingCaption;
			string eremotingQuery = RemotingErrorIdStrings.ERemotingQuery;
			string text = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessAction, "Set-PSSessionConfiguration");
			string ecsShouldProcessTarget = RemotingErrorIdStrings.EcsShouldProcessTarget;
			EnablePSRemotingCommand.enableRemotingSb.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[]
			{
				this.force,
				eremotingQuery,
				eremotingCaption,
				ecsShouldProcessTarget,
				text,
				flag,
				flag2,
				this.skipNetworkProfileCheck
			});
		}

		// Token: 0x040012DF RID: 4831
		private const string enableRemotingSbFormat = "\r\nfunction Enable-PSRemoting\r\n{{\r\n[CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Medium\")]\r\nparam(\r\n    [Parameter()] [bool] $Force,\r\n    [Parameter()] [string] $queryForRegisterDefault,    \r\n    [Parameter()] [string] $captionForRegisterDefault,\r\n    [Parameter()] [string] $queryForSet,    \r\n    [Parameter()] [string] $captionForSet,\r\n    [Parameter()] [bool] $skipNetworkProfileCheck\r\n)\r\n\r\n    end\r\n    {{\r\n        # Enable all Session Configurations\r\n        try {{\r\n            $null = $PSBoundParameters.Remove(\"queryForRegisterDefault\")  \r\n            $null = $PSBoundParameters.Remove(\"captionForRegisterDefault\") \r\n            $null = $PSBoundParameters.Remove(\"queryForSet\")  \r\n            $null = $PSBoundParameters.Remove(\"captionForSet\")  \r\n\r\n            $PSBoundParameters.Add(\"Name\",\"*\")\r\n\r\n            # first try to enable all the sessions\r\n            Enable-PSSessionConfiguration @PSBoundParameters\r\n\r\n            # make sure default powershell end points exist \r\n            #  ie., Microsoft.PowerShell\r\n            #       and Microsoft.PowerShell32 (wow64)\r\n            \r\n            $errorCount = $error.Count\r\n            $endPoint = Get-PSSessionConfiguration {0} -Force:$Force -ErrorAction silentlycontinue 2>&1\r\n            $newErrorCount = $error.Count\r\n\r\n            # remove the 'No Session Configuration matches criteria' errors\r\n            for ($index = 0; $index -lt ($newErrorCount - $errorCount); $index ++)\r\n            {{\r\n                $error.RemoveAt(0)\r\n            }}\r\n\r\n            $qMessage = $queryForRegisterDefault -f \"{0}\",\"Register-PSSessionConfiguration {0} -force\"\r\n            if ((!$endpoint) -and \r\n                ($force  -or $pscmdlet.ShouldProcess($qMessage, $captionForRegisterDefault)))\r\n            {{\r\n                $null = Register-PSSessionConfiguration {0} -force\r\n                set-item -WarningAction SilentlyContinue wsman:\\localhost\\plugin\\{0}\\Quotas\\MaxShellsPerUser -value \"25\" -confirm:$false\r\n                set-item -WarningAction SilentlyContinue wsman:\\localhost\\plugin\\{0}\\Quotas\\MaxIdleTimeoutms -value {4} -confirm:$false\r\n                restart-service winrm -confirm:$false\r\n            }}  \r\n\r\n            # Check Microsoft.PowerShell.Workflow endpoint\r\n            $errorCount = $error.Count\r\n            $endPoint = Get-PSSessionConfiguration {0}.workflow -Force:$Force -ErrorAction silentlycontinue 2>&1\r\n            $newErrorCount = $error.Count\r\n\r\n            # remove the 'No Session Configuration matches criteria' errors\r\n            for ($index = 0; $index -lt ($newErrorCount - $errorCount); $index ++)\r\n            {{\r\n                $error.RemoveAt(0)\r\n            }}\r\n\r\n            if (!$endpoint)\r\n            {{\r\n                $qMessage = $queryForRegisterDefault -f \"Microsoft.PowerShell.Workflow\",\"Register-PSSessionConfiguration Microsoft.PowerShell.Workflow -force\"\r\n                if ($force -or $pscmdlet.ShouldProcess($qMessage, $captionForRegisterDefault)) {{\r\n                    $tempxmlfile = [io.path]::Gettempfilename()\r\n                    \"{1}\" | out-file -force -filepath $tempxmlfile -confirm:$false\r\n                    $null = winrm create winrm/config/plugin?Name=Microsoft.PowerShell.Workflow -file:$tempxmlfile\r\n                    remove-item -path $tempxmlfile -force -confirm:$false\r\n                    restart-service winrm -confirm:$false\r\n                }}\r\n            }}\r\n\r\n            $pa = $env:PROCESSOR_ARCHITECTURE\r\n            if ($pa -eq \"x86\")\r\n            {{\r\n                # on 64-bit platforms, wow64 bit process has the correct architecture\r\n                # available in processor_architew6432 varialbe\r\n                $pa = $env:PROCESSOR_ARCHITEW6432\r\n            }}\r\n            if ((($pa -eq \"amd64\")) -and (test-path $env:windir\\syswow64\\pwrshplugin.dll))\r\n            {{\r\n                # Check availability of WOW64 endpoint. Register if not available.\r\n                $errorCount = $error.Count\r\n                $endPoint = Get-PSSessionConfiguration {0}32 -Force:$Force -ErrorAction silentlycontinue 2>&1\r\n                $newErrorCount = $error.Count\r\n\r\n                # remove the 'No Session Configuration matches criteria' errors\r\n                for ($index = 0; $index -lt ($newErrorCount - $errorCount); $index ++)\r\n                {{\r\n                    $error.RemoveAt(0)\r\n                }}\r\n\r\n                $qMessage = $queryForRegisterDefault -f \"{0}32\",\"Register-PSSessionConfiguration {0}32 -processorarchitecture x86 -force\"\r\n                if ((!$endpoint) -and \r\n                    ($force  -or $pscmdlet.ShouldProcess($qMessage, $captionForRegisterDefault)))\r\n                {{\r\n                    $null = Register-PSSessionConfiguration {0}32 -processorarchitecture x86 -force\r\n                    set-item -WarningAction SilentlyContinue wsman:\\localhost\\plugin\\{0}32\\Quotas\\MaxShellsPerUser -value \"25\" -confirm:$false\r\n                    set-item -WarningAction SilentlyContinue wsman:\\localhost\\plugin\\{0}32\\Quotas\\MaxIdleTimeoutms -value {4} -confirm:$false\r\n                    restart-service winrm -confirm:$false\r\n                }}\r\n            }}\r\n\r\n            # remove the 'network deny all' tag\r\n            Get-PSSessionConfiguration -Force:$Force | % {{\r\n                $sddl = $null\r\n                if ($_.psobject.members[\"SecurityDescriptorSddl\"])\r\n                {{\r\n                    $sddl = $_.psobject.members[\"SecurityDescriptorSddl\"].Value\r\n                }}\r\n\r\n                if ($sddl)\r\n                {{\r\n                    # Construct SID for network users\r\n                    [system.security.principal.wellknownsidtype]$evst = \"NetworkSid\"\r\n                    $networkSID = new-object system.security.principal.securityidentifier $evst,$null\r\n                    \r\n                    $securityIdentifierToPurge = $null\r\n                    $sd = new-object system.security.accesscontrol.commonsecuritydescriptor $false,$false,$sddl\r\n                    $sd.DiscretionaryAcl | % {{\r\n                        if (($_.acequalifier -eq \"accessdenied\") -and ($_.securityidentifier -match $networkSID) -and ($_.AccessMask -eq 268435456))\r\n                        {{\r\n                            $securityIdentifierToPurge = $_.securityidentifier\r\n                        }}\r\n                    }}\r\n\r\n                    if ($securityIdentifierToPurge)\r\n                    {{\r\n                        # Remove the specific ACE\r\n                        $sd.discretionaryacl.RemoveAccessSpecific('Deny', $securityIdentifierToPurge, 268435456, 'none', 'none')\r\n\r\n                        # if there is no discretionaryacl..add Builtin Administrators and Remote Management Users\r\n                        # to the DACL group as this is the default WSMan behavior\r\n                        if ($sd.discretionaryacl.count -eq 0)\r\n                        {{\r\n                            # Built-in administrators.\r\n                            [system.security.principal.wellknownsidtype]$bast = \"BuiltinAdministratorsSid\"\r\n                            $basid = new-object system.security.principal.securityidentifier $bast,$null\r\n                            $sd.DiscretionaryAcl.AddAccess('Allow',$basid, 268435456, 'none', 'none')\r\n\r\n                            # Remote Management Users, Win8+ only\r\n                            if ([System.Environment]::OSVersion.Version -ge \"6.2.0.0\")\r\n                            {{\r\n                                $rmSidId = new-object system.security.principal.securityidentifier \"{2}\"\r\n                                $sd.DiscretionaryAcl.AddAccess('Allow', $rmSidId, 268435456, 'none', 'none')\r\n                            }}\r\n\r\n                            # Interactive Users\r\n                            $iaSidId = new-object system.security.principal.securityidentifier \"{3}\"\r\n                            $sd.DiscretionaryAcl.AddAccess('Allow', $iaSidId, 268435456, 'none', 'none')\r\n                        }}\r\n\r\n                        $sddl = $sd.GetSddlForm(\"all\")\r\n                    }}\r\n                }} ## end of if($sddl)\r\n\r\n                $qMessage = $queryForSet -f $_.name,$sddl\r\n                if (($sddl) -and ($force -or $pscmdlet.ShouldProcess($qMessage, $captionForSet)))\r\n                {{\r\n                    $null = Set-PSSessionConfiguration -Name $_.Name -SecurityDescriptorSddl $sddl -NoServiceRestart -force -WarningAction 0\r\n                }}\r\n            }} ## end of foreach-object\r\n        }} \r\n        catch {{\r\n            throw\r\n        }}  # end of catch   \r\n    }} # end of end block\r\n}} # end of Enable-PSRemoting\r\n\r\nEnable-PSRemoting -force $args[0] -queryForRegisterDefault $args[1] -captionForRegisterDefault $args[2] -queryForSet $args[3] -captionForSet $args[4] -whatif:$args[5] -confirm:$args[6] -skipNetworkProfileCheck $args[7]\r\n";

		// Token: 0x040012E0 RID: 4832
		private const string _workflowConfigXml = "\r\n<PlugInConfiguration xmlns='http://schemas.microsoft.com/wbem/wsman/1/config/PluginConfiguration'\r\n    Name='Microsoft.Powershell.Workflow'\r\n    Filename='%windir%\\system32\\pwrshplugin.dll'\r\n    SDKVersion='2'\r\n    XmlRenderingType='text'\r\n    UseSharedProcess='true'\r\n    ProcessIdleTimeoutSec='1209600'\r\n    OutputBufferingMode='Block'\r\n    Enabled='True'\r\n>\r\n<InitializationParameters>\r\n<Param Name='PSVersion'  Value='{0}' />\r\n<Param Name='AssemblyName' Value='Microsoft.PowerShell.Workflow.ServiceCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL'/>\r\n<Param Name='PSSessionConfigurationTypeName' Value='Microsoft.PowerShell.Workflow.PSWorkflowSessionConfiguration'/>\r\n<Param Name='SessionConfigurationData'\r\n    Value ='\r\n        &lt;SessionConfigurationData&gt;\r\n            &lt;Param Name=&quot;ModulesToImport&quot; Value=&quot;%windir%\\system32\\windowspowershell\\v1.0\\Modules\\PSWorkflow&quot; /&gt;\r\n            &lt;Param Name=&quot;PrivateData&quot;&gt;\r\n                &lt;PrivateData&gt;\r\n                    &lt;Param Name=&quot;enablevalidation&quot; Value=&quot;true&quot; /&gt;\r\n                &lt;/PrivateData&gt;\r\n            &lt;/Param&gt;\r\n       &lt;/SessionConfigurationData&gt;\r\n       '\r\n/>\r\n</InitializationParameters> \r\n<Resources>\r\n    <Resource ResourceUri='http://schemas.microsoft.com/powershell/Microsoft.Powershell.Workflow' SupportsOptions='true' ExactMatch='true'>\r\n        <Security Uri='http://schemas.microsoft.com/powershell/Microsoft.PowerShell.Workflow' Sddl='{1}' ExactMatch='False'/>\r\n        <Capability Type='Shell' />        \r\n    </Resource>\r\n</Resources>\r\n<Quotas MaxMemoryPerShellMB='1024' MaxIdleTimeoutms='2147483647' MaxConcurrentUsers='5' IdleTimeoutms='7200000' MaxProcessesPerShell='15' MaxConcurrentCommandsPerShell='1000' MaxShells='25' MaxShellsPerUser='25' />\r\n</PlugInConfiguration>\r\n";

		// Token: 0x040012E1 RID: 4833
		private static ScriptBlock enableRemotingSb;

		// Token: 0x040012E2 RID: 4834
		private bool force;

		// Token: 0x040012E3 RID: 4835
		private bool skipNetworkProfileCheck;
	}
}
