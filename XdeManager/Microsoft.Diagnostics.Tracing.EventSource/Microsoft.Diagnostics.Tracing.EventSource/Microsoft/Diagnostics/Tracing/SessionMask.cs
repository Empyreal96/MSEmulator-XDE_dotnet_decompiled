using System;

namespace Microsoft.Diagnostics.Tracing
{
	// Token: 0x02000031 RID: 49
	internal struct SessionMask
	{
		// Token: 0x060001AE RID: 430 RVA: 0x0000B2F8 File Offset: 0x000094F8
		public SessionMask(SessionMask m)
		{
			this.m_mask = m.m_mask;
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000B307 File Offset: 0x00009507
		public SessionMask(uint mask = 0U)
		{
			this.m_mask = (mask & 15U);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000B313 File Offset: 0x00009513
		public bool IsEqualOrSupersetOf(SessionMask m)
		{
			return (this.m_mask | m.m_mask) == this.m_mask;
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0000B32B File Offset: 0x0000952B
		public static SessionMask All
		{
			get
			{
				return new SessionMask(15U);
			}
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000B334 File Offset: 0x00009534
		public static SessionMask FromId(int perEventSourceSessionId)
		{
			return new SessionMask(1U << perEventSourceSessionId);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000B341 File Offset: 0x00009541
		public ulong ToEventKeywords()
		{
			return (ulong)this.m_mask << 44;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000B34D File Offset: 0x0000954D
		public static SessionMask FromEventKeywords(ulong m)
		{
			return new SessionMask(checked((uint)(m >> 44)));
		}

		// Token: 0x1700005A RID: 90
		public bool this[int perEventSourceSessionId]
		{
			get
			{
				return ((ulong)this.m_mask & (ulong)(1L << (perEventSourceSessionId & 31))) != 0UL;
			}
			set
			{
				if (value)
				{
					this.m_mask |= 1U << perEventSourceSessionId;
					return;
				}
				this.m_mask &= ~(1U << perEventSourceSessionId);
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000B39E File Offset: 0x0000959E
		public static SessionMask operator |(SessionMask m1, SessionMask m2)
		{
			return new SessionMask(m1.m_mask | m2.m_mask);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000B3B4 File Offset: 0x000095B4
		public static SessionMask operator &(SessionMask m1, SessionMask m2)
		{
			return new SessionMask(m1.m_mask & m2.m_mask);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000B3CA File Offset: 0x000095CA
		public static SessionMask operator ^(SessionMask m1, SessionMask m2)
		{
			return new SessionMask(m1.m_mask ^ m2.m_mask);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000B3E0 File Offset: 0x000095E0
		public static SessionMask operator ~(SessionMask m)
		{
			return new SessionMask(15U & ~m.m_mask);
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000B3F2 File Offset: 0x000095F2
		public static explicit operator ulong(SessionMask m)
		{
			return (ulong)m.m_mask;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000B3FC File Offset: 0x000095FC
		public static explicit operator uint(SessionMask m)
		{
			return m.m_mask;
		}

		// Token: 0x040000F1 RID: 241
		internal const int SHIFT_SESSION_TO_KEYWORD = 44;

		// Token: 0x040000F2 RID: 242
		internal const uint MASK = 15U;

		// Token: 0x040000F3 RID: 243
		internal const uint MAX = 4U;

		// Token: 0x040000F4 RID: 244
		private uint m_mask;
	}
}
