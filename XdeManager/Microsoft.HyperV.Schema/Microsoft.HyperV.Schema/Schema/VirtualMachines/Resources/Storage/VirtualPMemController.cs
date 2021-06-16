using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Storage
{
	// Token: 0x02000042 RID: 66
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VirtualPMemController
	{
		// Token: 0x06000109 RID: 265 RVA: 0x00004F23 File Offset: 0x00003123
		public static bool IsJsonDefault(VirtualPMemController val)
		{
			return VirtualPMemController._default.JsonEquals(val);
		}

		// Token: 0x0600010A RID: 266 RVA: 0x00004F30 File Offset: 0x00003130
		public bool JsonEquals(object obj)
		{
			VirtualPMemController graph = obj as VirtualPMemController;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VirtualPMemController), new DataContractJsonSerializerSettings
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

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600010B RID: 267 RVA: 0x00004FD8 File Offset: 0x000031D8
		// (set) Token: 0x0600010C RID: 268 RVA: 0x00005002 File Offset: 0x00003202
		[DataMember(EmitDefaultValue = false, Name = "Backing")]
		private string _Backing
		{
			get
			{
				if (this.Backing == VirtualPMemBackingType.Virtual)
				{
					return null;
				}
				return this.Backing.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Backing = VirtualPMemBackingType.Virtual;
				}
				this.Backing = (VirtualPMemBackingType)Enum.Parse(typeof(VirtualPMemBackingType), value, true);
			}
		}

		// Token: 0x0400014F RID: 335
		private static readonly VirtualPMemController _default = new VirtualPMemController();

		// Token: 0x04000150 RID: 336
		[DataMember]
		public Dictionary<byte, VirtualPMemDevice> Devices;

		// Token: 0x04000151 RID: 337
		[DataMember(EmitDefaultValue = false)]
		public byte MaximumCount;

		// Token: 0x04000152 RID: 338
		[DataMember(EmitDefaultValue = false)]
		public ulong MaximumSizeBytes;

		// Token: 0x04000153 RID: 339
		public VirtualPMemBackingType Backing;
	}
}
