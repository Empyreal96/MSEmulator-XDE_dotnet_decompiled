using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.Diagnostics.Tracing.Internal;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000020 RID: 32
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	[StructLayout(LayoutKind.Explicit, Size = 16)]
	internal struct EventDescriptor
	{
		// Token: 0x06000120 RID: 288 RVA: 0x00009D30 File Offset: 0x00007F30
		public EventDescriptor(int traceloggingId, byte level, byte opcode, long keywords)
		{
			this.m_id = 0;
			this.m_version = 0;
			this.m_channel = 0;
			this.m_traceloggingId = traceloggingId;
			this.m_level = level;
			this.m_opcode = opcode;
			this.m_task = 0;
			this.m_keywords = keywords;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00009D6C File Offset: 0x00007F6C
		public EventDescriptor(int id, byte version, byte channel, byte level, byte opcode, int task, long keywords)
		{
			if (id < 0)
			{
				throw new ArgumentOutOfRangeException("id", Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum", new object[0]));
			}
			if (id > 65535)
			{
				throw new ArgumentOutOfRangeException("id", Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("ArgumentOutOfRange_NeedValidId", new object[]
				{
					1,
					ushort.MaxValue
				}));
			}
			this.m_traceloggingId = 0;
			checked
			{
				this.m_id = (ushort)id;
				this.m_version = version;
				this.m_channel = channel;
				this.m_level = level;
				this.m_opcode = opcode;
				this.m_keywords = keywords;
				if (task < 0)
				{
					throw new ArgumentOutOfRangeException("task", Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum", new object[0]));
				}
				if (task > 65535)
				{
					throw new ArgumentOutOfRangeException("task", Microsoft.Diagnostics.Tracing.Internal.Environment.GetResourceString("ArgumentOutOfRange_NeedValidId", new object[]
					{
						1,
						ushort.MaxValue
					}));
				}
				this.m_task = (ushort)task;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00009E6D File Offset: 0x0000806D
		public int EventId
		{
			get
			{
				return (int)this.m_id;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00009E75 File Offset: 0x00008075
		public byte Version
		{
			get
			{
				return this.m_version;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000124 RID: 292 RVA: 0x00009E7D File Offset: 0x0000807D
		public byte Channel
		{
			get
			{
				return this.m_channel;
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000125 RID: 293 RVA: 0x00009E85 File Offset: 0x00008085
		public byte Level
		{
			get
			{
				return this.m_level;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000126 RID: 294 RVA: 0x00009E8D File Offset: 0x0000808D
		public byte Opcode
		{
			get
			{
				return this.m_opcode;
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000127 RID: 295 RVA: 0x00009E95 File Offset: 0x00008095
		public int Task
		{
			get
			{
				return (int)this.m_task;
			}
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000128 RID: 296 RVA: 0x00009E9D File Offset: 0x0000809D
		public long Keywords
		{
			get
			{
				return this.m_keywords;
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00009EA5 File Offset: 0x000080A5
		public override bool Equals(object obj)
		{
			return obj is EventDescriptor && this.Equals((EventDescriptor)obj);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00009EBD File Offset: 0x000080BD
		public override int GetHashCode()
		{
			return (int)(this.m_id ^ (ushort)this.m_version ^ (ushort)this.m_channel ^ (ushort)this.m_level ^ (ushort)this.m_opcode ^ this.m_task) ^ checked((int)this.m_keywords);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00009EF0 File Offset: 0x000080F0
		public bool Equals(EventDescriptor other)
		{
			return this.m_id == other.m_id && this.m_version == other.m_version && this.m_channel == other.m_channel && this.m_level == other.m_level && this.m_opcode == other.m_opcode && this.m_task == other.m_task && this.m_keywords == other.m_keywords;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00009F69 File Offset: 0x00008169
		public static bool operator ==(EventDescriptor event1, EventDescriptor event2)
		{
			return event1.Equals(event2);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00009F73 File Offset: 0x00008173
		public static bool operator !=(EventDescriptor event1, EventDescriptor event2)
		{
			return !event1.Equals(event2);
		}

		// Token: 0x04000097 RID: 151
		[FieldOffset(0)]
		private int m_traceloggingId;

		// Token: 0x04000098 RID: 152
		[FieldOffset(0)]
		private ushort m_id;

		// Token: 0x04000099 RID: 153
		[FieldOffset(2)]
		private byte m_version;

		// Token: 0x0400009A RID: 154
		[FieldOffset(3)]
		private byte m_channel;

		// Token: 0x0400009B RID: 155
		[FieldOffset(4)]
		private byte m_level;

		// Token: 0x0400009C RID: 156
		[FieldOffset(5)]
		private byte m_opcode;

		// Token: 0x0400009D RID: 157
		[FieldOffset(6)]
		private ushort m_task;

		// Token: 0x0400009E RID: 158
		[FieldOffset(8)]
		private long m_keywords;
	}
}
