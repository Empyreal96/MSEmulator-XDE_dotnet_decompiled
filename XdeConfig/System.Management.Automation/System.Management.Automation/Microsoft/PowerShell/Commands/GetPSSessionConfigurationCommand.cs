using System;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Internal;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200031B RID: 795
	[OutputType(new string[]
	{
		"Microsoft.PowerShell.Commands.PSSessionConfigurationCommands#PSSessionConfiguration"
	})]
	[Cmdlet("Get", "PSSessionConfiguration", HelpUri = "http://go.microsoft.com/fwlink/?LinkID=144304")]
	public sealed class GetPSSessionConfigurationCommand : PSCmdlet
	{
		// Token: 0x06002603 RID: 9731 RVA: 0x000D4524 File Offset: 0x000D2724
		static GetPSSessionConfigurationCommand()
		{
			string script = string.Format(CultureInfo.InvariantCulture, "\r\nfunction ExtractPluginProperties([string]$pluginDir, $objectToWriteTo) \r\n{{\r\n    function Unescape-Xml($s) {{\r\n        if ($s) {{\r\n            $s = $s.Replace(\"&lt;\", \"<\");\r\n            $s = $s.Replace(\"&gt;\", \">\");\r\n            $s = $s.Replace(\"&quot;\", '\"');\r\n            $s = $s.Replace(\"&apos;\", \"'\");\r\n            $s = $s.Replace(\"&amp;\", \"&\");\r\n        }}        \r\n        return $s;\r\n    }}\r\n\r\n    $hashprovider = new-object system.collections.CaseInsensitiveHashCodeProvider\r\n    $comparer=new-object system.collections.CaseInsensitiveComparer\r\n    $h = new-object system.collections.hashtable([System.Collections.IHashCodeProvider]$hashprovider, [System.Collections.IComparer]$comparer)\r\n    \r\n    function Get-Details([string]$path, [hashtable]$h) {{\r\n        foreach ($o in (get-childitem -LiteralPath $path)) {{\r\n            if ($o.PSIsContainer) {{\r\n                Get-Details $o.PSPath $h\r\n            }} else {{\r\n                $h[$o.Name] = $o.Value\r\n            }}\r\n        }}\r\n    }}\r\n        \r\n    Get-Details $pluginDir $h\r\n        \r\n    if ($h[\"AssemblyName\"] -eq \"Microsoft.PowerShell.Workflow.ServiceCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\") {{\r\n        \r\n        $serviceCore = [Reflection.Assembly]::Load(\"Microsoft.Powershell.Workflow.ServiceCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\")\r\n        \r\n        if ($serviceCore -ne $null) {{\r\n\r\n            $ci = new-Object system.management.automation.cmdletinfo \"New-PSWorkflowExecutionOptions\", ([Microsoft.PowerShell.Commands.NewPSWorkflowExecutionOptionCommand])\r\n            $wf = [powershell]::Create(\"currentrunspace\").AddCommand($ci).Invoke()\r\n    \r\n            if($wf -ne $null -and $wf.Count -ne 0) {{              \r\n                $wf = $wf[0]\r\n\r\n                foreach ($o in $wf.GetType().GetProperties()) {{\r\n                    $h[$o.Name] = $o.GetValue($wf, $null)\r\n                }}\r\n            }}\r\n        }}\r\n    }}\r\n\r\n    if (test-path -LiteralPath $pluginDir\\InitializationParameters\\SessionConfigurationData) {{\r\n        $xscd = [xml](Unescape-xml (Unescape-xml (get-item -LiteralPath $pluginDir\\InitializationParameters\\SessionConfigurationData).Value))\r\n\r\n        foreach ($o in $xscd.SessionConfigurationData.Param) {{\r\n            if ($o.Name -eq \"PrivateData\") {{\r\n                foreach($wf in $o.PrivateData.Param) {{\r\n                    $h[$wf.Name] = $wf.Value\r\n                }}\r\n            }} else {{\r\n                $h[$o.Name] = $o.Value\r\n            }}\r\n        }}\r\n    }}\r\n\r\n    ## Extract DISC related information\r\n    if(test-path -LiteralPath $pluginDir\\InitializationParameters\\ConfigFilePath) {{\r\n        $DISCFilePath = (get-item -LiteralPath $pluginDir\\InitializationParameters\\ConfigFilePath).Value\r\n\r\n        if(test-path -LiteralPath $DISCFilePath) {{\r\n            $DISCFileContent = get-content $DISCFilePath | out-string\r\n            $DISCHash = invoke-expression $DISCFileContent\r\n\r\n            foreach ($o in $DISCHash.Keys) {{   \r\n                if ($o -ne \"PowerShellVersion\") {{\r\n                    $objectToWriteTo = $objectToWriteTo | add-member -membertype noteproperty -name $o -value $DISCHash[$o] -force -passthru\r\n                }}\r\n            }}\r\n        }}\r\n    }}\r\n\r\n    if ($h[\"SessionConfigurationData\"]) {{\r\n        $h[\"SessionConfigurationData\"] = Unescape-Xml (Unescape-Xml $h[\"SessionConfigurationData\"])\r\n    }}\r\n\r\n    foreach ($o in $h.Keys) {{\r\n        if ($o -eq 'sddl') {{\r\n            $objectToWriteTo = $objectToWriteTo | add-member -membertype noteproperty -name 'SecurityDescriptorSddl' -value $h[$o] -force -passthru\r\n        }} else {{\r\n            $objectToWriteTo = $objectToWriteTo | add-member -membertype noteproperty -name $o -value $h[$o] -force -passthru\r\n        }}\r\n    }}\r\n}}\r\n\r\n$shellNotErrMsgFormat = $args[1]\r\n$force = $args[2]\r\n$args[0] | foreach {{\r\n  $shellsFound = 0;\r\n  $filter = $_\r\n  dir 'WSMan:\\localhost\\Plugin\\' -Force:$force | ? {{ $_.name -like \"$filter\" }} | foreach {{\r\n     $customPluginObject = new-object object     \r\n     $customPluginObject.pstypenames.Insert(0, '{0}')\r\n     ExtractPluginProperties \"$($_.PSPath)\" $customPluginObject\r\n     # this is powershell based custom shell only if its plugin dll is pwrshplugin.dll\r\n     if (($customPluginObject.FileName) -and ($customPluginObject.FileName -match '{1}'))\r\n     {{\r\n        $shellsFound++\r\n        $customPluginObject\r\n     }}\r\n    }} # end of foreach\r\n   \r\n    if (!$shellsFound -and !([System.Management.Automation.WildcardPattern]::ContainsWildcardCharacters($_)))\r\n    {{\r\n      $errMsg = $shellNotErrMsgFormat -f $_\r\n      Write-Error $errMsg \r\n    }}     \r\n  }}\r\n", new object[]
			{
				"Microsoft.PowerShell.Commands.PSSessionConfigurationCommands#PSSessionConfiguration",
				"pwrshplugin.dll"
			});
			GetPSSessionConfigurationCommand.getPluginSb = ScriptBlock.Create(script);
			GetPSSessionConfigurationCommand.getPluginSb.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06002604 RID: 9732 RVA: 0x000D4574 File Offset: 0x000D2774
		// (set) Token: 0x06002605 RID: 9733 RVA: 0x000D457C File Offset: 0x000D277C
		[Parameter(Position = 0, Mandatory = false)]
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

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06002606 RID: 9734 RVA: 0x000D4585 File Offset: 0x000D2785
		// (set) Token: 0x06002607 RID: 9735 RVA: 0x000D4592 File Offset: 0x000D2792
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

		// Token: 0x06002608 RID: 9736 RVA: 0x000D45A0 File Offset: 0x000D27A0
		protected override void BeginProcessing()
		{
			RemotingCommandUtil.CheckRemotingCmdletPrerequisites();
			PSSessionConfigurationCommandUtilities.ThrowIfNotAdministrator();
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x000D45AC File Offset: 0x000D27AC
		protected override void ProcessRecord()
		{
			base.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.GcsScriptMessageV, "\r\nfunction ExtractPluginProperties([string]$pluginDir, $objectToWriteTo) \r\n{{\r\n    function Unescape-Xml($s) {{\r\n        if ($s) {{\r\n            $s = $s.Replace(\"&lt;\", \"<\");\r\n            $s = $s.Replace(\"&gt;\", \">\");\r\n            $s = $s.Replace(\"&quot;\", '\"');\r\n            $s = $s.Replace(\"&apos;\", \"'\");\r\n            $s = $s.Replace(\"&amp;\", \"&\");\r\n        }}        \r\n        return $s;\r\n    }}\r\n\r\n    $hashprovider = new-object system.collections.CaseInsensitiveHashCodeProvider\r\n    $comparer=new-object system.collections.CaseInsensitiveComparer\r\n    $h = new-object system.collections.hashtable([System.Collections.IHashCodeProvider]$hashprovider, [System.Collections.IComparer]$comparer)\r\n    \r\n    function Get-Details([string]$path, [hashtable]$h) {{\r\n        foreach ($o in (get-childitem -LiteralPath $path)) {{\r\n            if ($o.PSIsContainer) {{\r\n                Get-Details $o.PSPath $h\r\n            }} else {{\r\n                $h[$o.Name] = $o.Value\r\n            }}\r\n        }}\r\n    }}\r\n        \r\n    Get-Details $pluginDir $h\r\n        \r\n    if ($h[\"AssemblyName\"] -eq \"Microsoft.PowerShell.Workflow.ServiceCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\") {{\r\n        \r\n        $serviceCore = [Reflection.Assembly]::Load(\"Microsoft.Powershell.Workflow.ServiceCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\")\r\n        \r\n        if ($serviceCore -ne $null) {{\r\n\r\n            $ci = new-Object system.management.automation.cmdletinfo \"New-PSWorkflowExecutionOptions\", ([Microsoft.PowerShell.Commands.NewPSWorkflowExecutionOptionCommand])\r\n            $wf = [powershell]::Create(\"currentrunspace\").AddCommand($ci).Invoke()\r\n    \r\n            if($wf -ne $null -and $wf.Count -ne 0) {{              \r\n                $wf = $wf[0]\r\n\r\n                foreach ($o in $wf.GetType().GetProperties()) {{\r\n                    $h[$o.Name] = $o.GetValue($wf, $null)\r\n                }}\r\n            }}\r\n        }}\r\n    }}\r\n\r\n    if (test-path -LiteralPath $pluginDir\\InitializationParameters\\SessionConfigurationData) {{\r\n        $xscd = [xml](Unescape-xml (Unescape-xml (get-item -LiteralPath $pluginDir\\InitializationParameters\\SessionConfigurationData).Value))\r\n\r\n        foreach ($o in $xscd.SessionConfigurationData.Param) {{\r\n            if ($o.Name -eq \"PrivateData\") {{\r\n                foreach($wf in $o.PrivateData.Param) {{\r\n                    $h[$wf.Name] = $wf.Value\r\n                }}\r\n            }} else {{\r\n                $h[$o.Name] = $o.Value\r\n            }}\r\n        }}\r\n    }}\r\n\r\n    ## Extract DISC related information\r\n    if(test-path -LiteralPath $pluginDir\\InitializationParameters\\ConfigFilePath) {{\r\n        $DISCFilePath = (get-item -LiteralPath $pluginDir\\InitializationParameters\\ConfigFilePath).Value\r\n\r\n        if(test-path -LiteralPath $DISCFilePath) {{\r\n            $DISCFileContent = get-content $DISCFilePath | out-string\r\n            $DISCHash = invoke-expression $DISCFileContent\r\n\r\n            foreach ($o in $DISCHash.Keys) {{   \r\n                if ($o -ne \"PowerShellVersion\") {{\r\n                    $objectToWriteTo = $objectToWriteTo | add-member -membertype noteproperty -name $o -value $DISCHash[$o] -force -passthru\r\n                }}\r\n            }}\r\n        }}\r\n    }}\r\n\r\n    if ($h[\"SessionConfigurationData\"]) {{\r\n        $h[\"SessionConfigurationData\"] = Unescape-Xml (Unescape-Xml $h[\"SessionConfigurationData\"])\r\n    }}\r\n\r\n    foreach ($o in $h.Keys) {{\r\n        if ($o -eq 'sddl') {{\r\n            $objectToWriteTo = $objectToWriteTo | add-member -membertype noteproperty -name 'SecurityDescriptorSddl' -value $h[$o] -force -passthru\r\n        }} else {{\r\n            $objectToWriteTo = $objectToWriteTo | add-member -membertype noteproperty -name $o -value $h[$o] -force -passthru\r\n        }}\r\n    }}\r\n}}\r\n\r\n$shellNotErrMsgFormat = $args[1]\r\n$force = $args[2]\r\n$args[0] | foreach {{\r\n  $shellsFound = 0;\r\n  $filter = $_\r\n  dir 'WSMan:\\localhost\\Plugin\\' -Force:$force | ? {{ $_.name -like \"$filter\" }} | foreach {{\r\n     $customPluginObject = new-object object     \r\n     $customPluginObject.pstypenames.Insert(0, '{0}')\r\n     ExtractPluginProperties \"$($_.PSPath)\" $customPluginObject\r\n     # this is powershell based custom shell only if its plugin dll is pwrshplugin.dll\r\n     if (($customPluginObject.FileName) -and ($customPluginObject.FileName -match '{1}'))\r\n     {{\r\n        $shellsFound++\r\n        $customPluginObject\r\n     }}\r\n    }} # end of foreach\r\n   \r\n    if (!$shellsFound -and !([System.Management.Automation.WildcardPattern]::ContainsWildcardCharacters($_)))\r\n    {{\r\n      $errMsg = $shellNotErrMsgFormat -f $_\r\n      Write-Error $errMsg \r\n    }}     \r\n  }}\r\n"));
			string customShellNotFound = RemotingErrorIdStrings.CustomShellNotFound;
			object obj = "*";
			if (this.shellName != null)
			{
				obj = this.shellName;
			}
			ActionPreference errorActionPreferenceVariable = base.Context.ErrorActionPreferenceVariable;
			try
			{
				if (base.Context.CurrentCommandProcessor.CommandRuntime.IsErrorActionSet)
				{
					base.Context.ErrorActionPreferenceVariable = base.Context.CurrentCommandProcessor.CommandRuntime.ErrorAction;
				}
				GetPSSessionConfigurationCommand.getPluginSb.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[]
				{
					obj,
					customShellNotFound,
					this.force
				});
			}
			finally
			{
				base.Context.ErrorActionPreferenceVariable = errorActionPreferenceVariable;
			}
		}

		// Token: 0x040012B6 RID: 4790
		private const string getPluginSbFormat = "\r\nfunction ExtractPluginProperties([string]$pluginDir, $objectToWriteTo) \r\n{{\r\n    function Unescape-Xml($s) {{\r\n        if ($s) {{\r\n            $s = $s.Replace(\"&lt;\", \"<\");\r\n            $s = $s.Replace(\"&gt;\", \">\");\r\n            $s = $s.Replace(\"&quot;\", '\"');\r\n            $s = $s.Replace(\"&apos;\", \"'\");\r\n            $s = $s.Replace(\"&amp;\", \"&\");\r\n        }}        \r\n        return $s;\r\n    }}\r\n\r\n    $hashprovider = new-object system.collections.CaseInsensitiveHashCodeProvider\r\n    $comparer=new-object system.collections.CaseInsensitiveComparer\r\n    $h = new-object system.collections.hashtable([System.Collections.IHashCodeProvider]$hashprovider, [System.Collections.IComparer]$comparer)\r\n    \r\n    function Get-Details([string]$path, [hashtable]$h) {{\r\n        foreach ($o in (get-childitem -LiteralPath $path)) {{\r\n            if ($o.PSIsContainer) {{\r\n                Get-Details $o.PSPath $h\r\n            }} else {{\r\n                $h[$o.Name] = $o.Value\r\n            }}\r\n        }}\r\n    }}\r\n        \r\n    Get-Details $pluginDir $h\r\n        \r\n    if ($h[\"AssemblyName\"] -eq \"Microsoft.PowerShell.Workflow.ServiceCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\") {{\r\n        \r\n        $serviceCore = [Reflection.Assembly]::Load(\"Microsoft.Powershell.Workflow.ServiceCore, Version=3.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35, processorArchitecture=MSIL\")\r\n        \r\n        if ($serviceCore -ne $null) {{\r\n\r\n            $ci = new-Object system.management.automation.cmdletinfo \"New-PSWorkflowExecutionOptions\", ([Microsoft.PowerShell.Commands.NewPSWorkflowExecutionOptionCommand])\r\n            $wf = [powershell]::Create(\"currentrunspace\").AddCommand($ci).Invoke()\r\n    \r\n            if($wf -ne $null -and $wf.Count -ne 0) {{              \r\n                $wf = $wf[0]\r\n\r\n                foreach ($o in $wf.GetType().GetProperties()) {{\r\n                    $h[$o.Name] = $o.GetValue($wf, $null)\r\n                }}\r\n            }}\r\n        }}\r\n    }}\r\n\r\n    if (test-path -LiteralPath $pluginDir\\InitializationParameters\\SessionConfigurationData) {{\r\n        $xscd = [xml](Unescape-xml (Unescape-xml (get-item -LiteralPath $pluginDir\\InitializationParameters\\SessionConfigurationData).Value))\r\n\r\n        foreach ($o in $xscd.SessionConfigurationData.Param) {{\r\n            if ($o.Name -eq \"PrivateData\") {{\r\n                foreach($wf in $o.PrivateData.Param) {{\r\n                    $h[$wf.Name] = $wf.Value\r\n                }}\r\n            }} else {{\r\n                $h[$o.Name] = $o.Value\r\n            }}\r\n        }}\r\n    }}\r\n\r\n    ## Extract DISC related information\r\n    if(test-path -LiteralPath $pluginDir\\InitializationParameters\\ConfigFilePath) {{\r\n        $DISCFilePath = (get-item -LiteralPath $pluginDir\\InitializationParameters\\ConfigFilePath).Value\r\n\r\n        if(test-path -LiteralPath $DISCFilePath) {{\r\n            $DISCFileContent = get-content $DISCFilePath | out-string\r\n            $DISCHash = invoke-expression $DISCFileContent\r\n\r\n            foreach ($o in $DISCHash.Keys) {{   \r\n                if ($o -ne \"PowerShellVersion\") {{\r\n                    $objectToWriteTo = $objectToWriteTo | add-member -membertype noteproperty -name $o -value $DISCHash[$o] -force -passthru\r\n                }}\r\n            }}\r\n        }}\r\n    }}\r\n\r\n    if ($h[\"SessionConfigurationData\"]) {{\r\n        $h[\"SessionConfigurationData\"] = Unescape-Xml (Unescape-Xml $h[\"SessionConfigurationData\"])\r\n    }}\r\n\r\n    foreach ($o in $h.Keys) {{\r\n        if ($o -eq 'sddl') {{\r\n            $objectToWriteTo = $objectToWriteTo | add-member -membertype noteproperty -name 'SecurityDescriptorSddl' -value $h[$o] -force -passthru\r\n        }} else {{\r\n            $objectToWriteTo = $objectToWriteTo | add-member -membertype noteproperty -name $o -value $h[$o] -force -passthru\r\n        }}\r\n    }}\r\n}}\r\n\r\n$shellNotErrMsgFormat = $args[1]\r\n$force = $args[2]\r\n$args[0] | foreach {{\r\n  $shellsFound = 0;\r\n  $filter = $_\r\n  dir 'WSMan:\\localhost\\Plugin\\' -Force:$force | ? {{ $_.name -like \"$filter\" }} | foreach {{\r\n     $customPluginObject = new-object object     \r\n     $customPluginObject.pstypenames.Insert(0, '{0}')\r\n     ExtractPluginProperties \"$($_.PSPath)\" $customPluginObject\r\n     # this is powershell based custom shell only if its plugin dll is pwrshplugin.dll\r\n     if (($customPluginObject.FileName) -and ($customPluginObject.FileName -match '{1}'))\r\n     {{\r\n        $shellsFound++\r\n        $customPluginObject\r\n     }}\r\n    }} # end of foreach\r\n   \r\n    if (!$shellsFound -and !([System.Management.Automation.WildcardPattern]::ContainsWildcardCharacters($_)))\r\n    {{\r\n      $errMsg = $shellNotErrMsgFormat -f $_\r\n      Write-Error $errMsg \r\n    }}     \r\n  }}\r\n";

		// Token: 0x040012B7 RID: 4791
		private const string MODULEPATH = "ModulesToImport";

		// Token: 0x040012B8 RID: 4792
		private static readonly ScriptBlock getPluginSb;

		// Token: 0x040012B9 RID: 4793
		private string[] shellName;

		// Token: 0x040012BA RID: 4794
		private bool force;
	}
}
