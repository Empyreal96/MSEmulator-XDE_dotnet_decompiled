using System;
using System.Collections.Generic;

namespace DiscUtils.Vhdx
{
	// Token: 0x02000015 RID: 21
	internal sealed class LogSequence : List<LogEntry>
	{
		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004E52 File Offset: 0x00003052
		public LogEntry Head
		{
			get
			{
				if (base.Count <= 0)
				{
					return null;
				}
				return base[base.Count - 1];
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060000CB RID: 203 RVA: 0x00004E6D File Offset: 0x0000306D
		public LogEntry Tail
		{
			get
			{
				if (base.Count <= 0)
				{
					return null;
				}
				return base[0];
			}
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00004E84 File Offset: 0x00003084
		public bool Contains(long position)
		{
			if (base.Count <= 0)
			{
				return false;
			}
			if (this.Head.Position >= this.Tail.Position)
			{
				return position >= this.Tail.Position && position < this.Head.Position + 4096L;
			}
			return position >= this.Tail.Position || position < this.Head.Position + 4096L;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00004F00 File Offset: 0x00003100
		internal bool HigherSequenceThan(LogSequence otherSequence)
		{
			ulong num = (otherSequence.Count > 0) ? otherSequence.Head.SequenceNumber : 0UL;
			return ((base.Count > 0) ? this.Head.SequenceNumber : 0UL) > num;
		}
	}
}
