using System;
using System.Globalization;
using System.IO;
using DiscUtils.Internal;

namespace DiscUtils
{
	// Token: 0x02000013 RID: 19
	[VirtualDiskTransport("file")]
	internal sealed class FileTransport : VirtualDiskTransport
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000C7 RID: 199 RVA: 0x00002DCE File Offset: 0x00000FCE
		public override bool IsRawDisk
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00002DD4 File Offset: 0x00000FD4
		public override void Connect(Uri uri, string username, string password)
		{
			this._path = uri.LocalPath;
			this._extraInfo = uri.Fragment.TrimStart(new char[]
			{
				'#'
			});
			if (!Directory.Exists(Path.GetDirectoryName(this._path)))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "No such file '{0}'", new object[]
				{
					uri.OriginalString
				}), this._path);
			}
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00002E45 File Offset: 0x00001045
		public override VirtualDisk OpenDisk(FileAccess access)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00002E4C File Offset: 0x0000104C
		public override FileLocator GetFileLocator()
		{
			return new LocalFileLocator(Path.GetDirectoryName(this._path) + "\\");
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00002E68 File Offset: 0x00001068
		public override string GetFileName()
		{
			return Path.GetFileName(this._path);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00002E75 File Offset: 0x00001075
		public override string GetExtraInfo()
		{
			return this._extraInfo;
		}

		// Token: 0x04000021 RID: 33
		private string _extraInfo;

		// Token: 0x04000022 RID: 34
		private string _path;
	}
}
