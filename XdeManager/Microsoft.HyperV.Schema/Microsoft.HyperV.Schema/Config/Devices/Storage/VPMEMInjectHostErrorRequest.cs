using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Storage
{
	// Token: 0x0200013E RID: 318
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VPMEMInjectHostErrorRequest
	{
		// Token: 0x06000509 RID: 1289 RVA: 0x000106F8 File Offset: 0x0000E8F8
		public static bool IsJsonDefault(VPMEMInjectHostErrorRequest val)
		{
			return VPMEMInjectHostErrorRequest._default.JsonEquals(val);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x00010708 File Offset: 0x0000E908
		public bool JsonEquals(object obj)
		{
			VPMEMInjectHostErrorRequest graph = obj as VPMEMInjectHostErrorRequest;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VPMEMInjectHostErrorRequest), new DataContractJsonSerializerSettings
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

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600050B RID: 1291 RVA: 0x000107B0 File Offset: 0x0000E9B0
		// (set) Token: 0x0600050C RID: 1292 RVA: 0x000107CA File Offset: 0x0000E9CA
		[DataMember(Name = "Error")]
		private string _Error
		{
			get
			{
				VPMEMHostInjectedError error = this.Error;
				return this.Error.ToString();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					this.Error = VPMEMHostInjectedError.None;
				}
				this.Error = (VPMEMHostInjectedError)Enum.Parse(typeof(VPMEMHostInjectedError), value, true);
			}
		}

		// Token: 0x0400067D RID: 1661
		private static readonly VPMEMInjectHostErrorRequest _default = new VPMEMInjectHostErrorRequest();

		// Token: 0x0400067E RID: 1662
		[DataMember]
		public uint NFITHandle;

		// Token: 0x0400067F RID: 1663
		public VPMEMHostInjectedError Error;
	}
}
