using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Memory
{
	// Token: 0x020000CD RID: 205
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HostMemoryModificationRequest
	{
		// Token: 0x0600031B RID: 795 RVA: 0x0000B23C File Offset: 0x0000943C
		public static bool IsJsonDefault(HostMemoryModificationRequest val)
		{
			return HostMemoryModificationRequest._default.JsonEquals(val);
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000B24C File Offset: 0x0000944C
		public bool JsonEquals(object obj)
		{
			HostMemoryModificationRequest graph = obj as HostMemoryModificationRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HostMemoryModificationRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0000B2F4 File Offset: 0x000094F4
		// (set) Token: 0x0600031E RID: 798 RVA: 0x0000B30E File Offset: 0x0000950E
		[DataMember(Name = "Operation")]
		private string _Operation
		{
			get
			{
				HostMemoryOperation operation = this.Operation;
				return this.Operation.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Operation = HostMemoryOperation.MemoryReserveGrow;
				}
				this.Operation = (HostMemoryOperation)Enum.Parse(typeof(HostMemoryOperation), value, true);
			}
		}

		// Token: 0x040003F7 RID: 1015
		private static readonly HostMemoryModificationRequest _default = new HostMemoryModificationRequest();

		// Token: 0x040003F8 RID: 1016
		public HostMemoryOperation Operation;

		// Token: 0x040003F9 RID: 1017
		[DataMember]
		public object OperationDetails;
	}
}
