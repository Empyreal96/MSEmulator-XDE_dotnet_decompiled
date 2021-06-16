using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace HCS.Config.Devices.Chipset
{
	// Token: 0x02000157 RID: 343
	[GeneratedCode("MarsComp", "")]
	[DataContract]
	public class RealTimeClockDevice
	{
		// Token: 0x06000567 RID: 1383 RVA: 0x0001191C File Offset: 0x0000FB1C
		public static bool IsJsonDefault(RealTimeClockDevice val)
		{
			return RealTimeClockDevice._default.JsonEquals(val);
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0001192C File Offset: 0x0000FB2C
		public bool JsonEquals(object obj)
		{
			RealTimeClockDevice graph = obj as RealTimeClockDevice;
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(RealTimeClockDevice), new DataContractJsonSerializerSettings
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

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000569 RID: 1385 RVA: 0x000119D4 File Offset: 0x0000FBD4
		// (set) Token: 0x0600056A RID: 1386 RVA: 0x000119EB File Offset: 0x0000FBEB
		[DataMember(EmitDefaultValue = false, Name = "RtcDevice")]
		private RealTimeClock _RealTimeClock
		{
			get
			{
				if (!RealTimeClock.IsJsonDefault(this.RealTimeClock))
				{
					return this.RealTimeClock;
				}
				return null;
			}
			set
			{
				if (value != null)
				{
					this.RealTimeClock = value;
				}
			}
		}

		// Token: 0x0400071C RID: 1820
		private static readonly RealTimeClockDevice _default = new RealTimeClockDevice();

		// Token: 0x0400071D RID: 1821
		[DataMember(EmitDefaultValue = false, Name = "VDEVVersion")]
		public uint Version;

		// Token: 0x0400071E RID: 1822
		[DataMember(EmitDefaultValue = false, Name = "version")]
		public uint LegacyVersion;

		// Token: 0x0400071F RID: 1823
		public RealTimeClock RealTimeClock = new RealTimeClock();

		// Token: 0x04000720 RID: 1824
		[DataMember(EmitDefaultValue = false)]
		public bool ProvideUtc;
	}
}
