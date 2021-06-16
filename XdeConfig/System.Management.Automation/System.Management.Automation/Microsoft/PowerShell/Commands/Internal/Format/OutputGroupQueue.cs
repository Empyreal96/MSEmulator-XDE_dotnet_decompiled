using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.PowerShell.Commands.Internal.Format
{
	// Token: 0x020004F1 RID: 1265
	internal sealed class OutputGroupQueue
	{
		// Token: 0x06003654 RID: 13908 RVA: 0x001262D4 File Offset: 0x001244D4
		internal OutputGroupQueue(FormattedObjectsCache.ProcessCachedGroupNotification callBack, int objectCount)
		{
			this.notificationCallBack = callBack;
			this.objectCount = objectCount;
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x00126300 File Offset: 0x00124500
		internal OutputGroupQueue(FormattedObjectsCache.ProcessCachedGroupNotification callBack, TimeSpan groupingDuration)
		{
			this.notificationCallBack = callBack;
			this.groupingDuration = groupingDuration;
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x0012632C File Offset: 0x0012452C
		internal List<PacketInfoData> Add(PacketInfoData o)
		{
			FormatStartData formatStartData = o as FormatStartData;
			if (formatStartData != null)
			{
				this.formatStartData = formatStartData;
			}
			this.UpdateObjectCount(o);
			if (!this.processingGroup && o is GroupStartData)
			{
				this.processingGroup = true;
				this.currentObjectCount = 0;
				if (this.groupingDuration > TimeSpan.MinValue)
				{
					this.groupingTimer = Stopwatch.StartNew();
				}
				this.queue.Enqueue(o);
				return null;
			}
			if ((this.processingGroup && (o is GroupEndData || (this.objectCount > 0 && this.currentObjectCount >= this.objectCount))) || (this.groupingTimer != null && this.groupingTimer.Elapsed > this.groupingDuration))
			{
				this.currentObjectCount = 0;
				if (this.groupingTimer != null)
				{
					this.groupingTimer.Stop();
					this.groupingTimer = null;
				}
				this.queue.Enqueue(o);
				this.Notify();
				this.processingGroup = false;
				List<PacketInfoData> list = new List<PacketInfoData>();
				while (this.queue.Count > 0)
				{
					list.Add(this.queue.Dequeue());
				}
				return list;
			}
			if (this.processingGroup)
			{
				this.queue.Enqueue(o);
				return null;
			}
			return new List<PacketInfoData>
			{
				o
			};
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x00126468 File Offset: 0x00124668
		private void UpdateObjectCount(PacketInfoData o)
		{
			FormatEntryData formatEntryData = o as FormatEntryData;
			if (formatEntryData == null || formatEntryData.outOfBand)
			{
				return;
			}
			this.currentObjectCount++;
		}

		// Token: 0x06003658 RID: 13912 RVA: 0x00126498 File Offset: 0x00124698
		private void Notify()
		{
			if (this.notificationCallBack == null)
			{
				return;
			}
			List<PacketInfoData> list = new List<PacketInfoData>();
			foreach (PacketInfoData packetInfoData in this.queue)
			{
				FormatEntryData formatEntryData = packetInfoData as FormatEntryData;
				if (formatEntryData == null || !formatEntryData.outOfBand)
				{
					list.Add(packetInfoData);
				}
			}
			this.notificationCallBack(this.formatStartData, list);
		}

		// Token: 0x06003659 RID: 13913 RVA: 0x00126520 File Offset: 0x00124720
		internal PacketInfoData Dequeue()
		{
			if (this.queue.Count == 0)
			{
				return null;
			}
			return this.queue.Dequeue();
		}

		// Token: 0x04001BCC RID: 7116
		private Queue<PacketInfoData> queue = new Queue<PacketInfoData>();

		// Token: 0x04001BCD RID: 7117
		private int objectCount;

		// Token: 0x04001BCE RID: 7118
		private TimeSpan groupingDuration = TimeSpan.MinValue;

		// Token: 0x04001BCF RID: 7119
		private Stopwatch groupingTimer;

		// Token: 0x04001BD0 RID: 7120
		private FormattedObjectsCache.ProcessCachedGroupNotification notificationCallBack;

		// Token: 0x04001BD1 RID: 7121
		private FormatStartData formatStartData;

		// Token: 0x04001BD2 RID: 7122
		private bool processingGroup;

		// Token: 0x04001BD3 RID: 7123
		private int currentObjectCount;
	}
}
