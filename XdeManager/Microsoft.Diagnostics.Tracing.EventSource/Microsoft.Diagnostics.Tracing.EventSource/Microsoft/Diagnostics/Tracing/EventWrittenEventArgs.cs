using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Security;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x0200002A RID: 42
	public class EventWrittenEventArgs : EventArgs
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000175 RID: 373 RVA: 0x0000AF21 File Offset: 0x00009121
		// (set) Token: 0x06000176 RID: 374 RVA: 0x0000AF58 File Offset: 0x00009158
		public string EventName
		{
			get
			{
				if (this.m_eventName != null || this.EventId < 0)
				{
					return this.m_eventName;
				}
				return this.m_eventSource.m_eventData[this.EventId].Name;
			}
			internal set
			{
				this.m_eventName = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000AF61 File Offset: 0x00009161
		// (set) Token: 0x06000178 RID: 376 RVA: 0x0000AF69 File Offset: 0x00009169
		public int EventId { get; internal set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000179 RID: 377 RVA: 0x0000AF72 File Offset: 0x00009172
		public Guid ActivityId
		{
			[SecurityCritical]
			get
			{
				return EventSource.CurrentThreadActivityId;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x0600017A RID: 378 RVA: 0x0000AF79 File Offset: 0x00009179
		// (set) Token: 0x0600017B RID: 379 RVA: 0x0000AF81 File Offset: 0x00009181
		public Guid RelatedActivityId { [SecurityCritical] get; internal set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600017C RID: 380 RVA: 0x0000AF8A File Offset: 0x0000918A
		// (set) Token: 0x0600017D RID: 381 RVA: 0x0000AF92 File Offset: 0x00009192
		public ReadOnlyCollection<object> Payload { get; internal set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000AF9C File Offset: 0x0000919C
		// (set) Token: 0x0600017F RID: 383 RVA: 0x0000B005 File Offset: 0x00009205
		public ReadOnlyCollection<string> PayloadNames
		{
			get
			{
				if (this.m_payloadNames == null)
				{
					List<string> list = new List<string>();
					foreach (ParameterInfo parameterInfo in this.m_eventSource.m_eventData[this.EventId].Parameters)
					{
						list.Add(parameterInfo.Name);
					}
					this.m_payloadNames = new ReadOnlyCollection<string>(list);
				}
				return this.m_payloadNames;
			}
			internal set
			{
				this.m_payloadNames = value;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000B00E File Offset: 0x0000920E
		public EventSource EventSource
		{
			get
			{
				return this.m_eventSource;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000181 RID: 385 RVA: 0x0000B016 File Offset: 0x00009216
		public EventKeywords Keywords
		{
			get
			{
				if (this.EventId < 0)
				{
					return this.m_keywords;
				}
				return (EventKeywords)this.m_eventSource.m_eventData[this.EventId].Descriptor.Keywords;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000B04A File Offset: 0x0000924A
		public EventOpcode Opcode
		{
			get
			{
				if (this.EventId < 0)
				{
					return this.m_opcode;
				}
				return (EventOpcode)this.m_eventSource.m_eventData[this.EventId].Descriptor.Opcode;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000183 RID: 387 RVA: 0x0000B07E File Offset: 0x0000927E
		public EventTask Task
		{
			get
			{
				if (this.EventId < 0)
				{
					return EventTask.None;
				}
				return (EventTask)this.m_eventSource.m_eventData[this.EventId].Descriptor.Task;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000184 RID: 388 RVA: 0x0000B0AD File Offset: 0x000092AD
		public EventTags Tags
		{
			get
			{
				if (this.EventId < 0)
				{
					return this.m_tags;
				}
				return this.m_eventSource.m_eventData[this.EventId].Tags;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000185 RID: 389 RVA: 0x0000B0DC File Offset: 0x000092DC
		// (set) Token: 0x06000186 RID: 390 RVA: 0x0000B10B File Offset: 0x0000930B
		public string Message
		{
			get
			{
				if (this.EventId < 0)
				{
					return this.m_message;
				}
				return this.m_eventSource.m_eventData[this.EventId].Message;
			}
			internal set
			{
				this.m_message = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000187 RID: 391 RVA: 0x0000B114 File Offset: 0x00009314
		public EventChannel Channel
		{
			get
			{
				if (this.EventId < 0)
				{
					return EventChannel.None;
				}
				return (EventChannel)this.m_eventSource.m_eventData[this.EventId].Descriptor.Channel;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000188 RID: 392 RVA: 0x0000B143 File Offset: 0x00009343
		public byte Version
		{
			get
			{
				if (this.EventId < 0)
				{
					return 0;
				}
				return this.m_eventSource.m_eventData[this.EventId].Descriptor.Version;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000B172 File Offset: 0x00009372
		public EventLevel Level
		{
			get
			{
				if (this.EventId < 0)
				{
					return this.m_level;
				}
				return (EventLevel)this.m_eventSource.m_eventData[this.EventId].Descriptor.Level;
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000B1A6 File Offset: 0x000093A6
		internal EventWrittenEventArgs(EventSource eventSource)
		{
			this.m_eventSource = eventSource;
		}

		// Token: 0x040000CC RID: 204
		private string m_message;

		// Token: 0x040000CD RID: 205
		private string m_eventName;

		// Token: 0x040000CE RID: 206
		private EventSource m_eventSource;

		// Token: 0x040000CF RID: 207
		private ReadOnlyCollection<string> m_payloadNames;

		// Token: 0x040000D0 RID: 208
		internal EventTags m_tags;

		// Token: 0x040000D1 RID: 209
		internal EventOpcode m_opcode;

		// Token: 0x040000D2 RID: 210
		internal EventLevel m_level;

		// Token: 0x040000D3 RID: 211
		internal EventKeywords m_keywords;
	}
}
