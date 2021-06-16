using System;
using System.Collections.Generic;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000042 RID: 66
	internal class NtfsStream
	{
		// Token: 0x06000347 RID: 839 RVA: 0x00012A30 File Offset: 0x00010C30
		public NtfsStream(File file, NtfsAttribute attr)
		{
			this._file = file;
			this.Attribute = attr;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00012A46 File Offset: 0x00010C46
		public NtfsAttribute Attribute { get; }

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000349 RID: 841 RVA: 0x00012A4E File Offset: 0x00010C4E
		public AttributeType AttributeType
		{
			get
			{
				return this.Attribute.Type;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00012A5B File Offset: 0x00010C5B
		public string Name
		{
			get
			{
				return this.Attribute.Name;
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00012A68 File Offset: 0x00010C68
		public T GetContent<T>() where T : IByteArraySerializable, IDiagnosticTraceable, new()
		{
			byte[] buffer;
			using (Stream stream = this.Open(FileAccess.Read))
			{
				buffer = StreamUtilities.ReadExact(stream, (int)stream.Length);
			}
			T result = Activator.CreateInstance<T>();
			result.ReadFrom(buffer, 0);
			return result;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00012AC0 File Offset: 0x00010CC0
		public void SetContent<T>(T value) where T : IByteArraySerializable, IDiagnosticTraceable, new()
		{
			byte[] array = new byte[value.Size];
			value.WriteTo(array, 0);
			using (Stream stream = this.Open(FileAccess.Write))
			{
				stream.Write(array, 0, array.Length);
				stream.SetLength((long)array.Length);
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00012B28 File Offset: 0x00010D28
		public SparseStream Open(FileAccess access)
		{
			return this.Attribute.Open(access);
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00012B36 File Offset: 0x00010D36
		internal Range<long, long>[] GetClusters()
		{
			return this.Attribute.GetClusters();
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00012B44 File Offset: 0x00010D44
		internal StreamExtent[] GetAbsoluteExtents()
		{
			List<StreamExtent> list = new List<StreamExtent>();
			long num = (long)this._file.Context.BiosParameterBlock.BytesPerCluster;
			if (this.Attribute.IsNonResident)
			{
				foreach (Range<long, long> range in this.Attribute.GetClusters())
				{
					list.Add(new StreamExtent(range.Offset * num, range.Count * num));
				}
			}
			else
			{
				list.Add(new StreamExtent(this.Attribute.OffsetToAbsolutePos(0L), this.Attribute.Length));
			}
			return list.ToArray();
		}

		// Token: 0x04000155 RID: 341
		private readonly File _file;
	}
}
