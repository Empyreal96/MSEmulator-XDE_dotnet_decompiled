using System;
using System.Collections.ObjectModel;

namespace System.Management.Automation
{
	// Token: 0x0200002D RID: 45
	public class RemoteCommandInfo : CommandInfo
	{
		// Token: 0x0600021B RID: 539 RVA: 0x00008D74 File Offset: 0x00006F74
		private RemoteCommandInfo(string name, CommandTypes type) : base(name, type)
		{
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600021C RID: 540 RVA: 0x00008D7E File Offset: 0x00006F7E
		public override string Definition
		{
			get
			{
				return this.definition;
			}
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00008D88 File Offset: 0x00006F88
		internal static RemoteCommandInfo FromPSObjectForRemoting(PSObject psObject)
		{
			RemoteCommandInfo remoteCommandInfo = null;
			object psObjectPropertyBaseObject = SerializationUtilities.GetPsObjectPropertyBaseObject(psObject, "CommandInfo_CommandType");
			if (psObjectPropertyBaseObject != null)
			{
				CommandTypes propertyValue = RemotingDecoder.GetPropertyValue<CommandTypes>(psObject, "CommandInfo_CommandType");
				string propertyValue2 = RemotingDecoder.GetPropertyValue<string>(psObject, "CommandInfo_Name");
				remoteCommandInfo = new RemoteCommandInfo(propertyValue2, propertyValue);
				remoteCommandInfo.definition = RemotingDecoder.GetPropertyValue<string>(psObject, "CommandInfo_Definition");
				remoteCommandInfo.Visibility = RemotingDecoder.GetPropertyValue<SessionStateEntryVisibility>(psObject, "CommandInfo_Visibility");
			}
			return remoteCommandInfo;
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00008E28 File Offset: 0x00007028
		internal static void ToPSObjectForRemoting(CommandInfo commandInfo, PSObject psObject)
		{
			if (commandInfo != null)
			{
				RemotingEncoder.AddNoteProperty<CommandTypes>(psObject, "CommandInfo_CommandType", () => commandInfo.CommandType);
				RemotingEncoder.AddNoteProperty<string>(psObject, "CommandInfo_Definition", () => commandInfo.Definition);
				RemotingEncoder.AddNoteProperty<string>(psObject, "CommandInfo_Name", () => commandInfo.Name);
				RemotingEncoder.AddNoteProperty<SessionStateEntryVisibility>(psObject, "CommandInfo_Visibility", () => commandInfo.Visibility);
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00008EC9 File Offset: 0x000070C9
		public override ReadOnlyCollection<PSTypeName> OutputType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040000C9 RID: 201
		private string definition;
	}
}
