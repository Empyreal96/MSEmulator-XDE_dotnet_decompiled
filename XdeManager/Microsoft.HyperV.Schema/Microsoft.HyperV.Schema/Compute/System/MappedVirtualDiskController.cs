using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Config.Containers;

namespace HCS.Compute.System
{
	// Token: 0x0200019D RID: 413
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class MappedVirtualDiskController
	{
		// Token: 0x0600069D RID: 1693 RVA: 0x00014F20 File Offset: 0x00013120
		public static bool IsJsonDefault(MappedVirtualDiskController val)
		{
			return MappedVirtualDiskController._default.JsonEquals(val);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00014F30 File Offset: 0x00013130
		public bool JsonEquals(object obj)
		{
			MappedVirtualDiskController graph = obj as MappedVirtualDiskController;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(MappedVirtualDiskController), new DataContractJsonSerializerSettings
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

		// Token: 0x04000918 RID: 2328
		private static readonly MappedVirtualDiskController _default = new MappedVirtualDiskController();

		// Token: 0x04000919 RID: 2329
		[DataMember(EmitDefaultValue = false)]
		public Dictionary<byte, MappedVirtualDisk> MappedVirtualDisks;
	}
}
