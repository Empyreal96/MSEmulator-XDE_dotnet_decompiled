using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000047 RID: 71
	internal sealed class Quotas
	{
		// Token: 0x06000364 RID: 868 RVA: 0x00012F7C File Offset: 0x0001117C
		public Quotas(File file)
		{
			this._ownerIndex = new IndexView<Quotas.OwnerKey, Quotas.OwnerRecord>(file.GetIndex("$O"));
			this._quotaIndex = new IndexView<Quotas.OwnerRecord, Quotas.QuotaRecord>(file.GetIndex("$Q"));
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00012FB0 File Offset: 0x000111B0
		public static Quotas Initialize(File file)
		{
			Index index = file.CreateIndex("$O", AttributeType.None, AttributeCollationRule.Sid);
			Index index2 = file.CreateIndex("$Q", AttributeType.None, AttributeCollationRule.UnsignedLong);
			IndexView<Quotas.OwnerKey, Quotas.OwnerRecord> indexView = new IndexView<Quotas.OwnerKey, Quotas.OwnerRecord>(index);
			IndexView<Quotas.OwnerRecord, Quotas.QuotaRecord> indexView2 = new IndexView<Quotas.OwnerRecord, Quotas.QuotaRecord>(index2);
			Quotas.OwnerKey ownerKey = new Quotas.OwnerKey(new SecurityIdentifier(WellKnownSidType.BuiltinAdministratorsSid, null));
			Quotas.OwnerRecord ownerRecord = new Quotas.OwnerRecord(256);
			indexView[ownerKey] = ownerRecord;
			indexView2[new Quotas.OwnerRecord(1)] = new Quotas.QuotaRecord(null);
			indexView2[ownerRecord] = new Quotas.QuotaRecord(ownerKey.Sid);
			return new Quotas(file);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00013030 File Offset: 0x00011230
		public void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "QUOTAS");
			writer.WriteLine(indent + "  OWNER INDEX");
			foreach (KeyValuePair<Quotas.OwnerKey, Quotas.OwnerRecord> keyValuePair in this._ownerIndex.Entries)
			{
				writer.WriteLine(indent + "    OWNER INDEX ENTRY");
				writer.WriteLine(indent + "            SID: " + keyValuePair.Key.Sid);
				writer.WriteLine(indent + "       Owner Id: " + keyValuePair.Value.OwnerId);
			}
			writer.WriteLine(indent + "  QUOTA INDEX");
			foreach (KeyValuePair<Quotas.OwnerRecord, Quotas.QuotaRecord> keyValuePair2 in this._quotaIndex.Entries)
			{
				writer.WriteLine(indent + "    QUOTA INDEX ENTRY");
				writer.WriteLine(indent + "           Owner Id: " + keyValuePair2.Key.OwnerId);
				writer.WriteLine(indent + "           User SID: " + keyValuePair2.Value.Sid);
				writer.WriteLine(indent + "            Changed: " + keyValuePair2.Value.ChangeTime);
				writer.WriteLine(indent + "           Exceeded: " + keyValuePair2.Value.ExceededTime);
				writer.WriteLine(indent + "         Bytes Used: " + keyValuePair2.Value.BytesUsed);
				writer.WriteLine(indent + "              Flags: " + keyValuePair2.Value.Flags);
				writer.WriteLine(indent + "         Hard Limit: " + keyValuePair2.Value.HardLimit);
				writer.WriteLine(indent + "      Warning Limit: " + keyValuePair2.Value.WarningLimit);
				writer.WriteLine(indent + "            Version: " + keyValuePair2.Value.Version);
			}
		}

		// Token: 0x04000161 RID: 353
		private readonly IndexView<Quotas.OwnerKey, Quotas.OwnerRecord> _ownerIndex;

		// Token: 0x04000162 RID: 354
		private readonly IndexView<Quotas.OwnerRecord, Quotas.QuotaRecord> _quotaIndex;

		// Token: 0x02000087 RID: 135
		internal sealed class OwnerKey : IByteArraySerializable
		{
			// Token: 0x060004BF RID: 1215 RVA: 0x0001757B File Offset: 0x0001577B
			public OwnerKey()
			{
			}

			// Token: 0x060004C0 RID: 1216 RVA: 0x00017583 File Offset: 0x00015783
			public OwnerKey(SecurityIdentifier sid)
			{
				this.Sid = sid;
			}

			// Token: 0x17000155 RID: 341
			// (get) Token: 0x060004C1 RID: 1217 RVA: 0x00017592 File Offset: 0x00015792
			public int Size
			{
				get
				{
					return this.Sid.BinaryLength;
				}
			}

			// Token: 0x060004C2 RID: 1218 RVA: 0x0001759F File Offset: 0x0001579F
			public int ReadFrom(byte[] buffer, int offset)
			{
				this.Sid = new SecurityIdentifier(buffer, offset);
				return this.Sid.BinaryLength;
			}

			// Token: 0x060004C3 RID: 1219 RVA: 0x000175B9 File Offset: 0x000157B9
			public void WriteTo(byte[] buffer, int offset)
			{
				this.Sid.GetBinaryForm(buffer, offset);
			}

			// Token: 0x060004C4 RID: 1220 RVA: 0x000175C8 File Offset: 0x000157C8
			public override string ToString()
			{
				return string.Format(CultureInfo.InvariantCulture, "[Sid:{0}]", new object[]
				{
					this.Sid
				});
			}

			// Token: 0x04000250 RID: 592
			public SecurityIdentifier Sid;
		}

		// Token: 0x02000088 RID: 136
		internal sealed class OwnerRecord : IByteArraySerializable
		{
			// Token: 0x060004C5 RID: 1221 RVA: 0x000175E8 File Offset: 0x000157E8
			public OwnerRecord()
			{
			}

			// Token: 0x060004C6 RID: 1222 RVA: 0x000175F0 File Offset: 0x000157F0
			public OwnerRecord(int ownerId)
			{
				this.OwnerId = ownerId;
			}

			// Token: 0x17000156 RID: 342
			// (get) Token: 0x060004C7 RID: 1223 RVA: 0x000175FF File Offset: 0x000157FF
			public int Size
			{
				get
				{
					return 4;
				}
			}

			// Token: 0x060004C8 RID: 1224 RVA: 0x00017602 File Offset: 0x00015802
			public int ReadFrom(byte[] buffer, int offset)
			{
				this.OwnerId = EndianUtilities.ToInt32LittleEndian(buffer, offset);
				return 4;
			}

			// Token: 0x060004C9 RID: 1225 RVA: 0x00017612 File Offset: 0x00015812
			public void WriteTo(byte[] buffer, int offset)
			{
				EndianUtilities.WriteBytesLittleEndian(this.OwnerId, buffer, offset);
			}

			// Token: 0x060004CA RID: 1226 RVA: 0x00017621 File Offset: 0x00015821
			public override string ToString()
			{
				return "[OwnerId:" + this.OwnerId + "]";
			}

			// Token: 0x04000251 RID: 593
			public int OwnerId;
		}

		// Token: 0x02000089 RID: 137
		internal sealed class QuotaRecord : IByteArraySerializable
		{
			// Token: 0x060004CB RID: 1227 RVA: 0x0001763D File Offset: 0x0001583D
			public QuotaRecord()
			{
			}

			// Token: 0x060004CC RID: 1228 RVA: 0x00017645 File Offset: 0x00015845
			public QuotaRecord(SecurityIdentifier sid)
			{
				this.Version = 2;
				this.Flags = 1;
				this.ChangeTime = DateTime.UtcNow;
				this.WarningLimit = -1L;
				this.HardLimit = -1L;
				this.Sid = sid;
			}

			// Token: 0x17000157 RID: 343
			// (get) Token: 0x060004CD RID: 1229 RVA: 0x0001767D File Offset: 0x0001587D
			public int Size
			{
				get
				{
					return 48 + ((this.Sid == null) ? 0 : this.Sid.BinaryLength);
				}
			}

			// Token: 0x060004CE RID: 1230 RVA: 0x000176A0 File Offset: 0x000158A0
			public int ReadFrom(byte[] buffer, int offset)
			{
				this.Version = EndianUtilities.ToInt32LittleEndian(buffer, offset);
				this.Flags = EndianUtilities.ToInt32LittleEndian(buffer, offset + 4);
				this.BytesUsed = EndianUtilities.ToInt64LittleEndian(buffer, offset + 8);
				this.ChangeTime = DateTime.FromFileTimeUtc(EndianUtilities.ToInt64LittleEndian(buffer, offset + 16));
				this.WarningLimit = EndianUtilities.ToInt64LittleEndian(buffer, offset + 24);
				this.HardLimit = EndianUtilities.ToInt64LittleEndian(buffer, offset + 32);
				this.ExceededTime = EndianUtilities.ToInt64LittleEndian(buffer, offset + 40);
				if (buffer.Length - offset > 48)
				{
					this.Sid = new SecurityIdentifier(buffer, offset + 48);
					return 48 + this.Sid.BinaryLength;
				}
				return 48;
			}

			// Token: 0x060004CF RID: 1231 RVA: 0x00017748 File Offset: 0x00015948
			public void WriteTo(byte[] buffer, int offset)
			{
				EndianUtilities.WriteBytesLittleEndian(this.Version, buffer, offset);
				EndianUtilities.WriteBytesLittleEndian(this.Flags, buffer, offset + 4);
				EndianUtilities.WriteBytesLittleEndian(this.BytesUsed, buffer, offset + 8);
				EndianUtilities.WriteBytesLittleEndian(this.ChangeTime.ToFileTimeUtc(), buffer, offset + 16);
				EndianUtilities.WriteBytesLittleEndian(this.WarningLimit, buffer, offset + 24);
				EndianUtilities.WriteBytesLittleEndian(this.HardLimit, buffer, offset + 32);
				EndianUtilities.WriteBytesLittleEndian(this.ExceededTime, buffer, offset + 40);
				if (this.Sid != null)
				{
					this.Sid.GetBinaryForm(buffer, offset + 48);
				}
			}

			// Token: 0x060004D0 RID: 1232 RVA: 0x000177E4 File Offset: 0x000159E4
			public override string ToString()
			{
				return string.Concat(new object[]
				{
					"[V:",
					this.Version,
					",F:",
					this.Flags,
					",BU:",
					this.BytesUsed,
					",CT:",
					this.ChangeTime,
					",WL:",
					this.WarningLimit,
					",HL:",
					this.HardLimit,
					",ET:",
					this.ExceededTime,
					",SID:",
					this.Sid,
					"]"
				});
			}

			// Token: 0x04000252 RID: 594
			public long BytesUsed;

			// Token: 0x04000253 RID: 595
			public DateTime ChangeTime;

			// Token: 0x04000254 RID: 596
			public long ExceededTime;

			// Token: 0x04000255 RID: 597
			public int Flags;

			// Token: 0x04000256 RID: 598
			public long HardLimit;

			// Token: 0x04000257 RID: 599
			public SecurityIdentifier Sid;

			// Token: 0x04000258 RID: 600
			public int Version;

			// Token: 0x04000259 RID: 601
			public long WarningLimit;
		}
	}
}
