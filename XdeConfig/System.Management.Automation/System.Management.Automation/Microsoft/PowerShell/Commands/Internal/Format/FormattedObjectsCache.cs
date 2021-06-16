using System;
using System.Collections.Generic;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004F2 RID: 1266
	internal sealed class FormattedObjectsCache
	{
		// Token: 0x0600365A RID: 13914 RVA: 0x0012653C File Offset: 0x0012473C
		internal FormattedObjectsCache(bool cacheFrontEnd)
		{
			if (cacheFrontEnd)
			{
				this.frontEndQueue = new Queue<PacketInfoData>();
			}
		}

		// Token: 0x0600365B RID: 13915 RVA: 0x00126552 File Offset: 0x00124752
		internal void EnableGroupCaching(FormattedObjectsCache.ProcessCachedGroupNotification callBack, int objectCount)
		{
			if (callBack != null)
			{
				this.groupQueue = new OutputGroupQueue(callBack, objectCount);
			}
		}

		// Token: 0x0600365C RID: 13916 RVA: 0x00126564 File Offset: 0x00124764
		internal void EnableGroupCaching(FormattedObjectsCache.ProcessCachedGroupNotification callBack, TimeSpan groupingDuration)
		{
			if (callBack != null)
			{
				this.groupQueue = new OutputGroupQueue(callBack, groupingDuration);
			}
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x00126578 File Offset: 0x00124778
		internal List<PacketInfoData> Add(PacketInfoData o)
		{
			if (this.frontEndQueue == null && this.groupQueue == null)
			{
				return new List<PacketInfoData>
				{
					o
				};
			}
			if (this.frontEndQueue != null)
			{
				this.frontEndQueue.Enqueue(o);
				return null;
			}
			return this.groupQueue.Add(o);
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x001265C8 File Offset: 0x001247C8
		internal List<PacketInfoData> Drain()
		{
			if (this.frontEndQueue == null && this.groupQueue == null)
			{
				return null;
			}
			List<PacketInfoData> list = new List<PacketInfoData>();
			if (this.frontEndQueue != null)
			{
				if (this.groupQueue == null)
				{
					while (this.frontEndQueue.Count > 0)
					{
						list.Add(this.frontEndQueue.Dequeue());
					}
					return list;
				}
				while (this.frontEndQueue.Count > 0)
				{
					List<PacketInfoData> list2 = this.groupQueue.Add(this.frontEndQueue.Dequeue());
					if (list2 != null)
					{
						foreach (PacketInfoData item in list2)
						{
							list.Add(item);
						}
					}
				}
			}
			for (;;)
			{
				PacketInfoData packetInfoData = this.groupQueue.Dequeue();
				if (packetInfoData == null)
				{
					break;
				}
				list.Add(packetInfoData);
			}
			return list;
		}

		// Token: 0x04001BD4 RID: 7124
		private Queue<PacketInfoData> frontEndQueue;

		// Token: 0x04001BD5 RID: 7125
		private OutputGroupQueue groupQueue;

		// Token: 0x020004F3 RID: 1267
		// (Invoke) Token: 0x06003660 RID: 13920
		internal delegate void ProcessCachedGroupNotification(FormatStartData formatStartData, List<PacketInfoData> objects);
	}
}
