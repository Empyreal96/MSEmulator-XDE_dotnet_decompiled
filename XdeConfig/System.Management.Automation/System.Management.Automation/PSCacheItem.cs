using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace System.Management.Automation
{
	// Token: 0x020000A3 RID: 163
	[DataContract]
	internal class PSCacheItem
	{
		// Token: 0x060007CF RID: 1999 RVA: 0x000275FC File Offset: 0x000257FC
		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (this.Commands != null)
			{
				this.Commands = new Dictionary<string, List<CommandTypes>>(this.Commands, StringComparer.OrdinalIgnoreCase);
			}
			if (this.Classes != null && this.Classes.Count > 0)
			{
				this.Classes = new HashSet<string>(this.Classes.AsEnumerable<string>(), StringComparer.OrdinalIgnoreCase);
			}
			if (this.DscResources.Count > 0)
			{
				this.DscResources = new HashSet<string>(this.DscResources.AsEnumerable<string>(), StringComparer.OrdinalIgnoreCase);
			}
			if (this.Enums.Count > 0)
			{
				this.Enums = new HashSet<string>(this.Enums.AsEnumerable<string>(), StringComparer.OrdinalIgnoreCase);
			}
			if (this.Interfaces.Count > 0)
			{
				this.Interfaces = new HashSet<string>(this.Interfaces.AsEnumerable<string>(), StringComparer.OrdinalIgnoreCase);
			}
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x000276D4 File Offset: 0x000258D4
		public PSCacheItem()
		{
			this.Commands = new Dictionary<string, List<CommandTypes>>(StringComparer.OrdinalIgnoreCase);
			this.Classes = PSCacheItem.emptyHashSet;
			this.Enums = PSCacheItem.emptyHashSet;
			this.Interfaces = PSCacheItem.emptyHashSet;
			this.DscResources = PSCacheItem.emptyHashSet;
		}

		// Token: 0x04000391 RID: 913
		private static HashSet<string> emptyHashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000392 RID: 914
		[DataMember]
		public Dictionary<string, List<CommandTypes>> Commands;

		// Token: 0x04000393 RID: 915
		[DataMember]
		public HashSet<string> Classes;

		// Token: 0x04000394 RID: 916
		[DataMember]
		public HashSet<string> Enums;

		// Token: 0x04000395 RID: 917
		[DataMember]
		public HashSet<string> Interfaces;

		// Token: 0x04000396 RID: 918
		[DataMember]
		public HashSet<string> DscResources;
	}
}
