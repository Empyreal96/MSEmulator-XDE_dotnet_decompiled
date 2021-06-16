using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Vpci
{
	// Token: 0x0200002F RID: 47
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class FlexibleIoDevice
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x000042CC File Offset: 0x000024CC
		public static bool IsJsonDefault(FlexibleIoDevice val)
		{
			return FlexibleIoDevice._default.JsonEquals(val);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000042DC File Offset: 0x000024DC
		public bool JsonEquals(object obj)
		{
			FlexibleIoDevice graph = obj as FlexibleIoDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(FlexibleIoDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00004384 File Offset: 0x00002584
		// (set) Token: 0x060000C8 RID: 200 RVA: 0x000043AE File Offset: 0x000025AE
		[DataMember(EmitDefaultValue = false, Name = "HostingModel")]
		private string _HostingModel
		{
			get
			{
				if (this.HostingModel == FlexibleIoDeviceHostingModel.Internal)
				{
					return null;
				}
				return this.HostingModel.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.HostingModel = FlexibleIoDeviceHostingModel.Internal;
				}
				this.HostingModel = (FlexibleIoDeviceHostingModel)Enum.Parse(typeof(FlexibleIoDeviceHostingModel), value, true);
			}
		}

		// Token: 0x040000D0 RID: 208
		private static readonly FlexibleIoDevice _default = new FlexibleIoDevice();

		// Token: 0x040000D1 RID: 209
		[DataMember(EmitDefaultValue = false)]
		public Guid EmulatorId;

		// Token: 0x040000D2 RID: 210
		public FlexibleIoDeviceHostingModel HostingModel;

		// Token: 0x040000D3 RID: 211
		[DataMember(EmitDefaultValue = false)]
		public string[] Configuration;
	}
}
