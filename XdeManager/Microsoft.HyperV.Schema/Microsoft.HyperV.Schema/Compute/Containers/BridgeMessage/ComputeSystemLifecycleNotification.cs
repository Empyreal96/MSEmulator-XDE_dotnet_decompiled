using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001B8 RID: 440
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ComputeSystemLifecycleNotification
	{
		// Token: 0x0600071F RID: 1823 RVA: 0x0001677C File Offset: 0x0001497C
		public static bool IsJsonDefault(ComputeSystemLifecycleNotification val)
		{
			return ComputeSystemLifecycleNotification._default.JsonEquals(val);
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0001678C File Offset: 0x0001498C
		public bool JsonEquals(object obj)
		{
			ComputeSystemLifecycleNotification graph = obj as ComputeSystemLifecycleNotification;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ComputeSystemLifecycleNotification), new DataContractJsonSerializerSettings
			{
				UseSimpleDictionaryFormat = true
			});
			bool result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (MemoryStream memoryStream2 = new MemoryStream())
				{
					dataContractJsonSerializer.WriteObject(memoryStream, this);
					dataContractJsonSerializer.WriteObject(memoryStream2, graph);
					result = (Encoding.ASCII.GetString(memoryStream.ToArray()) == Encoding.ASCII.GetString(memoryStream2.ToArray()));
				}
			}
			return result;
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x00016834 File Offset: 0x00014A34
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x0001684E File Offset: 0x00014A4E
		[DataMember(Name = "Notification")]
		private string _Notification
		{
			get
			{
				LifecycleNotification notification = this.Notification;
				return this.Notification.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Notification = LifecycleNotification.None;
				}
				this.Notification = (LifecycleNotification)Enum.Parse(typeof(LifecycleNotification), value, true);
			}
		}

		// Token: 0x040009BE RID: 2494
		private static readonly ComputeSystemLifecycleNotification _default = new ComputeSystemLifecycleNotification();

		// Token: 0x040009BF RID: 2495
		[DataMember]
		public string ContainerId;

		// Token: 0x040009C0 RID: 2496
		[DataMember]
		public Guid ActivityId;

		// Token: 0x040009C1 RID: 2497
		public LifecycleNotification Notification;

		// Token: 0x040009C2 RID: 2498
		[DataMember(EmitDefaultValue = false)]
		public object NotificationData;
	}
}
