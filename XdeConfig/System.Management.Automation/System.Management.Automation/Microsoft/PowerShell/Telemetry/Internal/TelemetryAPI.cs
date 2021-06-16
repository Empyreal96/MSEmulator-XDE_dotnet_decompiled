using System;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Language;

namespace Microsoft.PowerShell.Telemetry.Internal
{
	// Token: 0x020008D4 RID: 2260
	public static class TelemetryAPI
	{
		// Token: 0x06005566 RID: 21862 RVA: 0x001C1B38 File Offset: 0x001BFD38
		public static void TraceMessage<T>(string message, T arguments)
		{
			TelemetryWrapper.TraceMessage<T>(message, arguments);
		}

		// Token: 0x06005567 RID: 21863 RVA: 0x001C1D78 File Offset: 0x001BFF78
		internal static void TraceExecutedCommand(CommandInfo commandInfo, CommandOrigin commandOrigin, bool isCommandExecutionSuccessful)
		{
			if (commandOrigin == CommandOrigin.Runspace && !string.IsNullOrEmpty(commandInfo.Name) && (!string.Equals(commandInfo.Name, "prompt", StringComparison.OrdinalIgnoreCase) || commandInfo.CommandType != CommandTypes.Function) && (!string.Equals(commandInfo.Name, "PSConsoleHostReadLine", StringComparison.OrdinalIgnoreCase) || commandInfo.CommandType != CommandTypes.Function) && (!string.Equals(commandInfo.Name, "Out-Default", StringComparison.OrdinalIgnoreCase) || !string.Equals(commandInfo.ModuleName, "Microsoft.PowerShell.Core", StringComparison.OrdinalIgnoreCase)) && (!string.Equals(commandInfo.Name, "Set-StrictMode", StringComparison.OrdinalIgnoreCase) || !string.Equals(commandInfo.ModuleName, "Microsoft.PowerShell.Core", StringComparison.OrdinalIgnoreCase)))
			{
				string moduleVersion = (commandInfo.Version == null) ? string.Empty : commandInfo.Version.ToString();
				TelemetryAPI.TraceMessage("PSCOMMAND_EXECUTE", new
				{
					ModuleName = commandInfo.ModuleName,
					ModuleVersion = moduleVersion,
					CommandName = commandInfo.Name,
					CommandType = commandInfo.CommandType.ToString(),
					IsCommandExecutionSuccessful = isCommandExecutionSuccessful
				});
			}
		}

		// Token: 0x06005568 RID: 21864 RVA: 0x001C1F30 File Offset: 0x001C0130
		internal static void TraceDefinedPowerShellType(TypeDefinitionAst typeDefinitionAst)
		{
			if (typeDefinitionAst.IsClass && typeDefinitionAst.Type != null && string.IsNullOrEmpty(typeDefinitionAst.Type.FullName))
			{
				TelemetryAPI.TraceMessage("PSTYPECLASS_DEFINED", new
				{
					ClassName = typeDefinitionAst.Type.FullName
				});
			}
		}
	}
}
