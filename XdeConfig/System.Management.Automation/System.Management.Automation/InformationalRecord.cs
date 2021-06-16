using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x02000204 RID: 516
	[DataContract]
	public abstract class InformationalRecord
	{
		// Token: 0x060017F6 RID: 6134 RVA: 0x00094158 File Offset: 0x00092358
		internal InformationalRecord(string message)
		{
			this.message = message;
			this.invocationInfo = null;
			this.pipelineIterationInfo = null;
			this.serializeExtendedInfo = false;
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x0009417C File Offset: 0x0009237C
		internal InformationalRecord(PSObject serializedObject)
		{
			this.message = (string)SerializationUtilities.GetPropertyValue(serializedObject, "InformationalRecord_Message");
			this.serializeExtendedInfo = (bool)SerializationUtilities.GetPropertyValue(serializedObject, "InformationalRecord_SerializeInvocationInfo");
			if (this.serializeExtendedInfo)
			{
				this.invocationInfo = new InvocationInfo(serializedObject);
				ArrayList arrayList = (ArrayList)SerializationUtilities.GetPsObjectPropertyBaseObject(serializedObject, "InformationalRecord_PipelineIterationInfo");
				this.pipelineIterationInfo = new ReadOnlyCollection<int>((int[])arrayList.ToArray(typeof(int)));
				return;
			}
			this.invocationInfo = null;
		}

		// Token: 0x170005DC RID: 1500
		// (get) Token: 0x060017F8 RID: 6136 RVA: 0x00094208 File Offset: 0x00092408
		// (set) Token: 0x060017F9 RID: 6137 RVA: 0x00094210 File Offset: 0x00092410
		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		// Token: 0x170005DD RID: 1501
		// (get) Token: 0x060017FA RID: 6138 RVA: 0x00094219 File Offset: 0x00092419
		public InvocationInfo InvocationInfo
		{
			get
			{
				return this.invocationInfo;
			}
		}

		// Token: 0x170005DE RID: 1502
		// (get) Token: 0x060017FB RID: 6139 RVA: 0x00094221 File Offset: 0x00092421
		public ReadOnlyCollection<int> PipelineIterationInfo
		{
			get
			{
				return this.pipelineIterationInfo;
			}
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x0009422C File Offset: 0x0009242C
		internal void SetInvocationInfo(InvocationInfo invocationInfo)
		{
			this.invocationInfo = invocationInfo;
			if (invocationInfo.PipelineIterationInfo != null)
			{
				int[] list = (int[])invocationInfo.PipelineIterationInfo.Clone();
				this.pipelineIterationInfo = new ReadOnlyCollection<int>(list);
			}
		}

		// Token: 0x170005DF RID: 1503
		// (get) Token: 0x060017FD RID: 6141 RVA: 0x00094265 File Offset: 0x00092465
		// (set) Token: 0x060017FE RID: 6142 RVA: 0x0009426D File Offset: 0x0009246D
		internal bool SerializeExtendedInfo
		{
			get
			{
				return this.serializeExtendedInfo;
			}
			set
			{
				this.serializeExtendedInfo = value;
			}
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x00094276 File Offset: 0x00092476
		public override string ToString()
		{
			return this.Message;
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x00094294 File Offset: 0x00092494
		internal virtual void ToPSObjectForRemoting(PSObject psObject)
		{
			RemotingEncoder.AddNoteProperty<string>(psObject, "InformationalRecord_Message", () => this.Message);
			if (!this.SerializeExtendedInfo || this.invocationInfo == null)
			{
				RemotingEncoder.AddNoteProperty<bool>(psObject, "InformationalRecord_SerializeInvocationInfo", () => false);
				return;
			}
			RemotingEncoder.AddNoteProperty<bool>(psObject, "InformationalRecord_SerializeInvocationInfo", () => true);
			this.invocationInfo.ToPSObjectForRemoting(psObject);
			RemotingEncoder.AddNoteProperty<object>(psObject, "InformationalRecord_PipelineIterationInfo", () => this.PipelineIterationInfo);
		}

		// Token: 0x04000A1B RID: 2587
		[DataMember]
		private string message;

		// Token: 0x04000A1C RID: 2588
		private InvocationInfo invocationInfo;

		// Token: 0x04000A1D RID: 2589
		private ReadOnlyCollection<int> pipelineIterationInfo;

		// Token: 0x04000A1E RID: 2590
		private bool serializeExtendedInfo;
	}
}
