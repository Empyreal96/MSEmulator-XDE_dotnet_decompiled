using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Schema.VirtualMachines.Resources.Compute
{
	// Token: 0x02000049 RID: 73
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Processor
	{
		// Token: 0x0600011F RID: 287 RVA: 0x0000534C File Offset: 0x0000354C
		public static bool IsJsonDefault(Processor val)
		{
			return Processor._default.JsonEquals(val);
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000535C File Offset: 0x0000355C
		public bool JsonEquals(object obj)
		{
			Processor graph = obj as Processor;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Processor), new DataContractJsonSerializerSettings
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

		// Token: 0x04000174 RID: 372
		private static readonly Processor _default = new Processor();

		// Token: 0x04000175 RID: 373
		[DataMember]
		public uint Count;

		// Token: 0x04000176 RID: 374
		[DataMember(EmitDefaultValue = false)]
		public ulong? Limit;

		// Token: 0x04000177 RID: 375
		[DataMember(EmitDefaultValue = false)]
		public ulong? Weight;

		// Token: 0x04000178 RID: 376
		[DataMember(EmitDefaultValue = false)]
		public bool ExposeVirtualizationExtensions;

		// Token: 0x04000179 RID: 377
		[DataMember(EmitDefaultValue = false)]
		public bool EnablePerfmonPmu;

		// Token: 0x0400017A RID: 378
		[DataMember(EmitDefaultValue = false)]
		public bool EnablePerfmonPebs;

		// Token: 0x0400017B RID: 379
		[DataMember(EmitDefaultValue = false)]
		public bool EnablePerfmonLbr;

		// Token: 0x0400017C RID: 380
		[DataMember(EmitDefaultValue = false)]
		public bool EnablePerfmonIpt;

		// Token: 0x0400017D RID: 381
		[DataMember(EmitDefaultValue = false)]
		public bool SynchronizeHostFeatures;

		// Token: 0x0400017E RID: 382
		[DataMember(EmitDefaultValue = false)]
		public bool EnableSchedulerAssist;
	}
}
