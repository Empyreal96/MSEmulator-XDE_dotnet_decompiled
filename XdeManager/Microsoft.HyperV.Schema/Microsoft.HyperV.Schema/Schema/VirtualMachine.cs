using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Common.Resources;
using HCS.Schema.Registry;
using HCS.Schema.VirtualMachines;
using HCS.Schema.VirtualMachines.Resources;
using HCS.Schema.VirtualMachines.Resources.Compute;

namespace HCS.Schema
{
	// Token: 0x02000005 RID: 5
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class VirtualMachine
	{
		// Token: 0x06000017 RID: 23 RVA: 0x0000238B File Offset: 0x0000058B
		public static bool IsJsonDefault(VirtualMachine val)
		{
			return VirtualMachine._default.JsonEquals(val);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002398 File Offset: 0x00000598
		public bool JsonEquals(object obj)
		{
			VirtualMachine graph = obj as VirtualMachine;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(VirtualMachine), new DataContractJsonSerializerSettings
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

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000019 RID: 25 RVA: 0x00002440 File Offset: 0x00000640
		// (set) Token: 0x0600001A RID: 26 RVA: 0x00002448 File Offset: 0x00000648
		[DataMember(Name = "Chipset")]
		private Chipset _Chipset
		{
			get
			{
				return this.Chipset;
			}
			set
			{
				if (value != null)
				{
					this.Chipset = value;
				}
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600001B RID: 27 RVA: 0x00002454 File Offset: 0x00000654
		// (set) Token: 0x0600001C RID: 28 RVA: 0x0000245C File Offset: 0x0000065C
		[DataMember(Name = "ComputeTopology")]
		private Topology _ComputeTopology
		{
			get
			{
				return this.ComputeTopology;
			}
			set
			{
				if (value != null)
				{
					this.ComputeTopology = value;
				}
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600001D RID: 29 RVA: 0x00002468 File Offset: 0x00000668
		// (set) Token: 0x0600001E RID: 30 RVA: 0x0000247F File Offset: 0x0000067F
		[DataMember(EmitDefaultValue = false, Name = "RegistryChanges")]
		private RegistryChanges _RegistryChanges
		{
			get
			{
				if (!RegistryChanges.IsJsonDefault(this.RegistryChanges))
				{
					return this.RegistryChanges;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.RegistryChanges = value;
				}
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000248B File Offset: 0x0000068B
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000024A2 File Offset: 0x000006A2
		[DataMember(EmitDefaultValue = false, Name = "LaunchOptions")]
		private LaunchOptions _LaunchOptions
		{
			get
			{
				if (!LaunchOptions.IsJsonDefault(this.LaunchOptions))
				{
					return this.LaunchOptions;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.LaunchOptions = value;
				}
			}
		}

		// Token: 0x04000012 RID: 18
		private static readonly VirtualMachine _default = new VirtualMachine();

		// Token: 0x04000013 RID: 19
		[DataMember(EmitDefaultValue = false)]
		public bool StopOnReset;

		// Token: 0x04000014 RID: 20
		public Chipset Chipset = new Chipset();

		// Token: 0x04000015 RID: 21
		public Topology ComputeTopology = new Topology();

		// Token: 0x04000016 RID: 22
		[DataMember(EmitDefaultValue = false)]
		public Devices Devices;

		// Token: 0x04000017 RID: 23
		[DataMember(EmitDefaultValue = false)]
		public GuestState GuestState;

		// Token: 0x04000018 RID: 24
		[DataMember(EmitDefaultValue = false)]
		public RestoreState RestoreState;

		// Token: 0x04000019 RID: 25
		public RegistryChanges RegistryChanges = new RegistryChanges();

		// Token: 0x0400001A RID: 26
		[DataMember(EmitDefaultValue = false)]
		public StorageQoS StorageQoS;

		// Token: 0x0400001B RID: 27
		[DataMember(EmitDefaultValue = false)]
		public SiloSettings RunInSilo;

		// Token: 0x0400001C RID: 28
		[DataMember(EmitDefaultValue = false)]
		public DebugOptions DebugOptions;

		// Token: 0x0400001D RID: 29
		public LaunchOptions LaunchOptions = new LaunchOptions();

		// Token: 0x0400001E RID: 30
		[DataMember(EmitDefaultValue = false)]
		public GuestConnection GuestConnection;
	}
}
