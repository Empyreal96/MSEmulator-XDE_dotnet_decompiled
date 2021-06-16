using System;
using System.IO;
using DiscUtils.Streams;

namespace DiscUtils.Ntfs
{
	// Token: 0x02000052 RID: 82
	internal class StructuredNtfsAttribute<T> : NtfsAttribute where T : IByteArraySerializable, IDiagnosticTraceable, new()
	{
		// Token: 0x060003B6 RID: 950 RVA: 0x00014E63 File Offset: 0x00013063
		public StructuredNtfsAttribute(File file, FileRecordReference containingFile, AttributeRecord record) : base(file, containingFile, record)
		{
			this._structure = Activator.CreateInstance<T>();
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00014E79 File Offset: 0x00013079
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x00014E87 File Offset: 0x00013087
		public T Content
		{
			get
			{
				this.Initialize();
				return this._structure;
			}
			set
			{
				this._structure = value;
				this._hasContent = true;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00014E97 File Offset: 0x00013097
		public bool HasContent
		{
			get
			{
				this.Initialize();
				return this._hasContent;
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00014EA8 File Offset: 0x000130A8
		public void Save()
		{
			byte[] array = new byte[this._structure.Size];
			this._structure.WriteTo(array, 0);
			using (Stream stream = base.Open(FileAccess.Write))
			{
				stream.Write(array, 0, array.Length);
				stream.SetLength((long)array.Length);
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00014F18 File Offset: 0x00013118
		public override string ToString()
		{
			this.Initialize();
			return this._structure.ToString();
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00014F34 File Offset: 0x00013134
		public override void Dump(TextWriter writer, string indent)
		{
			this.Initialize();
			writer.WriteLine(string.Concat(new string[]
			{
				indent,
				base.AttributeTypeName,
				" ATTRIBUTE (",
				(base.Name == null) ? "No Name" : base.Name,
				")"
			}));
			this._structure.Dump(writer, indent + "  ");
			this._primaryRecord.Dump(writer, indent + "  ");
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00014FC4 File Offset: 0x000131C4
		private void Initialize()
		{
			if (!this._initialized)
			{
				using (Stream stream = base.Open(FileAccess.Read))
				{
					byte[] buffer = StreamUtilities.ReadExact(stream, (int)base.Length);
					this._structure.ReadFrom(buffer, 0);
					this._hasContent = (stream.Length != 0L);
				}
				this._initialized = true;
			}
		}

		// Token: 0x0400018D RID: 397
		private bool _hasContent;

		// Token: 0x0400018E RID: 398
		private bool _initialized;

		// Token: 0x0400018F RID: 399
		private T _structure;
	}
}
