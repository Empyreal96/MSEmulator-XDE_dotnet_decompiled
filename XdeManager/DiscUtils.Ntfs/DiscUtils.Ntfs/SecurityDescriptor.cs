using System;
using System.IO;
using System.Security.AccessControl;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x0200004C RID: 76
	internal sealed class SecurityDescriptor : IByteArraySerializable, IDiagnosticTraceable
	{
		// Token: 0x0600038D RID: 909 RVA: 0x00013F8A File Offset: 0x0001218A
		public SecurityDescriptor()
		{
		}

		// Token: 0x0600038E RID: 910 RVA: 0x00013F92 File Offset: 0x00012192
		public SecurityDescriptor(RawSecurityDescriptor secDesc)
		{
			this.Descriptor = secDesc;
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600038F RID: 911 RVA: 0x00013FA1 File Offset: 0x000121A1
		// (set) Token: 0x06000390 RID: 912 RVA: 0x00013FA9 File Offset: 0x000121A9
		public RawSecurityDescriptor Descriptor { get; set; }

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000391 RID: 913 RVA: 0x00013FB2 File Offset: 0x000121B2
		public int Size
		{
			get
			{
				return this.Descriptor.BinaryLength;
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00013FBF File Offset: 0x000121BF
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.Descriptor = new RawSecurityDescriptor(buffer, offset);
			return this.Descriptor.BinaryLength;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00013FDC File Offset: 0x000121DC
		public void WriteTo(byte[] buffer, int offset)
		{
			ControlFlags controlFlags = this.Descriptor.ControlFlags;
			buffer[offset] = 1;
			buffer[offset + 1] = this.Descriptor.ResourceManagerControl;
			EndianUtilities.WriteBytesLittleEndian((ushort)controlFlags, buffer, offset + 2);
			for (int i = 4; i < 20; i++)
			{
				buffer[offset + i] = 0;
			}
			int num = 20;
			RawAcl discretionaryAcl = this.Descriptor.DiscretionaryAcl;
			if ((controlFlags & ControlFlags.DiscretionaryAclPresent) != ControlFlags.None && discretionaryAcl != null)
			{
				EndianUtilities.WriteBytesLittleEndian(num, buffer, offset + 16);
				discretionaryAcl.GetBinaryForm(buffer, offset + num);
				num += this.Descriptor.DiscretionaryAcl.BinaryLength;
			}
			else
			{
				EndianUtilities.WriteBytesLittleEndian(0, buffer, offset + 16);
			}
			RawAcl systemAcl = this.Descriptor.SystemAcl;
			if ((controlFlags & ControlFlags.SystemAclPresent) != ControlFlags.None && systemAcl != null)
			{
				EndianUtilities.WriteBytesLittleEndian(num, buffer, offset + 12);
				systemAcl.GetBinaryForm(buffer, offset + num);
				num += this.Descriptor.SystemAcl.BinaryLength;
			}
			else
			{
				EndianUtilities.WriteBytesLittleEndian(0, buffer, offset + 12);
			}
			EndianUtilities.WriteBytesLittleEndian(num, buffer, offset + 4);
			this.Descriptor.Owner.GetBinaryForm(buffer, offset + num);
			num += this.Descriptor.Owner.BinaryLength;
			EndianUtilities.WriteBytesLittleEndian(num, buffer, offset + 8);
			this.Descriptor.Group.GetBinaryForm(buffer, offset + num);
			num += this.Descriptor.Group.BinaryLength;
			if (num != this.Descriptor.BinaryLength)
			{
				throw new IOException("Failed to write Security Descriptor correctly");
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0001413D File Offset: 0x0001233D
		public void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "Descriptor: " + this.Descriptor.GetSddlForm(AccessControlSections.All));
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00014160 File Offset: 0x00012360
		public uint CalcHash()
		{
			byte[] array = new byte[this.Size];
			this.WriteTo(array, 0);
			uint num = 0U;
			for (int i = 0; i < array.Length / 4; i++)
			{
				num = EndianUtilities.ToUInt32LittleEndian(array, i * 4) + (num << 3 | num >> 29);
			}
			return num;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000141A8 File Offset: 0x000123A8
		internal static RawSecurityDescriptor CalcNewObjectDescriptor(RawSecurityDescriptor parent, bool isContainer)
		{
			RawAcl systemAcl = SecurityDescriptor.InheritAcl(parent.SystemAcl, isContainer);
			RawAcl discretionaryAcl = SecurityDescriptor.InheritAcl(parent.DiscretionaryAcl, isContainer);
			return new RawSecurityDescriptor(parent.ControlFlags, parent.Owner, parent.Group, systemAcl, discretionaryAcl);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000141E8 File Offset: 0x000123E8
		private static RawAcl InheritAcl(RawAcl parentAcl, bool isContainer)
		{
			AceFlags aceFlags = isContainer ? AceFlags.ContainerInherit : AceFlags.ObjectInherit;
			RawAcl rawAcl = null;
			if (parentAcl != null)
			{
				rawAcl = new RawAcl(parentAcl.Revision, parentAcl.Count);
				foreach (GenericAce genericAce in parentAcl)
				{
					if ((genericAce.AceFlags & aceFlags) != AceFlags.None)
					{
						GenericAce genericAce2 = genericAce.Copy();
						AceFlags aceFlags2 = genericAce.AceFlags;
						if ((aceFlags2 & AceFlags.NoPropagateInherit) != AceFlags.None)
						{
							aceFlags2 &= ~(AceFlags.ObjectInherit | AceFlags.ContainerInherit | AceFlags.NoPropagateInherit);
						}
						aceFlags2 &= ~AceFlags.InheritOnly;
						aceFlags2 |= AceFlags.Inherited;
						genericAce2.AceFlags = aceFlags2;
						rawAcl.InsertAce(rawAcl.Count, genericAce2);
					}
				}
			}
			return rawAcl;
		}
	}
}
