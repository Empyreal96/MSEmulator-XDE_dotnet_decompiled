using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Threading;

namespace System.Management.Automation
{
	// Token: 0x02000034 RID: 52
	[DataContract]
	public class InformationRecord
	{
		// Token: 0x0600028D RID: 653 RVA: 0x00009CA0 File Offset: 0x00007EA0
		public InformationRecord(object messageData, string source)
		{
			this.MessageData = messageData;
			this.Source = source;
			this.TimeGenerated = DateTime.Now;
			this.Tags = new List<string>();
			this.User = WindowsIdentity.GetCurrent().Name;
			this.Computer = PsUtils.GetHostName();
			this.ProcessId = (uint)Process.GetCurrentProcess().Id;
			this.NativeThreadId = PsUtils.GetNativeThreadId();
			this.ManagedThreadId = (uint)Thread.CurrentThread.ManagedThreadId;
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00009D1D File Offset: 0x00007F1D
		internal InformationRecord()
		{
		}

		// Token: 0x0600028F RID: 655 RVA: 0x00009D28 File Offset: 0x00007F28
		internal InformationRecord(InformationRecord baseRecord)
		{
			this.MessageData = baseRecord.MessageData;
			this.Source = baseRecord.Source;
			this.TimeGenerated = baseRecord.TimeGenerated;
			this.Tags = baseRecord.Tags;
			this.User = baseRecord.User;
			this.Computer = baseRecord.Computer;
			this.ProcessId = baseRecord.ProcessId;
			this.NativeThreadId = baseRecord.NativeThreadId;
			this.ManagedThreadId = baseRecord.ManagedThreadId;
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000290 RID: 656 RVA: 0x00009DA7 File Offset: 0x00007FA7
		// (set) Token: 0x06000291 RID: 657 RVA: 0x00009DAF File Offset: 0x00007FAF
		[DataMember]
		public object MessageData { get; internal set; }

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000292 RID: 658 RVA: 0x00009DB8 File Offset: 0x00007FB8
		// (set) Token: 0x06000293 RID: 659 RVA: 0x00009DC0 File Offset: 0x00007FC0
		[DataMember]
		public string Source { get; set; }

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06000294 RID: 660 RVA: 0x00009DC9 File Offset: 0x00007FC9
		// (set) Token: 0x06000295 RID: 661 RVA: 0x00009DD1 File Offset: 0x00007FD1
		[DataMember]
		public DateTime TimeGenerated { get; set; }

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06000296 RID: 662 RVA: 0x00009DDA File Offset: 0x00007FDA
		// (set) Token: 0x06000297 RID: 663 RVA: 0x00009DE2 File Offset: 0x00007FE2
		[DataMember]
		public List<string> Tags { get; internal set; }

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000298 RID: 664 RVA: 0x00009DEB File Offset: 0x00007FEB
		// (set) Token: 0x06000299 RID: 665 RVA: 0x00009DF3 File Offset: 0x00007FF3
		[DataMember]
		public string User { get; set; }

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x0600029A RID: 666 RVA: 0x00009DFC File Offset: 0x00007FFC
		// (set) Token: 0x0600029B RID: 667 RVA: 0x00009E04 File Offset: 0x00008004
		[DataMember]
		public string Computer { get; set; }

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x0600029C RID: 668 RVA: 0x00009E0D File Offset: 0x0000800D
		// (set) Token: 0x0600029D RID: 669 RVA: 0x00009E15 File Offset: 0x00008015
		[DataMember]
		public uint ProcessId { get; set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00009E1E File Offset: 0x0000801E
		// (set) Token: 0x0600029F RID: 671 RVA: 0x00009E26 File Offset: 0x00008026
		public uint NativeThreadId { get; set; }

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00009E2F File Offset: 0x0000802F
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x00009E37 File Offset: 0x00008037
		[DataMember]
		public uint ManagedThreadId { get; set; }

		// Token: 0x060002A2 RID: 674 RVA: 0x00009E40 File Offset: 0x00008040
		public override string ToString()
		{
			if (this.MessageData != null)
			{
				return this.MessageData.ToString();
			}
			return base.ToString();
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00009E5C File Offset: 0x0000805C
		internal static InformationRecord FromPSObjectForRemoting(PSObject inputObject)
		{
			InformationRecord informationRecord = new InformationRecord();
			informationRecord.MessageData = RemotingDecoder.GetPropertyValue<object>(inputObject, "MessageData");
			informationRecord.Source = RemotingDecoder.GetPropertyValue<string>(inputObject, "Source");
			informationRecord.TimeGenerated = RemotingDecoder.GetPropertyValue<DateTime>(inputObject, "TimeGenerated");
			informationRecord.Tags = new List<string>();
			ArrayList propertyValue = RemotingDecoder.GetPropertyValue<ArrayList>(inputObject, "Tags");
			foreach (object obj in propertyValue)
			{
				string item = (string)obj;
				informationRecord.Tags.Add(item);
			}
			informationRecord.User = RemotingDecoder.GetPropertyValue<string>(inputObject, "User");
			informationRecord.Computer = RemotingDecoder.GetPropertyValue<string>(inputObject, "Computer");
			informationRecord.ProcessId = RemotingDecoder.GetPropertyValue<uint>(inputObject, "ProcessId");
			informationRecord.NativeThreadId = RemotingDecoder.GetPropertyValue<uint>(inputObject, "NativeThreadId");
			informationRecord.ManagedThreadId = RemotingDecoder.GetPropertyValue<uint>(inputObject, "ManagedThreadId");
			return informationRecord;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00009F60 File Offset: 0x00008160
		internal PSObject ToPSObjectForRemoting()
		{
			PSObject psobject = RemotingEncoder.CreateEmptyPSObject();
			psobject.Properties.Add(new PSNoteProperty("MessageData", this.MessageData));
			psobject.Properties.Add(new PSNoteProperty("Source", this.Source));
			psobject.Properties.Add(new PSNoteProperty("TimeGenerated", this.TimeGenerated));
			psobject.Properties.Add(new PSNoteProperty("Tags", this.Tags));
			psobject.Properties.Add(new PSNoteProperty("User", this.User));
			psobject.Properties.Add(new PSNoteProperty("Computer", this.Computer));
			psobject.Properties.Add(new PSNoteProperty("ProcessId", this.ProcessId));
			psobject.Properties.Add(new PSNoteProperty("NativeThreadId", this.NativeThreadId));
			psobject.Properties.Add(new PSNoteProperty("ManagedThreadId", this.ManagedThreadId));
			return psobject;
		}
	}
}
