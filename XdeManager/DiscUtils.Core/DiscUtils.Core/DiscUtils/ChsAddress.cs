using System;

namespace DiscUtils
{
	// Token: 0x02000003 RID: 3
	public sealed class ChsAddress
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000020BE File Offset: 0x000002BE
		public ChsAddress(int cylinder, int head, int sector)
		{
			this.Cylinder = cylinder;
			this.Head = head;
			this.Sector = sector;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000020DB File Offset: 0x000002DB
		public int Cylinder { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020E3 File Offset: 0x000002E3
		public int Head { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000007 RID: 7 RVA: 0x000020EB File Offset: 0x000002EB
		public int Sector { get; }

		// Token: 0x06000008 RID: 8 RVA: 0x000020F4 File Offset: 0x000002F4
		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != base.GetType())
			{
				return false;
			}
			ChsAddress chsAddress = (ChsAddress)obj;
			return this.Cylinder == chsAddress.Cylinder && this.Head == chsAddress.Head && this.Sector == chsAddress.Sector;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000214C File Offset: 0x0000034C
		public override int GetHashCode()
		{
			return this.Cylinder.GetHashCode() ^ this.Head.GetHashCode() ^ this.Sector.GetHashCode();
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002188 File Offset: 0x00000388
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.Cylinder,
				"/",
				this.Head,
				"/",
				this.Sector,
				")"
			});
		}

		// Token: 0x04000002 RID: 2
		public static readonly ChsAddress First = new ChsAddress(0, 0, 1);
	}
}
