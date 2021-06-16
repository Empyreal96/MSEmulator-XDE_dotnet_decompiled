using System;
using System.Management.Automation;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.PowerShell.Commands
{
	// Token: 0x02000773 RID: 1907
	public class FileSystemContentDynamicParametersBase
	{
		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x06004C59 RID: 19545 RVA: 0x001945C0 File Offset: 0x001927C0
		// (set) Token: 0x06004C5A RID: 19546 RVA: 0x001945C8 File Offset: 0x001927C8
		[Parameter]
		public FileSystemCmdletProviderEncoding Encoding
		{
			get
			{
				return this.streamType;
			}
			set
			{
				this.streamType = value;
			}
		}

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x06004C5B RID: 19547 RVA: 0x001945D1 File Offset: 0x001927D1
		// (set) Token: 0x06004C5C RID: 19548 RVA: 0x001945D9 File Offset: 0x001927D9
		[Parameter]
		public string Stream { get; set; }

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x06004C5D RID: 19549 RVA: 0x001945E2 File Offset: 0x001927E2
		public Encoding EncodingType
		{
			get
			{
				return FileSystemContentDynamicParametersBase.GetEncodingFromEnum(this.streamType);
			}
		}

		// Token: 0x06004C5E RID: 19550 RVA: 0x001945F0 File Offset: 0x001927F0
		private static Encoding GetEncodingFromEnum(FileSystemCmdletProviderEncoding type)
		{
			Encoding unicode = System.Text.Encoding.Unicode;
			switch (type)
			{
			case FileSystemCmdletProviderEncoding.String:
				return System.Text.Encoding.Unicode;
			case FileSystemCmdletProviderEncoding.Unicode:
				return System.Text.Encoding.Unicode;
			case FileSystemCmdletProviderEncoding.BigEndianUnicode:
				return System.Text.Encoding.BigEndianUnicode;
			case FileSystemCmdletProviderEncoding.UTF8:
				return System.Text.Encoding.UTF8;
			case FileSystemCmdletProviderEncoding.UTF7:
				return System.Text.Encoding.UTF7;
			case FileSystemCmdletProviderEncoding.UTF32:
				return System.Text.Encoding.UTF32;
			case FileSystemCmdletProviderEncoding.Ascii:
				return System.Text.Encoding.ASCII;
			case FileSystemCmdletProviderEncoding.Default:
				return ClrFacade.GetDefaultEncoding();
			case FileSystemCmdletProviderEncoding.Oem:
			{
				uint oemcp = FileSystemContentDynamicParametersBase.NativeMethods.GetOEMCP();
				return System.Text.Encoding.GetEncoding((int)oemcp);
			}
			case FileSystemCmdletProviderEncoding.BigEndianUTF32:
				return System.Text.Encoding.GetEncoding("utf-32BE");
			}
			return System.Text.Encoding.Unicode;
		}

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x06004C5F RID: 19551 RVA: 0x0019469E File Offset: 0x0019289E
		public bool UsingByteEncoding
		{
			get
			{
				return this.streamType == FileSystemCmdletProviderEncoding.Byte;
			}
		}

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x06004C60 RID: 19552 RVA: 0x001946A9 File Offset: 0x001928A9
		public bool WasStreamTypeSpecified
		{
			get
			{
				return this.streamType != FileSystemCmdletProviderEncoding.String;
			}
		}

		// Token: 0x040024D4 RID: 9428
		private FileSystemCmdletProviderEncoding streamType = FileSystemCmdletProviderEncoding.String;

		// Token: 0x02000774 RID: 1908
		private static class NativeMethods
		{
			// Token: 0x06004C62 RID: 19554
			[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
			internal static extern uint GetOEMCP();
		}
	}
}
