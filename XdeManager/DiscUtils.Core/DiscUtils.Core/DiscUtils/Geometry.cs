using System;
using System.Globalization;

namespace DiscUtils
{
	// Token: 0x02000016 RID: 22
	public sealed class Geometry
	{
		// Token: 0x060000CE RID: 206 RVA: 0x00002E85 File Offset: 0x00001085
		public Geometry(int cylinders, int headsPerCylinder, int sectorsPerTrack)
		{
			this.Cylinders = cylinders;
			this.HeadsPerCylinder = headsPerCylinder;
			this.SectorsPerTrack = sectorsPerTrack;
			this.BytesPerSector = 512;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00002EAD File Offset: 0x000010AD
		public Geometry(int cylinders, int headsPerCylinder, int sectorsPerTrack, int bytesPerSector)
		{
			this.Cylinders = cylinders;
			this.HeadsPerCylinder = headsPerCylinder;
			this.SectorsPerTrack = sectorsPerTrack;
			this.BytesPerSector = bytesPerSector;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00002ED2 File Offset: 0x000010D2
		public Geometry(long capacity, int headsPerCylinder, int sectorsPerTrack, int bytesPerSector)
		{
			this.Cylinders = (int)(capacity / ((long)headsPerCylinder * (long)sectorsPerTrack * (long)bytesPerSector));
			this.HeadsPerCylinder = headsPerCylinder;
			this.SectorsPerTrack = sectorsPerTrack;
			this.BytesPerSector = bytesPerSector;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x00002F02 File Offset: 0x00001102
		public int BytesPerSector { get; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x00002F0A File Offset: 0x0000110A
		public long Capacity
		{
			get
			{
				return this.TotalSectorsLong * (long)this.BytesPerSector;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x00002F1A File Offset: 0x0000111A
		public int Cylinders { get; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x00002F22 File Offset: 0x00001122
		public int HeadsPerCylinder { get; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000D5 RID: 213 RVA: 0x00002F2A File Offset: 0x0000112A
		public bool IsBiosAndIdeSafe
		{
			get
			{
				return this.Cylinders <= 1024 && this.HeadsPerCylinder <= 16 && this.SectorsPerTrack <= 63;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x00002F52 File Offset: 0x00001152
		public bool IsBiosSafe
		{
			get
			{
				return this.Cylinders <= 1024 && this.HeadsPerCylinder <= 255 && this.SectorsPerTrack <= 63;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00002F7D File Offset: 0x0000117D
		public bool IsIdeSafe
		{
			get
			{
				return this.Cylinders <= 65536 && this.HeadsPerCylinder <= 16 && this.SectorsPerTrack <= 255;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00002FA8 File Offset: 0x000011A8
		public ChsAddress LastSector
		{
			get
			{
				return new ChsAddress(this.Cylinders - 1, this.HeadsPerCylinder - 1, this.SectorsPerTrack);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00002FC5 File Offset: 0x000011C5
		public static Geometry Null
		{
			get
			{
				return new Geometry(0, 0, 0, 512);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000DA RID: 218 RVA: 0x00002FD4 File Offset: 0x000011D4
		public int SectorsPerTrack { get; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00002FDC File Offset: 0x000011DC
		[Obsolete("Use TotalSectorsLong instead, to support very large disks.")]
		public int TotalSectors
		{
			get
			{
				return this.Cylinders * this.HeadsPerCylinder * this.SectorsPerTrack;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000DC RID: 220 RVA: 0x00002FF2 File Offset: 0x000011F2
		public long TotalSectorsLong
		{
			get
			{
				return (long)this.Cylinders * (long)this.HeadsPerCylinder * (long)this.SectorsPerTrack;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000300C File Offset: 0x0000120C
		public static Geometry LargeBiosGeometry(Geometry ideGeometry)
		{
			int num = ideGeometry.Cylinders;
			int num2 = ideGeometry.HeadsPerCylinder;
			int sectorsPerTrack = ideGeometry.SectorsPerTrack;
			while (num > 1024 && num2 <= 127)
			{
				num >>= 1;
				num2 <<= 1;
			}
			return new Geometry(num, num2, sectorsPerTrack);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00003050 File Offset: 0x00001250
		public static Geometry LbaAssistedBiosGeometry(long capacity)
		{
			int num;
			if (capacity <= 528482304L)
			{
				num = 16;
			}
			else if (capacity <= 1056964608L)
			{
				num = 32;
			}
			else if (capacity <= 2113929216L)
			{
				num = 64;
			}
			else if (capacity <= (long)((ulong)-67108864))
			{
				num = 128;
			}
			else
			{
				num = 255;
			}
			int num2 = 63;
			return new Geometry((int)Math.Min(1024L, capacity / ((long)num2 * (long)num * 512L)), num, num2, 512);
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000030C7 File Offset: 0x000012C7
		public static Geometry MakeBiosSafe(Geometry geometry, long capacity)
		{
			if (geometry == null)
			{
				return Geometry.LbaAssistedBiosGeometry(capacity);
			}
			if (geometry.IsBiosSafe)
			{
				return geometry;
			}
			return Geometry.LbaAssistedBiosGeometry(capacity);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000030E3 File Offset: 0x000012E3
		public static Geometry FromCapacity(long capacity)
		{
			return Geometry.FromCapacity(capacity, 512);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x000030F0 File Offset: 0x000012F0
		public static Geometry FromCapacity(long capacity, int sectorSize)
		{
			int num;
			if (capacity > 267382800L * (long)sectorSize)
			{
				num = 267382800;
			}
			else
			{
				num = (int)(capacity / (long)sectorSize);
			}
			int num2;
			int num3;
			if (num > 66059280)
			{
				num2 = 255;
				num3 = 16;
			}
			else
			{
				num2 = 17;
				int num4 = num / num2;
				num3 = (num4 + 1023) / 1024;
				if (num3 < 4)
				{
					num3 = 4;
				}
				if ((long)num4 >= (long)num3 * 1024L || num3 > 16)
				{
					num2 = 31;
					num3 = 16;
					num4 = num / num2;
				}
				if ((long)num4 >= (long)num3 * 1024L)
				{
					num2 = 63;
					num3 = 16;
				}
			}
			return new Geometry(num / num2 / num3, num3, num2, sectorSize);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00003181 File Offset: 0x00001381
		public long ToLogicalBlockAddress(ChsAddress chsAddress)
		{
			return this.ToLogicalBlockAddress(chsAddress.Cylinder, chsAddress.Head, chsAddress.Sector);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0000319C File Offset: 0x0000139C
		public long ToLogicalBlockAddress(int cylinder, int head, int sector)
		{
			if (cylinder < 0)
			{
				throw new ArgumentOutOfRangeException("cylinder", cylinder, "cylinder number is negative");
			}
			if (head >= this.HeadsPerCylinder)
			{
				throw new ArgumentOutOfRangeException("head", head, "head number is larger than disk geometry");
			}
			if (head < 0)
			{
				throw new ArgumentOutOfRangeException("head", head, "head number is negative");
			}
			if (sector > this.SectorsPerTrack)
			{
				throw new ArgumentOutOfRangeException("sector", sector, "sector number is larger than disk geometry");
			}
			if (sector < 1)
			{
				throw new ArgumentOutOfRangeException("sector", sector, "sector number is less than one (sectors are 1-based)");
			}
			return ((long)cylinder * (long)this.HeadsPerCylinder + (long)head) * (long)this.SectorsPerTrack + (long)sector - 1L;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00003250 File Offset: 0x00001450
		public ChsAddress ToChsAddress(long logicalBlockAddress)
		{
			if (logicalBlockAddress < 0L)
			{
				throw new ArgumentOutOfRangeException("logicalBlockAddress", logicalBlockAddress, "Logical Block Address is negative");
			}
			int cylinder = (int)(logicalBlockAddress / (long)(this.HeadsPerCylinder * this.SectorsPerTrack));
			int num = (int)(logicalBlockAddress % (long)(this.HeadsPerCylinder * this.SectorsPerTrack));
			int head = num / this.SectorsPerTrack;
			int sector = num % this.SectorsPerTrack + 1;
			return new ChsAddress(cylinder, head, sector);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000032B4 File Offset: 0x000014B4
		public Geometry TranslateToBios(GeometryTranslation translation)
		{
			return this.TranslateToBios(0L, translation);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000032C0 File Offset: 0x000014C0
		public Geometry TranslateToBios(long capacity, GeometryTranslation translation)
		{
			if (capacity <= 0L)
			{
				capacity = this.TotalSectorsLong * 512L;
			}
			switch (translation)
			{
			case GeometryTranslation.None:
				return this;
			case GeometryTranslation.Auto:
				if (this.IsBiosSafe)
				{
					return this;
				}
				return Geometry.LbaAssistedBiosGeometry(capacity);
			case GeometryTranslation.Lba:
				return Geometry.LbaAssistedBiosGeometry(capacity);
			case GeometryTranslation.Large:
				return Geometry.LargeBiosGeometry(this);
			default:
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Translation mode '{0}' not yet implemented", new object[]
				{
					translation
				}), "translation");
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00003344 File Offset: 0x00001544
		public override bool Equals(object obj)
		{
			if (obj == null || obj.GetType() != base.GetType())
			{
				return false;
			}
			Geometry geometry = (Geometry)obj;
			return this.Cylinders == geometry.Cylinders && this.HeadsPerCylinder == geometry.HeadsPerCylinder && this.SectorsPerTrack == geometry.SectorsPerTrack && this.BytesPerSector == geometry.BytesPerSector;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000033AC File Offset: 0x000015AC
		public override int GetHashCode()
		{
			return this.Cylinders.GetHashCode() ^ this.HeadsPerCylinder.GetHashCode() ^ this.SectorsPerTrack.GetHashCode() ^ this.BytesPerSector.GetHashCode();
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000033F4 File Offset: 0x000015F4
		public override string ToString()
		{
			if (this.BytesPerSector == 512)
			{
				return string.Concat(new object[]
				{
					"(",
					this.Cylinders,
					"/",
					this.HeadsPerCylinder,
					"/",
					this.SectorsPerTrack,
					")"
				});
			}
			return string.Concat(new object[]
			{
				"(",
				this.Cylinders,
				"/",
				this.HeadsPerCylinder,
				"/",
				this.SectorsPerTrack,
				":",
				this.BytesPerSector,
				")"
			});
		}
	}
}
