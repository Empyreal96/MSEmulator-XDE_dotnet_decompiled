using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.Process
{
	// Token: 0x02000083 RID: 131
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ProcessModifyRequest
	{
		// Token: 0x06000205 RID: 517 RVA: 0x00007E0B File Offset: 0x0000600B
		public static bool IsJsonDefault(ProcessModifyRequest val)
		{
			return ProcessModifyRequest._default.JsonEquals(val);
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00007E18 File Offset: 0x00006018
		public bool JsonEquals(object obj)
		{
			ProcessModifyRequest graph = obj as ProcessModifyRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ProcessModifyRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000207 RID: 519 RVA: 0x00007EC0 File Offset: 0x000060C0
		// (set) Token: 0x06000208 RID: 520 RVA: 0x00007EDA File Offset: 0x000060DA
		[DataMember(Name = "Operation")]
		private string _Operation
		{
			get
			{
				ModifyOperation operation = this.Operation;
				return this.Operation.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Operation = ModifyOperation.ConsoleSize;
				}
				this.Operation = (ModifyOperation)Enum.Parse(typeof(ModifyOperation), value, true);
			}
		}

		// Token: 0x040002D5 RID: 725
		private static readonly ProcessModifyRequest _default = new ProcessModifyRequest();

		// Token: 0x040002D6 RID: 726
		public ModifyOperation Operation;

		// Token: 0x040002D7 RID: 727
		[DataMember(EmitDefaultValue = false)]
		public ConsoleSize ConsoleSize;

		// Token: 0x040002D8 RID: 728
		[DataMember(EmitDefaultValue = false)]
		public CloseHandle CloseHandle;
	}
}
