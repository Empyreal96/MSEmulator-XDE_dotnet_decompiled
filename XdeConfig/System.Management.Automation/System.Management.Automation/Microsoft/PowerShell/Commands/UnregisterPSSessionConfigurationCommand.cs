using System;
using System.Collections;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Tracing;
using System.Security.Principal;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x0200031A RID: 794
	[Cmdlet("Unregister", "PSSessionConfiguration", SupportsShouldProcess = true, ConfirmImpact = ConfirmImpact.Low, HelpUri = "http://go.microsoft.com/fwlink/?LinkID=144308")]
	public sealed class UnregisterPSSessionConfigurationCommand : PSCmdlet
	{
		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x060025F8 RID: 9720 RVA: 0x000D431E File Offset: 0x000D251E
		// (set) Token: 0x060025F9 RID: 9721 RVA: 0x000D4326 File Offset: 0x000D2526
		[Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
		[ValidateNotNullOrEmpty]
		public string Name
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

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x060025FA RID: 9722 RVA: 0x000D432F File Offset: 0x000D252F
		// (set) Token: 0x060025FB RID: 9723 RVA: 0x000D433C File Offset: 0x000D253C
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

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x060025FC RID: 9724 RVA: 0x000D434A File Offset: 0x000D254A
		// (set) Token: 0x060025FD RID: 9725 RVA: 0x000D4357 File Offset: 0x000D2557
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

		// Token: 0x060025FE RID: 9726 RVA: 0x000D4368 File Offset: 0x000D2568
		static UnregisterPSSessionConfigurationCommand()
		{
			string script = string.Format(CultureInfo.InvariantCulture, "\r\nfunction Unregister-PSSessionConfiguration\r\n{{\r\n    [CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Low\")]\r\n    param(\r\n       $filter,\r\n       $action,\r\n       $targetTemplate,\r\n       $shellNotErrMsgFormat,\r\n       [bool]$force)\r\n\r\n    begin\r\n    {{\r\n    }}\r\n\r\n    process\r\n    {{\r\n        $shellsFound = 0\r\n        dir 'WSMan:\\localhost\\Plugin\\' -Force:$force | ? {{ $_.Name -like \"$filter\" }} | % {{\r\n            $pluginFileNamePath = join-path \"$($_.pspath)\" 'FileName'\r\n            if (!(test-path \"$pluginFileNamePath\"))\r\n            {{\r\n                return\r\n            }}\r\n\r\n           $pluginFileName = get-item -literalpath \"$pluginFileNamePath\"\r\n           if ((!$pluginFileName) -or ($pluginFileName.Value -notmatch '{0}'))\r\n           {{\r\n                return  \r\n           }}\r\n           \r\n           $shellsFound++\r\n\r\n           $shouldProcessTargetString = $targetTemplate -f $_.Name\r\n\r\n           $DISCConfigFilePath = [System.IO.Path]::Combine($_.PSPath, \"InitializationParameters\")\r\n           $DISCConfigFile = get-childitem -literalpath \"$DISCConfigFilePath\" | ? {{$_.Name -like \"configFilePath\"}}\r\n        \r\n           if($DISCConfigFile -ne $null)\r\n           {{\r\n               if(test-path -LiteralPath \"$($DISCConfigFile.Value)\") {{                      \r\n                       remove-item -literalpath \"$($DISCConfigFile.Value)\" -recurse -force -confirm:$false\r\n               }}\r\n           }}\r\n \r\n           if($force -or $pscmdlet.ShouldProcess($shouldProcessTargetString, $action))\r\n           {{\r\n                remove-item -literalpath \"$($_.pspath)\" -recurse -force -confirm:$false\r\n           }}\r\n        }}\r\n\r\n        if (!$shellsFound)\r\n        {{\r\n            $errMsg = $shellNotErrMsgFormat -f $filter\r\n            Write-Error $errMsg \r\n        }}\r\n    }} # end of Process block\r\n}}\r\n\r\nif ($args[7] -eq $null)\r\n{{\r\n    Unregister-PSSessionConfiguration -filter $args[0] -whatif:$args[1] -confirm:$args[2] -action $args[3] -targetTemplate $args[4] -shellNotErrMsgFormat $args[5] -force $args[6]\r\n}}\r\nelse\r\n{{\r\n    Unregister-PSSessionConfiguration -filter $args[0] -whatif:$args[1] -confirm:$args[2] -action $args[3] -targetTemplate $args[4] -shellNotErrMsgFormat $args[5] -force $args[6] -erroraction $args[7]\r\n}}\r\n", new object[]
			{
				"pwrshplugin.dll"
			});
			UnregisterPSSessionConfigurationCommand.removePluginSb = ScriptBlock.Create(script);
			UnregisterPSSessionConfigurationCommand.removePluginSb.LanguageMode = new PSLanguageMode?(PSLanguageMode.FullLanguage);
		}

		// Token: 0x060025FF RID: 9727 RVA: 0x000D43B0 File Offset: 0x000D25B0
		protected override void BeginProcessing()
		{
			RemotingCommandUtil.CheckRemotingCmdletPrerequisites();
			PSSessionConfigurationCommandUtilities.ThrowIfNotAdministrator();
		}

		// Token: 0x06002600 RID: 9728 RVA: 0x000D43BC File Offset: 0x000D25BC
		protected override void ProcessRecord()
		{
			base.WriteVerbose(StringUtil.Format(RemotingErrorIdStrings.RcsScriptMessageV, "\r\nfunction Unregister-PSSessionConfiguration\r\n{{\r\n    [CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Low\")]\r\n    param(\r\n       $filter,\r\n       $action,\r\n       $targetTemplate,\r\n       $shellNotErrMsgFormat,\r\n       [bool]$force)\r\n\r\n    begin\r\n    {{\r\n    }}\r\n\r\n    process\r\n    {{\r\n        $shellsFound = 0\r\n        dir 'WSMan:\\localhost\\Plugin\\' -Force:$force | ? {{ $_.Name -like \"$filter\" }} | % {{\r\n            $pluginFileNamePath = join-path \"$($_.pspath)\" 'FileName'\r\n            if (!(test-path \"$pluginFileNamePath\"))\r\n            {{\r\n                return\r\n            }}\r\n\r\n           $pluginFileName = get-item -literalpath \"$pluginFileNamePath\"\r\n           if ((!$pluginFileName) -or ($pluginFileName.Value -notmatch '{0}'))\r\n           {{\r\n                return  \r\n           }}\r\n           \r\n           $shellsFound++\r\n\r\n           $shouldProcessTargetString = $targetTemplate -f $_.Name\r\n\r\n           $DISCConfigFilePath = [System.IO.Path]::Combine($_.PSPath, \"InitializationParameters\")\r\n           $DISCConfigFile = get-childitem -literalpath \"$DISCConfigFilePath\" | ? {{$_.Name -like \"configFilePath\"}}\r\n        \r\n           if($DISCConfigFile -ne $null)\r\n           {{\r\n               if(test-path -LiteralPath \"$($DISCConfigFile.Value)\") {{                      \r\n                       remove-item -literalpath \"$($DISCConfigFile.Value)\" -recurse -force -confirm:$false\r\n               }}\r\n           }}\r\n \r\n           if($force -or $pscmdlet.ShouldProcess($shouldProcessTargetString, $action))\r\n           {{\r\n                remove-item -literalpath \"$($_.pspath)\" -recurse -force -confirm:$false\r\n           }}\r\n        }}\r\n\r\n        if (!$shellsFound)\r\n        {{\r\n            $errMsg = $shellNotErrMsgFormat -f $filter\r\n            Write-Error $errMsg \r\n        }}\r\n    }} # end of Process block\r\n}}\r\n\r\nif ($args[7] -eq $null)\r\n{{\r\n    Unregister-PSSessionConfiguration -filter $args[0] -whatif:$args[1] -confirm:$args[2] -action $args[3] -targetTemplate $args[4] -shellNotErrMsgFormat $args[5] -force $args[6]\r\n}}\r\nelse\r\n{{\r\n    Unregister-PSSessionConfiguration -filter $args[0] -whatif:$args[1] -confirm:$args[2] -action $args[3] -targetTemplate $args[4] -shellNotErrMsgFormat $args[5] -force $args[6] -erroraction $args[7]\r\n}}\r\n"));
			string text = StringUtil.Format(RemotingErrorIdStrings.CSShouldProcessAction, base.CommandInfo.Name);
			string csshouldProcessTarget = RemotingErrorIdStrings.CSShouldProcessTarget;
			string customShellNotFound = RemotingErrorIdStrings.CustomShellNotFound;
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
			UnregisterPSSessionConfigurationCommand.removePluginSb.InvokeUsingCmdlet(this, true, ScriptBlock.ErrorHandlingBehavior.WriteToCurrentErrorPipe, AutomationNull.Value, new object[0], AutomationNull.Value, new object[]
			{
				this.shellName,
				flag,
				flag2,
				text,
				csshouldProcessTarget,
				customShellNotFound,
				this.force,
				obj
			});
			arrayList = (ArrayList)base.Context.DollarErrorVariable;
			this.isErrorReported = (arrayList.Count > count);
		}

		// Token: 0x06002601 RID: 9729 RVA: 0x000D44F0 File Offset: 0x000D26F0
		protected override void EndProcessing()
		{
			Tracer tracer = new Tracer();
			tracer.EndpointUnregistered(this.Name, WindowsIdentity.GetCurrent().Name);
		}

		// Token: 0x040012B0 RID: 4784
		private const string removePluginSbFormat = "\r\nfunction Unregister-PSSessionConfiguration\r\n{{\r\n    [CmdletBinding(SupportsShouldProcess=$true, ConfirmImpact=\"Low\")]\r\n    param(\r\n       $filter,\r\n       $action,\r\n       $targetTemplate,\r\n       $shellNotErrMsgFormat,\r\n       [bool]$force)\r\n\r\n    begin\r\n    {{\r\n    }}\r\n\r\n    process\r\n    {{\r\n        $shellsFound = 0\r\n        dir 'WSMan:\\localhost\\Plugin\\' -Force:$force | ? {{ $_.Name -like \"$filter\" }} | % {{\r\n            $pluginFileNamePath = join-path \"$($_.pspath)\" 'FileName'\r\n            if (!(test-path \"$pluginFileNamePath\"))\r\n            {{\r\n                return\r\n            }}\r\n\r\n           $pluginFileName = get-item -literalpath \"$pluginFileNamePath\"\r\n           if ((!$pluginFileName) -or ($pluginFileName.Value -notmatch '{0}'))\r\n           {{\r\n                return  \r\n           }}\r\n           \r\n           $shellsFound++\r\n\r\n           $shouldProcessTargetString = $targetTemplate -f $_.Name\r\n\r\n           $DISCConfigFilePath = [System.IO.Path]::Combine($_.PSPath, \"InitializationParameters\")\r\n           $DISCConfigFile = get-childitem -literalpath \"$DISCConfigFilePath\" | ? {{$_.Name -like \"configFilePath\"}}\r\n        \r\n           if($DISCConfigFile -ne $null)\r\n           {{\r\n               if(test-path -LiteralPath \"$($DISCConfigFile.Value)\") {{                      \r\n                       remove-item -literalpath \"$($DISCConfigFile.Value)\" -recurse -force -confirm:$false\r\n               }}\r\n           }}\r\n \r\n           if($force -or $pscmdlet.ShouldProcess($shouldProcessTargetString, $action))\r\n           {{\r\n                remove-item -literalpath \"$($_.pspath)\" -recurse -force -confirm:$false\r\n           }}\r\n        }}\r\n\r\n        if (!$shellsFound)\r\n        {{\r\n            $errMsg = $shellNotErrMsgFormat -f $filter\r\n            Write-Error $errMsg \r\n        }}\r\n    }} # end of Process block\r\n}}\r\n\r\nif ($args[7] -eq $null)\r\n{{\r\n    Unregister-PSSessionConfiguration -filter $args[0] -whatif:$args[1] -confirm:$args[2] -action $args[3] -targetTemplate $args[4] -shellNotErrMsgFormat $args[5] -force $args[6]\r\n}}\r\nelse\r\n{{\r\n    Unregister-PSSessionConfiguration -filter $args[0] -whatif:$args[1] -confirm:$args[2] -action $args[3] -targetTemplate $args[4] -shellNotErrMsgFormat $args[5] -force $args[6] -erroraction $args[7]\r\n}}\r\n";

		// Token: 0x040012B1 RID: 4785
		private static readonly ScriptBlock removePluginSb;

		// Token: 0x040012B2 RID: 4786
		private bool isErrorReported;

		// Token: 0x040012B3 RID: 4787
		private string shellName;

		// Token: 0x040012B4 RID: 4788
		private bool force;

		// Token: 0x040012B5 RID: 4789
		private bool noRestart;
	}
}
