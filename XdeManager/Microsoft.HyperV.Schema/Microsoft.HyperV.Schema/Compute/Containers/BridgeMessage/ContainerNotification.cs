using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Compute.System;

namespace HCS.Compute.Containers.BridgeMessage
{
	// Token: 0x020001B1 RID: 433
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ContainerNotification
	{
		// Token: 0x060006FD RID: 1789 RVA: 0x00016142 File Offset: 0x00014342
		public static bool IsJsonDefault(ContainerNotification val)
		{
			return ContainerNotification._default.JsonEquals(val);
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x00016150 File Offset: 0x00014350
		public bool JsonEquals(object obj)
		{
			ContainerNotification graph = obj as ContainerNotification;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ContainerNotification), new DataContractJsonSerializerSettings
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

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x000161F8 File Offset: 0x000143F8
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x00016212 File Offset: 0x00014412
		[DataMember(Name = "Type")]
		private string _Type
		{
			get
			{
				NotificationType type = this.Type;
				return this.Type.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Type = NotificationType.None;
				}
				this.Type = (NotificationType)Enum.Parse(typeof(NotificationType), value, true);
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0001623F File Offset: 0x0001443F
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x00016259 File Offset: 0x00014459
		[DataMember(Name = "Operation")]
		private string _Operation
		{
			get
			{
				ActiveOperation operation = this.Operation;
				return this.Operation.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Operation = ActiveOperation.None;
				}
				this.Operation = (ActiveOperation)Enum.Parse(typeof(ActiveOperation), value, true);
			}
		}

		// Token: 0x0400099A RID: 2458
		private static readonly ContainerNotification _default = new ContainerNotification();

		// Token: 0x0400099B RID: 2459
		[DataMember]
		public string ContainerId;

		// Token: 0x0400099C RID: 2460
		[DataMember]
		public Guid ActivityId;

		// Token: 0x0400099D RID: 2461
		public NotificationType Type;

		// Token: 0x0400099E RID: 2462
		public ActiveOperation Operation;

		// Token: 0x0400099F RID: 2463
		[DataMember]
		public long Result;

		// Token: 0x040009A0 RID: 2464
		[DataMember(EmitDefaultValue = false)]
		public string ResultInfo;
	}
}
