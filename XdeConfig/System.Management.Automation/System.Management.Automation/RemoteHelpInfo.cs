using System;
using System.Collections.ObjectModel;
using Microsoft.PowerShell.Commands;

namespace System.Management.Automation
{
	// Token: 0x020001D1 RID: 465
	internal class RemoteHelpInfo : BaseCommandHelpInfo
	{
		// Token: 0x06001577 RID: 5495 RVA: 0x00086F64 File Offset: 0x00085164
		internal RemoteHelpInfo(ExecutionContext context, RemoteRunspace remoteRunspace, string localCommandName, string remoteHelpTopic, string remoteHelpCategory, HelpCategory localHelpCategory) : base(localHelpCategory)
		{
			using (PowerShell powerShell = PowerShell.Create())
			{
				powerShell.AddCommand("Get-Help");
				powerShell.AddParameter("Name", remoteHelpTopic);
				if (!string.IsNullOrEmpty(remoteHelpCategory))
				{
					powerShell.AddParameter("Category", remoteHelpCategory);
				}
				powerShell.Runspace = remoteRunspace;
				Collection<PSObject> collection;
				using (new PowerShellStopper(context, powerShell))
				{
					collection = powerShell.Invoke();
				}
				if (collection == null || collection.Count == 0)
				{
					throw new HelpNotFoundException(remoteHelpTopic);
				}
				this.deserializedRemoteHelp = collection[0];
				this.deserializedRemoteHelp.Methods.Remove("ToString");
				PSPropertyInfo pspropertyInfo = this.deserializedRemoteHelp.Properties["Name"];
				if (pspropertyInfo != null)
				{
					pspropertyInfo.Value = localCommandName;
				}
				PSObject details = base.Details;
				if (details != null)
				{
					pspropertyInfo = details.Properties["Name"];
					if (pspropertyInfo != null)
					{
						pspropertyInfo.Value = localCommandName;
					}
					else
					{
						details.InstanceMembers.Add(new PSNoteProperty("Name", localCommandName));
					}
				}
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x06001578 RID: 5496 RVA: 0x00087090 File Offset: 0x00085290
		internal override PSObject FullHelp
		{
			get
			{
				return this.deserializedRemoteHelp;
			}
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x00087098 File Offset: 0x00085298
		private string GetHelpProperty(string propertyName)
		{
			PSPropertyInfo pspropertyInfo = this.deserializedRemoteHelp.Properties[propertyName];
			if (pspropertyInfo == null)
			{
				return null;
			}
			return pspropertyInfo.Value as string;
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x0600157A RID: 5498 RVA: 0x000870C7 File Offset: 0x000852C7
		internal override string Component
		{
			get
			{
				return this.GetHelpProperty("Component");
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x0600157B RID: 5499 RVA: 0x000870D4 File Offset: 0x000852D4
		internal override string Functionality
		{
			get
			{
				return this.GetHelpProperty("Functionality");
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x0600157C RID: 5500 RVA: 0x000870E1 File Offset: 0x000852E1
		internal override string Role
		{
			get
			{
				return this.GetHelpProperty("Role");
			}
		}

		// Token: 0x04000924 RID: 2340
		private PSObject deserializedRemoteHelp;
	}
}
