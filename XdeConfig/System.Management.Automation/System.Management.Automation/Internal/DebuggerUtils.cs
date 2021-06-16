using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Management.Automation.Internal
{
	// Token: 0x020000FA RID: 250
	public static class DebuggerUtils
	{
		// Token: 0x06000DE0 RID: 3552 RVA: 0x0004B9C0 File Offset: 0x00049BC0
		public static bool ShouldAddCommandToHistory(string command)
		{
			if (command == null)
			{
				throw new PSArgumentNullException("command");
			}
			bool result;
			lock (DebuggerUtils._noHistoryCommandNames)
			{
				result = !DebuggerUtils._noHistoryCommandNames.Contains(command, StringComparer.OrdinalIgnoreCase);
			}
			return result;
		}

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0004BA1C File Offset: 0x00049C1C
		public static IEnumerable<string> GetWorkflowDebuggerFunctions()
		{
			return new Collection<string>
			{
				"function Set-DebuggerVariable\r\n        {\r\n            [CmdletBinding()]\r\n            param(\r\n                [Parameter(Position=0)]\r\n                [HashTable]\r\n                $Variables\r\n            )\r\n\r\n            foreach($key in $Variables.Keys)\r\n            {\r\n                microsoft.powershell.utility\\set-variable -Name $key -Value $Variables[$key] -Scope global\r\n            }\r\n    \r\n            Set-StrictMode -Off\r\n        }",
				"function Remove-DebuggerVariable\r\n        {\r\n            [CmdletBinding()]\r\n            param(\r\n                [Parameter(Position=0)]\r\n                [string[]]\r\n                $Name\r\n            )\r\n\r\n            foreach ($item in $Name)\r\n            {\r\n                microsoft.powershell.utility\\remove-variable -name $item -scope global\r\n            }\r\n\r\n            Set-StrictMode -Off\r\n        }",
				"function Get-PSCallStack\r\n        {\r\n            [CmdletBinding()]\r\n            param()\r\n\r\n            if ($PSWorkflowDebugger -ne $null)\r\n            {\r\n                foreach ($frame in $PSWorkflowDebugger.GetCallStack())\r\n                {\r\n                    Write-Output $frame\r\n                }\r\n            }\r\n\r\n            Set-StrictMode -Off\r\n        }"
			};
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0004BA51 File Offset: 0x00049C51
		public static void StartMonitoringRunspace(Debugger debugger, PSMonitorRunspaceInfo runspaceInfo)
		{
			if (debugger == null)
			{
				throw new PSArgumentNullException("debugger");
			}
			if (runspaceInfo == null)
			{
				throw new PSArgumentNullException("runspaceInfo");
			}
			debugger.StartMonitoringRunspace(runspaceInfo);
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x0004BA76 File Offset: 0x00049C76
		public static void EndMonitoringRunspace(Debugger debugger, PSMonitorRunspaceInfo runspaceInfo)
		{
			if (debugger == null)
			{
				throw new PSArgumentNullException("debugger");
			}
			if (runspaceInfo == null)
			{
				throw new PSArgumentNullException("runspaceInfo");
			}
			debugger.EndMonitoringRunspace(runspaceInfo);
		}

		// Token: 0x0400062F RID: 1583
		public const string SetVariableFunction = "function Set-DebuggerVariable\r\n        {\r\n            [CmdletBinding()]\r\n            param(\r\n                [Parameter(Position=0)]\r\n                [HashTable]\r\n                $Variables\r\n            )\r\n\r\n            foreach($key in $Variables.Keys)\r\n            {\r\n                microsoft.powershell.utility\\set-variable -Name $key -Value $Variables[$key] -Scope global\r\n            }\r\n    \r\n            Set-StrictMode -Off\r\n        }";

		// Token: 0x04000630 RID: 1584
		public const string RemoveVariableFunction = "function Remove-DebuggerVariable\r\n        {\r\n            [CmdletBinding()]\r\n            param(\r\n                [Parameter(Position=0)]\r\n                [string[]]\r\n                $Name\r\n            )\r\n\r\n            foreach ($item in $Name)\r\n            {\r\n                microsoft.powershell.utility\\remove-variable -name $item -scope global\r\n            }\r\n\r\n            Set-StrictMode -Off\r\n        }";

		// Token: 0x04000631 RID: 1585
		public const string GetPSCallStackOverrideFunction = "function Get-PSCallStack\r\n        {\r\n            [CmdletBinding()]\r\n            param()\r\n\r\n            if ($PSWorkflowDebugger -ne $null)\r\n            {\r\n                foreach ($frame in $PSWorkflowDebugger.GetCallStack())\r\n                {\r\n                    Write-Output $frame\r\n                }\r\n            }\r\n\r\n            Set-StrictMode -Off\r\n        }";

		// Token: 0x04000632 RID: 1586
		internal const string SetDebugModeFunctionName = "__Set-PSDebugMode";

		// Token: 0x04000633 RID: 1587
		internal const string SetDebuggerActionFunctionName = "__Set-PSDebuggerAction";

		// Token: 0x04000634 RID: 1588
		internal const string GetDebuggerStopArgsFunctionName = "__Get-PSDebuggerStopArgs";

		// Token: 0x04000635 RID: 1589
		internal const string SetDebuggerStepMode = "__Set-PSDebuggerStepMode";

		// Token: 0x04000636 RID: 1590
		internal const string SetPSUnhandledBreakpointMode = "__Set-PSUnhandledBreakpointMode";

		// Token: 0x04000637 RID: 1591
		private static SortedSet<string> _noHistoryCommandNames = new SortedSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"prompt",
			"Set-PSDebuggerAction",
			"Get-PSDebuggerStopArgs",
			"Set-PSDebugMode",
			"TabExpansion2"
		};
	}
}
