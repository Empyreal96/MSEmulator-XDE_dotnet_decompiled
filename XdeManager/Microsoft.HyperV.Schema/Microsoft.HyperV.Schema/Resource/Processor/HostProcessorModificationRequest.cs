using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.Processor
{
	// Token: 0x020000BD RID: 189
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class HostProcessorModificationRequest
	{
		// Token: 0x060002E1 RID: 737 RVA: 0x0000A73C File Offset: 0x0000893C
		public static bool IsJsonDefault(HostProcessorModificationRequest val)
		{
			return HostProcessorModificationRequest._default.JsonEquals(val);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000A74C File Offset: 0x0000894C
		public bool JsonEquals(object obj)
		{
			HostProcessorModificationRequest graph = obj as HostProcessorModificationRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(HostProcessorModificationRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x0000A7F4 File Offset: 0x000089F4
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x0000A80E File Offset: 0x00008A0E
		[DataMember(Name = "Operation")]
		private string _Operation
		{
			get
			{
				ModifyServiceOperation operation = this.Operation;
				return this.Operation.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Operation = ModifyServiceOperation.CreateGroup;
				}
				this.Operation = (ModifyServiceOperation)Enum.Parse(typeof(ModifyServiceOperation), value, true);
			}
		}

		// Token: 0x040003B8 RID: 952
		private static readonly HostProcessorModificationRequest _default = new HostProcessorModificationRequest();

		// Token: 0x040003B9 RID: 953
		public ModifyServiceOperation Operation;

		// Token: 0x040003BA RID: 954
		[DataMember]
		public object OperationDetails;
	}
}
