using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using HCS.Schema.Containers.CredentialGuard;
using HCS.Schema.Containers.Resources;
using HCS.Schema.DeviceAssignment;
using HCS.Schema.Registry;

namespace HCS.Schema
{
	// Token: 0x02000004 RID: 4
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class Container
	{
		// Token: 0x06000009 RID: 9 RVA: 0x000021E8 File Offset: 0x000003E8
		public static bool IsJsonDefault(Container val)
		{
			return Container._default.JsonEquals(val);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021F8 File Offset: 0x000003F8
		public bool JsonEquals(object obj)
		{
			Container graph = obj as Container;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(Container), new DataContractJsonSerializerSettings
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

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000022A0 File Offset: 0x000004A0
		// (set) Token: 0x0600000C RID: 12 RVA: 0x000022B7 File Offset: 0x000004B7
		[DataMember(EmitDefaultValue = false, Name = "GuestOs")]
		private GuestOs _GuestOs
		{
			get
			{
				if (!GuestOs.IsJsonDefault(this.GuestOs))
				{
					return this.GuestOs;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.GuestOs = value;
				}
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000022C3 File Offset: 0x000004C3
		// (set) Token: 0x0600000E RID: 14 RVA: 0x000022CB File Offset: 0x000004CB
		[DataMember(Name = "Storage")]
		private Storage _Storage
		{
			get
			{
				return this.Storage;
			}
			set
			{
				if (value != null)
				{
					this.Storage = value;
				}
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000022D7 File Offset: 0x000004D7
		// (set) Token: 0x06000010 RID: 16 RVA: 0x000022EE File Offset: 0x000004EE
		[DataMember(EmitDefaultValue = false, Name = "Networking")]
		private Networking _Networking
		{
			get
			{
				if (!Networking.IsJsonDefault(this.Networking))
				{
					return this.Networking;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.Networking = value;
				}
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000011 RID: 17 RVA: 0x000022FA File Offset: 0x000004FA
		// (set) Token: 0x06000012 RID: 18 RVA: 0x00002311 File Offset: 0x00000511
		[DataMember(EmitDefaultValue = false, Name = "HvSocket")]
		private HvSocket _HvSocket
		{
			get
			{
				if (!HvSocket.IsJsonDefault(this.HvSocket))
				{
					return this.HvSocket;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.HvSocket = value;
				}
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000013 RID: 19 RVA: 0x0000231D File Offset: 0x0000051D
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002334 File Offset: 0x00000534
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

		// Token: 0x04000006 RID: 6
		private static readonly Container _default = new Container();

		// Token: 0x04000007 RID: 7
		public GuestOs GuestOs = new GuestOs();

		// Token: 0x04000008 RID: 8
		public Storage Storage = new Storage();

		// Token: 0x04000009 RID: 9
		[DataMember(EmitDefaultValue = false)]
		public MappedDirectory[] MappedDirectories;

		// Token: 0x0400000A RID: 10
		[DataMember(EmitDefaultValue = false)]
		public MappedPipe[] MappedPipes;

		// Token: 0x0400000B RID: 11
		[DataMember(EmitDefaultValue = false)]
		public Memory Memory;

		// Token: 0x0400000C RID: 12
		[DataMember(EmitDefaultValue = false)]
		public Processor Processor;

		// Token: 0x0400000D RID: 13
		public Networking Networking = new Networking();

		// Token: 0x0400000E RID: 14
		public HvSocket HvSocket = new HvSocket();

		// Token: 0x0400000F RID: 15
		[DataMember(EmitDefaultValue = false)]
		public ContainerCredentialGuardState ContainerCredentialGuard;

		// Token: 0x04000010 RID: 16
		public RegistryChanges RegistryChanges = new RegistryChanges();

		// Token: 0x04000011 RID: 17
		[DataMember(EmitDefaultValue = false)]
		public Device[] AssignedDevices;
	}
}
