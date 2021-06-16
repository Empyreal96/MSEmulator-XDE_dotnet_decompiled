using System;
using System.Security.AccessControl;
using DiscUtils.Streams;

namespace DiscUtils.Registry
{
	// Token: 0x0200000E RID: 14
	internal sealed class SecurityCell : Cell
	{
		// Token: 0x06000068 RID: 104 RVA: 0x000045D0 File Offset: 0x000027D0
		public SecurityCell(RegistrySecurity secDesc) : this(-1)
		{
			this.SecurityDescriptor = secDesc;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000045E0 File Offset: 0x000027E0
		public SecurityCell(int index) : base(index)
		{
			this.PreviousIndex = -1;
			this.NextIndex = -1;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x0600006A RID: 106 RVA: 0x000045F7 File Offset: 0x000027F7
		// (set) Token: 0x0600006B RID: 107 RVA: 0x000045FF File Offset: 0x000027FF
		public int NextIndex { get; set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00004608 File Offset: 0x00002808
		// (set) Token: 0x0600006D RID: 109 RVA: 0x00004610 File Offset: 0x00002810
		public int PreviousIndex { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600006E RID: 110 RVA: 0x00004619 File Offset: 0x00002819
		// (set) Token: 0x0600006F RID: 111 RVA: 0x00004621 File Offset: 0x00002821
		public RegistrySecurity SecurityDescriptor { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000070 RID: 112 RVA: 0x0000462C File Offset: 0x0000282C
		public override int Size
		{
			get
			{
				int num = this.SecurityDescriptor.GetSecurityDescriptorBinaryForm().Length;
				return 20 + num;
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000071 RID: 113 RVA: 0x0000464B File Offset: 0x0000284B
		// (set) Token: 0x06000072 RID: 114 RVA: 0x00004653 File Offset: 0x00002853
		public int UsageCount { get; set; }

		// Token: 0x06000073 RID: 115 RVA: 0x0000465C File Offset: 0x0000285C
		public override int ReadFrom(byte[] buffer, int offset)
		{
			this.PreviousIndex = EndianUtilities.ToInt32LittleEndian(buffer, offset + 4);
			this.NextIndex = EndianUtilities.ToInt32LittleEndian(buffer, offset + 8);
			this.UsageCount = EndianUtilities.ToInt32LittleEndian(buffer, offset + 12);
			int num = EndianUtilities.ToInt32LittleEndian(buffer, offset + 16);
			byte[] array = new byte[num];
			Array.Copy(buffer, offset + 20, array, 0, num);
			this.SecurityDescriptor = new RegistrySecurity();
			this.SecurityDescriptor.SetSecurityDescriptorBinaryForm(array);
			return 20 + num;
		}

		// Token: 0x06000074 RID: 116 RVA: 0x000046D4 File Offset: 0x000028D4
		public override void WriteTo(byte[] buffer, int offset)
		{
			byte[] securityDescriptorBinaryForm = this.SecurityDescriptor.GetSecurityDescriptorBinaryForm();
			EndianUtilities.StringToBytes("sk", buffer, offset, 2);
			EndianUtilities.WriteBytesLittleEndian(this.PreviousIndex, buffer, offset + 4);
			EndianUtilities.WriteBytesLittleEndian(this.NextIndex, buffer, offset + 8);
			EndianUtilities.WriteBytesLittleEndian(this.UsageCount, buffer, offset + 12);
			EndianUtilities.WriteBytesLittleEndian(securityDescriptorBinaryForm.Length, buffer, offset + 16);
			Array.Copy(securityDescriptorBinaryForm, 0, buffer, offset + 20, securityDescriptorBinaryForm.Length);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004744 File Offset: 0x00002944
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"SecDesc:",
				this.SecurityDescriptor.GetSecurityDescriptorSddlForm(AccessControlSections.All),
				" (refCount:",
				this.UsageCount,
				")"
			});
		}
	}
}
