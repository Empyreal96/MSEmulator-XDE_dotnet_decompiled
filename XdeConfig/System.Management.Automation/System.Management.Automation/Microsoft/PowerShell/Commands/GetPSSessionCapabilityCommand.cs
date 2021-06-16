using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Internal;
using System.Management.Automation.Runspaces;
using System.Security;
using System.Security.Principal;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000321 RID: 801
	[Cmdlet("Get", "PSSessionCapability", HelpUri = "http://go.microsoft.com/fwlink/?LinkId=623709")]
	[OutputType(new Type[]
	{
		typeof(CommandInfo),
		typeof(InitialSessionState)
	})]
	public sealed class GetPSSessionCapabilityCommand : PSCmdlet
	{
		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x0600263F RID: 9791 RVA: 0x000D66F5 File Offset: 0x000D48F5
		// (set) Token: 0x06002640 RID: 9792 RVA: 0x000D66FD File Offset: 0x000D48FD
		[Parameter(Mandatory = true, Position = 0)]
		public string ConfigurationName { get; set; }

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06002641 RID: 9793 RVA: 0x000D6706 File Offset: 0x000D4906
		// (set) Token: 0x06002642 RID: 9794 RVA: 0x000D670E File Offset: 0x000D490E
		[Parameter(Mandatory = true, Position = 1)]
		public string Username { get; set; }

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06002643 RID: 9795 RVA: 0x000D6717 File Offset: 0x000D4917
		// (set) Token: 0x06002644 RID: 9796 RVA: 0x000D671F File Offset: 0x000D491F
		[Parameter]
		public SwitchParameter Full { get; set; }

		// Token: 0x06002645 RID: 9797 RVA: 0x000D6744 File Offset: 0x000D4944
		protected override void BeginProcessing()
		{
			RemotingCommandUtil.CheckRemotingCmdletPrerequisites();
			PSSessionConfigurationCommandUtilities.ThrowIfNotAdministrator();
			Collection<PSObject> collection = null;
			using (PowerShell powerShell = PowerShell.Create(RunspaceMode.CurrentRunspace))
			{
				powerShell.AddCommand("Get-PSSessionConfiguration").AddParameter("Name", this.ConfigurationName).AddParameter("ErrorAction", "Stop");
				try
				{
					collection = powerShell.Invoke();
				}
				catch (ActionPreferenceStopException ex)
				{
					base.ThrowTerminatingError(new ErrorRecord(ex.ErrorRecord.Exception, "CouldNotFindSessionConfiguration", ErrorCategory.ObjectNotFound, this.ConfigurationName));
				}
			}
			Func<string, bool> roleVerifier = (string role) => true;
			if (!string.IsNullOrEmpty(this.Username))
			{
				if (this.Username.IndexOf("\\", StringComparison.OrdinalIgnoreCase) >= 0)
				{
					roleVerifier = null;
					string[] array = this.Username.Split(new char[]
					{
						'\\'
					});
					if (array.Length == 2)
					{
						this.Username = array[1] + "@" + array[0];
					}
				}
				try
				{
					WindowsPrincipal windowsPrincipal = new WindowsPrincipal(new WindowsIdentity(this.Username));
					roleVerifier = ((string role) => windowsPrincipal.IsInRole(role));
				}
				catch (SecurityException innerException)
				{
					string message = StringUtil.Format(RemotingErrorIdStrings.CouldNotResolveUsername, this.Username);
					ArgumentException exception = new ArgumentException(message, innerException);
					ErrorRecord errorRecord = new ErrorRecord(exception, "CouldNotResolveUsername", ErrorCategory.InvalidArgument, this.Username);
					base.ThrowTerminatingError(errorRecord);
					return;
				}
			}
			foreach (PSObject psobject in collection)
			{
				string text = null;
				PSPropertyInfo pspropertyInfo = psobject.Properties["ConfigFilePath"];
				if (pspropertyInfo != null)
				{
					text = (pspropertyInfo.Value as string);
				}
				if (text == null)
				{
					string o = (string)psobject.Properties["Name"].Value;
					string message2 = StringUtil.Format(RemotingErrorIdStrings.SessionConfigurationMustBeFileBased, o);
					ArgumentException exception2 = new ArgumentException(message2);
					ErrorRecord errorRecord2 = new ErrorRecord(exception2, "SessionConfigurationMustBeFileBased", ErrorCategory.InvalidArgument, psobject);
					base.WriteError(errorRecord2);
				}
				else
				{
					InitialSessionState initialSessionState = InitialSessionState.CreateFromSessionConfigurationFile(text, roleVerifier);
					if (this.Full)
					{
						base.WriteObject(initialSessionState);
					}
					else
					{
						using (PowerShell powerShell2 = PowerShell.Create(initialSessionState))
						{
							powerShell2.AddCommand("Get-Command").AddParameter("CommandType", "All");
							foreach (PSObject sendToPipeline in powerShell2.Invoke())
							{
								base.WriteObject(sendToPipeline);
							}
						}
					}
				}
			}
		}
	}
}
