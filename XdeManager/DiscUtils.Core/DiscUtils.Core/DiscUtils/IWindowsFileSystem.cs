using System;
using System.Security.AccessControl;

namespace DiscUtils
{
	// Token: 0x0200001E RID: 30
	public interface IWindowsFileSystem : IFileSystem
	{
		// Token: 0x0600012A RID: 298
		RawSecurityDescriptor GetSecurity(string path);

		// Token: 0x0600012B RID: 299
		void SetSecurity(string path, RawSecurityDescriptor securityDescriptor);

		// Token: 0x0600012C RID: 300
		ReparsePoint GetReparsePoint(string path);

		// Token: 0x0600012D RID: 301
		void SetReparsePoint(string path, ReparsePoint reparsePoint);

		// Token: 0x0600012E RID: 302
		void RemoveReparsePoint(string path);

		// Token: 0x0600012F RID: 303
		string GetShortName(string path);

		// Token: 0x06000130 RID: 304
		void SetShortName(string path, string shortName);

		// Token: 0x06000131 RID: 305
		WindowsFileInformation GetFileStandardInformation(string path);

		// Token: 0x06000132 RID: 306
		void SetFileStandardInformation(string path, WindowsFileInformation info);

		// Token: 0x06000133 RID: 307
		string[] GetAlternateDataStreams(string path);

		// Token: 0x06000134 RID: 308
		long GetFileId(string path);

		// Token: 0x06000135 RID: 309
		bool HasHardLinks(string path);
	}
}
