using System;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation.Remoting
{
	// Token: 0x0200036B RID: 875
	internal sealed class DefaultRemotePowerShellConfiguration : PSSessionConfiguration
	{
		// Token: 0x06002B2B RID: 11051 RVA: 0x000ED7E8 File Offset: 0x000EB9E8
		public override InitialSessionState GetInitialSessionState(PSSenderInfo senderInfo)
		{
			InitialSessionState result = InitialSessionState.CreateDefault2();
			if (senderInfo.ConnectionString != null && senderInfo.ConnectionString.Contains("MSP=7a83d074-bb86-4e52-aa3e-6cc73cc066c8"))
			{
				PSSessionConfigurationData.IsServerManager = true;
			}
			return result;
		}

		// Token: 0x06002B2C RID: 11052 RVA: 0x000ED81C File Offset: 0x000EBA1C
		public override InitialSessionState GetInitialSessionState(PSSessionConfigurationData sessionConfigurationData, PSSenderInfo senderInfo, string configProviderId)
		{
			if (sessionConfigurationData == null)
			{
				throw new ArgumentNullException("sessionConfigurationData");
			}
			if (senderInfo == null)
			{
				throw new ArgumentNullException("senderInfo");
			}
			if (configProviderId == null)
			{
				throw new ArgumentNullException("configProviderId");
			}
			InitialSessionState initialSessionState = InitialSessionState.CreateDefault2();
			if (sessionConfigurationData != null && sessionConfigurationData.ModulesToImportInternal != null)
			{
				foreach (object obj in sessionConfigurationData.ModulesToImportInternal)
				{
					string text = obj as string;
					if (text != null)
					{
						text = Environment.ExpandEnvironmentVariables(text);
						initialSessionState.ImportPSModule(new string[]
						{
							text
						});
					}
					else
					{
						ModuleSpecification moduleSpecification = obj as ModuleSpecification;
						if (moduleSpecification != null)
						{
							Collection<ModuleSpecification> modules = new Collection<ModuleSpecification>
							{
								moduleSpecification
							};
							initialSessionState.ImportPSModule(modules);
						}
					}
				}
			}
			if (senderInfo.ConnectionString != null && senderInfo.ConnectionString.Contains("MSP=7a83d074-bb86-4e52-aa3e-6cc73cc066c8"))
			{
				PSSessionConfigurationData.IsServerManager = true;
			}
			return initialSessionState;
		}
	}
}
