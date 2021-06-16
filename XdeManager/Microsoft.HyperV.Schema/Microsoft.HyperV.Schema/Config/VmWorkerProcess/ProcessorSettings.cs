using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.VmWorkerProcess
{
	// Token: 0x020000F6 RID: 246
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class ProcessorSettings
	{
		// Token: 0x060003B9 RID: 953 RVA: 0x0000CE00 File Offset: 0x0000B000
		public static bool IsJsonDefault(ProcessorSettings val)
		{
			return ProcessorSettings._default.JsonEquals(val);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x0000CE10 File Offset: 0x0000B010
		public bool JsonEquals(object obj)
		{
			ProcessorSettings graph = obj as ProcessorSettings;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(ProcessorSettings), new DataContractJsonSerializerSettings
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

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060003BB RID: 955 RVA: 0x0000CEB8 File Offset: 0x0000B0B8
		// (set) Token: 0x060003BC RID: 956 RVA: 0x0000CEC0 File Offset: 0x0000B0C0
		[DataMember(Name = "vnuma")]
		private NumaProcessors _Numa
		{
			get
			{
				return this.Numa;
			}
			set
			{
				if (value != null)
				{
					this.Numa = value;
				}
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060003BD RID: 957 RVA: 0x0000CECC File Offset: 0x0000B0CC
		// (set) Token: 0x060003BE RID: 958 RVA: 0x0000CED4 File Offset: 0x0000B0D4
		[DataMember(Name = "enlightenments")]
		private ProcessorEnlightenments _Enlightenments
		{
			get
			{
				return this.Enlightenments;
			}
			set
			{
				if (value != null)
				{
					this.Enlightenments = value;
				}
			}
		}

		// Token: 0x040004B8 RID: 1208
		private static readonly ProcessorSettings _default = new ProcessorSettings();

		// Token: 0x040004B9 RID: 1209
		[DataMember(EmitDefaultValue = false, Name = "element_name")]
		public string ElementName;

		// Token: 0x040004BA RID: 1210
		[DataMember(EmitDefaultValue = false, Name = "pool_id")]
		public string ResourcePoolId;

		// Token: 0x040004BB RID: 1211
		[DataMember(Name = "count")]
		public uint Count;

		// Token: 0x040004BC RID: 1212
		[DataMember(Name = "hwthreads")]
		public uint ThreadsPerCore;

		// Token: 0x040004BD RID: 1213
		[DataMember(Name = "limit")]
		public long Limit;

		// Token: 0x040004BE RID: 1214
		[DataMember(Name = "reservation")]
		public long Reservation;

		// Token: 0x040004BF RID: 1215
		[DataMember(Name = "weight")]
		public long Weight;

		// Token: 0x040004C0 RID: 1216
		[DataMember(Name = "limit_cpuid")]
		public bool LimitCPUID;

		// Token: 0x040004C1 RID: 1217
		[DataMember(Name = "features")]
		public bool LimitProcessorFeatures;

		// Token: 0x040004C2 RID: 1218
		[DataMember(Name = "hide_hypervisor_present")]
		public bool HideHypervisorPresent;

		// Token: 0x040004C3 RID: 1219
		[DataMember(Name = "nested_virtualization")]
		public bool EnableNestedVirtualization;

		// Token: 0x040004C4 RID: 1220
		[DataMember(Name = "enable_perfmon_pmu")]
		public bool EnablePerfmonPmu;

		// Token: 0x040004C5 RID: 1221
		[DataMember(Name = "enable_perfmon_lbr")]
		public bool EnablePerfmonLbr;

		// Token: 0x040004C6 RID: 1222
		[DataMember(Name = "enable_perfmon_pebs")]
		public bool EnablePerfmonPebs;

		// Token: 0x040004C7 RID: 1223
		[DataMember(Name = "enable_perfmon_ipt")]
		public bool EnablePerfmonIpt;

		// Token: 0x040004C8 RID: 1224
		[DataMember(EmitDefaultValue = false, Name = "synchronize_host_features")]
		public bool SynchronizeHostFeatures;

		// Token: 0x040004C9 RID: 1225
		[DataMember(EmitDefaultValue = false, Name = "disable_speculation_controls")]
		public bool? DisableSpeculationControls;

		// Token: 0x040004CA RID: 1226
		public NumaProcessors Numa = new NumaProcessors();

		// Token: 0x040004CB RID: 1227
		[DataMember(EmitDefaultValue = false, Name = "allow_vp_overcommit")]
		public bool AllowVPOvercommit;

		// Token: 0x040004CC RID: 1228
		[DataMember(Name = "cpu_group_id")]
		public long CpuGroupId;

		// Token: 0x040004CD RID: 1229
		[DataMember(Name = "cpu_group_guid")]
		public Guid CpuGroupGuid;

		// Token: 0x040004CE RID: 1230
		public ProcessorEnlightenments Enlightenments = new ProcessorEnlightenments();
	}
}
