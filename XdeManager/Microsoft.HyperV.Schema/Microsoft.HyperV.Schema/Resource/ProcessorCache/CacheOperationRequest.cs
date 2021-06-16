using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Resource.ProcessorCache
{
	// Token: 0x020000AD RID: 173
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class CacheOperationRequest
	{
		// Token: 0x060002A1 RID: 673 RVA: 0x00009AEB File Offset: 0x00007CEB
		public static bool IsJsonDefault(CacheOperationRequest val)
		{
			return CacheOperationRequest._default.JsonEquals(val);
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00009AF8 File Offset: 0x00007CF8
		public bool JsonEquals(object obj)
		{
			CacheOperationRequest graph = obj as CacheOperationRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(CacheOperationRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x00009BA0 File Offset: 0x00007DA0
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x00009BBA File Offset: 0x00007DBA
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
					this.Operation = ModifyServiceOperation.SetCosBitmask;
				}
				this.Operation = (ModifyServiceOperation)Enum.Parse(typeof(ModifyServiceOperation), value, true);
			}
		}

		// Token: 0x04000382 RID: 898
		private static readonly CacheOperationRequest _default = new CacheOperationRequest();

		// Token: 0x04000383 RID: 899
		public ModifyServiceOperation Operation;

		// Token: 0x04000384 RID: 900
		[DataMember]
		public object OperationDetails;
	}
}
