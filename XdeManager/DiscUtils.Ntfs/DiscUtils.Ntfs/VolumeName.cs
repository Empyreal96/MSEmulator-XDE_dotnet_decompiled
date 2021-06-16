using System;
using System.IO;
using System.Text;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000056 RID: 86
	internal sealed class VolumeName : IByteArraySerializable, IDiagnosticTraceable
	{
		// Token: 0x060003CB RID: 971 RVA: 0x0001530E File Offset: 0x0001350E
		public VolumeName()
		{
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00015316 File Offset: 0x00013516
		public VolumeName(string name)
		{
			this.Name = name;
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060003CD RID: 973 RVA: 0x00015325 File Offset: 0x00013525
		// (set) Token: 0x060003CE RID: 974 RVA: 0x0001532D File Offset: 0x0001352D
		public string Name { get; private set; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060003CF RID: 975 RVA: 0x00015336 File Offset: 0x00013536
		public int Size
		{
			get
			{
				return Encoding.Unicode.GetByteCount(this.Name);
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00015348 File Offset: 0x00013548
		public int ReadFrom(byte[] buffer, int offset)
		{
			this.Name = Encoding.Unicode.GetString(buffer, offset, buffer.Length - offset);
			return buffer.Length - offset;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00015366 File Offset: 0x00013566
		public void WriteTo(byte[] buffer, int offset)
		{
			Encoding.Unicode.GetBytes(this.Name, 0, this.Name.Length, buffer, offset);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00015387 File Offset: 0x00013587
		public void Dump(TextWriter writer, string indent)
		{
			writer.WriteLine(indent + "  Volume Name: " + this.Name);
		}
	}
}
