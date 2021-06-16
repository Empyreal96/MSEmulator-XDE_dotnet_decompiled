using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema
{
	// Token: 0x02000006 RID: 6
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ComputeSystem
	{
		// Token: 0x06000023 RID: 35 RVA: 0x000024EE File Offset: 0x000006EE
		public static bool IsJsonDefault(ComputeSystem val)
		{
			return ComputeSystem._default.JsonEquals(val);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000024FC File Offset: 0x000006FC
		public bool JsonEquals(object obj)
		{
			ComputeSystem graph = obj as ComputeSystem;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ComputeSystem), new DataContractJsonSerializerSettings
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

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000025 RID: 37 RVA: 0x000025A4 File Offset: 0x000007A4
		// (set) Token: 0x06000026 RID: 38 RVA: 0x000025AC File Offset: 0x000007AC
		[DataMember(Name = "SchemaVersion")]
		private Version _SchemaVersion
		{
			get
			{
				return this.SchemaVersion;
			}
			set
			{
				if (value != null)
				{
					this.SchemaVersion = value;
				}
			}
		}

		// Token: 0x0400001F RID: 31
		private static readonly ComputeSystem _default = new ComputeSystem();

		// Token: 0x04000020 RID: 32
		[DataMember]
		public string Owner;

		// Token: 0x04000021 RID: 33
		public Version SchemaVersion = new Version();

		// Token: 0x04000022 RID: 34
		[DataMember(EmitDefaultValue = false)]
		public string HostingSystemId;

		// Token: 0x04000023 RID: 35
		[DataMember(EmitDefaultValue = false)]
		public object HostedSystem;

		// Token: 0x04000024 RID: 36
		[DataMember(EmitDefaultValue = false)]
		public Container Container;

		// Token: 0x04000025 RID: 37
		[DataMember(EmitDefaultValue = false)]
		public VirtualMachine VirtualMachine;

		// Token: 0x04000026 RID: 38
		[DataMember(EmitDefaultValue = false)]
		public bool ShouldTerminateOnLastHandleClosed;
	}
}
